using APICatalogo.Models;
using APICatalogo.Pagination;
using X.PagedList;

namespace APICatalog.Repositories.Interfaces;

public interface ICategoriaRepository : IRepository<Categoria>
{
    Task<IPagedList<Categoria>> GetCategoriasAsync(CategoriasParameters categoriasParams);
    Task<IPagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriasFiltroNome categoriasParams);
}
