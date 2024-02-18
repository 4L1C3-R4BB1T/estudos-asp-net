using Catalogo.Application.DTOs;
using Catalogo.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Catalogo.API.Controllers;

[ApiController]
[Route("api/v1/[Controller]")]
public class ProdutosController : Controller
{
    private readonly IProdutoService _produtoService;

    public ProdutosController(IProdutoService produtoService)
    {
        _produtoService = produtoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get()
    {
        var produtos = await _produtoService.GetProdutos();
        return Ok(produtos);
    }

    [HttpGet("{id}", Name = "GetProduto")]
    public async Task<ActionResult<ProdutoDTO>> Get(int id)
    {
        var produto = await _produtoService.GetById(id);
        if (produto == null)
            return NotFound();
        return Ok(produto);
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] ProdutoDTO produtoDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        await _produtoService.Add(produtoDto);
        return new CreatedAtRouteResult("GetProduto", new { id = produtoDto.Id }, produtoDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, [FromBody] ProdutoDTO produtoDto)
    {
        if (id != produtoDto.Id)
            return BadRequest();
        await _produtoService.Update(produtoDto);
        return Ok(produtoDto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ProdutoDTO>> Delete(int id)
    {
        var produtoDto = await _produtoService.GetById(id);
        if (produtoDto == null)
            return NotFound();
        await _produtoService.Remove(id);
        return Ok(produtoDto);
    }
}
