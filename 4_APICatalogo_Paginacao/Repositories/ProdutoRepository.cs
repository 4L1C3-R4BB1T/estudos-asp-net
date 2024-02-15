using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Respositories;

namespace APICatalogo.Interfaces.Repositories;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    public ProdutoRepository(AppDbContext context) : base(context) { }

    // public IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParams)
    // {
    //     return GetAll()
    //         .OrderBy(p => p.Nome)
    //         .Skip((produtosParams.PageNumber - 1) * produtosParams.PageSize)
    //         .Take(produtosParams.PageSize)
    //         .ToList();
    // }

    public PagedList<Produto> GetProdutos(ProdutosParameters produtosParams)
    {
        var produtos = GetAll().OrderBy(p => p.ProdutoId).AsQueryable();

        var produtosOrdenados = PagedList<Produto>
            .ToPagedList(produtos, produtosParams.PageNumber, produtosParams.PageSize);

        return produtosOrdenados;
    }

    public PagedList<Produto> GetProdutosFiltroPreco(ProdutosFiltroPreco prodFiltroParams)
    {
        var produtos = GetAll().AsQueryable();

        if (prodFiltroParams.Preco.HasValue && !string.IsNullOrEmpty(prodFiltroParams.PrecoCriterio))
        {
            if (prodFiltroParams.PrecoCriterio.Equals("maior", StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos
                    .Where(p => p.Preco > prodFiltroParams.Preco.Value)
                    .OrderBy(p => p.Preco);
            }
            else if (prodFiltroParams.PrecoCriterio.Equals("menor", StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos
                    .Where(p => p.Preco < prodFiltroParams.Preco.Value)
                    .OrderBy(p => p.Preco);
            }
            else if (prodFiltroParams.PrecoCriterio.Equals("igual", StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos
                    .Where(p => p.Preco == prodFiltroParams.Preco.Value)
                    .OrderBy(p => p.Preco);
            }
        }
        var produtosFiltrados = PagedList<Produto>.ToPagedList(produtos,
            prodFiltroParams.PageNumber, prodFiltroParams.PageSize);

        return produtosFiltrados;
    }

    public IEnumerable<Produto> GetProdutosPorCategoria(int id)
    {
        return GetAll().Where(c => c.CategoriaId == id);
    }
}
