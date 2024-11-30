using Microsoft.EntityFrameworkCore;

using SmartMedCC_AndreSousa.Database;
using SmartMedCC_AndreSousa.Models;
using SmartMedCC_AndreSousa.Repositories;

namespace SmartMedCC_AndreSousa.Tests.Repositories
{
    /// <summary>
    ///     This class contains unit tests for <see cref="MedicationsRepository"/>.
    /// </summary>
    [TestFixture]
    public class MedicationsRepositoryTests
    {
        private SmartMedCCDbContext _dbContext;
        private MedicationsRepository _repository;

        [SetUp]
        public void Setup()
        {
            var dbOptions = new DbContextOptionsBuilder<SmartMedCCDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _dbContext = new SmartMedCCDbContext(dbOptions);
            _repository = new MedicationsRepository(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Dispose();
        }

        /// <summary>
        ///     Given a new <see cref="Medication"/><br />
        ///     When <see cref="MedicationsRepository.AddAsync(Medication)" /> is called<br />
        ///     Then the <see cref="Medication"/> is saved on the database.
        /// </summary>
        /// <returns>The <see cref="Task"/> of the operation.</returns>
        [Test]
        public async Task GivenANewMedicationWhenAddAsyncIsCalledThenTheMedicationIsSavedOnTheDatabase()
        {
            // Arrange
            var medication = new Medication(1, "Ben-u-ron", 31);

            // Act
            var result = await _repository.AddAsync(medication);

            // Assert
            var persistedMedication = _dbContext.Medications.FirstOrDefault(m => m.Id == result.Id);

            Assert.Multiple(() =>
            {
                Assert.That(persistedMedication, Is.Not.Null);
                Assert.That(persistedMedication!.Name, Is.EqualTo("Ben-u-ron"));
                Assert.That(persistedMedication.Quantity, Is.EqualTo(31));
            });
        }

        /// <summary>
        ///     Given a previously added <see cref="Medication"/><br />
        ///     When <see cref="MedicationsRepository.DeleteAsync(Medication)" /> is called<br />
        ///     Then the <see cref="Medication"/> can't be retrieved from the database anymore.
        /// </summary>
        /// <returns>The <see cref="Task"/> of the operation.</returns>
        [Test]
        public async Task GivenAPreviouslyAddedMedicationWhenDeleteAsyncIsCalledThenTheMedicationCantBeRetrievedFromTheDatabaseAnymore()
        {
            // Arrange
            var medication = new Medication(1, "Ben-u-ron", 1);

            _dbContext.Medications.Add(medication);

            await _dbContext.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(medication);

            // Assert
            var persistedMedication = _dbContext.Medications.FirstOrDefault(m => m.Id == medication.Id);

            Assert.That(persistedMedication, Is.Null);
        }

        /// <summary>
        ///     Given a previously added <see cref="Medication"/><br />
        ///     When <see cref="MedicationsRepository.GetByIdAsync(int)" /> is called<br />
        ///     Then the object returned from the database matches the previously added <see cref="Medication"/>.
        /// </summary>
        /// <returns>The <see cref="Task"/> of the operation.</returns>
        [Test]
        public async Task GivenAPreviouslyAddedMedicationWhenGetByIdAsyncIsCalledThenTheObjectReturnedFromTheDatabaseMatchesThePreviouslyAddedMedication()
        {
            // Arrange
            var medication = new Medication(1, "Ben-u-ron", 4);

            _dbContext.Medications.Add(medication);

            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(medication.Id);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result!.Id, Is.EqualTo(medication.Id));
                Assert.That(result.Name, Is.EqualTo(medication.Name));
                Assert.That(result.Quantity, Is.EqualTo(medication.Quantity));
            });
        }

        /// <summary>
        ///     Given no items have been added to the database yet
        ///     When <see cref="MedicationsRepository.GetByIdAsync(int)" /> is called<br />
        ///     Then no <see cref="Medication"/> is returned.
        /// </summary>
        /// <returns>The <see cref="Task"/> of the operation.</returns>
        [Test]
        public async Task GivenNoItemsHaveBeenAddedToTheDatabaseYetWhenGetByIdAsyncIsCalledThenNoMedicationIsReturned()
        {
            // Act
            var result = await _repository.GetByIdAsync(999);

            // Assert
            Assert.That(result, Is.Null);
        }

        /// <summary>
        ///     Given some <see cref="Medication"/> have been previously added<br />
        ///     When <see cref="MedicationsRepository.GetAllAsync()" /> is called<br />
        ///     Then the objects returned from the database matches the previously added <see cref="Medication"/>.
        /// </summary>
        /// <returns>The <see cref="Task"/> of the operation.</returns>
        [Test]
        public async Task GivenSomeMedicationsHaveBeenPreviouslyAddedWhenGetAllAsyncIsCalledThenTheObjectsReturnedFromTheDatabaseMatchesThePreviouslyAddedMedications()
        {
            // Arrange
            var medications = new List<Medication>
        {
            new(1, "Ben-u-ron", 1),
            new(2, "Gestacare", 4)
        };

            _dbContext.Medications.AddRange(medications);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count(), Is.EqualTo(2));
                Assert.That(result.Any(m => m.Name == "Ben-u-ron" && m.Quantity == 1), Is.True);
                Assert.That(result.Any(m => m.Name == "Gestacare" && m.Quantity == 4), Is.True);
            });
        }

        /// <summary>
        ///     Given a previously added <see cref="Medication"/><br />
        ///     And the name of <see cref="Medication"/> is changed<br />
        ///     When <see cref="MedicationsRepository.UpdateAsync(Medication)" /> is called<br />
        ///     Then the object returned from the database matches the previously updated <see cref="Medication"/>.
        /// </summary>
        /// <returns>The <see cref="Task"/> of the operation.</returns>
        [Test]
        public async Task GivenAPreviouslyAddedMedicationAndTheNameOfMedicationIsChangedWhenUpdateAsyncIsCalledThenTheObjectReturnedFromtheDatabaseMatchesThePreviouslyUpdatedMedication()
        {
            // Arrange
            var medication = new Medication(1, "Ben-u-ron", 4);

            _dbContext.Medications.Add(medication);

            await _dbContext.SaveChangesAsync();

            medication.Name = "Ben-u-ron2";

            // Act
            await _repository.UpdateAsync(medication);

            // Assert
            var updatedMedication = _dbContext.Medications.FirstOrDefault(m => m.Id == medication.Id);

            Assert.Multiple(() =>
            {
                Assert.That(updatedMedication, Is.Not.Null);
                Assert.That(updatedMedication!.Name, Is.EqualTo("Ben-u-ron2"));
                Assert.That(updatedMedication.Quantity, Is.EqualTo(4));
            });
        }
    }
}