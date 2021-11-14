using AutoMapper;
using Phonebook.Services.User.Dtos;
using Phonebook.Services.User.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phonebook.Services.User.Mappings
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Person, PersonDto>().ReverseMap();
            CreateMap<Person, CreatePersonDto>().ReverseMap();
            CreateMap<Person, EditPersonDto>().ReverseMap();

            CreateMap<PersonContact, PersonContactDto>().ReverseMap();
            CreateMap<PersonContact, CreatePersonContactDto>().ReverseMap();


        }
    }
}
