using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Respositories;

namespace APICatalogo.Interfaces.Repositories;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(AppDbContext context) : base(context) { }
}
