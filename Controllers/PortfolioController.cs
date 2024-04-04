using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Extentions;
using api.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/porfolio")]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager ;
        private readonly IStockRepository _stockRepo ;
        private readonly IPortfolioRepository _portfolioRepo ;
        public PortfolioController
        (
         UserManager<AppUser> userManager ,
         IStockRepository stockRepo ,
         IPortfolioRepository portfolioRepo
        )
        {
            _userManager = userManager ;
            _stockRepo = stockRepo ;
            _portfolioRepo = portfolioRepo ;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username) ;
            var userPortfolio = await _portfolioRepo.GetUserPortfolioAsync(appUser) ;
            return Ok(userPortfolio);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePortfolio(string symbol)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var stock = await _stockRepo.GetBySymbolAsync(symbol);

            if(stock == null){
                return BadRequest("stock not found") ;
            }

            var userPortfolio = await _portfolioRepo.GetUserPortfolioAsync(appUser);
            if(userPortfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower() )){
                return BadRequest("cannot add same stock to portfolio");
            } 
            var porfolioModel = new Portfolio
            {
                StockId = stock.Id ,
                AppUserId = appUser.Id 
            };
            await _portfolioRepo.CreateAsync(porfolioModel) ;

            if(porfolioModel == null)
                return StatusCode(500 , "could not create");
            return Created();
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteProtfolio(string symbol)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);

            var userPortfolio = await _portfolioRepo.GetUserPortfolioAsync(appUser);

            var targetPorfolio = userPortfolio.Where(s=>s.Symbol.ToLower() == symbol.ToLower());

            if(targetPorfolio.Count() == 1)
            {
                await _portfolioRepo.DeleteAsync(appUser , symbol) ;
                return Ok();
            }
            return BadRequest("stock not in your portfolio");

        }
    }
}