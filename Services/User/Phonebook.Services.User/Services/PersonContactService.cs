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
    internal class PersonContactService : IPersonContactService
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
            var personContants = _personContactCollection.Find(s => s.PersonID == personUUID);

            if (personContants == null)
            {
                return ResponseDto<PersonContactDto>.Fail(personUUID + "ID'i kullanıcıya ait iletişim bilgieri bulunamadı.", 404);
            }

            var deleteResult = await _personContactCollection.DeleteManyAsync(s => s.PersonID == personUUID);

            return ResponseDto<PersonContactDto>.Success(200);

        }
    }
}
