using System.Collections.Generic;

namespace AdventureWorks.Service.Dtos.Collections
{
    public class PersonDto : PersonNameDto
    {
        public IEnumerable<PhoneDto> PhoneNumbers { get; set; }
        public IEnumerable<CreditCardDto> CreditCards { get; set; }
        public IEnumerable<EmailDto> Emails { get; set; }
    }
}