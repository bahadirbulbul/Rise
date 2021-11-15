﻿using AutoMapper;
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
    internal class PersonService : IPersonService
    {
        private readonly IMongoCollection<Person> _personCollection;
        private readonly IMapper _mapper;
        private readonly IPersonContactService _personContactService;

        public PersonService(IMapper mapper, IDBSettings dbSettings, IPersonContactService personContactService)
        {
            var client = new MongoClient(dbSettings.ConnectionString);
            var db = client.GetDatabase(dbSettings.DatabaseName);

            _personCollection = db.GetCollection<Person>(dbSettings.PersonCollectionName);
            _mapper = mapper;
            _personContactService = personContactService;
        }

        //Rehberdeki kişilerin listelenmesi
        public async Task<ResponseDto<List<PersonDto>>> GetAllAsync()
        {
            var persons = await _personCollection.Find(Person => true).ToListAsync();
            return ResponseDto<List<PersonDto>>.Success(_mapper.Map<List<PersonDto>>(persons), 200);
        }

        //Rehberde kişi oluşturma
        public async Task<ResponseDto<CreatePersonDto>> CreateAsync(CreatePersonDto person)
        {
            await _personCollection.InsertOneAsync(_mapper.Map<Person>(person));

            return ResponseDto<CreatePersonDto>.Success(person, 200);
        }

        //Rehberdeki bir kişinin detay bilgileri
        public async Task<ResponseDto<PersonDto>> GetByIdAsync(string uuid)
        {
            var person = await _personCollection.Find<Person>(x => x.UUID == uuid).FirstOrDefaultAsync();
            if (person == null)
            {
                return ResponseDto<PersonDto>.Fail("Kişi bulunamadı", 404);
            }

            return ResponseDto<PersonDto>.Success(_mapper.Map<PersonDto>(person), 200);
        }

        //Rehberden kişi kaldırma
        public async Task<ResponseDto<PersonDto>> DeleteByIdAsync(string uuid)
        {
            var person = await _personCollection.Find<Person>(x => x.UUID == uuid).FirstOrDefaultAsync();
            if (person == null)
            {
                return ResponseDto<PersonDto>.Fail("Kişi bulunamadı", 404);
            }
            //person'a ait iletişim bilgileri siliniyor...
            var deleteContactResult = await _personContactService.DeleteAllByPersonIdAsync(uuid);

            //person kaydı siliniyor
            var deleteResult = await _personCollection.DeleteOneAsync(person.UUID);

            return ResponseDto<PersonDto>.Success(_mapper.Map<PersonDto>(person), 200);
        }
    }
}
