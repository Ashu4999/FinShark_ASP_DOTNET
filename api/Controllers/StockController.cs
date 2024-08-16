using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _stockRepo;
        public StockController(IStockRepository stockRepo)
        {
            _stockRepo = stockRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetStocks([FromQuery] QueryObject queryObject)
        {
            var stocks = await _stockRepo.GetAllAsync(queryObject);
            var stockDto = stocks.Select(s => s.ToStockDto());
            return Ok(stockDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            //  var stock = _context.Stocks.FirstOrDefault(s => s.Id == id);
            var foundStock = await _stockRepo.GetByIdAsync(id);

            if (foundStock == null)
                return NotFound();

            return Ok(foundStock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            // use to validate payload using dto
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var stockModel = stockDto.ToStockFromCreateDto();
            await _stockRepo.CreateAsync(stockModel);
            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {
            // use to validate payload using dto
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            
            var foundStock = await _stockRepo.UpdateAsync(id, updateDto);

            if (foundStock == null)
                return NotFound();

            return Ok(foundStock.ToStockDto());
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var foundStock = await _stockRepo.DeleteAsync(id);

            if (foundStock == null)
                return NotFound();

            return NoContent();
        }
    }
}