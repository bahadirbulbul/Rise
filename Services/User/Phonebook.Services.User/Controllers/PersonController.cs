using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Phonebook.Services.User.Services;
using Phonebook.Shared.ControllerBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phonebook.Services.User.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    internal class PersonController : BaseController
    {
        private readonly IPersonService _personService;

        internal PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpGet("{uuid}")]
        public async Task<IActionResult> GetById(string uuid)
        {
            return CreateActionResultInstance(await _personService.GetByIdAsync(uuid));
        }
    }
}
