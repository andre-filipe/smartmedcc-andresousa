using SmartMedCC_AndreSousa.Models;
using SmartMedCC_AndreSousa.Repositories;
using SmartMedCC_AndreSousa.DTOs;

namespace SmartMedCC_AndreSousa.Services
{
    /// <summary>
    ///     The <see cref="Medication"/> service.
    /// </summary>
    public class MedicationsService : IMedicationsService
    {
        private readonly IMedicationsRepository _medicationsRepository;

        /// <summary>
        ///     The <see cref="MedicationsRepository"/> constructor.
        /// </summary>
        /// <param name="medicationsRepository">The repository to be used to interact with <see cref="Medication"/> information.</param>
        public MedicationsService(IMedicationsRepository medicationsRepository)
        {
            _medicationsRepository = medicationsRepository;
        }

        /// <inheritdoc />
        public async Task<MedicationDTO> AddMedicationAsync(Medication medication)
        {
            var newMedication = await _medicationsRepository.AddAsync(medication);

            return new MedicationDTO
            {
                Id = newMedication.Id,
                Name = newMedication.Name,
                Quantity = newMedication.Quantity,
                Timestamp = medication.Timestamp
            };
        }

        /// <inheritdoc />
        public async Task DeleteMedicationAsync(int id)
        {
            var medicationToBeDeleted = await _medicationsRepository.GetByIdAsync(id);

            if (medicationToBeDeleted == null)
            {
                throw new KeyNotFoundException($"Medication with id: {id} not found.");
            }

            await _medicationsRepository.DeleteAsync(medicationToBeDeleted);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<MedicationDTO>> GetAllMedicationsAsync()
        {
            var medications = await _medicationsRepository.GetAllAsync();

            return medications.Select(medication => new MedicationDTO
            {
                Id = medication.Id,
                Name = medication.Name,
                Quantity = medication.Quantity,
                Timestamp = medication.Timestamp
            }).ToList();
        }

        /// <inheritdoc />
        public async Task<MedicationDTO?> GetMedicationByIdAsync(int id)
        {
            var medication = await _medicationsRepository.GetByIdAsync(id);

            if (medication == null)
            {
                throw new KeyNotFoundException($"Medication with id: {id} not found.");
            }

            return new MedicationDTO
            {
                Id = medication.Id,
                Name = medication.Name,
                Quantity = medication.Quantity, 
                Timestamp = medication.Timestamp
            };
        }

        /// <inheritdoc />
        public async Task UpdateMedicationAsync(Medication updatedMedication)
        {
            var currentMedication = await _medicationsRepository.GetByIdAsync(updatedMedication.Id);

            if (currentMedication == null)
            {
                throw new KeyNotFoundException($"Medication with id: {updatedMedication.Id} not found.");
            }

            currentMedication.Name = updatedMedication.Name;
            currentMedication.Quantity = updatedMedication.Quantity;

            await _medicationsRepository.UpdateAsync(currentMedication);
        }
    }
}
