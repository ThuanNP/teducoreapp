using System;
using TeduCoreApp.Application.Interfaces;

namespace TeduCoreApp.Application.Implementations
{
    public abstract class BaseService : IBaseService
    {
        public void Dispose() => GC.SuppressFinalize(this);
    }
}
