using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using xLiAd.DapperEx.Repository;

namespace GrpcCacher.Core
{
    public abstract class GrpcCacherConfigBase
    {
        private Dictionary<Type, IGrpcCacherConfigItem> configItems = new Dictionary<Type, IGrpcCacherConfigItem>();
        public GrpcCacherConfigBase AddDatasource<T>(Expression<Func<T, DateTime>> lastUpdateTimeField) where T:class
        {
            if (configItems.ContainsKey(typeof(T)))
                throw new Exception($"类型{typeof(T).Name}已存在！");
            configItems.Add(typeof(T), BornItem(lastUpdateTimeField));
            return this;
        }

        public Func<IDbConnection, IRepository<T>> FuncRepository<T>()
        {
            return x => GetConfigItem<T>().GetRepository(x);
        }

        public GrpcCacherConfigItemBase<T> GetConfigItem<T>()
        {
            if (!configItems.ContainsKey(typeof(T)))
                throw new Exception($"类型{typeof(T).Name}不存在！");
            var item = configItems[typeof(T)] as GrpcCacherConfigItemBase<T>;
            return item;
        }

        protected abstract GrpcCacherConfigItemBase<T> BornItem<T>(Expression<Func<T, DateTime>> lastUpdateTimeField);
    }

    public abstract class GrpcCacherConfigItemBase<T> : IGrpcCacherConfigItem
    {
        public GrpcCacherConfigItemBase(Expression<Func<T, DateTime>> lastUpdateTimeField)
        {
            LastUpdateTimeField = lastUpdateTimeField;
        }

        public Expression<Func<T, DateTime>> LastUpdateTimeField { get; }

        public Type Type => typeof(T);

        public abstract IRepository<T> GetRepository(IDbConnection connection);


        internal List<T> ListValues = new List<T>();

        internal DateTime LastUpdateTime = DateTime.MinValue;
    }

    public interface IGrpcCacherConfigItem
    {
        Type Type { get; }
    }
}
