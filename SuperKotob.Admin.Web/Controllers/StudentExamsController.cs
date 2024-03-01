using CsvHelper;
using OfficeOpenXml;
using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data;
using SuperKotob.Admin.Utils.Configuration;
using SuperKotob.Admin.Web.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Tollab.Admin.Data.Models;
using Tollab.Admin.Data.Models.Views;

namespace Tollab.Admin.Web.Controllers
{
    [Authorize]
    public class StudentExamsController : BaseWebController<StudentExam, StudentExam>
    {
        TollabContext db = new TollabContext();
        public StudentExamsController(IBusinessService<StudentExam, StudentExam> service, IAppConfigurations appConfigurations) : base(service, appConfigurations)
        {
        }

        public async Task<ActionResult> StudentExamReport()
        {
            var requestInputs = GetRequestInputs();//Request.Params.Keys;

            var response = new StudentExamView();
            if (requestInputs != null)
            {
                response.RequestInputs = requestInputs;
                foreach (var item in requestInputs.ToDictionary())
                {
                    Type type = response.GetType();
                    foreach (PropertyInfo prop in type.GetProperties())
                    {
                        PropertyInfo itemProp = response.GetType().GetProperty(prop.Name, BindingFlags.Public | BindingFlags.Instance);

                        var name = itemProp.Name;
                        if (string.Equals(item.Key, itemProp.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            itemProp.SetValue(response, item.Value);

                        }
                    }

                }
            }
            return View(response);
        }

        [HttpPost]
        public async Task<ActionResult> StudentExamReport(StudentExamView  studentExamView)
        {
            List<StudentExamView>  studentExamViews = new List<StudentExamView>();
            if (studentExamView.StudentId > 0)
            {
                studentExamViews = await db.StudentExamViews.Where(i => i.Id == studentExamView.StudentId).ToListAsync();

            }
            else if (studentExamView.ExamId > 0)
            {
                studentExamViews = await db.StudentExamViews.Where(i => i.ExamId == studentExamView.ExamId).ToListAsync();
            }
            ViewBag.StudentId = studentExamView.StudentId;
            ViewBag.ExamId = studentExamView.ExamId;

            return View("studentExamReportData", studentExamViews);
        }

        public async Task<ActionResult> GetExcelFile(long? ExamId, long? StudentId)
        {
            List<StudentExamView> StudentExamViews = new List<StudentExamView>();
            if (StudentId > 0)
            {
                StudentExamViews = await db.StudentExamViews.Where(i => i.Id == StudentId).ToListAsync();

            }
            else if (ExamId > 0)
            {
                StudentExamViews = await db.StudentExamViews.Where(i => i.ExamId == ExamId).ToListAsync();
            }
            return Download("StudentExams" + "_" + DateTime.Now.ToString("R" + "dd_MM_yyyy_HH_mm_ss") + "", StudentExamViews.AsEnumerable());

        }

        public ActionResult Download(string name, IEnumerable list)
        {
            string path = GetReportFilePath(name);
            WriteReportsFile(list, path, name);
            return File(path, "application/vnd.ms-excel", $"{name}.xls");

        }

        private static void WriteReportsFile(IEnumerable list, string path, string name)
        {
            var file = System.IO.File.Create(path);
            var sw = new StreamWriter(file, Encoding.UTF8);
            var csv = new CsvWriter(sw);
            csv.Configuration.Delimiter = ",";
            csv.WriteRecords(list);
            sw.Close();
            file.Close();
        }

        private string GetReportFilePath(string name)
        {
            var dir = Server.MapPath($"~/Uploads/Reports/");
            var path = Path.Combine(dir, $"{name}.xls");
            Directory.CreateDirectory(dir);
            return path;
        }


        //----------------------------------------------------------//

    }
}