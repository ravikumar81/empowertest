using AutoMapper;
using Empower.DTO;
using Empower.Entities;

namespace Empower.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "MappingProfile"; }
        }
        public MappingProfile()
        {           
            
        }
    }
}