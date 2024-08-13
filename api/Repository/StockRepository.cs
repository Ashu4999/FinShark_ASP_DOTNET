using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;
        public StockRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }


        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);
            if (stockModel == null)
                return null;
            
            _context.Stocks.Remove(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<List<Stock>> GetAllAsync()
        {
            return await _context.Stocks.ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stocks.FindAsync(id);
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
        {
            var foundStock = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);

            if (foundStock == null) {
                return null;
            }

            foundStock.Symbol = stockDto.Symbol;
            foundStock.CompanyName = stockDto.CompanyName;
            foundStock.Purchase = stockDto.Purchase;
            foundStock.LastDiv = stockDto.LastDiv;
            foundStock.Industry = stockDto.Industry;
            foundStock.MarketCap = stockDto.MarketCap;

            await _context.SaveChangesAsync();
            return foundStock;
        }
    }
}