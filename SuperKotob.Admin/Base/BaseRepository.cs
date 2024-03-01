using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Models;
using SuperKotob.Admin.Models;
using SuperKotob.Admin.Utils.Mapping;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperKotob.Models;
using System.Data.Entity.Migrations;
using System.Reflection;
using System.Data.Entity.Validation;

namespace SuperKotob.Admin.Data.Repositories
{
    public class BaseRepository<TModel> : IRepository<TModel> where TModel : class, IDataModel
    {
        TollabContext __DataContext;
        protected TollabContext DataContext
        {
            get
            {
                if (__DataContext == null)
                {
                    __DataContext = new TollabContext();

                    __DataContext.Configuration.LazyLoadingEnabled = false;
                }
                return __DataContext;
            }
        }

        public virtual async Task<DataResponse<TModel>> GetAsync(long id)
        {
            if (id < 1)
                return new DataResponse<TModel>((TModel)null, "Id should be more than zero");

            var requestInputs = new RequestInputs(new Dictionary<string, string>()
            {
                ["id"] = id.ToString()
            });

            return await GetAsync(requestInputs);
        }

        public virtual async Task<DataResponse<TModel>> GetAsync(RequestInputs requestInputs = null)
        {
            long page = requestInputs.GetValue("page", 1);

            var response = new DataResponse<TModel>();
            var dbSet = DataContext.Set<TModel>();
            var query = SetInculdes(dbSet, requestInputs);
            query = SetWhere(query, requestInputs);
            var orderedQuery = SetOrdering(query, requestInputs);

            var count = query.Count();
            response.Paging = new PagingMeta(page, count, 25);

            response.Data = PagedQuery(query, response.Paging);

            return response;
        }

        protected virtual IQueryable<TModel> SetWhere(IQueryable<TModel> query, RequestInputs requestInputs)
        {
            var id = requestInputs.GetValue("id", "");
            if (!string.IsNullOrWhiteSpace(id))
            {
                requestInputs["ids"] = requestInputs["id"];
            }

            if (requestInputs.ContainsKey("ids"))
            {
                var strIds = requestInputs.GetValue<string>("ids");

                var ids = strIds?
                    .Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(item =>
                    {
                        long x;
                        long.TryParse(item, out x);
                        return x;
                    })
                    .Where(item => item > 0);

                if (ids != null && ids.Count() > 0)
                {
                    query = query.Where(item => ids.Contains(item.Id));
                }
            }
            return query;
        }

        protected virtual IQueryable<TModel> SetInculdes(DbSet<TModel> dbSet, RequestInputs requestInputs)
        {
            return dbSet.AsNoTracking().AsQueryable();
        }
        protected virtual IOrderedQueryable<TModel> SetOrdering(IQueryable<TModel> query, RequestInputs requestInputs)
        {
            var orderedQuery = query.OrderByDescending(item => item.Id);
            return orderedQuery;
        }
        protected PagingMeta GetPagingMeta(IQueryable<TModel> query, RequestInputs requestInputs)
        {
            int? page = requestInputs.GetValue("page", default(int?));

            page = page.HasValue ? page : 1;
            var pageSize = 20;
            var totalCount = query.Count();

            return new PagingMeta(page.Value, totalCount, pageSize);
        }

        public virtual async Task<DataResponse<TModel>> CreateAsync(TModel model)
        {
            try
            {

              //  model.CreatedOn = DateTime.Now;
                DataContext.Set<TModel>().Add(model);
                DataContext.SaveChanges();

                return new DataResponse<TModel>(model);
            }
            catch (Exception ex)
            {

                return new DataResponse<TModel>();
            }
        }



