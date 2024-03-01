using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using SuperKotob.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Data.Models;

namespace Tollab.Admin.UseCases.Students
{
   public class StudentRepository : BaseRepository<Student>
    {
        protected override IQueryable<Student> SetInculdes(DbSet<Student> dbSet, RequestInputs requestInputs)
        {
            var d = dbSet.Include(item => item.Country);
            return d;
        }
        protected override IQueryable<Student> SetWhere(IQueryable<Student> query, RequestInputs requestInputs)
        {
            query = base.SetWhere(query, requestInputs);
            var countryId = requestInputs.GetValue("CountryId", default(long?));
            if (countryId.HasValue && countryId.Value > 0)
                query = query.Where(item => item.CountryId == countryId.Value);

            var isGender = requestInputs.GetValue("IsGender", default(string));
            if (isGender == "1")
                query = query.Where(item => item.Gender == true);
            if (isGender == "2")
                query = query.Where(item => item.Gender == false);

            var isEnabled = requestInputs.GetValue("IsEnabled", default(string));
            if (isEnabled == "1")
                query = query.Where(item => item.Enabled == true);
            if (isEnabled == "2")
                query = query.Where(item => item.Enabled == false);

            return query;
        }

        public override async Task<DataResponse<LookupItem>> AutoCompleteAsync(string text, bool isIdList, RequestInputs requestInputs, string field)
        {
            if (field != null)
            {

                String[] parts = field.Split('.');

                if (parts.Count() == 2)
                {
                    String part1 = parts[0];
                    String part2 = parts[1];
                    field = "[" + part1 + "]." + part2;
                }
            }
            string where = "";
            if (text != null)
            {
                if (isIdList)
                {
                    where = " where Id = " + text + "";
                }
                else
                {
                    //var vtype = type.GetProperty(field).PropertyType;
                    //var stype =  typeof(String);

                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        //if (vtype ==  stype)
                        //{
                        //    where = " where " + field + " like N'%" + text + "%'";
                        //}
                        //else
                        where = " where " + field + " like N'%" + text + "%'";

                    }
                }
            }
            try
            {


                var query = "SELECT Id, ISNULL(CAST(Phone AS NVARCHAR(250)),'') + ' - '+ ISNULL(CAST(Name AS NVARCHAR(250)),'') AS Title FROM Student " + where + " Order By " + field;
                var lookuplist = DataContext.Database.SqlQuery<LookupItem>(query).ToList();

                return new DataResponse<LookupItem>(lookuplist);
            }
            catch (Exception ex)
            {

            }

            return null;
        }

    }
}
