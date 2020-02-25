namespace Blogger.Options
{
    public class DbOptions
    {
        public string Host {set; get;}
        public string Name {set; get;}
        public string Security {set; get;}

        public override string ToString()
        {
            return $"Data Source={Host};Initial Catalog={Name};Integrated Security={Security}";
        }
    }
}