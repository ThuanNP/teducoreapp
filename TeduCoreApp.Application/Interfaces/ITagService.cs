using System;
using System.Collections.Generic;
using System.Text;
using TeduCoreApp.Application.ViewModels.Common;

namespace TeduCoreApp.Application.Interfaces
{
    public interface ITagService: IDisposable
    {
        List<TagViewModel> GetAll();

        TagViewModel GetById(string id);
    }
}
