using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TechZone.Model.Models;
using TechZone.Web.Models;

namespace TechZone.Web.Mappings
{
    public interface IMappingService
    {
        IMapper Mapper { get; }
    }

    public class MappingService : IMappingService
    {
        public IMapper Mapper { get; }

        public MappingService()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PostCategory, PostCategoryViewModel>();
            });

            Mapper = config.CreateMapper();
        }
    }
}