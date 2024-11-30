using SmartMedCC_AndreSousa.Models;

namespace SmartMedCC_AndreSousa.Repositories
{
    /// <summary>
    ///     The <see cref="Medication"/> repository interface.
    /// </summary>
    public interface IMedicationsRepository
    {
        /// <summary>
        ///     Adds a medication to the database.
        /// </summary>
        /// <param name="medication">The medication to be added.</param>
        /// <returns>A <see cref="Task"/> of the add operation, returning the brand new <see cref="Medication"/>.</returns>
        Task<Medication> AddAsync(Medication medication);

        /// <summary>
        ///     Deletes a medication from the database.
        /// </summary>
        /// <param name="medicationToBeDeleted">The <see cref="Medication"/> about to be deleted.</param>
        /// <returns>A <see cref="Task"/> of the delete operation.</returns>
        Task DeleteAsync(Medication medicationToBeDeleted);

        /// <summary>
        ///     Gets all the <see cref="Medication" /> available on the database.
        /// </summary>
        /// <returns>A <see cref="Task"/> of the get operation, returning all available <see cref="Medication"/>.</returns>
        Task<IEnumerable<Medication>> GetAllAsync();

        /// <summary>
        ///     Gets a <see cref="Medication" /> from the database, based on a given <see cref="Medication.Id"/>.
        /// </summary>
        /// <param name="id">The id of the <see cref="Medication"/> to be retrieved.</param>
        /// <returns>A <see cref="Task"/> of the get operation, returning the <see cref="Medication"/>. If no <see cref="Medication"/> is found, returns <c>null</c>.</returns>
        Task<Medication?> GetByIdAsync(int id);


        /// <summary>
        ///     Updates the information of a given <see cref="Medication"/> in the database.
        /// </summary>
        /// <param name="updatedMedication">The updated <see cref="Medication"/>.</param>
        /// <returns>A <see cref="Task"/> of the update operation.</returns>
        Task UpdateAsync(Medication updatedMedication);
    }
}
