using AutoMapper;
using Moq;
using Phonebook.Services.User.Dtos;
using Phonebook.Services.User.Models;
using Phonebook.Services.User.Services;
using Phonebook.Shared;
using Phonebook.Shared.Enums;
using Phonebook.Shared.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Phonebook.Services.User.Test.ServicesTests
{
    public class PersonServiceTests
    {
        private readonly Mock<IRepository<Person>> _personMockRepository;
        private readonly Mock<IRepository<PersonContact>> _contactMockRepository;

        private readonly IMapper _mapper;
        private readonly PersonService _personService;
        private readonly PersonContactService _personContactService;

        private readonly List<Person> _persons;
        private readonly Person _person;
        private readonly Person _personHasNoContact;

        private readonly PersonContact _contact;
        private readonly List<PersonContact> _personContacts;

        public PersonServiceTests()
        {
            _personMockRepository = new Mock<IRepository<Person>>();
            _contactMockRepository = new Mock<IRepository<PersonContact>>();

            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new Mappings.Mapping());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
            _personContactService = new PersonContactService(_contactMockRepository.Object, _mapper);
            _personService = new PersonService(_mapper, _personContactService, _personMockRepository.Object);

            _contact = new PersonContact
            {
                ContactType = Helper.GetDisplayName(ContactTypeEnum.Location),
                Description = "Ankara",
                PersonID = "1",
                UUID = "asdad34sdfsd"
            };

            _person = new Person
            {
                CompanyName = "Rise Tech",
                Name = "Ali Veli",
                Surname = "Bülbül",
                UUID = "2",
                PersonContacts = new List<PersonContact> { _contact }
            };

            _persons = new List<Person> { _person };
            _personContacts = new List<PersonContact> { _contact };
            _personHasNoContact = new Person
            {
                PersonContacts = null
            };
        }

        [Fact]
        public async Task GetAllAsync_ExistRecords_ReturnRecords()
        {
            _personMockRepository.Setup(x => x.GetListWithFiltersAsync(s => true)).Returns(Task.FromResult(_persons));

            var returnedValue = await _personService.GetAllAsync();

            Assert.True(1 == returnedValue.Data.Count());
            Assert.Equal(200, returnedValue.StatusCode);
            Assert.Contains(Helper.GetDisplayName(ContactTypeEnum.Location), returnedValue.Data.First().PersonContacts.First().ContactType);
        }

        [Fact]
        public async Task CreateAsync_InsertRecord_ReturnInsertedRecord()
        {
            _personMockRepository.Setup(s => s.AddAsync(It.IsAny<Person>())).Returns(Task.FromResult(_person));

            var actual = await _personService.CreateAsync(_mapper.Map<CreatePersonDto>(_person));

            Assert.NotNull(actual.Data);
            Assert.Equal(200, actual.StatusCode);
        }

        [Theory]
        [InlineData("11111")]
        public async Task GetByIdAsync_PersonIsNotExist_ReturnFailMessageAndCode(string UUID)
        {
            Person nullPerson = null;

            _personMockRepository.Setup(s => s.GetAsync(UUID)).Returns(Task.FromResult(nullPerson));
            var actual = await _personService.GetByIdAsync(UUID);

            Assert.Null(actual.Data);
            Assert.Equal<int>(404, actual.StatusCode);
            Assert.Equal("Kişi bulunamadı", actual.Errors.First().ToString());
        }

        [Theory]
        [InlineData("1")]
        public async Task GetByIdAsync_PersonExist_ReturnPersonAndContacts(string UUID)
        {
            _personMockRepository.Setup(s => s.GetAsync(s => s.UUID == UUID)).Returns(Task.FromResult(_person));
            _contactMockRepository.Setup(s => s.GetListWithFiltersAsync(s => s.PersonID == UUID)).Returns(Task.FromResult(_personContacts));
            var actual = await _personService.GetByIdAsync(UUID);

            Assert.NotNull(actual.Data);
            Assert.NotNull(actual.Data.PersonContacts);
            Assert.Equal(200, actual.StatusCode);
        }

        [Theory]
        [InlineData("11111")]
        public async Task DeleteByIdAsync_PersonNotExist_ReturnFailMessageAndCode(string UUID)
        {
            Person nullPerson = null;

            _personMockRepository.Setup(s => s.GetAsync(s => s.UUID == UUID)).Returns(Task.FromResult(nullPerson));
            var actual = await _personService.DeleteByIdAsync(UUID);

            Assert.Equal<int>(404, actual.StatusCode);
            Assert.Equal("Kişi bulunamadı", actual.Errors.First().ToString());
        }

        [Theory]
        [InlineData("1")]
        public async Task DeleteByIdAsync_PersonExist_ReturnSuccess(string UUID)
        {
            _personMockRepository.Setup(s => s.GetAsync(s => s.UUID == UUID)).Returns(Task.FromResult(_person));
            var actual = await _personService.DeleteByIdAsync(UUID);

            Assert.Equal<int>(200, actual.StatusCode);
        }
    }
}
