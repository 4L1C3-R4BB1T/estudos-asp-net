using APICatalogo.DTO;
using APICatalogo.DTO.Mappings;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Newtonsoft.Json;
using X.PagedList;

namespace APICatalogo.Controllers;

[ApiController]
[Route("[controller]")]
[EnableRateLimiting("fixedwindow")]
public class CategoriasController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CategoriasController> _logger;

    public CategoriasController(IUnitOfWork unitOfWork, ILogger<CategoriasController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    private ActionResult<IEnumerable<CategoriaDTO>> ObterCategorias(IPagedList<Categoria> categorias)
    {
        var metadata = new
        {
            categorias.Count,
            categorias.PageSize,
            categorias.PageCount,
            categorias.TotalItemCount,
            categorias.HasNextPage,
            categorias.HasPreviousPage
        };
        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
        var categoriasDto = categorias.ToCategoriaDTOList();
        return Ok(categoriasDto);
    }

    [HttpGet("pagination")]
    public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get(
        [FromQuery] CategoriasParameters categoriasParameters)
    {
        var categorias = await _unitOfWork.CategoriaRepository.GetCategoriasAsync(categoriasParameters);
        return ObterCategorias(categorias);
    }

    [HttpGet("filter/nome/pagination")]
    public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasFiltradas(
        [FromQuery] CategoriasFiltroNome categoriasFiltro)
    {
        var categoriasFiltradas = await _unitOfWork.CategoriaRepository
            .GetCategoriasFiltroNomeAsync(categoriasFiltro);
        return ObterCategorias(categoriasFiltradas);
    }

    [HttpGet]
    [DisableRateLimiting]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get()
    {
        var categorias = await _unitOfWork.CategoriaRepository.GetAllAsync();

        if (categorias is null)
            return NotFound("Categorias não encontradas.");

        var categoriasDTO = categorias.ToCategoriaDTOList();

        return Ok(categoriasDTO);
    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public async Task<ActionResult<CategoriaDTO>> Get(int id)
    {
        var categoria = await _unitOfWork.CategoriaRepository.GetByIdAsync(c => c.CategoriaId == id);

        if (categoria is null)
        {
            _logger.LogWarning($"Categoria com id = {id} não encontrada.");
            return NotFound($"Categoria com id = {id} não encontrada.");
        }

        var categoriaDTO = categoria.ToCategoriaDTO();

        return Ok(categoria);
    }

    [HttpPost]
    public async Task<ActionResult<CategoriaDTO>> Post(CategoriaDTO categoriaDTO)
    {
        if (categoriaDTO is null)
        {
            _logger.LogWarning($"Dados inválidos.");
            return BadRequest("Dados inválidos.");
        }

        var categoria = categoriaDTO.ToCategoria();

        var created = _unitOfWork.CategoriaRepository.Create(categoria);
        await _unitOfWork.CommitAsync();

        var createdDTO = created.ToCategoriaDTO();

        return new CreatedAtRouteResult("ObterCategoria",
            new { id = createdDTO.CategoriaId }, createdDTO);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CategoriaDTO>> Put(int id, CategoriaDTO categoriaDTO)
    {
        if (id != categoriaDTO.CategoriaId)
        {
            _logger.LogWarning($"Dados inválidos.");
            return BadRequest("Dados inválidos.");
        }

        var categoria = categoriaDTO.ToCategoria();

        var updated = _unitOfWork.CategoriaRepository.Update(categoria);
        await _unitOfWork.CommitAsync();

        var updatedDTO = updated.ToCategoriaDTO();

        return Ok(updatedDTO);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<CategoriaDTO>> Delete(int id)
    {
        var categoria = await _unitOfWork.CategoriaRepository.GetByIdAsync(c => c.CategoriaId == id);

        if (categoria is null)
        {
            _logger.LogWarning($"Categoria com id = {id} não encontrada.");
            return NotFound($"Categoria com id = {id} não encontrada.");
        }

        var deleted = _unitOfWork.CategoriaRepository.Delete(categoria);
        await _unitOfWork.CommitAsync();

        var deletedDTO = deleted.ToCategoriaDTO();

        return Ok(deletedDTO);
    }
}
