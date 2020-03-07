using System;
using System.Linq;
using System.Threading.Tasks;
using AdventureWorks.Client;
using AdventureWorks.Service.Dtos.Collections;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AdventureWorks.Service
{
    public class AdventureWorksService : IAdventureWorksService
    {
        private readonly ILogger<AdventureWorksService> _logger;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        protected readonly AdventureWorksContext _repo;
        public AdventureWorksService(
            ILogger<AdventureWorksService> logger,
            IMapper mapper, IConfiguration configuration, AdventureWorksContext repo)
        {
            _logger = logger;
            _mapper = mapper;
            _configuration = configuration;
            _repo = repo;
        }

        #region  Private Helpers


        #endregion

        public async Task<PersonNameDto> GetPersonsNameAsync(int businessEntityId)
        {
            var person = (await _repo.Person.Where(c => c.BusinessEntityId == businessEntityId).AsNoTracking().ToListAsync())
                .FirstOrDefault();
            if (person == null)
                return null;
            return _mapper.Map<PersonNameDto>(person);
        }

        public async Task<PersonNameDto> PatchPersonsNameAsync(int businessEntityId, PersonNameBaseDto personNameDto)
        {
            var person = (await _repo.Person.Where(c => c.BusinessEntityId == businessEntityId).ToListAsync()).FirstOrDefault();
            if (person == null)
                return null;
            person.FirstName = personNameDto.FirstName;
            person.MiddleName = personNameDto.MiddleName;
            person.LastName = personNameDto.LastName;
            person.ModifiedDate = DateTime.UtcNow;  
            _repo.SaveChanges();
            return _mapper.Map<PersonNameDto>(person);  
        }

        public async Task<PersonDto> GetPersonAsync(int businessEntityId)
        {
            var person = (await _repo.Person.Include(c => c.BusinessEntity).Include(c => c.EmailAddress)
                .Include(c => c.BusinessEntityContact)
                .Include(c => c.PersonPhone)
                .Include(c => c.PersonCreditCard)
                .ThenInclude(c => c.CreditCard)
                .Include(c => c.BusinessEntityContact)
                .Include(c => c.Customer)
                .Where(c => c.BusinessEntityId == businessEntityId).AsNoTracking().ToListAsync()).FirstOrDefault();
            if (person == null)
                return null;
            return _mapper.Map<PersonDto>(person);
        }
    }
}
    