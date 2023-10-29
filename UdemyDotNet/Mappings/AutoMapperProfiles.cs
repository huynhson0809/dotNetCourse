using AutoMapper;
using UdemyDotNet.Models.Domain;
using UdemyDotNet.Models.DTO;

namespace UdemyDotNet.Mappings
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            // ta đang làm ví dụ, là nếu 2 cái ta cần map có các thuộc tính giống nhau thì chỉ cần 
            CreateMap<UserDto, UserDomain>().ReverseMap(); //reverse để chúng ta có thể thực hiện map 2 chiều 
            // nếu nó có các thuộc tính không giống nhau thì ta phải làm thế này
            CreateMap<UserDto, UserDomain>()
                .ForMember(x => x.Name, option => option.MapFrom(x => x.FullName))
                .ReverseMap();
            
            CreateMap<Region, RegionDto>().ReverseMap();
            CreateMap<AddRegionRequestDto, Region>().ReverseMap();
            CreateMap<UpdateRegionRequestDto, Region>().ReverseMap();
        }
    }

    public class UserDto
    {
        public string FullName { get; set; }
    }

    public class UserDomain
    {
        public string Name { get; set; }
    }
}
