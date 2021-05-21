using System;
using System.Collections.Generic;
using System.Text;
using xLiAd.DapperEx.Repository;

namespace GrpcCacher.Core
{
    public class GrpcCacherItemService<T>
    {
        private readonly IRepository<T> repository;
        private readonly GrpcCacherConfigItemBase<T> configItem;
        public GrpcCacherItemService(GrpcCacherConfigItemBase<T> configItem)
        {
            this.configItem = configItem;
            this.repository = configItem.GetRepository();
        }
    }
}
