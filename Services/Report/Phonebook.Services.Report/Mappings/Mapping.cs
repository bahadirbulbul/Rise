using AutoMapper;
using Phonebook.Services.Report.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phonebook.Services.Report.Mappings
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Models.Report, ReportDto>().ReverseMap();
        }
    }
}
