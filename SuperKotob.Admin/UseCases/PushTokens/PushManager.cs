using Newtonsoft.Json.Linq;
using PushSharp.Apple;
using PushSharp.Core;
using PushSharp.Google;
using SuperKotob.Admin.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SuperKotob.UseCases.PushTokens
{
    public class PushManager : IPushManager
    {
        public void Push(String token, String message, string os,string title,string url)
        {
            if (os == "ios")
                PushToIphoneDevice(token, message,  title,  url);
            else
                PushToAndroidDevice(token, message,  title,  url);
        }
        public void PushToIphoneDevice(String token, String message, string title, string url)
        {
            string cerPath = ConfigurationManager.AppSettings["push_ios_cer"];
            string cerPassword = ConfigurationManager.AppSettings["push_ios_password"];
            var config = new ApnsConfiguration(ApnsConfiguration.ApnsServerEnvironment.Production,
        cerPath, cerPassword);

            // Create a new broker
            var apnsBroker = new ApnsServiceBroker(config);


            apnsBroker.OnNotificationSucceeded += (notification) =>
            {
                Console.WriteLine("Apple Notification Sent!");
            };

            // Start the broker
            apnsBroker.Start();


            // Queue a notification to send
            apnsBroker.QueueNotification(new ApnsNotification
            {
                DeviceToken = token,
                Payload = JObject.Parse("{\"aps\":{\"alert\":\"" + message + "\",\"badge\":1}}")
               //Payload = JObject.Parse("{\"aps\":{\"alert\":{\"message\":\""+message+ "\"title\":\""+title+ "\"url\":\""+url+"\"},\"url-args\":[]}}")

            });


            // Stop the broker, wait for it to finish   
            // This isn't done after every message, but after you're
            // done with the broker
            apnsBroker.Stop();

        }
        public void PushToAndroidDevice(String token, String message, string title, string url)
        {
            string cerPath = ConfigurationManager.AppSettings["push_android_cer"];
            string cerPassword = ConfigurationManager.AppSettings["push_android_password"];
            // Configuration
            var config = new GcmConfiguration(cerPath, cerPassword, null);

            // Create a new broker
            var gcmBroker = new GcmServiceBroker(config);

            // Wire up events
            //start
            // Wire up events
            gcmBroker.OnNotificationFailed += (notification, aggregateEx) => {

                aggregateEx.Handle(ex => {

                    // See what kind of exception it was to further diagnose
                    if (ex is GcmNotificationException)
                    {
                        var notificationException = (GcmNotificationException)ex;

                        // Deal with the failed notification
                        var gcmNotification = notificationException.Notification;
                        var description = notificationException.Description;

                        Console.WriteLine($"GCM Notification Failed: ID={gcmNotification.MessageId}, Desc={description}");
                    }
                    else if (ex is GcmMulticastResultException)
                    {
                        var multicastException = (GcmMulticastResultException)ex;

                        foreach (var succeededNotification in multicastException.Succeeded)
                        {
                            Console.WriteLine($"GCM Notification Succeeded: ID={succeededNotification.MessageId}");
                        }

                        foreach (var failedKvp in multicastException.Failed)
                        {
                            var n = failedKvp.Key;
                            var e = failedKvp.Value;

                            Console.WriteLine($"GCM Notification Failed: ID={n.MessageId}, Desc={e.Message}");
                        }

                    }
                    else if (ex is DeviceSubscriptionExpiredException)
                    {
                        var expiredException = (DeviceSubscriptionExpiredException)ex;

                        var oldId = expiredException.OldSubscriptionId;
                        var newId = expiredException.NewSubscriptionId;

                        Console.WriteLine($"Device RegistrationId Expired: {oldId}");

                        if (!string.IsNullOrWhiteSpace(newId))
                        {
                            // If this value isn't null, our subscription changed and we should update our database
                            Console.WriteLine($"Device RegistrationId Changed To: {newId}");
                        }
                    }
                    else if (ex is RetryAfterException)
                    {
                        var retryException = (RetryAfterException)ex;
                        // If you get rate limited, you should stop sending messages until after the RetryAfterUtc date
                        Console.WriteLine($"GCM Rate Limited, don't send more until after {retryException.RetryAfterUtc}");
                    }
                    else
                    {
                        Console.WriteLine("GCM Notification Failed for some unknown reason");
                    }

                    // Mark it as handled
                    return true;
                });
            };

            //end
            gcmBroker.OnNotificationSucceeded += (notification) =>
            {
                Console.WriteLine("GCM Notification Sent!");
            };

            // Start the broker
            gcmBroker.Start();


            // Queue a notification to send
            gcmBroker.QueueNotification(new GcmNotification
            {
                RegistrationIds = new List<string> {token },

                // Data = JObject.Parse("{ \"message\" : \"" + message + "\",\"time\" : \"" + System.DateTime.Now.ToString() + "\"}")
                Data = JObject.Parse(
                        "{" +
                            "\"message\" : \"" + message + "\"," +
                            "\"title\" : \"" + title + "\"," +
                            "\"url\" : \"" + url + "\"" +
                        "}")
            });


            // Stop the broker, wait for it to finish   
            // This isn't done after every message, but after you're
            // done with the broker
            gcmBroker.Stop();

        }
    }
}