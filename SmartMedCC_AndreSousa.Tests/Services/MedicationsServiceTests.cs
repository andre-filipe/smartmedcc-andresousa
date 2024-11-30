using NSubstitute;

using SmartMedCC_AndreSousa.Models;
using SmartMedCC_AndreSousa.Repositories;
using SmartMedCC_AndreSousa.Services;

namespace SmartMedCC_AndreSousa.Tests.Services
{
    /// <summary>
    ///     This class contains unit tests for <see cref="MedicationsService"/>.
    /// </summary>
    [TestFixture]
    public class MedicationsServiceTests
    {
        private IMedicationsRepository _medicationsRepository;
        private MedicationsService _medicationsService;

        [SetUp]
        public void SetUp()
        {
            _medicationsRepository = Substitute.For<IMedicationsRepository>();
            _medicationsService = new MedicationsService(_medicationsRepository);
        }

        /// <summary>
        ///     Given a new <see cref="Medication"/>
        ///     When <see cref="MedicationsService.AddMedicationAsync(Medication)"/> is called
        ///     Then the <see cref="MedicationsRepository.AddAsync(Medication)"/> is called
        ///     And the new <see cref="Medication "/> is returned.
        /// </summary>
        /// <returns>The <see cref="Task"/> of the operation.</returns>
        [Test]
        public async Task GivenANewMedicationWhenAddMedicationAsyncIsCalledThenTheRepositoryCallsAnAddAsyncAndTheNewMedicationIsReturned()
        {
            // Arrange
            var newMedication = new Medication(1, "Ben-u-ron", 5);

            _medicationsRepository.AddAsync(Arg.Any<Medication>()).Returns(newMedication);

            // Act
            var result = await _medicationsService.AddMedicationAsync(newMedication);

            // Assert
            Assert.Multiple(async () =>
            {
                await _medicationsRepository.Received(1).AddAsync(newMedication);

                Assert.That(result, Is.Not.Null);
                Assert.That(result!.Id, Is.EqualTo(newMedication.Id));
                Assert.That(result.Name, Is.EqualTo(newMedication.Name));
                Assert.That(result.Quantity, Is.EqualTo(newMedication.Quantity));
            });
        }

        /// <summary>
        ///     Given an existing <see cref="Medication"/>
        ///     When <see cref="MedicationsService.DeleteMedicationAsync(int)"/> is called
        ///     Then the <see cref="MedicationsRepository.DeleteAsync(Medication)"/> is called
        /// </summary>
        /// <returns>The <see cref="Task"/> of the operation.</returns>
        [Test]
        public async Task GivenAnExistingMedicationWhenDeleteMedicationAsyncIsCalledThenTheRepositoryCallsDeleteAsync()
        {
            // Arrange
            var medication = new Medication(1, "Ben-u-ron", 5);

            _medicationsRepository.GetByIdAsync(1).Returns(medication);

            // Act
            await _medicationsService.DeleteMedicationAsync(1);

            // Assert
            await _medicationsRepository.Received(1).DeleteAsync(Arg.Is<Medication>(m => m.Id == medication.Id &&
                                                                                         m.Name == medication.Name &&
                                                                                         m.Quantity == medication.Quantity &&
                                                                                         m.Timestamp == medication.Timestamp));
        }

        /// <summary>
        ///     Given a nonexisting <see cref="Medication"/>
        ///     When <see cref="MedicationsService.DeleteMedicationAsync(int)"/> is called
        ///     Then the <see cref="MedicationsRepository.DeleteAsync(Medication)"/> is not called
        ///     And an <see cref="KeyNotFoundException"/> is thrown
        /// </summary>
        [Test]
        public void GivenAnExistingMedicationWhenDeleteMedicationAsyncIsCalledThenTheRepositoryDoesntCallDeleteAsyncAndAKeyNotFoundExceptionIsThrown()
        {
            // Arrange
            _medicationsRepository.GetByIdAsync(1).Returns((Medication)null);

            // Act and Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(() => _medicationsService.DeleteMedicationAsync(1));

            Assert.Multiple(async () =>
            {
                await _medicationsRepository.DidNotReceive().DeleteAsync(Arg.Any<Medication>());

                Assert.That(ex.Message, Is.EqualTo("Medication with id: 1 not found."));
            });
        }

        /// <summary>
        ///     Given an existing list of <see cref="Medication"/><br />
        ///     When <see cref="MedicationsService.GetAllMedicationsAsync()"/> is called<br />
        ///     Then the <see cref="MedicationsRepository.GetAllAsync()"/> is called<br />
        ///     And the returned object matches the list of <see cref="Medication" />.
        /// </summary>
        /// <returns>The <see cref="Task"/> of the operation.</returns>
        [Test]
        public async Task GivenAnExistingListOfMedicationsWhenGetAllMEdicationsAsyncIsCalledThenTheRepositoryCallsGetAllAsyncAndTheReturnedObjectMatchesTheListOfMedication()
        {
            // Arrange
            var medications = new List<Medication>
            {
                new(1, "Ben-u-ron", 3),
                new(2, "Gestacare", 1)
            };

            _medicationsRepository.GetAllAsync().Returns(medications);

            // Act
            var result = await _medicationsService.GetAllMedicationsAsync();

            // Assert
            Assert.Multiple(async () =>
            {
                await _medicationsRepository.Received(1).GetAllAsync();

                var resultValues = result.ToList();

                Assert.That(result.ToList().Count, Is.EqualTo(medications.Count));

                for (int i = 0; i < medications.Count; i++)
                {
                    Assert.That(resultValues[i].Id, Is.EqualTo(medications[i].Id));
                    Assert.That(resultValues[i].Name, Is.EqualTo(medications[i].Name));
                    Assert.That(resultValues[i].Quantity, Is.EqualTo(medications[i].Quantity));
                }
            });
        }

