﻿using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using OnDaGo.API.Models;
using OnDaGo.API.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnDaGo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FareMatrixController : ControllerBase
    {
        private readonly FareMatrixService _fareMatrixService;

        public FareMatrixController(FareMatrixService fareMatrixService)
        {
            _fareMatrixService = fareMatrixService;
        }

        [HttpGet]
        public async Task<ActionResult<List<FareMatrixItem>>> GetFareMatrix()
        {
            var fareMatrix = await _fareMatrixService.GetFareMatrixAsync();
            return Ok(fareMatrix);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchFare(string id, [FromBody] FareMatrixUpdateModel updateModel)
        {
            if (string.IsNullOrWhiteSpace(id) || updateModel == null)
            {
                return BadRequest("Invalid update data or ID.");
            }

            var existingFare = await _fareMatrixService.GetFareByIdAsync(id);
            if (existingFare == null)
            {
                return NotFound($"Fare with ID {id} not found.");
            }

            
            System.Diagnostics.Debug.WriteLine($"Patching Fare with ID: {id}. Fare: {updateModel.Fare}, DiscountedFare: {updateModel.DiscountedFare}");

            
            if (updateModel.Fare.HasValue)
            {
                existingFare.Fare = updateModel.Fare.Value;
            }

            if (updateModel.DiscountedFare.HasValue)
            {
                existingFare.DiscountedFare = updateModel.DiscountedFare.Value;
            }

            await _fareMatrixService.UpdateFareAsync(id, existingFare);
            return NoContent();
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<FareMatrixItem>> GetFareById(string id)
        {
            var fare = await _fareMatrixService.GetFareByIdAsync(id);
            if (fare == null) return NotFound();
            return Ok(fare);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFare([FromBody] FareMatrixItem fare)
        {
            if (fare == null || string.IsNullOrWhiteSpace(fare.Origin) || string.IsNullOrWhiteSpace(fare.Destination))
            {
                return BadRequest("Invalid fare data.");
            }

            await _fareMatrixService.CreateFareAsync(fare);
            return CreatedAtAction(nameof(GetFareById), new { id = fare.Id.ToString() }, fare);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFare(string id, [FromBody] FareMatrixItem updatedFare)
        {
            var fare = await _fareMatrixService.GetFareByIdAsync(id);
            if (fare == null) return NotFound();

            updatedFare.Id = new ObjectId(id);
            await _fareMatrixService.UpdateFareAsync(id, updatedFare);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFare(string id)
        {
            var fare = await _fareMatrixService.GetFareByIdAsync(id);
            if (fare == null) return NotFound();

            await _fareMatrixService.DeleteFareAsync(id);
            return NoContent();
        }
    }

}
