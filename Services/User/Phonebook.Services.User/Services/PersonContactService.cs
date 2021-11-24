using AutoMapper;
using MongoDB.Driver;
using Phonebook.Services.User.Dtos;
using Phonebook.Services.User.Models;
using Phonebook.Shared;
using Phonebook.Shared.Dtos;
using Phonebook.Shared.Enums;
using Phonebook.Shared.Messages;
using Phonebook.Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phonebook.Services.User.Services
{
    public class PersonContactService : IPersonContactService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<PersonContact> _repository;

        public PersonContactService(IRepository<PersonContact> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ResponseDto<CreatePersonContactDto>> CreateAsync(CreatePersonContactDto personContact)
        {
            await _repository.AddAsync(_mapper.Map<PersonContact>(personContact));

            return ResponseDto<CreatePersonContactDto>.Success(personContact, 200);
        }

        public async Task<ResponseDto<PersonContactDto>> DeleteByIdAsync(string uuid)
        {
            var personContact = await _repository.GetAsync(x => x.UUID == uuid);

            if (personContact == null)
            {
                return ResponseDto<PersonContactDto>.Fail("İletişim bilgisi bulunamadı", 404);
            }
            var deleteResult = await _repository.DeleteAsync(personContact.UUID);

            return ResponseDto<PersonContactDto>.Success(_mapper.Map<PersonContactDto>(personContact), 200);
        }

        public async Task<ResponseDto<PersonContactDto>> DeleteAllByPersonIdAsync(string personUUID)
        {
            var personContacts = await _repository.GetListWithFiltersAsync(s => s.PersonID == personUUID);

            if (personContacts == null)
            {
                return ResponseDto<PersonContactDto>.Fail(personUUID + " ID'li kullanıcıya ait iletişim bilgieri bulunamadı.", 404);
            }
            foreach (var contact in personContacts)
            {
                var deleteResult = await _repository.DeleteAsync(s => s.UUID == contact.UUID);
            }

            return ResponseDto<PersonContactDto>.Success(200);
        }

        public async Task<ResponseDto<List<PersonContactDto>>> GetAllByPersonUUIDAsync(string personUUID)
        {
            var personContacts = await _repository.GetListWithFiltersAsync(s => s.PersonID == personUUID);

            if (personContacts == null)
            {
                return ResponseDto<List<PersonContactDto>>.Fail(personUUID + " ID'li kullanıcıya ait iletişim bilgieri bulunamadı.", 404);
            }
            return ResponseDto<List<PersonContactDto>>.Success(_mapper.Map<List<PersonContact>, List<PersonContactDto>>(personContacts), 200);
        }

        public async Task<List<PrepareReportDataCommand>> PrepareReportData()
        {
            //DTO rapor datası ile doldurulacak.
            //var dto = new PrepareReportDataCommand
            //{
            //    GSMCount = 1,
            //    Location = "ankara",
            //    PersonCount = 4
            //};

            //Konum bilgisi bulunan iletişim bilgileri.
            var list = await _repository.GetListWithFiltersAsync(_ => true);
            var report = from T in (
                          (from P in list
                           where
                             P.ContactType == Helper.GetDisplayName(ContactTypeEnum.Location)
                           group P by new
                           {
                               P.Description,
                               P.PersonID
                           } into g
                           select new
                           {
                               g.Key.Description,
                               KisiSayisi = g.Count(p => p.PersonID != null),
                               g.Key.PersonID,
                               GSMCount =
                               (from PC in list
                                where
                                 PC.ContactType == Helper.GetDisplayName(ContactTypeEnum.GSM) &&
                                 PC.PersonID == g.Key.PersonID
                                select new
                                {
                                    PC
                                }).Count()
                           }))
                         group T by new
                         {
                             T.Description
                         } into g
                         select new PrepareReportDataCommand
                         {
                             GSMCount = g.Sum(p => p.GSMCount),
                             PersonCount = g.Sum(p => p.KisiSayisi),
                             Location = g.Key.Description
                         };


            return report.ToList();
        }
    }
}