        /// <summary>
        ///     Given an existing <see cref="Medication"/><br />
        ///     When <see cref="MedicationsService.GetMedicationByIdAsync(int)"/> is called<br />
        ///     Then the <see cref="MedicationsRepository.GetByIdAsync(int)"/> is called<br />
        ///     And the returned object matches the existing <see cref="Medication">
        /// </summary>
        /// <returns>The <see cref="Task"/> of the operation.</returns>
        [Test]
        public async Task GivenAnExistingMedicationWhenGetMedicationByIdAsyncIsCalledThenTheRepositoryCallsGetByIdAsyncAndTheReturnedObjectMatchesTheExistingMedication()
        {
            // Arrange
            var medication = new Medication(1, "Ben-u-ron", 5);

            _medicationsRepository.GetByIdAsync(1).Returns(medication);

            // Act
            var result = await _medicationsService.GetMedicationByIdAsync(1);

            // Assert
            Assert.Multiple(async () =>
            {
                await _medicationsRepository.Received(1).GetByIdAsync(1);

                Assert.That(result, Is.Not.Null);
                Assert.That(result!.Id, Is.EqualTo(medication.Id));
                Assert.That(result.Name, Is.EqualTo(medication.Name));
                Assert.That(result.Quantity, Is.EqualTo(medication.Quantity));
            });
        }

        /// <summary>
        ///     Given a nonexisting <see cref="Medication"/><br />
        ///     When <see cref="MedicationsService.GetMedicationByIdAsync(int)"/> is called<br />
        ///     Then a <see cref="KeyNotFoundException"/> is thrown.
        /// </summary>
        [Test]
        public void GivenANonExistingMedicationWhenGetMedicationByIdAsyncIsCalledThenAKeyNotFoundExceptionIsThrown()
        {
            // Arrange
            _medicationsRepository.GetByIdAsync(1).Returns((Medication)null);

            // Act and Assert
            Assert.Multiple(() =>
            {
                var ex = Assert.ThrowsAsync<KeyNotFoundException>(() => _medicationsService.GetMedicationByIdAsync(1));

                Assert.That(ex.Message, Is.EqualTo("Medication with id: 1 not found."));
            });
        }

        /// <summary>
        ///     Given an existing <see cref="Medication"/>
        ///     When <see cref="MedicationsService.UpdateMedicationAsync(Medication)"/> is called<br />
        ///     And a new name is provided<br />
        ///     Then <see cref="MedicationsRepository.UpdateAsync(Medication)"/> is called
        ///     And the <see cref="Medication"/> is updated.
        /// </summary>
        /// <returns>The <see cref="Task"/> of the operation.</returns>
        [Test]
        public async Task GivenAnExistingMedicationWhenUpdateMedicationAsyncIsCalledAndANewNameIsProvidedThenTheRepositoryCallsUpdateAsyncAndTheMedicationIsUpdated()
        {
            // Arrange
            var currentMedication = new Medication(1, "Ben-u-ron", 5);
            var updatedMedication = new Medication(1, "Ben-u-ron2", 2);

            _medicationsRepository.GetByIdAsync(1).Returns(currentMedication);

            // Act
            await _medicationsService.UpdateMedicationAsync(updatedMedication);

            // Assert
            await _medicationsRepository.Received(1).UpdateAsync(Arg.Is<Medication>(medication =>
                medication.Id == updatedMedication.Id &&
                medication.Name == updatedMedication.Name &&
                medication.Quantity == updatedMedication.Quantity
            ));
        }

        /// <summary>
        ///     Given an existing <see cref="Medication"/>
        ///     When <see cref="MedicationsService.UpdateMedicationAsync(Medication)"/> is called<br />
        ///     Then <see cref="MedicationsRepository.UpdateAsync(Medication)"/> is not called
        ///     And a <see cref="KeyNotFoundException"/> is thrown.
        /// </summary>
        [Test]
        public void GivenANonExistingMedicationWhenUpdateMedicationAsyncIsCalledThenTheRepositoryDoesntCallUpdateAsyncAndAKeyNotFoundExceptionIsThrown()
        {
            // Arrange
            var updatedMedication = new Medication(1, string.Empty, 1);
            _medicationsRepository.GetByIdAsync(1).Returns((Medication)null);

            // Act and Assert
            Assert.Multiple(async () =>
            {
                var ex = Assert.ThrowsAsync<KeyNotFoundException>(() => _medicationsService.UpdateMedicationAsync(updatedMedication));

                await _medicationsRepository.DidNotReceive().UpdateAsync(Arg.Any<Medication>());

                Assert.That(ex.Message, Is.EqualTo("Medication with id: 1 not found."));
            });
        }
    }
}