using Microsoft.AspNetCore.Mvc;
using Phonebook.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonebook.Shared.ControllerBases
{
    public class BaseController : ControllerBase
    {
        public IActionResult CreateActionResultInstance<T>(ResponseDto<T> response)
        {
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }
    }
}
