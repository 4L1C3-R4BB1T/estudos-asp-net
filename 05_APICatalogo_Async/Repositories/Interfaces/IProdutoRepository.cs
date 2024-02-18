using APICatalogo.Models;
using APICatalogo.Pagination;
using X.PagedList;

namespace APICatalog.Repositories.Interfaces;

public interface IProdutoRepository : IRepository<Produto>
{
    Task<IPagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParams);
    Task<IPagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroParams);
    Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id);
}
