using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace api.Repository;

public class StockRepository : IStockRepository
{
    private readonly ApplicationDBContext _context ;
    public StockRepository(ApplicationDBContext context)
    {
        _context = context ;
    }

    

    public async Task<List<Stock>> GetAllAsync(QueryObject query)
    {
        var stocks =  _context.Stocks.Include(s=>s.Comments).ThenInclude(a=>a.AppUser).AsQueryable();

        if(!string.IsNullOrWhiteSpace(query.CompanyName))
        {
            stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName)) ;
        }
        if(!string.IsNullOrWhiteSpace(query.Symbol))
        {
            stocks = stocks.Where(s=>s.Symbol.Contains(query.Symbol));
        }
        if(!string.IsNullOrWhiteSpace(query.SortBy))
        {
            if(query.SortBy.Equals("Symbol" , StringComparison.OrdinalIgnoreCase))
            {
                stocks = query.IsDescending ?
                    stocks.OrderByDescending(s=>s.Symbol) :
                    stocks.OrderBy(s=>s.Symbol) ;
            }
        }
        //TODO: validate that numbers are not negative
        var skipNumber = (query.PageNumber -1) * query.PageSize ;
        var takeNumber = query.PageSize ;
        return await stocks.Skip(skipNumber).Take(takeNumber).ToListAsync() ;
    }

    public async Task<Stock?> GetByIdAsync(int id)
    {
        return await _context.Stocks.Include(s=>s.Comments).ThenInclude(a=>a.AppUser).FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<Stock> CreateAsync(Stock stockModel)
    {
        await _context.Stocks.AddAsync(stockModel);
        await _context.SaveChangesAsync();
        return stockModel;
    }

    public async Task<Stock?> UpdateAsync(int id , Stock stockModel)
    {
        var currStock = await _context.Stocks.FirstOrDefaultAsync(x=>x.Id == id) ;

        if(currStock==null){
            return null;
        }

        currStock.Symbol = stockModel.Symbol ;
        currStock.CompanyName = stockModel.CompanyName;
        currStock.Purchase = stockModel.Purchase;
        currStock.LastDiv = stockModel.LastDiv;
        currStock.Industry = stockModel.Industry;
        currStock.MarketCap = stockModel.MarketCap;

        await _context.SaveChangesAsync();

        return currStock;
    }
    public async Task<Stock?> DeleteAsync(int id)
    {
        var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id) ;

        if(stockModel==null){
            return null;
        }
        _context.Stocks.Remove(stockModel);
        await _context.SaveChangesAsync();
        return stockModel;
    }

    public async Task<bool> StockExists(int id)
    {
        return await _context.Stocks.AnyAsync(s => s.Id == id);
    }

    public async Task<Stock?> GetBySymbolAsync(string symbol)
    {
        return await _context.Stocks.FirstOrDefaultAsync(s => s.Symbol == symbol);
    }
}