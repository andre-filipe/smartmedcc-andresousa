using SmartMedCC_AndreSousa.Models;
using System.Xml.Linq;

namespace SmartMedCC_AndreSousa.DTOs
{
    /// <summary>
    ///     Data Transfer Object for the <see cref="Medication"/> entity.
    /// </summary>
    public class MedicationDTO
    {
        /// <summary>
        ///     The medication id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     The medication name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     The medication quantity.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        ///     The medication creation date.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is MedicationDTO dto &&
                   Id == dto.Id &&
                   Name == dto.Name &&
                   Quantity == dto.Quantity &&
                   Timestamp == dto.Timestamp;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Quantity, Timestamp);
        }
    }
}
