using Phonebook.Services.User.Dtos;
using Phonebook.Shared.Dtos;
using Phonebook.Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phonebook.Services.User.Services
{
    public interface IPersonContactService
    {
        Task<ResponseDto<PersonContactDto>> DeleteAllByPersonIdAsync(string personUUID);
        Task<ResponseDto<PersonContactDto>> DeleteByIdAsync(string uuid);
        Task<ResponseDto<CreatePersonContactDto>> CreateAsync(CreatePersonContactDto personContact);
        Task<ResponseDto<List<PersonContactDto>>> GetAllByPersonUUIDAsync(string personUUID);
        Task<List<PrepareReportDataCommand>> PrepareReportData();
    }
}
