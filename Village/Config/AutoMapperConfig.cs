using AutoMapper;

namespace Village.Config
{
    public class AutoMapperConfig
    {
        public static void Register()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<WebProfile>();
            });
        }
    }
}
