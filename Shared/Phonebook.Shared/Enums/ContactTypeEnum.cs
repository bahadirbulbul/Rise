using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonebook.Shared.Enums
{
    public enum ContactTypeEnum
    {
        [Display(Name = "Location")]
        Location,

        [Display(Name = "GSM")]
        GSM,

        [Display(Name = "Email")]
        Email,
    }
}
