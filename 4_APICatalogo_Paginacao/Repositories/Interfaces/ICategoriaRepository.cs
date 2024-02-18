using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalog.Repositories.Interfaces;

public interface ICategoriaRepository : IRepository<Categoria>
{
    PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParams);
    PagedList<Categoria> GetCategoriasFiltroNome(CategoriasFiltroNome categoriasParams);
}
