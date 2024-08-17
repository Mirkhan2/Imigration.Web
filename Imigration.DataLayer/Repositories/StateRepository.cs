using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imigration.DataLayer.Context;
using Imigration.Domains.Entities.Location;
using Imigration.Domains.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Imigration.DataLayer.Repositories
{
    public class StateRepository : IStateRepository
    {
        #region Ctor
        private readonly ImigrationDbContext _context;
        public StateRepository(ImigrationDbContext context)
        {
                _context = context;
        }


        #endregion
        public async Task<List<State>> GetAllState(long? stateId = null)
        {
            var states = _context.States.Where(s => !s.IsDelete).AsQueryable();
            if (stateId.HasValue)
            {
                states = states.Where(s => s.ParentId.HasValue && s.ParentId == stateId.Value);
            }
            else
            {
                states = states.Where(s => s.ParentId == null);
            }
            return await states.ToListAsync();
        }
    }
}
