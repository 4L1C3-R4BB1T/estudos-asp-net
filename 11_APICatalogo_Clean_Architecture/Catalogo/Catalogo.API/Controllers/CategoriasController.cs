using Catalogo.Application.DTOs;
using Catalogo.Application.Interfaces;
using Catalogo.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Catalogo.API.Controllers;

[ApiController]
[Route("api/v1/[Controller]")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;

    public CategoriasController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get()
    {
        try
        {
            var categorias = await _categoriaService.GetCategorias();
            return Ok(categorias);
        }
        catch
        {
            throw;
        }
    }

    [HttpGet("{id}", Name = "GetCategoria")]
    public async Task<ActionResult<Categoria>> Get(int id)
    {
        var categoria = await _categoriaService.GetById(id);
        if (categoria == null)
            return NotFound();
        return Ok(categoria);
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] CategoriaDTO categoriaDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        await _categoriaService.Add(categoriaDto);
        return new CreatedAtRouteResult("GetCategoria", new { id = categoriaDto.Id }, categoriaDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, [FromBody] CategoriaDTO categoriaDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        if (id != categoriaDto.Id)
            return BadRequest();
        await _categoriaService.Update(categoriaDto);
        return Ok(categoriaDto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Categoria>> Delete(int id)
    {
        var categoriaDto = await _categoriaService.GetById(id);
        if (categoriaDto == null)
            return NotFound();
        await _categoriaService.Remove(id);
        return Ok(categoriaDto);
    }
}
