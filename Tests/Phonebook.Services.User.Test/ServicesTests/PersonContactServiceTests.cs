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
    public class PersonContactServiceTests
    {
        private readonly Mock<IRepository<PersonContact>> _mockRepository;
        private readonly IMapper _mapper;
        private readonly PersonContactService _contactService;

        private readonly PersonContact _contact;
        private readonly List<PersonContact> _contacts;
        public PersonContactServiceTests()
        {
            _mockRepository = new Mock<IRepository<PersonContact>>();
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new Mappings.Mapping());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }

            _contactService = new PersonContactService(_mockRepository.Object, _mapper);

            _contact = new PersonContact
            {
                ContactType = Helper.GetDisplayName(ContactTypeEnum.Location),
                Description = "Elazığ",
                PersonID = "12345",
                UUID = "23456"
            };
            _contacts = new List<PersonContact> { _contact };
        }

        [Fact]
        public async Task CreateAsync_InsertRecord_ReturnInsertedRecord()
        {
            _mockRepository.Setup(s => s.AddAsync(It.IsAny<PersonContact>())).Returns(Task.FromResult(_contact));

            var actual = await _contactService.CreateAsync(_mapper.Map<CreatePersonContactDto>(_contact));

            Assert.NotNull(actual.Data);
            Assert.Equal(200, actual.StatusCode);
        }

        [Theory]
        [InlineData("11111")]
        public async Task DeleteByIdAsync_ContactNotExist_ReturnFailMessageAndCode(string UUID)
        {
            PersonContact nullContact = null;

            _mockRepository.Setup(s => s.GetAsync(s => s.UUID == UUID)).Returns(Task.FromResult(nullContact));
            var actual = await _contactService.DeleteByIdAsync(UUID);

            Assert.Equal<int>(404, actual.StatusCode);
            Assert.Equal("İletişim bilgisi bulunamadı", actual.Errors.First().ToString());
        }

        [Theory]
        [InlineData("23456")]
        public async Task DeleteByIdAsync_ContactExist_ReturnSuccess(string UUID)
        {
            _mockRepository.Setup(s => s.GetAsync(s => s.UUID == UUID)).Returns(Task.FromResult(_contact));
            var actual = await _contactService.DeleteByIdAsync(UUID);

            Assert.Equal<int>(200, actual.StatusCode);
        }

        [Theory]
        [InlineData("12345")]
        public async Task DeleteAllByPersonIdAsync_ContactNotExist_ReturnFailMessageAndCode(string personUUID)
        {
            List<PersonContact> nullContact = null;

            _mockRepository.Setup(s => s.GetListWithFiltersAsync(s => s.PersonID == personUUID)).Returns(Task.FromResult(nullContact));

            var actual = await _contactService.DeleteAllByPersonIdAsync(personUUID);

            Assert.Equal<int>(404, actual.StatusCode);
            Assert.Equal(personUUID + " ID'li kullanıcıya ait iletişim bilgieri bulunamadı.", actual.Errors.First().ToString());

        }

        [Theory]
        [InlineData("23456")]
        public async Task DeleteAllByPersonIdAsync_ContactExist_ReturnSuccess(string personUUID)
        {
            _mockRepository.Setup(s => s.GetListWithFiltersAsync(s => s.PersonID == personUUID)).Returns(Task.FromResult(_contacts));
            var actual = await _contactService.DeleteAllByPersonIdAsync(personUUID);

            Assert.Equal<int>(200, actual.StatusCode);
        }

        [Theory]
        [InlineData("54321")]
        public async Task GetAllByPersonUUIDAsync_ContactNotExist_ReturnFailMessageAndCode(string personUUID)
        {
            List<PersonContact> nullContacts = null;
            _mockRepository.Setup(s => s.GetListWithFiltersAsync(s => s.PersonID == personUUID)).Returns(Task.FromResult(nullContacts));

            var actual = await _contactService.GetAllByPersonUUIDAsync(personUUID);

            Assert.Equal<int>(404, actual.StatusCode);
            Assert.Equal(personUUID + " ID'li kullanıcıya ait iletişim bilgieri bulunamadı.", actual.Errors.First().ToString());
        }

        [Theory]
        [InlineData("12345")]
        public async Task GetAllByPersonUUIDAsync_ContactExist_ReturnSuccessAndContacts(string personUUID)
        {
            _mockRepository.Setup(s => s.GetListWithFiltersAsync(s => s.PersonID == personUUID)).Returns(Task.FromResult(_contacts));

            var actual = await _contactService.GetAllByPersonUUIDAsync(personUUID);

            Assert.Single<PersonContactDto>(actual.Data);
            Assert.Equal<int>(200, actual.StatusCode);
        }
    }
}
