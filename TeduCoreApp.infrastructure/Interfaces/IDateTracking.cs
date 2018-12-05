using System;

namespace TeduCoreApp.infrastructure.Interfaces
{
    public interface IDateTracking
    {
        DateTime DateCreated { get; set; }
        DateTime? DateModified { get; set; }
    }
}