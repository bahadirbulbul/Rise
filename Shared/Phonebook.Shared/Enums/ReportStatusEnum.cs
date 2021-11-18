using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonebook.Shared.Enums
{
    public enum ReportStatusEnum
    {
        [Display(Name = "Hazırlanıyor")]
        Hazırlanıyor,

        [Display(Name = "Tamamlandı")]
        Tamamlandı
    }
}
