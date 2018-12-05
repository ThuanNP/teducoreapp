using TeduCoreApp.infrastructure.Enums;

namespace TeduCoreApp.infrastructure.Interfaces
{
    public interface ISwitchable
    {
        Status Status { get; set; }
    }
}