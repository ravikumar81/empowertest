using AutoMapper;
using Empower.Api.Mapping;

namespace Empower.Api.Configuration
{
    public class AutoMapperConfiguration {
        public static void Configure() {
            Mapper.Initialize(cfg => {
                cfg.AddProfile<MappingProfile>();
            });
            
        }

        public static void Reset() {
            Mapper.Reset();
            
        }
    }
}