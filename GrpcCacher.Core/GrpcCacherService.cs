using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using xLiAd.DapperEx.Repository;

namespace GrpcCacher.Core
{
    public class GrpcCacherService : IGrpcCacherService
    {
        private readonly GrpcCacherConfigBase config;
        private readonly IDbConnection connection;
        public GrpcCacherService(GrpcCacherConfigBase config, IDbConnection connection)
        {
            this.config = config;
            this.connection = connection;
        }

        private GrpcCacherConfigItemBase<T> GetConfigItem<T>()
        {
            return config.GetConfigItem<T>();
        }

        public GrpcCacherItemService<T> GetItemService<T>()
        {
            return new GrpcCacherItemService<T>(GetConfigItem<T>(), connection);
        }
    }

    public interface IGrpcCacherService
    {
        GrpcCacherItemService<T> GetItemService<T>();
    }
}
