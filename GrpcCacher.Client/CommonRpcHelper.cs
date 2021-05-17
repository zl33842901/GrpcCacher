using Grpc.Net.Client;
using System;

namespace GrpcCacher.Client
{
    public class CommonRpcHelper
    {
        private readonly IGrpcSettings grpcSettings;
        public CommonRpcHelper(IGrpcSettings grpcSettings)
        {
            this.grpcSettings = grpcSettings;
        }

        public string CallApi(string apiKey, string data)
        {
            GrpcChannel channel = GrpcChannel.ForAddress(grpcSettings.Host);
            var service = channel.CreateGrpcService()
            service.
            //var grpcClient = new CommonRpc.CommonRpcClient(channel);
            //var result = grpcClient.CallApi(new RequestMessage() { ApiUrl = apiKey, ApiParam = data }, new CallOptions(null, DateTime.Now.ToUniversalTime().AddSeconds(grpcSettings.TimeoutBySeconds)));
            //return result.ApiResult;

            return string.Empty;
        }
    }
}
