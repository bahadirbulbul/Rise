using Phonebook.Shared;
using Phonebook.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phonebook.Services.User.Dtos
{
    public class CreatePersonContactDto
    {
        //private ContactTypeEnum contactType;

        //public string ContactType
        //{
        //    get { return Helper.GetDisplayName(contactType); }
        //    set { contactType = Helper.GetDisplayName(value); }
        //}


        public string PersonID { get; set; }
        public string ContactType { get; set; }
        public string Description { get; set; }
    }
}
