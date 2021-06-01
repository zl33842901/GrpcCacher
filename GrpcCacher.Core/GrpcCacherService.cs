using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using xLiAd.DapperEx.Repository;
using System.Linq;

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

        private IGrpcCacherConfigItem GetConfigItem(string typeName)
        {
            return config.GetConfigItem(typeName);
        }

        public GrpcCacherItemService<T> GetItemService<T>()
        {
            return new GrpcCacherItemService<T>(GetConfigItem<T>(), connection);
        }

        public IGrpcCacherItemService GetItemService(string typeName)
        {
            var item = GetConfigItem(typeName);
            var type = typeof(GrpcCacherItemService<>);
            type = type.MakeGenericType(item.Type);
            var constructors = type.GetConstructors();
            var constructor = constructors.Where(x => x.GetParameters().Last().ParameterType == typeof(int)).First();
            var result = constructor.Invoke(new object[] { item, connection, 1 }) as IGrpcCacherItemService;
            return result;
        }
    }

    public interface IGrpcCacherService
    {
        GrpcCacherItemService<T> GetItemService<T>();
        IGrpcCacherItemService GetItemService(string typeName);
    }
}
