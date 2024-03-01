using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Data.Models;

namespace Tollab.Admin.UseCases.StudentPackages
{
   public class StudentPackageRepository : BaseRepository<StudentPackage>
    {
        protected override IQueryable<StudentPackage> SetInculdes(DbSet<StudentPackage> dbSet, RequestInputs requestInputs)
    {
        return dbSet.Include(item => item.Student).Include(item => item.Package);
    }
    protected override IQueryable<StudentPackage> SetWhere(IQueryable<StudentPackage> query, RequestInputs requestInputs)
    {
        query = base.SetWhere(query, requestInputs);
        var courseId = requestInputs.GetValue("PackageId", default(long?));
        if (courseId.HasValue && courseId.Value > 0)
            query = query.Where(item => item.PackageId == courseId.Value);

        var studentId = requestInputs.GetValue("StudentId", default(long?));
        if (studentId.HasValue && studentId.Value > 0)
            query = query.Where(item => item.StudentId == studentId.Value);

        return query;
    }
}
}
