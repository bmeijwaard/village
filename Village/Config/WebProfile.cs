using AutoMapper;
using Village.Data.Domain;
using Village.Models.Example;

namespace Village.Config
{
    public class WebProfile : Profile
    {
        public WebProfile()
        {
            CreateMap<ExampleViewModel, Example>();
            CreateMap<Example, ExampleViewModel>();
        }
    }
}
