using System;
using System.Threading;
using System.Threading.Tasks;
using AdventureWorks.Service.Dtos.Collections;

namespace AdventureWorks.Service
{
    public interface IAdventureWorksService
    {
        Task<PersonNameDto?> GetPersonsNameAsync(Guid rowGuid, CancellationToken cancellation);
        Task<PersonNameDto> PatchPersonsNameAsync(Guid rowGuid, PersonNameBaseDto personNameDto,
            CancellationToken cancellation);
        Task<PersonDto> GetPersonAsync(Guid rowGuid, CancellationToken cancellation);
    }
}
