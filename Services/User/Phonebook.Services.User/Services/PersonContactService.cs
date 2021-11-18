using AutoMapper;
using MongoDB.Driver;
using Phonebook.Services.User.Dtos;
using Phonebook.Services.User.Models;
using Phonebook.Services.User.Settings;
using Phonebook.Shared;
using Phonebook.Shared.Dtos;
using Phonebook.Shared.Enums;
using Phonebook.Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phonebook.Services.User.Services
{
    public class PersonContactService : IPersonContactService
    {
        private readonly IMongoCollection<PersonContact> _personContactCollection;
        private readonly IMapper _mapper;

        public PersonContactService(IMapper mapper, IDBSettings dbSettings)
        {
            var client = new MongoClient(dbSettings.ConnectionString);
            var db = client.GetDatabase(dbSettings.DatabaseName);

            _personContactCollection = db.GetCollection<PersonContact>(dbSettings.PersonContactCollectionName);
            _mapper = mapper;
        }
        public async Task<ResponseDto<CreatePersonContactDto>> CreateAsync(CreatePersonContactDto personContact)
        {
            await _personContactCollection.InsertOneAsync(_mapper.Map<PersonContact>(personContact));

            return ResponseDto<CreatePersonContactDto>.Success(personContact, 200);
        }

        public async Task<ResponseDto<PersonContactDto>> DeleteByIdAsync(string uuid)
        {
            var personContact = await _personContactCollection.Find<PersonContact>(x => x.UUID == uuid).FirstOrDefaultAsync();

            if (personContact == null)
            {
                return ResponseDto<PersonContactDto>.Fail("İletişim bilgisi bulunamadı", 404);
            }
            var deleteResult = await _personContactCollection.DeleteOneAsync(personContact.UUID);

            return ResponseDto<PersonContactDto>.Success(_mapper.Map<PersonContactDto>(personContact), 200);
        }

        public async Task<ResponseDto<PersonContactDto>> DeleteAllByPersonIdAsync(string personUUID)
        {
            var personContants = await _personContactCollection.FindAsync(s => s.PersonID == personUUID);

            if (personContants == null)
            {
                return ResponseDto<PersonContactDto>.Fail(personUUID + "ID'i kullanıcıya ait iletişim bilgieri bulunamadı.", 404);
            }

            var deleteResult = await _personContactCollection.DeleteManyAsync(s => s.PersonID == personUUID);

            return ResponseDto<PersonContactDto>.Success(200);
        }
        public async Task<ResponseDto<List<PersonContactDto>>> GetAllByPersonUUID(string personUUID)
        {
            try
            {
                var personContacts = await _personContactCollection.Find(s => s.PersonID == personUUID).ToListAsync();

                if (personContacts == null)
                {
                    return ResponseDto<List<PersonContactDto>>.Fail(personUUID + "ID'i kullanıcıya ait iletişim bilgieri bulunamadı.", 404);
                }

                return ResponseDto<List<PersonContactDto>>.Success(_mapper.Map<List<PersonContactDto>>(personContacts), 200);
            }
            catch (Exception ex)
            {

                throw;
            }
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
            var list = await _personContactCollection.Find(_ => true).ToListAsync();
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
