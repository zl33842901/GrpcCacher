using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcCacher.Interfaces
{
    public static class MessageHelper
    {
        public static ResponseMessage ToResponseError(this string message)
        {
            return new ResponseMessage()
            {
                Data = "[]",
                Message = message,
                Success = false,
                Total = 0
            };
        }
    }
}
