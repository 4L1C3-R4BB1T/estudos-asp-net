using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Respositories;
using X.PagedList;

namespace APICatalogo.Interfaces.Repositories;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    public ProdutoRepository(AppDbContext context) : base(context) { }

    public async Task<IPagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParams)
    {
        var produtos = await GetAllAsync();

        var produtosOrdenados = produtos.OrderBy(p => p.ProdutoId).AsQueryable();

        var resultado = await produtosOrdenados
            .ToPagedListAsync(produtosParams.PageNumber, produtosParams.PageSize);

        return resultado;
    }

    public async Task<IPagedList<Produto>> GetProdutosFiltroPrecoAsync(
        ProdutosFiltroPreco prodFiltroParams)
    {
        var produtos = await GetAllAsync();

        if (prodFiltroParams.Preco.HasValue &&
            !string.IsNullOrEmpty(prodFiltroParams.PrecoCriterio))
        {
            if (prodFiltroParams.PrecoCriterio
                .Equals("maior", StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos
                    .Where(p => p.Preco > prodFiltroParams.Preco.Value)
                    .OrderBy(p => p.Preco);
            }
            else if (prodFiltroParams.PrecoCriterio
                .Equals("menor", StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos
                    .Where(p => p.Preco < prodFiltroParams.Preco.Value)
                    .OrderBy(p => p.Preco);
            }
            else if (prodFiltroParams.PrecoCriterio
                .Equals("igual", StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos
                    .Where(p => p.Preco == prodFiltroParams.Preco.Value)
                    .OrderBy(p => p.Preco);
            }
        }

        var produtosFiltrados = await produtos
            .ToPagedListAsync(prodFiltroParams.PageNumber, prodFiltroParams.PageSize);

        return produtosFiltrados;
    }

    public async Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id)
    {
        var produtos = await GetAllAsync();

        var produtosCategoria = produtos.Where(p => p.CategoriaId == id);

        return produtosCategoria;
    }
}
