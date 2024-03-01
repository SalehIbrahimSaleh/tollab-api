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
using System.Reflection;
using Tollab.Admin.Data.Models.Views;
using System.Collections;
using System.Text;
using System.IO;
using CsvHelper;
using OfficeOpenXml;
using System.Data.SqlClient;

namespace Tollab.Admin.Web.Controllers
{
    [Authorize(Roles = "Admin")]

    public class StudentLivesController : BaseWebController<StudentLive, StudentLive>
    {
        private TollabContext db = new TollabContext();
        public StudentLivesController(IBusinessService<StudentLive, StudentLive> service, IAppConfigurations appConfigurations) : base(service, appConfigurations)
        {
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<ActionResult> Create(StudentLive item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var IsEnrollment = db.StudentLives.AsNoTracking().Where(i => i.StudentId == item.StudentId && i.LiveId == item.LiveId).FirstOrDefault();
                    if (IsEnrollment!=null)
                    {
                        ViewBag.Error = "هذه الدورة اضيفت لهذا الطالب مسبقاً";
                        return View(item);
                    }
                    //string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["TollabContext"].ConnectionString;
                    //SqlConnection cnn = new SqlConnection(cnnString);
                    //SqlCommand cmd = new SqlCommand();
                    //cmd.Connection = cnn;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    //cmd.CommandText = "HandleTeacherPercentageLiveAndApp";
                    //cmd.Parameters.AddWithValue("@LiveId", item.LiveId);
                    //cmd.Parameters.AddWithValue("@StudentId", item.StudentId);

                    //cnn.Open();
                    //object o = cmd.ExecuteScalar();
                    //cnn.Close();

                    await BusinessService.CreateAsync(item);
                    return RedirectToAction("Index");
                }

                 return View(item);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }



        public async Task<ActionResult> StudentLiveReport()
        {
            var requestInputs = GetRequestInputs();//Request.Params.Keys;

            var response = new StudentLiveView();
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
            ViewBag.StudentLives = new List<StudentLiveView>();

            return View(response);
        }

        [HttpPost]
        public async Task<ActionResult> StudentLiveReport(StudentLiveView studentLive)
        {
            List<StudentLiveView> StudentLives = new List<StudentLiveView>();
            string sql = @"Select * from StudentLiveView where 1=1 ";
            if (studentLive.StudentId > 0)
            {
                sql = sql + " And Id = " + studentLive.StudentId + "";
            }
            if (studentLive.LiveId>0)
            {
                sql = sql + " And  LiveId =" + studentLive.LiveId + "";
            }
            if (studentLive.LiveId > 0 || studentLive.StudentId > 0)
            {
                StudentLives = await db.Database.SqlQuery<StudentLiveView>(sql).ToListAsync();

            }
            ViewBag.StudentId = studentLive.StudentId;
            ViewBag.LiveId = studentLive.LiveId;
            ViewBag.StudentLives = StudentLives;
            return View( );
        }






        public async  Task<ActionResult> GetExcelFile(long? LiveId, long? StudentId)
        {
            List<StudentLiveView> StudentLives = new List<StudentLiveView>();
            if (StudentId > 0)
            {
                //StudentLives=await db.StudentLiveViews.Where(i => i.Id == studentLive.StudentId).ToListAsync();

                StudentLives = await db.Database.SqlQuery<StudentLiveView>("Select * from StudentLiveView where Id =" + StudentId + "").ToListAsync();
            }
            else if (LiveId > 0)
            {
                // StudentLives = await db.StudentLiveViews.Where(i => i.LiveId == studentLive.LiveId).ToListAsync();
                StudentLives = await db.Database.SqlQuery<StudentLiveView>("Select * from StudentLiveView where LiveId =" + LiveId + "").ToListAsync();

            }
            return ExportData("StudentLives" + "_" + DateTime.Now.ToString("R" + "dd_MM_yyyy_HH_mm_ss") + "", StudentLives);

        }




         
        public ActionResult ExportData(string name,List<StudentLiveView> StudentLives)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            //string rootFolder = _hostingEnvironment.WebRootPath;
            //string fileName = @"ExportCustomers.xlsx";
            var dir = Server.MapPath($"~/Uploads/Reports/");
            var path = Path.Combine(dir, $"{name}.xlsx");

            FileInfo file = new FileInfo(path);

            using (ExcelPackage package = new ExcelPackage(file))
            {


                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("StudentLives");
                int totalRows = StudentLives.Count();

                worksheet.Cells[1, 1].Value = "Id";
                worksheet.Cells[1, 2].Value = "Student Name";
                worksheet.Cells[1, 3].Value = "Student Number";
                worksheet.Cells[1, 4].Value = "Parent Name";
                worksheet.Cells[1, 5].Value = "Parent Phone";
                worksheet.Cells[1, 6].Value = "Parent Name2";
                worksheet.Cells[1, 7].Value = "Parent Phone2";
                worksheet.Cells[1, 8].Value = "Course Name";
                worksheet.Cells[1, 9].Value = "Course Track";
                worksheet.Cells[1, 10].Value = "Exam Summary";
                int i = 0;
                for (int row = 2; row <= totalRows + 1; row++)
                {
                    worksheet.Cells[row, 1].Value = StudentLives[i].Id;
                    worksheet.Cells[row, 2].Value = StudentLives[i].Name;
                    worksheet.Cells[row, 3].Value = StudentLives[i].StudentNumber;
                    worksheet.Cells[row, 4].Value = StudentLives[i].ParentName;
                    worksheet.Cells[row, 5].Value = StudentLives[i].ParentPhone;
                    worksheet.Cells[row, 6].Value = StudentLives[i].ParentName2;
                    worksheet.Cells[row, 7].Value = StudentLives[i].ParentPhone2;
                    i++;
                }

                package.Save();

                return File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", name+".xlsx");

            }

            return null;
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

 
    }


}
