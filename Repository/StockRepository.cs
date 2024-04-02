using api.Dtos.Stock;
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

    

    public async Task<List<Stock>> GetAllAsync()
    {
        return await _context.Stock.Include(s=>s.Comments).ToListAsync();
    }

    public async Task<Stock?> GetByIdAsync(int id)
    {
        return await _context.Stock.Include(s=>s.Comments).FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<Stock> CreateAsync(Stock stockModel)
    {
        await _context.Stock.AddAsync(stockModel);
        await _context.SaveChangesAsync();
        return stockModel;
    }

    public async Task<Stock?> UpdateAsync(int id , Stock stockModel)
    {
        var currStock = await _context.Stock.FirstOrDefaultAsync(x=>x.Id == id) ;

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
        var stockModel = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id) ;

        if(stockModel==null){
            return null;
        }
        _context.Stock.Remove(stockModel);
        await _context.SaveChangesAsync();
        return stockModel;
    }

    public async Task<bool> StockExists(int id)
    {
        return await _context.Stock.AnyAsync(s => s.Id == id);
    }
}