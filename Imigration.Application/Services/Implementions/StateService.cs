using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imigration.Application.Services.Interfaces;
using Imigration.Domains.Interfaces;
using Imigration.Domains.ViewModels.Common;

namespace Imigration.Application.Services.Implementions
{
    public class StateService : IStateService
    {
        #region Ctor
        private readonly IStateRepository _stateRepository;
        public StateService(IStateRepository stateRepository)
        {
            _stateRepository = stateRepository;
        }


        #endregion

        public async Task<List<SelectListViewModel>> GetAllStates(long? stateId = null)
        {
         var states =    await _stateRepository.GetAllState(stateId);
       
            return states.Select(s => new SelectListViewModel
            {
                Id = s.Id,
                Title = s.Title,
                

            }).ToList();
        }

    }
}
