using System.Threading;
using System.Threading.Tasks;
using AdventureWorks.Service.Dtos.Collections;

namespace AdventureWorks.Service
{
    public interface IAdventureWorksService
    {
        Task<PersonNameDto> GetPersonsNameAsync(int businessEntityId, CancellationToken cancellation);
        Task<PersonNameDto> PatchPersonsNameAsync(int businessEntityId, PersonNameBaseDto personNameDto,
            CancellationToken cancellation);
        Task<PersonDto> GetPersonAsync(int businessEntityId, CancellationToken cancellation);
    }
}
