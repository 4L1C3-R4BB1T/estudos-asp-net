using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalog.Repositories.Interfaces;

public interface IProdutoRepository : IRepository<Produto>
{
    PagedList<Produto> GetProdutos(ProdutosParameters produtosParams);
    PagedList<Produto> GetProdutosFiltroPreco(ProdutosFiltroPreco produtosFiltroParams);
    IEnumerable<Produto> GetProdutosPorCategoria(int id);
}
