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
    }
}