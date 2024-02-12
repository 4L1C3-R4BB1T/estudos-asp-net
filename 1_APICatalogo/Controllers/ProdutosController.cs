using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;

[ApiController]
[Route("[controller]")]
public class ProdutosController : ControllerBase
{
    private readonly IProdutoRepository _repository;

    public ProdutosController(IProdutoRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Produto>> Get()
    {
        var produtos = _repository.GetAll().ToList();

        if (produtos is null)
            return NotFound("Produtos não encontrados.");

        return Ok(produtos);
    }

    [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
    public ActionResult<Produto> Get(int id)
    {
        var produto = _repository.GetById(id);

        if (produto is null)
            return NotFound($"Produto com id = {id} não encontrado.");

        return Ok(produto);
    }

    [HttpPost]
    public ActionResult Post(Produto produto)
    {
        if (produto is null)
            return BadRequest("Dados inválidos.");

        var created = _repository.Create(produto);

        return new CreatedAtRouteResult("ObterProduto",
            new { id = created.ProdutoId }, created);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Produto produto)
    {
        if (id != produto.ProdutoId)
            return BadRequest("Dados inválidos.");

        if (_repository.Update(produto))
            return Ok(produto);

        return StatusCode(500, "$Falha ao atualizar o produto de id = {id}.");
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var produto = _repository.GetById(id);

        if (produto is null)
            return NotFound($"Produto com id = {id} não encontrado.");

        if (_repository.Delete(id))
            return Ok($"Produto com id = {id} foi excluído.");

        return StatusCode(500, "$Falha ao excluir o produto de id = {id}.");
    }
}
