using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Extentions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{   
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioContoller : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepo;
        private readonly IPortfolioRepository _portfolioRepo;
        public PortfolioContoller(UserManager<AppUser> userManager, IStockRepository stockRepo, IPortfolioRepository portfolioRepo)
        {
            _userManager = userManager;
            _stockRepo = stockRepo;
            _portfolioRepo = portfolioRepo;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio() {
            var userEmail = User.GetUserEmail();
            var foundUser = await _userManager.FindByEmailAsync(userEmail);
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(foundUser);
            return Ok(userPortfolio);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio([FromQuery] string symbol) {
            var userEmail = User.GetUserEmail();
            var foundUser = await _userManager.FindByEmailAsync(userEmail);
            var foundStock = await _stockRepo.GetBySymbol(symbol);    

            if (foundStock == null) {
                return BadRequest("Stock not found");
            }

            var userPortfolio = await _portfolioRepo.GetUserPortfolio(foundUser);   
            var IsStockExists = userPortfolio.Any(s => s.Symbol.ToLower() == symbol.ToLower());

            if (IsStockExists) 
                return BadRequest("Can't add same stock to the portfolio");
            
            var portfolioModel = new Portfolio {
                AppUserId = foundUser.Id,
                StockId = foundStock.Id,
            };

            await _portfolioRepo.CreateAsync(portfolioModel);

            if (portfolioModel == null)
                return StatusCode(500, "Problem to add stock to portfolio");

            return Created();
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio([FromQuery] string symbol) {
            var userEmail = User.GetUserEmail();
            var foundUser = await _userManager.FindByEmailAsync(userEmail);

            var userPortfolio = await _portfolioRepo.GetUserPortfolio(foundUser);
            var filteredStock = userPortfolio.Where(s => s.Symbol.ToLower() == symbol.ToLower()).ToList();

            if (filteredStock.Count() < 1) {
                return BadRequest("Stock not found in portfolio");
            }

            await _portfolioRepo.DeleteAsync(foundUser, symbol);
            return NoContent();
        }
    }
}