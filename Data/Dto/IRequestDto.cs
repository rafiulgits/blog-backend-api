namespace Blogger.Data.Dto
{
    public interface IRequestDto<T>
    {
        bool IsValid(DtoTypes.RequestType type);
        T GetPersistentObject();
    }
}