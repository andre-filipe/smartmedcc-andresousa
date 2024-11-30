using SmartMedCC_AndreSousa.Models;
using SmartMedCC_AndreSousa.DTOs;

namespace SmartMedCC_AndreSousa.Services
{
    /// <summary>
    ///     The <see cref="Medication"/> service interface.
    /// </summary>
    public interface IMedicationsService
    {
        /// <summary>
        ///     Adds a medication to the system.
        /// </summary>
        /// <param name="medication">The medication to be added.</param>
        /// <returns>A <see cref="Task"/> of the add operation, returning the brand new <see cref="MedicationDTO"/>.</returns>
        Task<MedicationDTO> AddMedicationAsync(Medication medication);

        /// <summary>
        ///     Deletes a medication from the system.
        /// </summary>
        /// <param name="id">The <see cref="Medication.Id"/> of the <see cref="Medication"/> about to be deleted.</param>
        /// <returns>A <see cref="Task"/> of the delete operation.</returns>
        Task DeleteMedicationAsync(int id);

        /// <summary>
        ///     Gets all the <see cref="MedicationDTO" /> available on the system.
        /// </summary>
        /// <returns>A <see cref="Task"/> of the get operation, returning all available <see cref="MedicationDTO"/>.</returns>
        Task<IEnumerable<MedicationDTO>> GetAllMedicationsAsync();

        /// <summary>
        ///     Gets a <see cref="MedicationDTO" /> from the system, based on a given <see cref="Medication.Id"/>.
        /// </summary>
        /// <param name="id">The <see cref="Medication.Id"/> of the <see cref="Medication"/> about to be searched.</param>
        /// <returns>A <see cref="Task"/> of the get operation, returning the <see cref="MedicationDTO"/>. If no <see cref="Medication"/> is found, returns <c>null</c>.</returns>
        Task<MedicationDTO?> GetMedicationByIdAsync(int id);

        /// <summary>
        ///     Updates the information of a given <see cref="Medication"/>.
        /// </summary>
        /// <returns>A <see cref="Task"/> of the update operation.</returns>
        Task UpdateMedicationAsync(Medication updatedMedication);
    }
}
