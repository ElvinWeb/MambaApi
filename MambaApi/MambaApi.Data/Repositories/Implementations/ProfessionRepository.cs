using MambaApi.Core.Entities;
using MambaApi.Core.Repositories;
using MambaApi.Data.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MambaApi.Data.Repositories.Implementations
{
    public class ProfessionRepository : GenericRepository<Profession>, IProfessionRepository
    {
        public ProfessionRepository(AppDbContext context) : base(context)
        {

        }
    }
}
