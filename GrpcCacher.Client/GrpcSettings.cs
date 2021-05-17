using System;
using System.Collections.Generic;
using System.Text;

namespace GrpcCacher.Client
{
    public class GrpcSettings : IGrpcSettings
    {
        /// <summary>
        /// 主机和端口 如： 127.0.0.1:8000
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// 超时时间（秒）
        /// </summary>
        public int TimeoutBySeconds { get; set; }
    }

    public interface IGrpcSettings
    {
        string Host { get; }
        int TimeoutBySeconds { get; }
    }
}
