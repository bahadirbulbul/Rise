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
    public class PersonController : BaseController
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpGet("{uuid}")]
        public async Task<IActionResult> GetById(string uuid)
        {
            return CreateActionResultInstance(await _personService.GetByIdAsync(uuid));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return CreateActionResultInstance(await _personService.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePersonDto dto)
        {
            var response = await _personService.CreateAsync(dto);

            return CreateActionResultInstance(response);
        }

        [HttpDelete("{uuid}")]
        public async Task<IActionResult> Delete(string uuid)
        {
            var response = await _personService.DeleteByIdAsync(uuid);

            return CreateActionResultInstance(response);
        }
    }
}
