namespace Blogger.Data.Dto
{
    public interface IRequestDto<T>
    {
        bool IsValid();
        T GetPersistentObject();
    }
}