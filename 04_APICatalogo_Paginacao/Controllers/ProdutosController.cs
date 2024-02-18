using APICatalog.Repositories.Interfaces;
using APICatalogo.DTO;
using APICatalogo.Models;
using APICatalogo.Pagination;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace APICatalogo.Controllers;

[ApiController]
[Route("[controller]")]
public class ProdutosController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProdutosController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    private ActionResult<IEnumerable<ProdutoDTO>> ObterProdutos(PagedList<Produto> produtos)
    {
        var metadata = new
        {
            produtos.TotalCount,
            produtos.PageSize,
            produtos.CurrentPage,
            produtos.TotalPages,
            produtos.HasNext,
            produtos.HasPrevious
        };
        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
        var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
        return Ok(produtosDto);
    }

    [HttpGet("produtos/{id}")]
    public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosCategoria(int id)
    {
        var produtos = _unitOfWork.ProdutoRepository.GetProdutosPorCategoria(id);

        if (produtos is null)
            return NotFound("Produtos não encontrados.");

        var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

        return Ok(produtosDTO);
    }

    [HttpGet("pagination")]
    public ActionResult<IEnumerable<ProdutoDTO>> Get(
        [FromQuery] ProdutosParameters produtosParameters)
    {
        var produtos = _unitOfWork.ProdutoRepository.GetProdutos(produtosParameters);
        return ObterProdutos(produtos);
    }

    [HttpGet("filter/preco/pagination")]
    public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosFilterPreco(
        [FromQuery] ProdutosFiltroPreco produtosFilterParameters)
    {
        var produtos = _unitOfWork.ProdutoRepository
            .GetProdutosFiltroPreco(produtosFilterParameters);
        return ObterProdutos(produtos);
    }

    [HttpGet]
    public ActionResult<IEnumerable<ProdutoDTO>> Get()
    {
        var produtos = _unitOfWork.ProdutoRepository.GetAll().ToList();

        if (produtos is null)
            return NotFound("Produtos não encontrados.");

        var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

        return Ok(produtosDTO);
    }

    [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
    public ActionResult<ProdutoDTO> Get(int id)
    {
        var produto = _unitOfWork.ProdutoRepository.GetById(p => p.ProdutoId == id);

        if (produto is null)
            return NotFound($"Produto com id = {id} não encontrado.");

        var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

        return Ok(produtoDTO);
    }

    [HttpPost]
    public ActionResult<ProdutoDTO> Post(ProdutoDTO produtoDTO)
    {
        if (produtoDTO is null)
            return BadRequest("Dados inválidos.");

        var produto = _mapper.Map<Produto>(produtoDTO);

        var created = _unitOfWork.ProdutoRepository.Create(produto);
        _unitOfWork.Commit();

        var createdDTO = _mapper.Map<ProdutoDTO>(created);

        return new CreatedAtRouteResult("ObterProduto",
            new { id = createdDTO.ProdutoId }, createdDTO);
    }

    [HttpPatch("{id}/UpdatePartial")]
    public ActionResult<ProdutoDTOUpdateResponse> Patch(int id,
        JsonPatchDocument<ProdutoDTOUpdateRequest> patchProdutoDto)
    {
        // valida input 
        if (patchProdutoDto == null || id <= 0)
            return BadRequest("Dados inválidos.");

        // obtem o produto pelo Id
        var produto = _unitOfWork.ProdutoRepository.GetById(p => p.ProdutoId == id);

        // se não econtrou retorna
        if (produto == null)
            return NotFound($"Produto com id = {id} não encontrado.");

        // mapeia produto para ProdutoDTOUpdateRequest
        var produtoUpdateRequest = _mapper.Map<ProdutoDTOUpdateRequest>(produto);

        // aplica as alterações definidas no documento JSON Patch ao objeto ProdutoDTOUpdateRequest
        patchProdutoDto.ApplyTo(produtoUpdateRequest, ModelState);

        if (!ModelState.IsValid || !TryValidateModel(produtoUpdateRequest))
            return BadRequest(ModelState);

        // mapeia as alterações de volta para a entidade Produto
        _mapper.Map(produtoUpdateRequest, produto);

        // atualiza a entidade no repositório
        _unitOfWork.ProdutoRepository.Update(produto);
        // salva as alterações no banco de dados
        _unitOfWork.Commit();

        // retorna ProdutoDTOUpdateResponse
        return Ok(_mapper.Map<ProdutoDTOUpdateResponse>(produto));
    }

    [HttpPut("{id:int}")]
    public ActionResult<ProdutoDTO> Put(int id, ProdutoDTO produtoDTO)
    {
        if (id != produtoDTO.ProdutoId)
            return BadRequest("Dados inválidos.");

        var produto = _mapper.Map<Produto>(produtoDTO);

        var updated = _unitOfWork.ProdutoRepository.Update(produto);
        _unitOfWork.Commit();

        var updatedDTO = _mapper.Map<ProdutoDTO>(updated);

        return Ok(updatedDTO);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<ProdutoDTO> Delete(int id)
    {
        var produto = _unitOfWork.ProdutoRepository.GetById(p => p.ProdutoId == id);

        if (produto is null)
            return NotFound($"Produto com id = {id} não encontrado.");

        var deleted = _unitOfWork.ProdutoRepository.Delete(produto);
        _unitOfWork.Commit();

        var deletedDTO = _mapper.Map<ProdutoDTO>(deleted);

        return Ok(deletedDTO);
    }
}
