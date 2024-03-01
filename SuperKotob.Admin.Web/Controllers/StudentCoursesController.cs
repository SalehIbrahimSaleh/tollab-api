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

    public class StudentCoursesController : BaseWebController<StudentCourse, StudentCourse>
    {
        private TollabContext db = new TollabContext();
        public StudentCoursesController(IBusinessService<StudentCourse, StudentCourse> service, IAppConfigurations appConfigurations) : base(service, appConfigurations)
        {
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<ActionResult> Create(StudentCourse item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var IsEnrollment = db.StudentCourses.AsNoTracking().Where(i => i.StudentId == item.StudentId && i.CourseId == item.CourseId).FirstOrDefault();
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
                    //cmd.CommandText = "HandleTeacherPercentageAndApp";
                    //cmd.Parameters.AddWithValue("@CourseId", item.CourseId);
                    //cmd.Parameters.AddWithValue("@StudentId", item.StudentId);
                    
                    //cnn.Open();
                    //object o = cmd.ExecuteScalar();
                    //cnn.Close();

                    item.EnrollmentDate = DateTime.UtcNow;
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



        public async Task<ActionResult> StudentCourseReport()
        {
            var requestInputs = GetRequestInputs();//Request.Params.Keys;

            var response = new StudentCourseView();
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
            ViewBag.StudentCourses = new List<StudentCourseView>();

            return View(response);
        }

        [HttpPost]
        public async Task<ActionResult> StudentCourseReport(StudentCourseView studentCourse)
        {
            List<StudentCourseView> StudentCourses = new List<StudentCourseView>();
            string sql = @"Select * from StudentCourseView where 1=1 ";
            if (studentCourse.StudentId > 0)
            {
                sql = sql + " And Id = " + studentCourse.StudentId + "";
            }
            if (studentCourse.CourseId>0)
            {
                sql = sql + " And  courseId =" + studentCourse.CourseId + "";
            }
            if (studentCourse.CourseId > 0 || studentCourse.StudentId > 0)
            {
                StudentCourses = await db.Database.SqlQuery<StudentCourseView>(sql).ToListAsync();

            }
            ViewBag.StudentId = studentCourse.StudentId;
            ViewBag.CourseId = studentCourse.CourseId;
            ViewBag.StudentCourses = StudentCourses;
            return View( );
        }






        public async  Task<ActionResult> GetExcelFile(long? CourseId, long? StudentId)
        {
            List<StudentCourseView> StudentCourses = new List<StudentCourseView>();
            if (StudentId > 0)
            {
                //StudentCourses=await db.StudentCourseViews.Where(i => i.Id == studentCourse.StudentId).ToListAsync();

                StudentCourses = await db.Database.SqlQuery<StudentCourseView>("Select * from StudentCourseView where Id =" + StudentId + "").ToListAsync();
            }
            else if (CourseId > 0)
            {
                // StudentCourses = await db.StudentCourseViews.Where(i => i.CourseId == studentCourse.CourseId).ToListAsync();
                StudentCourses = await db.Database.SqlQuery<StudentCourseView>("Select * from StudentCourseView where courseId =" + CourseId + "").ToListAsync();

            }
            return ExportData("StudentCourses" + "_" + DateTime.Now.ToString("R" + "dd_MM_yyyy_HH_mm_ss") + "", StudentCourses);

        }




         
        public ActionResult ExportData(string name,List<StudentCourseView> StudentCourses)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            //string rootFolder = _hostingEnvironment.WebRootPath;
            //string fileName = @"ExportCustomers.xlsx";
            var dir = Server.MapPath($"~/Uploads/Reports/");
            var path = Path.Combine(dir, $"{name}.xlsx");

            FileInfo file = new FileInfo(path);

            using (ExcelPackage package = new ExcelPackage(file))
            {


                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("StudentCourses");
                int totalRows = StudentCourses.Count();

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
                    worksheet.Cells[row, 1].Value = StudentCourses[i].Id;
                    worksheet.Cells[row, 2].Value = StudentCourses[i].Name;
                    worksheet.Cells[row, 3].Value = StudentCourses[i].StudentNumber;
                    worksheet.Cells[row, 4].Value = StudentCourses[i].ParentName;
                    worksheet.Cells[row, 5].Value = StudentCourses[i].ParentPhone;
                    worksheet.Cells[row, 6].Value = StudentCourses[i].ParentName2;
                    worksheet.Cells[row, 7].Value = StudentCourses[i].ParentPhone2;
                    worksheet.Cells[row, 8].Value = StudentCourses[i].CourseName;
                    worksheet.Cells[row, 9].Value = StudentCourses[i].CourseTrack;
                    worksheet.Cells[row, 10].Value = StudentCourses[i].ExamSummary;
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


//        public async Task<ActionResult> DeleteAllINPCoursesOrTracks()
//        {
//            var sql =
//@"
//  delete  from StudentCourse where ReferenceNumber In (select  ReferenceNumber from StudentTransaction  where Reason like '%apple%' and CourseId is not NULL)
//  delete  from TeacherTransaction where ReferenceNumber In ( select  ReferenceNumber from StudentTransaction  where Reason like '%apple%' and CourseId is not NULL)
//  delete  from StudentTransaction  where Reason like '%apple%' and CourseId is not NULL
  
//  delete from StudentCourse where ReferenceNumber In (select  ReferenceNumber from StudentTransaction  where Reason like '%apple%' and TrackId is not NULL)
//  delete from TrackSubscription where ReferenceNumber In (select  ReferenceNumber from StudentTransaction  where Reason like '%apple%' and TrackId is not NULL)
//  delete from TeacherTransaction where ReferenceNumber In (select  ReferenceNumber from StudentTransaction  where Reason like '%apple%' and TrackId is not NULL)
//  delete from StudentTransaction  where Reason like '%apple%' and TrackId is not NULL ";


//            var delete=await db.Database.ExecuteSqlCommandAsync(sql);

//            return RedirectToAction("Success", "Home");
//        }
    }


}
