using APICatalogo.DTO;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using X.PagedList;

namespace APICatalogo.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class ProdutosController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProdutosController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    private ActionResult<IEnumerable<ProdutoDTO>> ObterProdutos(IPagedList<Produto> produtos)
    {
        var metadata = new
        {
            produtos.Count,
            produtos.PageSize,
            produtos.PageCount,
            produtos.TotalItemCount,
            produtos.HasNextPage,
            produtos.HasPreviousPage
        };
        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
        var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
        return Ok(produtosDto);
    }

    [HttpGet("produtos/{id}")]
    public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosCategoria(int id)
    {
        var produtos = await _unitOfWork.ProdutoRepository.GetProdutosPorCategoriaAsync(id);

        if (produtos is null)
            return NotFound("Produtos não encontrados.");

        var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

        return Ok(produtosDTO);
    }

    [HttpGet("pagination")]
    public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get(
        [FromQuery] ProdutosParameters produtosParameters)
    {
        var produtos = await _unitOfWork.ProdutoRepository.GetProdutosAsync(produtosParameters);
        return ObterProdutos(produtos);
    }

    [HttpGet("filter/preco/pagination")]
    public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosFilterPreco(
        [FromQuery] ProdutosFiltroPreco produtosFilterParameters)
    {
        var produtos = await _unitOfWork.ProdutoRepository
            .GetProdutosFiltroPrecoAsync(produtosFilterParameters);
        return ObterProdutos(produtos);
    }

    /// <summary>
    /// Exibe uma relação dos produtos
    /// </summary>
    /// <returns>Retorna uma lista de objetos Produto</returns>
    [HttpGet]
    [Authorize(Policy = "UserOnly")]
    public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get()
    {
        var produtos = await _unitOfWork.ProdutoRepository.GetAllAsync();

        if (produtos is null)
            return NotFound("Produtos não encontrados.");

        var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

        return Ok(produtosDTO);
    }

    /// <summary>
    /// Obtem o produto pelo seu identificador id
    /// </summary>
    /// <param name="id">Código do produto</param>
    /// <returns>Um objeto Produto</returns>
    [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
    public async Task<ActionResult<ProdutoDTO>> Get(int id)
    {
        var produto = await _unitOfWork.ProdutoRepository.GetByIdAsync(p => p.ProdutoId == id);

        if (produto is null)
            return NotFound($"Produto com id = {id} não encontrado.");

        var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

        return Ok(produtoDTO);
    }

    [HttpPost]
    public async Task<ActionResult<ProdutoDTO>> Post(ProdutoDTO produtoDTO)
    {
        if (produtoDTO is null)
            return BadRequest("Dados inválidos.");

        var produto = _mapper.Map<Produto>(produtoDTO);

        var created = _unitOfWork.ProdutoRepository.Create(produto);
        await _unitOfWork.CommitAsync();

        var createdDTO = _mapper.Map<ProdutoDTO>(created);

        return new CreatedAtRouteResult("ObterProduto",
            new { id = createdDTO.ProdutoId }, createdDTO);
    }

    [HttpPatch("{id}/UpdatePartial")]
    public async Task<ActionResult<ProdutoDTOUpdateResponse>> Patch(int id,
        JsonPatchDocument<ProdutoDTOUpdateRequest> patchProdutoDto)
    {
        if (patchProdutoDto == null || id <= 0)
            return BadRequest("Dados inválidos.");

        var produto = await _unitOfWork.ProdutoRepository.GetByIdAsync(p => p.ProdutoId == id);

        if (produto == null)
            return NotFound($"Produto com id = {id} não encontrado.");

        var produtoUpdateRequest = _mapper.Map<ProdutoDTOUpdateRequest>(produto);

        patchProdutoDto.ApplyTo(produtoUpdateRequest, ModelState);

        if (!ModelState.IsValid || !TryValidateModel(produtoUpdateRequest))
            return BadRequest(ModelState);

        _mapper.Map(produtoUpdateRequest, produto);

        _unitOfWork.ProdutoRepository.Update(produto);
        await _unitOfWork.CommitAsync();

        return Ok(_mapper.Map<ProdutoDTOUpdateResponse>(produto));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProdutoDTO>> Put(int id, ProdutoDTO produtoDTO)
    {
        if (id != produtoDTO.ProdutoId)
            return BadRequest("Dados inválidos.");

        var produto = _mapper.Map<Produto>(produtoDTO);

        var updated = _unitOfWork.ProdutoRepository.Update(produto);
        await _unitOfWork.CommitAsync();

        var updatedDTO = _mapper.Map<ProdutoDTO>(updated);

        return Ok(updatedDTO);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ProdutoDTO>> Delete(int id)
    {
        var produto = await _unitOfWork.ProdutoRepository.GetByIdAsync(p => p.ProdutoId == id);

        if (produto is null)
            return NotFound($"Produto com id = {id} não encontrado.");

        var deleted = _unitOfWork.ProdutoRepository.Delete(produto);
        await _unitOfWork.CommitAsync();

        var deletedDTO = _mapper.Map<ProdutoDTO>(deleted);

        return Ok(deletedDTO);
    }
}
