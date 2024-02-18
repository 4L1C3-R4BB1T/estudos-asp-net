using APICatalog.Repositories.Interfaces;
using APICatalogo.DTO;
using APICatalogo.DTO.Mappings;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace APICatalogo.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly IUnitOfWork
     _unitOfWork;
    private readonly ILogger<CategoriasController> _logger;

    public CategoriasController(IUnitOfWork unitOfWork, ILogger<CategoriasController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    private ActionResult<IEnumerable<CategoriaDTO>> ObterCategorias(PagedList<Categoria> categorias)
    {
        var metadata = new
        {
            categorias.TotalCount,
            categorias.PageSize,
            categorias.CurrentPage,
            categorias.TotalPages,
            categorias.HasNext,
            categorias.HasPrevious
        };
        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
        var categoriasDto = categorias.ToCategoriaDTOList();
        return Ok(categoriasDto);
    }

    [HttpGet("pagination")]
    public ActionResult<IEnumerable<CategoriaDTO>> Get(
        [FromQuery] CategoriasParameters categoriasParameters)
    {
        var categorias = _unitOfWork.CategoriaRepository.GetCategorias(categoriasParameters);
        return ObterCategorias(categorias);
    }

    [HttpGet("filter/nome/pagination")]
    public ActionResult<IEnumerable<CategoriaDTO>> GetCategoriasFiltradas(
        [FromQuery] CategoriasFiltroNome categoriasFiltro)
    {
        var categoriasFiltradas = _unitOfWork.CategoriaRepository
            .GetCategoriasFiltroNome(categoriasFiltro);
        return ObterCategorias(categoriasFiltradas);
    }

    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public ActionResult<IEnumerable<CategoriaDTO>> Get()
    {
        var categorias = _unitOfWork.CategoriaRepository.GetAll().ToList();

        if (categorias is null)
            return NotFound("Categorias não encontradas.");

        var categoriasDTO = categorias.ToCategoriaDTOList();

        return Ok(categoriasDTO);
    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public ActionResult<CategoriaDTO> Get(int id)
    {
        var categoria = _unitOfWork.CategoriaRepository.GetById(c => c.CategoriaId == id);

        if (categoria is null)
        {
            _logger.LogWarning($"Categoria com id = {id} não encontrada.");
            return NotFound($"Categoria com id = {id} não encontrada.");
        }

        var categoriaDTO = categoria.ToCategoriaDTO();

        return Ok(categoria);
    }

    [HttpPost]
    public ActionResult<CategoriaDTO> Post(CategoriaDTO categoriaDTO)
    {
        if (categoriaDTO is null)
        {
            _logger.LogWarning($"Dados inválidos.");
            return BadRequest("Dados inválidos.");
        }

        var categoria = categoriaDTO.ToCategoria();

        var created = _unitOfWork.CategoriaRepository.Create(categoria);
        _unitOfWork.Commit();

        var createdDTO = created.ToCategoriaDTO();

        return new CreatedAtRouteResult("ObterCategoria",
            new { id = createdDTO.CategoriaId }, createdDTO);
    }

    [HttpPut("{id:int}")]
    public ActionResult<CategoriaDTO> Put(int id, CategoriaDTO categoriaDTO)
    {
        if (id != categoriaDTO.CategoriaId)
        {
            _logger.LogWarning($"Dados inválidos.");
            return BadRequest("Dados inválidos.");
        }

        var categoria = categoriaDTO.ToCategoria();

        var updated = _unitOfWork.CategoriaRepository.Update(categoria);
        _unitOfWork.Commit();

        var updatedDTO = updated.ToCategoriaDTO();

        return Ok(updatedDTO);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<CategoriaDTO> Delete(int id)
    {
        var categoria = _unitOfWork.CategoriaRepository.GetById(c => c.CategoriaId == id);

        if (categoria is null)
        {
            _logger.LogWarning($"Categoria com id = {id} não encontrada.");
            return NotFound($"Categoria com id = {id} não encontrada.");
        }

        var deleted = _unitOfWork.CategoriaRepository.Delete(categoria);
        _unitOfWork.Commit();

        var deletedDTO = deleted.ToCategoriaDTO();

        return Ok(deletedDTO);
    }
}
