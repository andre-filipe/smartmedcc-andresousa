using System.ComponentModel.DataAnnotations;

namespace SmartMedCC_AndreSousa.Models
{
    /// <summary>
    ///     A regular medication.
    /// </summary>
    public class Medication
    {
        /// <summary>
        ///     Initializes an instance of <see cref="Medication"/>.
        /// </summary>
        /// <param name="name">The name of the medication.</param>
        /// <param name="quantity">The quantity of the medication.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public Medication(string name, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero");

            Name = name;
            Quantity = quantity;
        }

        /// <summary>
        /// Initializes an instance of <see cref="Medication"/> for an existing entity (with Id).
        /// </summary>
        /// <param name="id">The id of the medication.</param>
        /// <param name="name">The name of the medication.</param>
        /// <param name="quantity">The quantity of the medication.</param>
        public Medication(int id, string name, int quantity) : this(name, quantity)
        {
            Id = id;
        }

        /// <summary>
        ///     The medication Id.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        ///     The medication name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     The medication quantity.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        ///     The creation date/time.
        /// </summary>
        public DateTime Timestamp { get; private set; }
    }
}
