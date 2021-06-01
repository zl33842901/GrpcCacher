using Grpc.Core;
using GrpcCacher.Interfaces.Services;
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
        private readonly ICommandProcessService commandProcessService;
        public CommonRpcService(ILogger<CommonRpcService> logger, ICommandProcessService commandProcessService)
        {
            _logger = logger;
            this.commandProcessService = commandProcessService;
        }

        public override async Task<ResponseMessage> CallApi(RequestMessage request, ServerCallContext context)
        {
            try
            {
                return await commandProcessService.Process(request);
            }
            catch(Exception ex)
            {
                return $"³ö´íÁË£º{ex.Message}".ToResponseError();
            }
        }
    }
}
