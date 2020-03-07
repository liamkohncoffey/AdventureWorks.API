using System.Threading;
using System.Threading.Tasks;
using AdventureWorks.Service.Dtos.Collections;

namespace AdventureWorks.Service
{
    public interface IAdventureWorksService
    {
        Task<PersonNameDto> GetPersonsNameAsync(int businessEntityId);
        Task<PersonNameDto> PatchPersonsNameAsync(int businessEntityId, PersonNameBaseDto personNameDto);
        Task<PersonDto> GetPersonAsync(int businessEntityId);
    }
}
