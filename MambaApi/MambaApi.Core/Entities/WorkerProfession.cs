using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MambaApi.Core.Entities
{
    public class WorkerProfession : BaseEntity
    {
        public int WorkerId { get; set; }
        public int ProfessionId { get; set; }
        public Worker Worker { get; set; }
        public Profession Profession { get; set; }
    }
}
