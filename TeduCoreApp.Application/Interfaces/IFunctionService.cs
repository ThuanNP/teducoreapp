using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeduCoreApp.Application.ViewModels.System;

namespace TeduCoreApp.Application.Interfaces
{
    public interface IFunctionService: IDisposable
    {      

        FunctionViewModel GetById(string id);

        Task<List<FunctionViewModel>> GetAll(string filter);

        IEnumerable<FunctionViewModel> GetAllWithParentId(string parentId);       

        void Add(FunctionViewModel functionViewModel);

        void Update(FunctionViewModel functionViewModel);

        void Delete(string id);

        bool CheckExistedId(string id);

        void UpdateParentId(string sourceId, string targetId, Dictionary<string, int> items);

        void ReOrder(string sourceId, string targetId);

        void Save();

    }
}
