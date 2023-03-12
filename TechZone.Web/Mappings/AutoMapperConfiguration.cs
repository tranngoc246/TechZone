using AutoMapper;
using System.Security.Cryptography;
using TechZone.Model.Models;
using TechZone.Web.Models;

namespace TechZone.Web.Mappings
{
    public class AutoMapperConfiguration
    {
        public static IMapper mapper;
        public static void Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Post, PostViewModel>();
                cfg.CreateMap<PostCategory, PostCategoryViewModel>();
                cfg.CreateMap<Tag, TagViewModel>();
            });
            mapper = config.CreateMapper();
        }

    }
}