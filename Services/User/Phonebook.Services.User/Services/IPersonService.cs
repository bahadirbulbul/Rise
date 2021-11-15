using Phonebook.Services.User.Dtos;
using Phonebook.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phonebook.Services.User.Services
{
    public interface IPersonService
    {
        Task<ResponseDto<List<PersonDto>>> GetAllAsync();
        Task<ResponseDto<CreatePersonDto>> CreateAsync(CreatePersonDto person);
        Task<ResponseDto<PersonDto>> GetByIdAsync(string uuid);
        Task<ResponseDto<NoContent>> DeleteByIdAsync(string uuid);
    }
}
