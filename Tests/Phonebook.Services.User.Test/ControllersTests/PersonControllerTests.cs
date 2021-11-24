using Microsoft.AspNetCore.Mvc;
using Moq;
using Phonebook.Services.User.Controllers;
using Phonebook.Services.User.Dtos;
using Phonebook.Services.User.Models;
using Phonebook.Services.User.Services;
using Phonebook.Shared.ControllerBases;
using Phonebook.Shared.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Phonebook.Services.User.Test.ControllersTests
{
    public class PersonControllerTests
    {
        private readonly Mock<IPersonService> _mockService;
        private readonly PersonController _personController;

        private readonly PersonDto _person;

        private readonly ResponseDto<PersonDto> _returnedDto;
        private readonly ResponseDto<List<PersonDto>> _returnedDtos;
        public PersonControllerTests()
        {
            _mockService = new Mock<IPersonService>();
            _personController = new PersonController(_mockService.Object);

            _person = new PersonDto
            {
                CompanyName = "Rise",
                Name = "Bahadır",
                Surname = "Bülbül",
                UUID = "12345",
                PersonContacts = new List<PersonContactDto>
               {
                  new PersonContactDto
                  {
                      UUID="2345",
                      ContactType="Location",
                      Description="Samsun",
                      PersonID="12345"
                  }
               }
            };
            _returnedDto = new ResponseDto<PersonDto>
            {
                Data = _person,
                Errors = null,
                StatusCode = 200,
                IsSuccess = true
            };

            _returnedDtos = new ResponseDto<List<PersonDto>>
            {
                Errors = null,
                StatusCode = 200,
                IsSuccess = true
            };
            _returnedDtos.Data = new List<PersonDto>();
            _returnedDtos.Data.Add(_person);
        }


        [Theory]
        [InlineData("1")]
        public async Task GetById_WhenCalled_ReturnSuccess(string UUID)
        {
            _mockService.Setup(p => p.GetByIdAsync(UUID)).Returns(Task.FromResult(_returnedDto));
            var actual = await _personController.GetById(UUID) as ObjectResult;
            var actualDto = actual.Value as Shared.Dtos.ResponseDto<Dtos.PersonDto>;

            Assert.Equal(200, actual.StatusCode);
            Assert.Null(actualDto.Errors);
        }

        [Fact]
        public async Task GetAll_WhenCalled_ReturnList()
        {
            _mockService.Setup(p => p.GetAllAsync()).Returns(Task.FromResult(_returnedDtos));
            var actual = await _personController.GetAll() as ObjectResult;

            Assert.Equal(200, actual.StatusCode);
        }

       

    }
}
