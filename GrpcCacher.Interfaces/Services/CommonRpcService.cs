using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcCacher.Interfaces
{
    public class CommonRpcService : CommonRpc.CommonRpcBase
    {
        private readonly ILogger<CommonRpcService> _logger;
        public CommonRpcService(ILogger<CommonRpcService> logger)
        {
            _logger = logger;
        }

        public override Task<ResponseMessage> CallApi(RequestMessage request, ServerCallContext context)
        {
            return Task.FromResult(new ResponseMessage() { ApiResult="abc" });
        }
    }
}
