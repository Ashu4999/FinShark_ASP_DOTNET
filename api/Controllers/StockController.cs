using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult GetStocks()
        {
            var stocks = _context.Stocks.ToList().Select(s => s.ToStockDto());
            return Ok(stocks);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            //  var stock = _context.Stocks.FirstOrDefault(s => s.Id == id);
            var foundStock = _context.Stocks.Find(id);

            if (foundStock == null)
                return NotFound();

            return Ok(foundStock.ToStockDto());
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateStockRequestDto stockDto)
        {
            var stockModel = stockDto.ToStockFromCreateDto();
            // saved changed
            _context.Stocks.Add(stockModel);
            // commit changes to database
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {
            var foundStock = _context.Stocks.FirstOrDefault(s => s.Id == id);

            if (foundStock == null)
                return NotFound();

            foundStock.Symbol = updateDto.Symbol;
            foundStock.CompanyName = updateDto.CompanyName;
            foundStock.Purchase = updateDto.Purchase;
            foundStock.LastDiv = updateDto.LastDiv;
            foundStock.Industry = updateDto.Industry;
            foundStock.MarketCap = updateDto.MarketCap;

            _context.SaveChanges();
            return Ok(foundStock.ToStockDto());
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id) 
        {
            var foundStock = _context.Stocks.FirstOrDefault(s => s.Id == id);

            if (foundStock == null)
                return NotFound();

            _context.Stocks.Remove(foundStock);
            _context.SaveChanges();
            return NoContent();
        }
    }
}