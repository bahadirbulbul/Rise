﻿using Phonebook.Services.User.Dtos;
using Phonebook.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phonebook.Services.User.Services
{
    interface IPersonContactService
    {
        Task<ResponseDto<PersonContactDto>> DeleteAllByPersonIdAsync(string personUUID);
        Task<ResponseDto<PersonContactDto>> DeleteByIdAsync(string uuid);
        Task<ResponseDto<CreatePersonContactDto>> CreateAsync(CreatePersonContactDto personContact);
        Task<ResponseDto<List<PersonContactDto>>> GetAllByPersonUUID(string personUUID);
    }
}
