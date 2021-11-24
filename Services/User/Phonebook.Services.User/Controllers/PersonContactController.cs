using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Phonebook.Services.User.Dtos;
using Phonebook.Services.User.Services;
using Phonebook.Shared.ControllerBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phonebook.Services.User.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PersonContactController : BaseController
    {
        private readonly IPersonContactService _personContactService;

        public PersonContactController(IPersonContactService personContactService)
        {
            _personContactService = personContactService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllByPersonUUID(string uuid)
        {
            var contacts = await _personContactService.GetAllByPersonUUIDAsync(uuid);
            return CreateActionResultInstance(contacts);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePersonContactDto dto)
        {
            var response = await _personContactService.CreateAsync(dto);
            return CreateActionResultInstance(response);
        }

        [HttpDelete("{uuid}")]
        public async Task<IActionResult> Delete(string uuid)
        {
            var response = await _personContactService.DeleteByIdAsync(uuid);
            return CreateActionResultInstance(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAllByPersonId(string personUUID)
        {
            var response = await _personContactService.DeleteAllByPersonIdAsync(personUUID);
            return CreateActionResultInstance(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetReportData()
        {
            var reportData = await _personContactService.PrepareReportData();
            return Ok(reportData);
        }
    }
}
