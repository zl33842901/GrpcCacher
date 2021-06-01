﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using xLiAd.DapperEx.Repository;

namespace GrpcCacher.Core
{
    public class GrpcCacherItemService<T> : IGrpcCacherItemService
    {
        private static ConcurrentBag<T> DataList;
        private static DateTime lastLoadTime = DateTime.MinValue;

        private readonly IRepository<T> repository;
        private readonly GrpcCacherConfigItemBase<T> configItem;
        private readonly IDbConnection conn;
        /// <summary>
        /// 允许的数据缓存时间
        /// </summary>
        private readonly TimeSpan dataMaxCacheTime;
        public GrpcCacherItemService(GrpcCacherConfigItemBase<T> configItem, IDbConnection conn, TimeSpan dataMaxCacheTime)
        {
            this.configItem = configItem;
            this.conn = conn;
            this.repository = configItem.GetRepository(conn);
            this.dataMaxCacheTime = dataMaxCacheTime;
        }
        public GrpcCacherItemService(GrpcCacherConfigItemBase<T> configItem, IDbConnection conn, int dataMaxCacheTimeByMinutes = 1)
            :this(configItem, conn, TimeSpan.FromMinutes(dataMaxCacheTimeByMinutes)) { }

        private Expression<Func<T, bool>> ToExpressionGreaterThanMe<TKey>(TKey i, Expression<Func<T, TKey>> lastUpdateField)
        {
            var member = lastUpdateField.Body as MemberExpression;
            if (member == null)
                throw new Exception("lastUpdateField 不是标准的属性表达式：" + lastUpdateField.ToString());
            ParameterExpression para = Expression.Parameter(typeof(T), lastUpdateField.Parameters.Single().Name);
            Expression condition1 = Expression.GreaterThan(member, Expression.Constant(i, typeof(TKey)));
            Expression<Func<T, bool>> lamb = Expression.Lambda<Func<T, bool>>(condition1, para);
            return lamb;
        }

        private Expression<Func<T, bool>> ToExpressionContains<TKey>(List<TKey> Li, Expression<Func<T, TKey>> keyField)
        {
            var member = keyField.Body as MemberExpression;
            if (member == null)
                throw new Exception("keyField 不是标准的属性表达式：" + keyField.ToString());
            ParameterExpression para = Expression.Parameter(typeof(T), keyField.Parameters.Single().Name);
            MethodCallExpression met = Expression.Call(Expression.Constant(Li), typeof(List<TKey>).GetMethod("Contains"), member);
            Expression<Func<T, bool>> lamb = Expression.Lambda<Func<T, bool>>(met, para);
            return lamb;
        }

        private async Task LoadInfo()
        {
            if (DataList == null)
            {
                DataList = new ConcurrentBag<T>(await repository.AllAsync());
                lastLoadTime = DateTime.Now;
            }
            else
            {
                if(lastLoadTime.Add(dataMaxCacheTime) < DateTime.Now)
                {
                    Expression<Func<T, bool>> expression = ToExpressionGreaterThanMe(lastLoadTime, configItem.LastUpdateTimeField);
                    var list = await repository.WhereAsync(expression);
                    var listId = list.Select(configItem.KeyField.Compile()).ToList();
                    var l = DataList.ToList();
                    var result = l.Where(ToExpressionContains(listId, configItem.KeyField).Compile()).Concat(list);
                    DataList = new ConcurrentBag<T>(result);
                }
            }
        }

        public async Task<List<T>> Where(Expression<Func<T, bool>> expression)
        {
            await LoadInfo();
            var result = DataList.Where(expression.Compile()).ToList();
            return result;
        }

        public async Task<List<T>> WhereOrder<TKey>(Expression<Func<T, bool>> expression, Expression<Func<T, TKey>> order, bool desc = false, int top = 0)
        {
            await LoadInfo();
            var list = DataList.Where(expression.Compile());
            if (desc)
                list = list.OrderByDescending(order.Compile());
            else
                list = list.OrderBy(order.Compile());
            if (top > 0)
                list = list.Take(top);
            return list.ToList();
        }

        public async Task<(List<T>, int)> PageList<TKey>(Expression<Func<T, bool>> expression, Expression<Func<T, TKey>> order, bool desc, int pageIndex, int pageSize)
        {
            await LoadInfo();
            var list = DataList.Where(expression.Compile());
            var total = list.Count();
            if (desc)
                list = list.OrderByDescending(order.Compile());
            else
                list = list.OrderBy(order.Compile());
            list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return (list.ToList(), total);
        }

        public async Task<int> Count(Expression<Func<T, bool>> expression)
        {
            await LoadInfo();
            var result = DataList.Count(expression.Compile());
            return result;
        }
    }

    public interface IGrpcCacherItemService { }
}
