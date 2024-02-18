using Catalogo.Domain.Entities;
using Catalogo.Domain.Interfaces;
using Catalogo.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalogo.Infrastructure.Repositories;

public class ProdutoRepository : IProdutoRepository
{
    private ApplicationDbContext _productContext;
    
    public ProdutoRepository(ApplicationDbContext context)
    {
        _productContext = context;
    }

    public async Task<Produto> CreateAsync(Produto product)
    {
        _productContext.Add(product);
        await _productContext.SaveChangesAsync();
        return product;
    }

    public async Task<Produto> GetByIdAsync(int? id)
    {
        return await _productContext.Produtos.Include(c => c.Categoria)
            .SingleOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Produto>> GetProdutosAsync()
    {
        return await _productContext.Produtos.ToListAsync();
    }

    public async Task<Produto> RemoveAsync(Produto product)
    {
        _productContext.Remove(product);
        await _productContext.SaveChangesAsync();
        return product;
    }

    public async Task<Produto> UpdateAsync(Produto product)
    {
        _productContext.Update(product);
        await _productContext.SaveChangesAsync();
        return product;
    }
}
