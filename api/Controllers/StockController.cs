using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public StockController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetStocks()
        {
            var stocks = await _context.Stocks.ToListAsync();
            var stockDto = stocks.Select(s => s.ToStockDto());
            return Ok(stockDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            //  var stock = _context.Stocks.FirstOrDefault(s => s.Id == id);
            var foundStock = await _context.Stocks.FindAsync(id);

            if (foundStock == null)
                return NotFound();

            return Ok(foundStock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            var stockModel = stockDto.ToStockFromCreateDto();
            // saved changed
            await _context.Stocks.AddAsync(stockModel);
            // commit changes to database
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {
            var foundStock = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);

            if (foundStock == null)
                return NotFound();

            foundStock.Symbol = updateDto.Symbol;
            foundStock.CompanyName = updateDto.CompanyName;
            foundStock.Purchase = updateDto.Purchase;
            foundStock.LastDiv = updateDto.LastDiv;
            foundStock.Industry = updateDto.Industry;
            foundStock.MarketCap = updateDto.MarketCap;

            await _context.SaveChangesAsync();
            return Ok(foundStock.ToStockDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id) 
        {   
            Console.WriteLine(id);
            var foundStock = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);

            if (foundStock == null)
                return NotFound();

            _context.Stocks.Remove(foundStock);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}