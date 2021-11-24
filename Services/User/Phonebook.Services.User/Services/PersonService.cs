using AutoMapper;
using Phonebook.Services.User.Dtos;
using Phonebook.Services.User.Models;
using Phonebook.Shared.Dtos;
using Phonebook.Shared.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Phonebook.Services.User.Services
{
    public class PersonService : IPersonService
    {
        private readonly IMapper _mapper;
        private readonly IPersonContactService _personContactService;
        private readonly IRepository<Person> _repository;

        public PersonService(IMapper mapper,IPersonContactService personContactService, IRepository<Person> repository)
        {
            _repository = repository;
            _mapper = mapper;
            _personContactService = personContactService;
        }

        //Rehberdeki kişilerin listelenmesi
        public async Task<ResponseDto<List<PersonDto>>> GetAllAsync()
        {
            var persons = await _repository.GetListWithFiltersAsync(x => true);
            return ResponseDto<List<PersonDto>>.Success(_mapper.Map<List<PersonDto>>(persons), 200);
        }

        //Rehberde kişi oluşturma
        public async Task<ResponseDto<CreatePersonDto>> CreateAsync(CreatePersonDto person)
        {
            await _repository.AddAsync(_mapper.Map<Person>(person));

            return ResponseDto<CreatePersonDto>.Success(person, 200);
        }

        //Rehberdeki bir kişinin detay bilgileri
        public async Task<ResponseDto<PersonDto>> GetByIdAsync(string uuid)
        {
            var person = await _repository.GetAsync(x => x.UUID == uuid);
            if (person == null)
            {
                return ResponseDto<PersonDto>.Fail("Kişi bulunamadı", 404);
            }
            var personDTO = _mapper.Map<PersonDto>(person);

            //person'a ait iletişim bilgilerini getir. eğer iletişim bilgisi yoksa boş liste ata; varsa doldur.
            var personContacts = await _personContactService.GetAllByPersonUUIDAsync(uuid);
            if (personContacts.StatusCode != 200)
                personDTO.PersonContacts = new List<PersonContactDto>();
            else
                personDTO.PersonContacts = personContacts.Data;
            
            return ResponseDto<PersonDto>.Success(personDTO, 200);
        }

        //Rehberden kişi kaldırma
        public async Task<ResponseDto<NoContent>> DeleteByIdAsync(string uuid)
        {
            var person = await _repository.GetAsync(x => x.UUID == uuid);
            if (person == null)
            {
                return ResponseDto<NoContent>.Fail("Kişi bulunamadı", 404);
            }
            //person'a ait iletişim bilgileri siliniyor...
            var deleteContactResult = await _personContactService.DeleteAllByPersonIdAsync(uuid);

            //person kaydı siliniyor
            var deleteResult = await _repository.DeleteAsync(s => s.UUID == uuid);

            return ResponseDto<NoContent>.Success(200);
        }
    }
}
