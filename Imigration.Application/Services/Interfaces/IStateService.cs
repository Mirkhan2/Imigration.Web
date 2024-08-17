using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imigration.Domains.ViewModels.Common;

namespace Imigration.Application.Services.Interfaces
{
    public interface IStateService
    {
        //list keshvar be safhe <select id v desc<
        Task<List<SelectListViewModel>> GetAllStates(long? stateId = null);  
    }
}
