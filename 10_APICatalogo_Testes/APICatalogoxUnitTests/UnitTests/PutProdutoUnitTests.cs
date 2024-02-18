using APICatalogo.Controllers;
using APICatalogo.DTO;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogoxUnitTests.UnitTests;

public class PutProdutoUnitTests : IClassFixture<ProdutosUnitTestController>
{
    private readonly ProdutosController _controller;

    public PutProdutoUnitTests(ProdutosUnitTestController controller)
    {
        _controller = new ProdutosController(controller.repository, controller.mapper);
    }

    [Fact]
    public async Task PutProduto_Return_OkResult()
    {
        var prodId = 14;
        var updatedProdutoDto = new ProdutoDTO
        {
            ProdutoId = prodId,
            Nome = "Produto Atualizado - Testes",
            Descricao = "Minha Descricao",
            ImagemUrl = "imagem1.jpg",
            CategoriaId = 2
        };
        var result = await _controller.Put(prodId, updatedProdutoDto) as ActionResult<ProdutoDTO>;
        result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task PutProduto_Return_BadRequest()
    {
        var prodId = 1000;
        var meuProduto = new ProdutoDTO
        {
            ProdutoId = 14,
            Nome = "Produto Atualizado - Testes",
            Descricao = "Minha Descricao alterada",
            ImagemUrl = "imagem11.jpg",
            CategoriaId = 2
        };
        var data = await _controller.Put(prodId, meuProduto);
        data.Result.Should().BeOfType<BadRequestResult>().Which.StatusCode.Should().Be(400);
    }
}
