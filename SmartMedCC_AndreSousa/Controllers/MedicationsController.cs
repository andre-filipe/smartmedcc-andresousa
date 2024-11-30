using Microsoft.AspNetCore.Mvc;

using SmartMedCC_AndreSousa.Models;
using SmartMedCC_AndreSousa.DTOs;
using SmartMedCC_AndreSousa.Services;

namespace SmartMedCC_AndreSousa.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MedicationsController : ControllerBase
    {
        private readonly IMedicationsService _medicationsService;
        private readonly ILogger<MedicationsController> _logger;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MedicationsController" /> class.
        /// </summary>
        /// <param name="medicationsService">
        ///     A service containing the logic for medications.
        /// </param>
        /// <param name="logger">
        ///     The <see cref="ILogger" /> used to register <see cref="MedicationsController" />-related occurrences.
        /// </param>
        public MedicationsController(IMedicationsService medicationsService, ILogger<MedicationsController> logger)
        {
            _medicationsService = medicationsService;
            _logger = logger;
        }

        /// <summary>
        ///     Creates a new <see cref="Medication" />.
        /// </summary>
        /// <param name="medicationDto">The <see cref="MedicationDTO" /> to be created.</param>
        /// <returns>
        ///     An <see cref="IActionResult"/> containing the brand new <see cref="MedicationDTO" />.
        /// </returns>
        /// <response code="201">Successful creation (returns the brand new <see cref="MedicationDTO"/>).</response>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MedicationDTO medicationDto)
        {
            try
            {
                var medication = new Medication(medicationDto.Name, medicationDto.Quantity);
                var newMedication = await _medicationsService.AddMedicationAsync(medication);

                var newMedicationDto = new MedicationDTO
                {
                    Id = newMedication.Id,
                    Name = newMedication.Name,
                    Quantity = newMedication.Quantity,
                    Timestamp = newMedication.Timestamp
                };

                _logger.LogInformation("CREATE -> New medication {MedicationName} (id: {Id}) created successfully", medication.Name, medication.Id);

                return CreatedAtAction(nameof(GetById), new { id = newMedicationDto.Id }, newMedicationDto);
            }
            catch (ArgumentException)
            {
                return BadRequest("The quantity should be higher than zero.");
            }
        }

        /// <summary>
        ///     Deletes a <see cref="Medication" />.
        /// </summary>
        /// <param name="id">The id of the <see cref="Medication"/> about to be deleted.</param>
        /// <returns>
        ///     An <see cref="IActionResult"/> with no content, in case of successful deletion.
        /// </returns>
        /// <response code="204">Successful request (the <see cref="Medication" /> has been deleted).</response>
        /// <response code="404">No <see cref="Medication" /> was found with the assigned id.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _medicationsService.DeleteMedicationAsync(id);

                _logger.LogInformation("DELETE -> Medication with id: {Id} has been deleted successfully", id);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning("Medication with id: {Id} has been requested but could not be found", id);

                return NotFound($"The medication with id: {id} could not be found on the database.");
            }
        }

        /// <summary>
        ///     Gets all available <see cref="MedicationDTO" />.
        /// </summary>
        /// <returns>
        ///     An <see cref="IActionResult"/> containing a list of <see cref="MedicationDTO" />.
        /// </returns>
        /// <response code="200">Successful request (returns a list of <see cref="MedicationDTO" />).</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var medications = await _medicationsService.GetAllMedicationsAsync();
            var medicationDtos = medications.Select(m => new MedicationDTO
            {
                Id = m.Id,
                Name = m.Name,
                Quantity = m.Quantity,
                Timestamp = m.Timestamp
            }).ToList();

            return Ok(medicationDtos);
        }

        /// <summary>
        ///     Gets a <see cref="MedicationDTO" /> based on a given id.
        /// </summary>
        /// <param name="id">The id of the <see cref="MedicationDTO" /> to be retrieved.</param>
        /// <returns>
        ///     An <see cref="IActionResult" /> containing the requested <see cref="MedicationDTO" />.
        /// </returns>
        /// <response code="200">Successful request (returns the <see cref="MedicationDTO" />).</response>
        /// <response code="404">No <see cref="MedicationDTO" /> was found with the specified id.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var medication = await _medicationsService.GetMedicationByIdAsync(id);

                var medicationDto = new MedicationDTO
                {
                    Id = medication!.Id,
                    Name = medication.Name,
                    Quantity = medication.Quantity,
                    Timestamp = medication.Timestamp
                };

                return Ok(medicationDto);
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning("Medication with id: {Id} has been requested but could not be found", id);

                return NotFound($"The medication with id: {id} could not be found on the database.");
            }
        }

        /// <summary>
        ///     Updates an existing <see cref="Medication" />.
        /// </summary>
        /// <param name="medicationDto">The <see cref="MedicationDTO" /> with the new values.</param>
        /// <returns>
        ///     An <see cref="IActionResult"/> with the updated <see cref="MedicationDTO" />, in case of successful update.
        /// </returns>
        /// <response code="204">Successful request (the <see cref="MedicationDTO" /> has been updated).</response>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] MedicationDTO medicationDto)
        {
            try
            {
                var medication = new Medication(medicationDto.Id, medicationDto.Name, medicationDto.Quantity);

                await _medicationsService.UpdateMedicationAsync(medication);

                _logger.LogInformation("UPDATE -> Medication {Medication} with id: {Id} has been updated successfully", medication.Name, medication.Id);

                return NoContent();
            }
            catch (ArgumentException)
            {
                return BadRequest("The quantity should be higher than zero.");
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning("Medication with id: {Id} has been requested but could not be found", medicationDto.Id);

                return NotFound($"The medication with id: {medicationDto.Id} could not be found on the database.");
            }
        }
    }
}
