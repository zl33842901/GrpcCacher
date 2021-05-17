using GrpcCacher.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using xLiAd.DapperEx.Repository;

namespace GrpcCacher.SqlServer
{
    public class GrpcCacherConfig : GrpcCacherConfigBase
    {
        protected override GrpcCacherConfigItemBase<T> BornItem<T>(Expression<Func<T, DateTime>> lastUpdateTimeField)
        {
            return new GrpcCacherConfigItem<T>(lastUpdateTimeField);
        }
    }

    public class GrpcCacherConfigItem<T> : GrpcCacherConfigItemBase<T>
    {
        public GrpcCacherConfigItem(Expression<Func<T, DateTime>> lastUpdateTimeField) : base(lastUpdateTimeField) { }

        public override IRepository<T> GetRepository(IDbConnection connection)
        {
            return new Repository<T>(connection);
        }
    }
}
