using APICatalogo.Interfaces.Repositories;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;

[ApiController]
[Route("[controller]")]
public class ProdutosController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public ProdutosController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet("produtos/{id}")]
    public ActionResult<IEnumerable<Produto>> GetProdutosCategoria(int id)
    {
        var produtos = _unitOfWork.ProdutoRepository.GetProdutosPorCategoria(id);

        if (produtos is null)
            return NotFound("Produtos não encontrados.");

        return Ok(produtos);
    }

    [HttpGet]
    public ActionResult<IEnumerable<Produto>> Get()
    {
        var produtos = _unitOfWork.ProdutoRepository.GetAll().ToList();

        if (produtos is null)
            return NotFound("Produtos não encontrados.");

        return Ok(produtos);
    }

    [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
    public ActionResult<Produto> Get(int id)
    {
        var produto = _unitOfWork.ProdutoRepository.GetById(p => p.ProdutoId == id);

        if (produto is null)
            return NotFound($"Produto com id = {id} não encontrado.");

        return Ok(produto);
    }

    [HttpPost]
    public ActionResult Post(Produto produto)
    {
        if (produto is null)
            return BadRequest("Dados inválidos.");

        var created = _unitOfWork.ProdutoRepository.Create(produto);
        _unitOfWork.Commit();

        return new CreatedAtRouteResult("ObterProduto",
            new { id = created.ProdutoId }, created);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Produto produto)
    {
        if (id != produto.ProdutoId)
            return BadRequest("Dados inválidos.");

        var updated = _unitOfWork.ProdutoRepository.Update(produto);
        _unitOfWork.Commit();

        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var produto = _unitOfWork.ProdutoRepository.GetById(p => p.ProdutoId == id);

        if (produto is null)
            return NotFound($"Produto com id = {id} não encontrado.");

        var deleted = _unitOfWork.ProdutoRepository.Delete(produto);
        _unitOfWork.Commit();

        return Ok(deleted);
    }
}
