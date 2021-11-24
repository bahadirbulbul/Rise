using Microsoft.AspNetCore.Mvc;
using Moq;
using Phonebook.Services.User.Controllers;
using Phonebook.Services.User.Dtos;
using Phonebook.Services.User.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Phonebook.Services.User.Test.ControllersTests
{
    public class PersonContactControllerTests
    {
        private readonly Mock<IPersonContactService> _mockService;
        private readonly PersonContactController _contactController;

        public PersonContactControllerTests()
        {
            _mockService = new Mock<IPersonContactService>();
            _contactController = new PersonContactController(_mockService.Object);
        }


        [Theory]
        [InlineData("1")]
        public async Task GetAllByPersonUUID_WhenCalled_ReturnSuccess(string UUID)
        {
            _mockService.Setup(p => p.GetAllByPersonUUIDAsync(UUID)).Returns(Task.FromResult(new Shared.Dtos.ResponseDto<List<Dtos.PersonContactDto>>
            {
                IsSuccess = true,
                StatusCode = 200,
                Errors = null,
                Data = new List<Dtos.PersonContactDto>
            {
               new Dtos.PersonContactDto
               {
                   ContactType="GSM",
                   Description="5547431039",
                   PersonID="12345",
                   UUID="23456"
               }
            }
            }));
            var actual = await _contactController.GetAllByPersonUUID(UUID) as ObjectResult;
            var actualDto = actual.Value as Shared.Dtos.ResponseDto<List<PersonContactDto>>;

            Assert.Equal(200, actual.StatusCode);
            Assert.Null(actualDto.Errors);
        }

        [Fact]
        public async Task Create_WhenCalled_ReturnSuccess()
        {
            var dto = new CreatePersonContactDto
            {
                ContactType = "Email",
                Description = "bulbulbahadir1@gmail.com",
                PersonID = "12345"
            };
            _mockService.Setup(p => p.CreateAsync(dto)).Returns(Task.FromResult(new Shared.Dtos.ResponseDto<CreatePersonContactDto>
            {
                Data = null,
                Errors = null,
                IsSuccess = true,
                StatusCode = 200
            }));
            var actual = await _contactController.Create(dto) as ObjectResult;
            var actualDto = actual.Value as Shared.Dtos.ResponseDto<CreatePersonContactDto>;

            Assert.True(actualDto.IsSuccess);
            Assert.Null(actualDto.Errors);
        }

        [Theory]
        [InlineData("1")]
        public async Task Delete_WhenCalled_ReturnSuccess(string UUID)
        {
            _mockService.Setup(p => p.DeleteByIdAsync(UUID)).Returns(Task.FromResult(new Shared.Dtos.ResponseDto<PersonContactDto>
            {
                Data = null,
                StatusCode = 200,
                IsSuccess = true,
                Errors = null
            }));
            var actual = await _contactController.Delete(UUID) as ObjectResult;
            var actualDto = actual.Value as Shared.Dtos.ResponseDto<PersonContactDto>;

            Assert.Equal(200, actual.StatusCode);
            Assert.Null(actualDto.Errors);
        }

        [Theory]
        [InlineData("1")]
        public async Task DeleteAllByPersonId_WhenCalled_ReturnSuccess(string UUID)
        {
            _mockService.Setup(p => p.DeleteAllByPersonIdAsync(UUID)).Returns(Task.FromResult(new Shared.Dtos.ResponseDto<PersonContactDto>
            {
                Data = null,
                StatusCode = 200,
                IsSuccess = true,
                Errors = null
            }));
            var actual = await _contactController.DeleteAllByPersonId(UUID) as ObjectResult;
            var actualDto = actual.Value as Shared.Dtos.ResponseDto<PersonContactDto>;

            Assert.Equal(200, actual.StatusCode);
            Assert.Null(actualDto.Errors);
        }

    }
}
