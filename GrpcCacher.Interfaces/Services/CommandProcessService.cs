using GrpcCacher.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcCacher.Interfaces.Services
{
    public class CommandProcessService : ICommandProcessService
    {
        private readonly IGrpcCacherService grpcCacherService;
        public CommandProcessService(IGrpcCacherService grpcCacherService)
        {
            this.grpcCacherService = grpcCacherService;
        }

        public async Task<ResponseMessage> Process(RequestMessage request)
        {
            var service = grpcCacherService.GetItemService(request.TableStandardName);



            return new ResponseMessage() { Data = "", Message = "", Success = true, Total = 0 };
        }
    }

    public interface ICommandProcessService
    {
        Task<ResponseMessage> Process(RequestMessage request);
    }
}
