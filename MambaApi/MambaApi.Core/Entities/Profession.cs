using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MambaApi.Core.Entities
{
    public class Profession : BaseEntity
    {
        public string Name { get; set; }
        public List<WorkerProfession> WorkerProfessions { get; set; }
    }
}
