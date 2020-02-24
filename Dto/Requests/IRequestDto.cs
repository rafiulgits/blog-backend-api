namespace Blogger.Dto
{
    public interface IRequestDto<T>
    {
        bool IsValid();
        T GetObject();
    }
}