        public virtual async Task<DataResponse<TModel>> UpdateAsync(TModel model)
        {
            try
            {
              //  var CreatedDate =  __DataContext.Database.SqlQuery<DateTime>("select CreatedOn from "+model+" where Id = "+model.Id+" ").FirstOrDefault();
                //model.CreatedOn = CreatedDate;
                  // model.ModifiedOn = DateTime.Now;
              //var state = DataContext.Entry(model).State;
              //DataContext.Entry(model).State = EntityState.Detached;

                DataContext.Set<TModel>().AddOrUpdate(model);
                //DataContext.Entry(model).State = EntityState.Modified;
                DataContext.SaveChanges();
                return new DataResponse<TModel>()
                {
                    Data = new List<TModel>() { model }
                };

            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"",
                            ve.PropertyName,
                            eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName),
                            ve.ErrorMessage);
                    }
                }
                throw;
            }
        }
        public virtual async Task<DataResponse<TModel>> DeleteAsync(long id)
        {
            TModel item = null;
            try
            {
                var set = DataContext.Set<TModel>();
                item = set.FirstOrDefault(x => x.Id == id);
                set.Remove(item);

                DataContext.SaveChanges();
            }
            catch (Exception ex)
            {
                var message = ex.GetBaseException().Message;
                if (message.Contains("conflicted with"))
                {
                    var txt = "\", table \"";
                    var txt2 = "\", column";
                    var start = message.IndexOf(txt) + txt.Length;
                    var end = message.IndexOf(txt2);
                    var count = end - start;
                    var table = message.Substring(start, count);
                    table = table.Replace("dbo.", "");
                    return new DataResponse<TModel>()
                    {
                        Data = new List<TModel>
                        {
                            item
                        },
                        Errors = new List<string>
                         {
                             $"Can not delete, one or more {table} are still connected with"
                         }
                    };

                }
                throw;
            }
            return new DataResponse<TModel>()
            {

            };
        }
        public virtual async Task<DataResponse<TModel>> DeleteAsync(IList<long> ids)
        {
            var set = DataContext.Set<TModel>();
            var item = set.FirstOrDefault(x => ids.Contains(x.Id));
            set.Remove(item);

            DataContext.SaveChanges();
            return new DataResponse<TModel>()
            {

            };
        }






        protected virtual IList<TModel> PagedQuery(IQueryable<TModel> query, PagingMeta paging)
        {
            var d = query
              .OrderByDescending(item => item.Id)
              .Skip(paging.Skip)
              .Take(paging.PageSize);
              

            var list =d.ToList();

            return list;

           
        }
        protected virtual IList<TModel> PagedQuery(IOrderedQueryable<TModel> query, PagingMeta paging)
        {
            var list = query
                .Skip(paging.Skip)
                .Take(paging.PageSize)
                .ToList();

            return list;
        }

        public virtual async Task<DataResponse<TModel>> GetWithLookupsAsync(long? id = null)
        {
            if (id == null)
                return await GetAsync();

            return await GetAsync(id.Value);
        }

        public virtual async Task<DataResponse<LookupItem>> AutoCompleteAsync(string text, bool isIdList, RequestInputs requestInputs, string field)
        {
            if (field != null)
            {
               
                String[] parts = field.Split('.');
                
                if (parts.Count()==2)
                {
                    String part1 = parts[0];
                    String part2 = parts[1];
                    field = "[" + part1 + "]." + part2;
                }
            }
            string where = "";
            Type type = typeof(TModel);
            if (text != null)
            {
                if (isIdList)
                {
                    if(text.Contains(","))
                        where = " where Id in " + text + "";
                    else
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


                var query = "SELECT Id, ISNULL(CAST(" + field + " AS NVARCHAR(250)),'') AS Title FROM [" + type.Name + "] " + where + " Order By " + field;
                var lookuplist = DataContext.Database.SqlQuery<LookupItem>(query).ToList();
                
                return new DataResponse<LookupItem>(lookuplist);
            }
            catch (Exception ex)
            {

            }

            return null;
        }



        //
        public virtual async Task<DataResponse<LookupItem>> AutoCompleteAsync(string text, bool isIdList, RequestInputs requestInputs, string field, string Wherefield)
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
            Type type = typeof(TModel);
            if (isIdList)
            {
                where = " where " + Wherefield + text + "";
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

            try
            {


                var query = "SELECT Id,CAST(" + field + " AS NVARCHAR(250)) AS Title FROM [" + type.Name + "] " + where + " Order By " + field;
                var lookuplist = DataContext.Database.SqlQuery<LookupItem>(query).ToList();

                return new DataResponse<LookupItem>(lookuplist);
            }
            catch (Exception ex)
            {

            }

            return null;
        }



        //public virtual async Task<DataResponse<LookupItem>> AutoCompleteAreaMangersOnly(string text, bool isIdList, RequestInputs requestInputs)
        //{
        //    var list = DataContext.Set<TModel>().AsQueryable();
        //    if (isIdList)
        //    {
        //        var ids = text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)
        //            .Select(long.Parse)
        //            .ToList();
        //        list = list.Where(item => ids.Contains(item.Id));
        //    }
        //    else
        //    {
        //        if (!string.IsNullOrWhiteSpace(text))
        //        {
        //            var parts = text.Split(',').ToList();
        //            bool allIds = true;
        //            long tmp;
        //            foreach (var p in parts)
        //                if (!long.TryParse(p, out tmp))
        //                    allIds = false;

        //            if (allIds)
        //            {
        //                var ids = parts.Select(long.Parse).ToList();
        //                list = list.Where(item => ids.Contains(item.Id));
        //            }
        //            else
        //            {
        //                list = list.Where(item => item.NameArabic.Contains(text));
        //            }
        //        }
        //    }
        //    var orderedList = list.OrderBy(item => item.NameArabic)
        //    .Skip(0)
        //    .Take(15)
        //    .Select(item => new LookupItem()
        //    {
        //        Id = item.Id,
        //        Text = item.NameArabic,


        //    })
        //    .ToList();

        //    orderedList = orderedList.Select(item =>
        //    {
        //        item.Text += $" ({item.Id})";
        //        return item;
        //    }).ToList();

        //    return new DataResponse<LookupItem>(orderedList);
        //}

    }
}
