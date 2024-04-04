using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Migrations;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDBContext _context ;
        public PortfolioRepository(ApplicationDBContext context)
        {
            _context = context ;
        }

        public async Task<Portfolio> CreateAsync(Portfolio portfolio)
        {
            await _context.Portfolios.AddAsync(portfolio);
            await _context.SaveChangesAsync();
            return portfolio ;
        }

        public async Task<Portfolio?> DeleteAsync(AppUser appUser, string symbol)
        {
            var portfolioModel = await _context.Portfolios.FirstOrDefaultAsync(x=>x.AppUserId==appUser.Id && x.Stock.Symbol.ToLower()==symbol.ToLower());
            if(portfolioModel == null)return null ;
            _context.Portfolios.Remove(portfolioModel) ;
            await _context.SaveChangesAsync() ;
            return portfolioModel;
        }

        public async Task<List<Stock>> GetUserPortfolioAsync(AppUser user)
        {
            return await _context.Portfolios.Where(u => u.AppUserId == user.Id)
            .Select(p => new Stock{
                Id = p.StockId ,
                Symbol = p.Stock.Symbol ,
                CompanyName = p.Stock.CompanyName ,
                Purchase = p.Stock.Purchase ,
                LastDiv = p.Stock.LastDiv ,
                Industry = p.Stock.Industry ,
                MarketCap = p.Stock.MarketCap
            }).ToListAsync();
        }
    }
}