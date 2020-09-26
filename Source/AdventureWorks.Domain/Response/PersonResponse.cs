using System.Collections.Generic;

namespace AdventureWorks.Domain.Response
{
    public class PersonResponse : PersonNameResponse
    {
        public IEnumerable<PhoneResponse> PhoneNumbers { get; set; }
        public IEnumerable<CreditCardResponse> CreditCards { get; set; }
        public IEnumerable<EmailResponse> Emails { get; set; }
    }
}