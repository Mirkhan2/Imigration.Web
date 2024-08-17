using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imigration.Domains.Entities.Location;

namespace Imigration.Domains.Interfaces
{
    public interface IStateRepository
    {
        Task<List<State>> GetAllState(long? stateId = null);
    }
       
}
