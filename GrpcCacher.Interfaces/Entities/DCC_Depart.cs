using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcCacher.Interfaces.Entities
{
    public class DCC_Depart
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameChain { get; set; }

        public DateTime LastUpdateTime { get; set; }
    }
}
