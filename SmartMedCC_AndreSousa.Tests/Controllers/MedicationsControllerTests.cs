using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using NSubstitute;
using NSubstitute.ExceptionExtensions;

using SmartMedCC_AndreSousa.Controllers;
using SmartMedCC_AndreSousa.DTOs;
using SmartMedCC_AndreSousa.Models;
using SmartMedCC_AndreSousa.Services;

namespace SmartMedCC_AndreSousa.Tests.Controllers
{
    /// <summary>
    ///     This class contains unit tests for <see cref="MedicationsController"/>.
    /// </summary>
    [TestFixture]
    public class MedicationsControllerTests
    {
        private IMedicationsService _medicationsService;
        private MedicationsController _controller;

        [SetUp]
        public void SetUp()
        {
            _medicationsService = Substitute.For<IMedicationsService>();
            _controller = new MedicationsController(_medicationsService, Substitute.For<ILogger<MedicationsController>>());
        }

        /// <summary>
        ///     Given a new <see cref="MedicationDTO"/><br />
        ///     When <see cref="MedicationsController.Create(MedicationDTO)" /> is called<br />
        ///     Then an <see cref="IActionResult"/> is returned<br />
        ///     And the status code is 201<br />
        ///     And the returned object matches the submitted <see cref="MedicationDTO" />.
        /// </summary>
        /// <returns>The <see cref="Task"/> of the operation.</returns>
        [Test]
        [Category("POST")]
        public async Task GivenANewMedicationWhenCreateIsCalledThenAnActionIsReturnedAndTheStatusCodeIs201AndTheReturnedObjectMatchesTheSubmittedMedication()
        {
            // Arrange
            var medicationDto = new MedicationDTO { Id = 1, Name = "Ben-u-ron", Quantity = 4 };

            _medicationsService.AddMedicationAsync(Arg.Any<Medication>()).Returns(Task.FromResult(medicationDto));

            // Act
            var result = await _controller.Create(medicationDto) as CreatedAtActionResult;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result!.StatusCode, Is.EqualTo(201));
                Assert.That(result.Value, Is.EqualTo(medicationDto));
            });
        }

        /// <summary>
        ///     Given a new <see cref="MedicationDTO"/> with invalid quantity<br />
        ///     When <see cref="MedicationsController.Create(MedicationDTO)" /> is called<br />
        ///     Then a <see cref="BadRequestObjectResult" /> is returned.
        /// </summary>
        /// <returns>The <see cref="Task"/> of the operation.</returns>
        [TestCase(0)]
        [TestCase(-1)]
        [Category("POST")]
        public async Task GivenANewMedicationWithInvalidQuantityWhenCreateIsCalledThenABadRequestIsReturned(int quantity)
        {
            // Arrange
            var medicationDto = new MedicationDTO { Id = 1, Name = "Ben-u-ron", Quantity = quantity };
            // Act
            var result = await _controller.Create(medicationDto);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        /// <summary>
        ///     Given an existing <see cref="MedicationDTO"/><br />
        ///     When <see cref="MedicationsController.Delete(int)" /> is called<br />
        ///     Then a <see cref="NoContentResult" /> is returned.
        /// </summary>
        /// <returns>The <see cref="Task"/> of the operation.</returns>
        [Test]
        [Category("DELETE")]
        public async Task GivenAnExistingMedicationWhenDeleteIsCalledThenANoContentResultIsReturned()
        {
            // Arrange
            var medicationId = 1;

            _medicationsService.DeleteMedicationAsync(medicationId).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(medicationId);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        /// <summary>
        ///     Given a nonexisting <see cref="MedicationDTO"/><br />
        ///     When <see cref="MedicationsController.Delete(int)" /> is called<br />
        ///     Then a <see cref="NotFoundObjectResult" /> is returned.
        /// </summary>
        /// <returns>The <see cref="Task"/> of the operation.</returns>
        [Test]
        [Category("DELETE")]
        public async Task GivenANonExistingMedicationWhenDeleteIsCalledThenANotFoundResultIsReturned()
        {
            // Arrange
            var medicationId = 1;
            _medicationsService.DeleteMedicationAsync(medicationId).Throws(new KeyNotFoundException());

            // Act
            var result = await _controller.Delete(medicationId);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        /// <summary>
        ///     Given a list of existing <see cref="MedicationDTO"/><br />
        ///     When <see cref="MedicationsController.GetAll()" /> is called<br />
        ///     Then an <see cref="IActionResult"/> is returned<br />
        ///     And the status code is 200<br />
        ///     And the returned object matches the list of <see cref="MedicationDTO" />.
        /// </summary>
        /// <returns>The <see cref="Task"/> of the operation.</returns>
        [Test]
        [Category("GET")]
        public async Task GivenAListOfExistingMedicationsWhenGetAllIsCalledThenAnActionIsReturnedAndTheStatusCodeIs200AndTheReturnedObjectMatchesTheListOfMedications()
        {
            // Arrange
            var medicationsDto = new List<MedicationDTO>
            {
                new MedicationDTO { Id = 1, Name = "Ben-u-ron" },
                new MedicationDTO { Id = 2, Name = "Gestacare" }
            };

            _medicationsService.GetAllMedicationsAsync().Returns(medicationsDto);

            // Act
            var result = await _controller.GetAll() as OkObjectResult;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result!.StatusCode, Is.EqualTo(200));
                Assert.That(result.Value, Is.EqualTo(medicationsDto));
            });
        }

        /// <summary>
        ///     Given an existing <see cref="MedicationDTO"/><br />
        ///     When <see cref="MedicationsController.GetById(int)" /> is called<br />
        ///     Then an <see cref="IActionResult"/> is returned<br />
        ///     And the status code is 200<br />
        ///     And the returned object matches the existing <see cref="MedicationDTO"/>.
        /// </summary>
        /// <returns>The <see cref="Task"/> of the operation.</returns>
        [Test]
        [Category("GET")]
        public async Task GivenAnExistingMedicationWhenGetByIdIsCalledThenAnActionIsReturnedAndTheStatusCodeIs200AndTheReturnedObjectMatchesTheExistingMedication()
        {
            // Arrange
            var medicationDto = new MedicationDTO { Id = 1, Name = "Ben-u-ron" };

            _medicationsService.GetMedicationByIdAsync(1).Returns(medicationDto);

            // Act
            var result = await _controller.GetById(1) as OkObjectResult;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result!.StatusCode, Is.EqualTo(200));
                Assert.That(result.Value, Is.EqualTo(medicationDto));
            });
        }

        /// <summary>
        ///     Given a nonexisting <see cref="MedicationDTO"/><br />
        ///     When <see cref="MedicationsController.GetById(int)" /> is called<br />
        ///     Then a <see cref="NotFoundObjectResult" /> is returned.
        /// </summary>
        /// <returns>The <see cref="Task"/> of the operation.</returns>
        [Test]
        [Category("GET")]
        public async Task GivenANonExistingMedicationWhenGetByIdIsCalledThenANotFoundResultIsReturned()
        {
            // Arrange
            _medicationsService.GetMedicationByIdAsync(1).Throws(new KeyNotFoundException());

            // Act
            var result = await _controller.GetById(1);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        /// <summary>
        ///     Given an existing <see cref="MedicationDTO"/><br />
        ///     When <see cref="MedicationsController.Update(MedicationDTO)" /> is called<br />
        ///     Then a <see cref="NoContentResult" /> is returned.
        /// </summary>
        /// <returns>The <see cref="Task"/> of the operation.</returns>
        [Test]
        [Category("PUT")]
        public async Task GivenAnExistingMedicationWhenUpdateIsCalledThenANoContentResultIsReturned()
        {
            // Arrange
            var medicationDto = new MedicationDTO { Id = 1, Name = "Ben-u-ron2", Quantity = 7 };

            _medicationsService.UpdateMedicationAsync(Arg.Any<Medication>()).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(medicationDto) as NoContentResult;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result!.StatusCode, Is.EqualTo(204));
            });
        }

        /// <summary>
        ///     Given a nonexisting <see cref="MedicationDTO"/><br />
        ///     When <see cref="MedicationsController.Update(MedicationDTO)" /> is called<br />
        ///     Then a <see cref="NotFoundObjectResult" /> is returned.
        /// </summary>
        /// <returns>The <see cref="Task"/> of the operation.</returns>
        [Test]
        [Category("PUT")]
        public async Task GivenANonExistingMedicationWhenUpdateIsCalledThenANotFoundResultIsReturned()
        {
            // Arrange
            var medicationDto = new MedicationDTO { Id = 1, Name = "Ben-u-ron2", Quantity = 5 };

            _medicationsService.UpdateMedicationAsync(Arg.Is<Medication>(m => m.Id == medicationDto.Id &&
                                                                              m.Name == medicationDto.Name &&
                                                                              m.Quantity == medicationDto.Quantity)).Throws(new KeyNotFoundException());

            // Act
            var result = await _controller.Update(medicationDto);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        /// <summary>
        ///     Given an existing <see cref="MedicationDTO"/><br />
        ///     And its quantity is changed to an invalid value
        ///     When <see cref="MedicationsController.Update(MedicationDTO)" /> is called<br />
        ///     Then a <see cref="BadRequestObjectResult" /> is returned.
        /// </summary>
        /// <returns>The <see cref="Task"/> of the operation.</returns>
        [TestCase(0)]
        [TestCase(-1)]
        [Category("PUT")]
        public async Task GivenAnExistingMedicationAndItsQuantityIsChangedToAnInvalidValueWhenUpdateIsCalledThenABadRequestIsReturned(int quantity)
        {
            // Arrange
            var medicationDto = new MedicationDTO { Id = 1, Name = "Ben-u-ron2", Quantity = quantity };

            // Act
            var result = await _controller.Update(medicationDto);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }
    }
}
