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

        private IRepository<T> GetRepository<T>()
        {
            return config.FuncRepository<T>()(connection);
        }


    }

    public interface IGrpcCacherService
    {

    }
}
