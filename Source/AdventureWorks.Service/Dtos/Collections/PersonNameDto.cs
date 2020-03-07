using System;

namespace AdventureWorks.Service.Dtos.Collections
{
    public class PersonNameDto : PersonNameBaseDto
    {
        public DateTime? LastUpdated { get; set; }
        public string FullName => CreateFullName();

        private string CreateFullName()
        {
            var fullName = "";
            if (!string.IsNullOrEmpty(FirstName))
                fullName += FirstName;
            if(!string.IsNullOrEmpty(MiddleName))
                fullName += string.IsNullOrEmpty(fullName) ? MiddleName : $" {MiddleName}";
            if(!string.IsNullOrEmpty(LastName))
                fullName += string.IsNullOrEmpty(LastName) ? LastName : $" {LastName}";
            return fullName;
        }
    }
}
