using AdventureWorks.Client.Models;
using AdventureWorks.Service.Dtos.Collections;
using AutoMapper;

namespace AdventureWorks.Service.Automapper
{
    public class AdventureWorksProfile : Profile
    {
        public AdventureWorksProfile()
        {
            AllowNullCollections = true;

            this.CreateMap<Person, PersonNameDto>()
                .ForMember(d => d.FirstName,
                    opt => opt.MapFrom(src => src.FirstName))
                .ForMember(d => d.LastName,
                    opt => opt.MapFrom(src => src.LastName))
                .ForMember(d => d.MiddleName,
                    opt => opt.MapFrom(src => src.MiddleName))
                .ForMember(d => d.LastUpdated,
                    opt => opt.MapFrom(src => src.ModifiedDate))
                .ForAllOtherMembers(src => src.Ignore());

            this.CreateMap<PersonNameDto, Person>()
                .ForMember(d => d.FirstName,
                    opt => opt.MapFrom(src => src.FirstName))
                .ForMember(d => d.LastName,
                    opt => opt.MapFrom(src => src.LastName))
                .ForMember(d => d.MiddleName,
                    opt => opt.MapFrom(src => src.MiddleName))
                .ForMember(d => d.ModifiedDate,
                    opt => opt.MapFrom(src => src.LastUpdated))
                .ForAllOtherMembers(src => src.Ignore());



            this.CreateMap<PersonPhone, PhoneDto>()
                .ForMember(d => d.PhoneNumber,
                    opt => opt.MapFrom(src => src.PhoneNumber))
                .ForAllOtherMembers(src => src.Ignore());

            this.CreateMap<PersonCreditCard, CreditCardDto>()
                .ForMember(d => d.CardNumber,
                    opt => opt.MapFrom(src => src.CreditCard.CardNumber))
                .ForAllOtherMembers(src => src.Ignore());

            this.CreateMap<EmailAddress, EmailDto>()
                .ForMember(d => d.Email,
                    opt => opt.MapFrom(src => src.Email))
                .ForAllOtherMembers(src => src.Ignore());

            
            this.CreateMap<Person, PersonDto>()
                .ForMember(d => d.FirstName,
                    opt => opt.MapFrom(src => src.FirstName))
                .ForMember(d => d.LastName,
                    opt => opt.MapFrom(src => src.LastName))
                .ForMember(d => d.MiddleName,
                    opt => opt.MapFrom(src => src.MiddleName))
                .ForMember(d => d.LastUpdated,
                    opt => opt.MapFrom(src => src.ModifiedDate))
                .ForMember(d => d.CreditCards,
                    opt => opt.MapFrom(src => src.PersonCreditCard))
                .ForMember(d => d.Emails,
                    opt => opt.MapFrom(src => src.EmailAddress))
                .ForMember(d => d.PhoneNumbers,
                    opt => opt.MapFrom(src => src.PersonPhone))
                .ForAllOtherMembers(src => src.Ignore());
        }
    }
}
