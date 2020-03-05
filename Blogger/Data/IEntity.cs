namespace Blogger.Data
{
    public interface IEntity<T>
    {
        T Id {set; get;}
    }
}