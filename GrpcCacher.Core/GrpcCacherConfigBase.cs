using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using xLiAd.DapperEx.Repository;

namespace GrpcCacher.Core
{
    public abstract class GrpcCacherConfigBase
    {
        private Dictionary<Type, IGrpcCacherConfigItem> configItems = new Dictionary<Type, IGrpcCacherConfigItem>();
        public GrpcCacherConfigBase AddDatasource<T>(Expression<Func<T, DateTime>> lastUpdateTimeField, Expression<Func<T, int>> keyField) where T:class
        {
            if (configItems.ContainsKey(typeof(T)))
                throw new Exception($"类型{typeof(T).Name}已存在！");
            configItems.Add(typeof(T), BornItem(lastUpdateTimeField, keyField));
            return this;
        }

        public GrpcCacherConfigItemBase<T> GetConfigItem<T>()
        {
            if (!configItems.ContainsKey(typeof(T)))
                throw new Exception($"类型{typeof(T).Name}不存在！");
            var item = configItems[typeof(T)] as GrpcCacherConfigItemBase<T>;
            return item;
        }

        public IGrpcCacherConfigItem GetConfigItem(string typeName)
        {
            var items = configItems.Where(x => x.Key.Name.Equals(typeName));
            if(!items.Any())
                throw new Exception($"类型{typeName}不存在！");
            var item = items.First();
            return item.Value;
        }

        protected abstract GrpcCacherConfigItemBase<T> BornItem<T>(Expression<Func<T, DateTime>> lastUpdateTimeField, Expression<Func<T, int>> keyField);
    }

    public abstract class GrpcCacherConfigItemBase<T> : IGrpcCacherConfigItem
    {
        public GrpcCacherConfigItemBase(Expression<Func<T, DateTime>> lastUpdateTimeField, Expression<Func<T, int>> keyField)
        {
            LastUpdateTimeField = lastUpdateTimeField;
            KeyField = keyField;
        }

        public Expression<Func<T, DateTime>> LastUpdateTimeField { get; }

        public Expression<Func<T, int>> KeyField { get; }

        public Type Type => typeof(T);

        public abstract IRepository<T> GetRepository(IDbConnection connection);
    }

    public interface IGrpcCacherConfigItem
    {
        Type Type { get; }
    }
}
