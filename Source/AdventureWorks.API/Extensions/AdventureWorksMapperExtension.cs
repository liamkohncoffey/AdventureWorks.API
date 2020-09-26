using System.Linq;
using AdventureWorks.Domain.Response;
using AdventureWorks.Service.Dtos.Collections;

namespace AdventureWorks.Api.Extensions
{
    public static class AdventureWorksMapperExtension
    {
        public static PersonNameResponse Map(this PersonNameDto personName) => new PersonNameResponse
        {
            FirstName = personName.FullName,
            LastName = personName.LastName,
            LastUpdated = personName.LastUpdated,
            MiddleName = personName.MiddleName
        };
        
        public static PersonResponse Map(this PersonDto person) => new PersonResponse
        {
            FirstName = person.FullName,
            LastName = person.LastName,
            LastUpdated = person.LastUpdated,
            MiddleName = person.MiddleName,
            CreditCards = person.CreditCards.Select(c => new CreditCardResponse
            {
                CardNumber = c.CardNumber
            }),
            Emails = person.Emails.Select(c => new EmailResponse
            {
                Email = c.Email
            }),
            PhoneNumbers = person.PhoneNumbers.Select(c => new PhoneResponse
            {
                PhoneNumber = c.PhoneNumber
            })
        };
    }
}