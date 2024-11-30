using Microsoft.EntityFrameworkCore;

using SmartMedCC_AndreSousa.Database;
using SmartMedCC_AndreSousa.Models;

namespace SmartMedCC_AndreSousa.Repositories
{
    /// <summary>
    ///     The <see cref="Medication"/> repository.
    /// </summary>
    public class MedicationsRepository : IMedicationsRepository
    {
        private readonly SmartMedCCDbContext _context;

        /// <summary>
        ///     The <see cref="MedicationsRepository"/> constructor.
        /// </summary>
        /// <param name="context">The <see cref="SmartMedCCDbContext"/> to be used throughout the database operations.</param>
        public MedicationsRepository(SmartMedCCDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<Medication> AddAsync(Medication medication)
        {
            _context.Medications.Add(medication);
            await _context.SaveChangesAsync();

            return medication;
        }

        /// <inheritdoc />
        public async Task DeleteAsync(Medication medicationToBeDeleted)
        {
            _context.Medications.Remove(medicationToBeDeleted);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Medication>> GetAllAsync() => await _context.Medications.ToListAsync();

        /// <inheritdoc />
        public async Task<Medication?> GetByIdAsync(int id) => await _context.Medications.FindAsync(id);

        /// <inheritdoc />
        public async Task UpdateAsync(Medication updatedMedication)
        {
            _context.Medications.Update(updatedMedication);
            await _context.SaveChangesAsync();
        }
    }
}
