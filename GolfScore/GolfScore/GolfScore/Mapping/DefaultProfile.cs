namespace GolfScore.Mapping
{
    public class DefaultProfile: Profile
    {
        public DefaultProfile()
        {
            CreateMap<Rally, LocalData.Rally>();
            CreateMap<LocalData.Rally, Rally>();

            CreateMap<NewsListItem, LocalData.NewsItem>();
            CreateMap<NewsItem, LocalData.NewsItem>();
            CreateMap<LocalData.NewsItem, NewsItem>();
            CreateMap<LocalData.NewsItem, NewsListItem>();
            CreateMap<NewsListItem, NewsItem>();
            CreateMap<Gallery, LocalData.Gallery>();
            CreateMap<LocalData.Gallery, Gallery>();

        }


    }
}
