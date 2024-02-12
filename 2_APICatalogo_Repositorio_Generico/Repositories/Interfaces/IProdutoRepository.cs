using APICatalogo.Models;

namespace APICatalogo.Interfaces.Repositories;

public interface IProdutoRepository : IRepository<Produto>
{
    IEnumerable<Produto> GetProdutosPorCategoria(int id);
}
