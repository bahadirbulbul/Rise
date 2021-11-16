using AutoMapper;
using MongoDB.Driver;
using Phonebook.Services.User.Dtos;
using Phonebook.Services.User.Models;
using Phonebook.Services.User.Settings;
using Phonebook.Shared.Dtos;
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

        public async Task<ResponseDto<ContactReportDto>> PrepareReportData()
        {
            //DTO rapor datası ile doldurulacak.
            var dto = new ContactReportDto();

            //Konum bilgisi bulunan iletişim bilgileri.
            var list = await _personContactCollection.Find(s => s.ContactType == "Location").ToListAsync();

            var userCount = list.Select(s => s.PersonID).Distinct().Count();
            var GSMCount = list.Where(s => s.ContactType == "GSM").Count();
            //var result = _personContactCollection.Aggregate()
            //            .Group(
            //                x => x.ContactType == "Konum",
            //                g => new
            //                {
            //                    Result = g.Sum(
            //                               x => x.ContactType == "GSM".Count()
            //                               ).Max(),

            //                    g.Select(y => y.ContactType == "GSM").ToList().Count()
            //                }
            //            ).ToList();

            //result.ForEach(doc => Console.WriteLine(doc.ToJson()));



            //foreach (var item in collection)
            //{

            //}
            return ResponseDto<ContactReportDto>.Success(dto, 200);
        }
    }
}
