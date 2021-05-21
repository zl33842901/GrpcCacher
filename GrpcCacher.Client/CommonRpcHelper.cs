using Grpc.Core;
using Grpc.Net.Client;
using GrpcCacher.Interfaces;
using System;
using System.Threading.Tasks;

namespace GrpcCacher.Client
{
    public class CommonRpcHelper
    {
        private readonly IGrpcSettings grpcSettings;
        public CommonRpcHelper(IGrpcSettings grpcSettings)
        {
            this.grpcSettings = grpcSettings;
        }

        public async Task<string> CallApi(string apiKey, string data)
        {
            GrpcChannel channel = GrpcChannel.ForAddress(grpcSettings.Host);
            Interfaces.CommonRpc.CommonRpcClient client = new Interfaces.CommonRpc.CommonRpcClient(channel);
            
            var result = await client.CallApiAsync(new RequestMessage() {  }, new CallOptions(null, DateTime.Now.ToUniversalTime().AddSeconds(grpcSettings.TimeoutBySeconds)));
            //return result.ApiResult;

            return string.Empty;
        }
    }
}
