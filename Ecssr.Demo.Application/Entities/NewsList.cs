namespace Ecssr.Demo.Application.Entities
{
    public class NewsList
    {
        public IList<News> News { get; set; }
        public Pagination Pagination{get; set; }
    }
}
