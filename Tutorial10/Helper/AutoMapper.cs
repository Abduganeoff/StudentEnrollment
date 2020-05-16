using AutoMapper;
using Tutorial10.DTOs.Response;
using Tutorial10.Entities;

namespace Tutorial10.Helper
{
    public class AutoMapper : Profile
    {

        public AutoMapper()
        {
            CreateMap<Student, StudentsToListResponse>()
                .ForMember(dest => dest.Age, opt => opt
                    .MapFrom(scr => scr.BirthDate.CaclculateAge()));

            CreateMap<Enrollment, EnrollmentToStudentResponse>()
                .ForMember(dest => dest.Study, opt => opt.MapFrom(src => src.IdStudyNavigation.Name));
        }
    }
}
