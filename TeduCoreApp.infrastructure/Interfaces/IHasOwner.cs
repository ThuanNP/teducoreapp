namespace TeduCoreApp.infrastructure.Interfaces
{
    public interface IHasOwner<T>
    {
        T OwnerId { get; set; }
    }
}