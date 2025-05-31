using API.Models.Customers;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers.Customers
{
    /// <summary>
    /// API Controller for managing Package operations.
    /// </summary>
    [ApiController]
    [Route("api/packages")]
    public class PackageController : ControllerBase
    {
        private readonly IPackageService _packageService;

        /// <summary>
        /// Constructor with dependency injection for IPackageService.
        /// </summary>
        /// <param name="packageService">The package service instance</param>
        public PackageController(IPackageService packageService)
        {
            _packageService = packageService;
        }

        /// <summary>
        /// Creates a new package for a customer.
        /// </summary>
        /// <param name="package">Package data containing creation information</param>
        /// <param name="customerId">The ID of the customer to create the package for</param>
        /// <returns>HTTP response with the created package or error message</returns>
        [HttpPost("customer/{customerId}")]
        public async Task<IActionResult> CreatePackage([FromBody] Package package, int customerId)
        {
            var result = await _packageService.CreatePackageAsync(package, customerId);
            return result.IsSuccess
                ? Ok(result.Data)
                : BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Retrieves a package by its unique ID.
        /// </summary>
        /// <param name="id">Unique identifier of the package</param>
        /// <returns>HTTP response with the package or error message</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPackageById(string id)
        {
            var result = await _packageService.GetPackageByIdAsync(id);
            return result.IsSuccess
                ? Ok(result.Data)
                : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Retrieves all packages for a customer by their ID.
        /// </summary>
        /// <param name="customerId">The ID of the customer whose packages are to be retrieved</param>
        /// <returns>HTTP response with the list of packages or error message</returns>
        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetPackagesByCustomerId(int customerId)
        {
            var result = await _packageService.GetPackagesByCustomerIdAsync(customerId);
            return result.IsSuccess
                ? Ok(result.Data)
                : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Updates an existing package.
        /// </summary>
        /// <param name="id">The ID of the package to update</param>
        /// <param name="package">The updated package data</param>
        /// <returns>HTTP response with the updated package or error message</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePackage(string id, [FromBody] Package package)
        {
            if (id != package.Id)
            {
                return BadRequest("Package ID mismatch");
            }

            var result = await _packageService.UpdatePackageAsync(package);
            return result.IsSuccess
                ? Ok(result.Data)
                : BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Removes a package by its unique ID.
        /// </summary>
        /// <param name="id">Unique identifier of the package to remove</param>
        /// <returns>HTTP response indicating success or failure</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePackage(string id)
        {
            var result = await _packageService.DeletePackageAsync(id);
            return result.IsSuccess
                ? NoContent()
                : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Adds an item to a package.
        /// </summary>
        /// <param name="id">The ID of the package</param>
        /// <param name="itemSku">The SKU of the item to add</param>
        /// <param name="quantity">The quantity to add (defaults to 1)</param>
        /// <returns>HTTP response with the updated package or error message</returns>
        [HttpPost("{id}/items/{itemSku}")]
        public async Task<IActionResult> AddItemToPackage(string id, int itemSku, [FromQuery] int quantity = 1)
        {
            var result = await _packageService.AddItemToPackageAsync(id, itemSku, quantity);
            return result.IsSuccess
                ? Ok(result.Data)
                : BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Removes an item from a package.
        /// </summary>
        /// <param name="id">The ID of the package</param>
        /// <param name="itemSku">The SKU of the item to remove</param>
        /// <returns>HTTP response with the updated package or error message</returns>
        [HttpDelete("{id}/items/{itemSku}")]
        public async Task<IActionResult> RemoveItemFromPackage(string id, int itemSku)
        {
            var result = await _packageService.RemoveItemFromPackageAsync(id, itemSku);
            return result.IsSuccess
                ? Ok(result.Data)
                : BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Adds a note to a package.
        /// </summary>
        /// <param name="id">The ID of the package</param>
        /// <param name="note">The note data to add</param>
        /// <returns>HTTP response with the updated package or error message</returns>
        [HttpPost("{id}/notes")]
        public async Task<IActionResult> AddNoteToPackage(string id, [FromBody] PackageNote note)
        {
            var result = await _packageService.AddNoteToPackageAsync(id, note);
            return result.IsSuccess
                ? Ok(result.Data)
                : BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Updates the status of a package.
        /// </summary>
        /// <param name="id">The ID of the package</param>
        /// <param name="status">The new status value</param>
        /// <param name="updatedBy">The user who updated the status</param>
        /// <param name="notes">Optional notes about the status update</param>
        /// <returns>HTTP response with the updated package or error message</returns>
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdatePackageStatus(string id, [FromQuery] string status, [FromQuery] string updatedBy, [FromQuery] string notes = "")
        {
            var result = await _packageService.UpdatePackageStatusAsync(id, status, updatedBy, notes);
            return result.IsSuccess
                ? Ok(result.Data)
                : BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Retrieves the pricing information for a package.
        /// </summary>
        /// <param name="id">The ID of the package</param>
        /// <returns>HTTP response with the total price or error message</returns>
        [HttpGet("{id}/pricing/total")]
        public async Task<IActionResult> CalculateTotalPrice(string id)
        {
            var result = await _packageService.CalculateTotalPriceAsync(id);
            return result.IsSuccess
                ? Ok(result.Data)
                : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Retrieves the discounted pricing information for a package.
        /// </summary>
        /// <param name="id">The ID of the package</param>
        /// <returns>HTTP response with the discounted price or error message</returns>
        [HttpGet("{id}/pricing/discounted")]
        public async Task<IActionResult> CalculateDiscountedPrice(string id)
        {
            var result = await _packageService.CalculateDiscountedPriceAsync(id);
            return result.IsSuccess
                ? Ok(result.Data)
                : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Retrieves the contract value information for a package.
        /// </summary>
        /// <param name="id">The ID of the package</param>
        /// <returns>HTTP response with the total contract value or error message</returns>
        [HttpGet("{id}/contract/value")]
        public async Task<IActionResult> CalculateTotalContractValue(string id)
        {
            var result = await _packageService.CalculateTotalContractValueAsync(id);
            return result.IsSuccess
                ? Ok(result.Data)
                : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Retrieves the contract status information for a package.
        /// </summary>
        /// <param name="id">The ID of the package</param>
        /// <returns>HTTP response with the contract active status or error message</returns>
        [HttpGet("{id}/contract/status")]
        public async Task<IActionResult> IsContractActive(string id)
        {
            var result = await _packageService.IsContractActiveAsync(id);
            return result.IsSuccess
                ? Ok(result.Data)
                : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Retrieves the remaining time information for a package's contract.
        /// </summary>
        /// <param name="id">The ID of the package</param>
        /// <returns>HTTP response with the remaining contract time or error message</returns>
        [HttpGet("{id}/contract/remaining-time")]
        public async Task<IActionResult> GetRemainingContractTime(string id)
        {
            var result = await _packageService.GetRemainingContractTimeAsync(id);
            return result.IsSuccess
                ? Ok(result.Data)
                : NotFound(result.ErrorMessage);
        }
    }
}