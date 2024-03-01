using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SuperKotob.Admin.Data;
using Tollab.Admin.Data.Models;
using SuperKotob.Admin.Web.Controllers;
using SuperKotob.Admin.Core;
using SuperKotob.Admin.Utils.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace Tollab.Admin.Web.Controllers
{
    [Authorize]
    public class SystemSettingsController : BaseWebController<SystemSetting,SystemSetting>
    {
        private const string EncryptionKey = "AyHagaSaadWESCN9";
        public SystemSettingsController(IBusinessService<SystemSetting, SystemSetting> service, IAppConfigurations appConfigurations) : base(service, appConfigurations)
        {
        }
        private static  byte[] iv2 = {
            0x16, 0x61, 0x0F, 0x3A, 0x37, 0x3D, 0x1B, 0x51,
            0x4A, 0x39, 0x5A, 0x79, 0x29, 0x08, 0x01, 0x22
        };

        public override async Task<ActionResult> Index()
        {
            try
            {
                var requestInputs = GetRequestInputs();
                var response = await BusinessService.GetAsync(requestInputs);
                response.RequestInputs = requestInputs;
                ViewBag.RequestInputs = requestInputs;
                string viewName = GetViewName("Index");
                foreach (var item in response.Data)
                {
                    /*var ValueDecrypted = DecryptText(item.SettingValue, EncryptionKey);
                    item.SettingValue = ValueDecrypted;*/

                    /*
                    byte[] UserNamebytesToBeDecrypted = Convert.FromBase64String(item.SettingValue);
                    var d = Decrypt(UserNamebytesToBeDecrypted, EncryptionKey);
                    item.SettingValue = Encoding.UTF8.GetString(d);*/

                    SHA256 mySHA256 = SHA256Managed.Create();
                    byte[] key = mySHA256.ComputeHash(Encoding.ASCII.GetBytes(EncryptionKey));
                    string plaintext = DecryptSaad(item.SettingValue, key);
                    item.SettingValue = plaintext;
                }
                return View(viewName, response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return null;
        }

        public override async Task<ActionResult> Edit(long? id)
        {
            if (id == null || id.Value < 1)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var response = await BusinessService.GetAsync(id.Value);

            if (!response.HasData || response.Data.FirstOrDefault() == null)
                return HttpNotFound();

            var order = response.Data.FirstOrDefault();
            /*var ValueDecrypted = DecryptText(order.SettingValue, EncryptionKey);
            order.SettingValue = ValueDecrypted;*/


            //byte[] UserNamebytesToBeDecrypted = Convert.FromBase64String(order.SettingValue);
            //var d = Decrypt(UserNamebytesToBeDecrypted, EncryptionKey);
            //order.SettingValue = Encoding.UTF8.GetString(d);

            SHA256 mySHA256 = SHA256Managed.Create();
            byte[] key = mySHA256.ComputeHash(Encoding.ASCII.GetBytes(EncryptionKey));
            string plaintext = DecryptSaad(order.SettingValue, key);
            order.SettingValue = plaintext;


            string viewName = GetViewName("Edit");
            return View(viewName, order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<ActionResult> Edit(SystemSetting systemSetting)
        {
            if (ModelState.IsValid)
            {
                //1
                //var EncryptedValue= EncryptText(systemSetting.SettingValue, EncryptionKey);
                //systemSetting.SettingValue = EncryptedValue;

                //2
                //byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(systemSetting.SettingValue);
                //byte[] passwordBytes = Encoding.UTF8.GetBytes(EncryptionKey);
                //byte[] bytesEncrypteder = Encrypt(bytesToBeEncrypted, EncryptionKey);
                //string EncryptedString = Convert.ToBase64String(bytesEncrypteder);
                //systemSetting.SettingValue = EncryptedString;

                //3
                //byte[] UserNamebytesToBeDecrypted = Convert.FromBase64String(EncryptedString);
                //var d = Decrypt(UserNamebytesToBeDecrypted, EncryptionKey);

                //string result = Encoding.UTF8.GetString(d);


                SHA256 mySHA256 = SHA256Managed.Create();
                byte[] key = mySHA256.ComputeHash(Encoding.ASCII.GetBytes(EncryptionKey));
                string ciphertext = EncryptSaad(systemSetting.SettingValue, key);

                systemSetting.SettingValue = ciphertext;
                await BusinessService.UpdateAsync(systemSetting);
                return RedirectToIndex(systemSetting);
            }
             return View(systemSetting);
        }

        //public RijndaelManaged GetRijndaelManaged(String secretKey)
        //{
        //    var keyBytes = new byte[16];
        //    var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
        //    Array.Copy(secretKeyBytes, keyBytes, Math.Min(keyBytes.Length, secretKeyBytes.Length));
        //    return new RijndaelManaged
        //    {
        //        Mode = CipherMode.CBC,
        //        Padding = PaddingMode.Zeros,
        //        KeySize = 128,
        //        BlockSize = 128,
        //        Key = keyBytes,
        //        IV = keyBytes
        //    };
        //}

        //public byte[] Encrypt(byte[] plainBytes, RijndaelManaged rijndaelManaged)
        //{
        //    //Encrypt
        //    return rijndaelManaged.CreateEncryptor()
        //        .TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        //}

        //public byte[] Encrypt(byte[] plainText, String key)
        //{
        //    return Encrypt(plainText, GetRijndaelManaged(key));
        //}
        //public byte[] Decrypt(byte[] encryptedData, RijndaelManaged rijndaelManaged)
        //{
        //    return rijndaelManaged.CreateDecryptor()
        //        .TransformFinalBlock(encryptedData, 0, encryptedData.Length);
        //}

        //public byte[] Decrypt(byte[] encryptedText, String key)
        //{
        //    var encryptedBytes = encryptedText;
        //    return Decrypt(encryptedBytes, GetRijndaelManaged(key));
        //}

        /////////////////////////////////////////////////

        //public byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        //{
        //    byte[] encryptedBytes = null;

        //    // Set your salt here, change it to meet your flavor:
        //    // The salt bytes must be at least 8 bytes.
        //    byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        using (RijndaelManaged AES = new RijndaelManaged())
        //        {
        //            AES.KeySize = 256;
        //            AES.BlockSize = 128;

        //            var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
        //            AES.Key = key.GetBytes(AES.KeySize / 8);
        //            AES.IV = key.GetBytes(AES.BlockSize / 8);

        //            AES.Mode = CipherMode.CBC;

        //            using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
        //            {
        //                cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
        //                cs.Close();
        //            }
        //            encryptedBytes = ms.ToArray();
        //        }
        //    }

        //    return encryptedBytes;
        //}

        //public byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        //{
        //    byte[] decryptedBytes = null;

        //    // Set your salt here, change it to meet your flavor:
        //    // The salt bytes must be at least 8 bytes.
        //    byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        using (RijndaelManaged AES = new RijndaelManaged())
        //        {
        //            AES.KeySize = 256;
        //            AES.BlockSize = 128;

        //            var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
        //            AES.Key = key.GetBytes(AES.KeySize / 8);
        //            AES.IV = key.GetBytes(AES.BlockSize / 8);

        //            AES.Mode = CipherMode.CBC;

        //            using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
        //            {
        //                cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
        //                cs.Close();
        //            }
        //            decryptedBytes = ms.ToArray();
        //        }
        //    }

        //    return decryptedBytes;
        //}

        //public string EncryptText(string input, string password)
        //{
        //    // Get the bytes of the string
        //    byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(input);
        //    byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

        //    // Hash the password with SHA256
        //    passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

        //    byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes);

        //    string result = Convert.ToBase64String(bytesEncrypted);

        //    return result;
        //}
        //public string DecryptText(string input, string password)
        //{
        //    // Get the bytes of the string
        //    byte[] bytesToBeDecrypted = Convert.FromBase64String(input);
        //    byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        //    passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

        //    byte[] bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, passwordBytes);

        //    string result = Encoding.UTF8.GetString(bytesDecrypted);

        //    return result;
        //}

        public static string EncryptSaad(string plainText, byte[] key)
        {

            Byte[] toEncryptArray = Encoding.ASCII.GetBytes(plainText);

            System.Security.Cryptography.RijndaelManaged rm = new System.Security.Cryptography.RijndaelManaged
            {
                Key = key,
                Mode = System.Security.Cryptography.CipherMode.CFB,
                Padding = System.Security.Cryptography.PaddingMode.None,
                IV = iv2,
                FeedbackSize = 8,
                BlockSize = 128
            };

            System.Security.Cryptography.ICryptoTransform cTransform = rm.CreateEncryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string DecryptSaad(string cipherText, byte[] key)
        {

            Byte[] toEncryptArray = Convert.FromBase64String(cipherText);

            System.Security.Cryptography.RijndaelManaged rm = new System.Security.Cryptography.RijndaelManaged
            {
                Key = key,
                Mode = System.Security.Cryptography.CipherMode.CFB,
                Padding = System.Security.Cryptography.PaddingMode.None,
                IV = iv2,
                FeedbackSize = 8
            };

            System.Security.Cryptography.ICryptoTransform cTransform = rm.CreateDecryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Encoding.ASCII.GetString(resultArray);
        }




    }

}
