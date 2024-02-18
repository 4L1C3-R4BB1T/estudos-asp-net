using APICatalogo.Controllers;
using APICatalogo.DTO;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogoxUnitTests.UnitTests;

public class GetProdutoUnitTests : IClassFixture<ProdutosUnitTestController>
{
    private readonly ProdutosController _controller;

    public GetProdutoUnitTests(ProdutosUnitTestController controller)
    {
        _controller = new ProdutosController(controller.repository, controller.mapper);
    }

    [Fact]
    public async Task GetProdutoById_Return_OKResult()
    {
        var prodId = 2; // Arrange
        var data = await _controller.Get(prodId); // Act
        // Assert (fluentassertions)
        data.Result.Should().BeOfType<OkObjectResult>()
            .Which.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task GetProdutoById_Return_NotFound()
    {
        var prodId = 999;
        var data = await _controller.Get(prodId);
        data.Result.Should().BeOfType<NotFoundObjectResult>()
            .Which.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task GetProdutoById_Return_BadRequest()
    {
        int prodId = -1;
        var data = await _controller.Get(prodId);
        data.Result.Should().BeOfType<BadRequestObjectResult>()
            .Which.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task GetProdutos_Return_ListOfProdutoDTO()
    {
        var data = await _controller.Get();
        data.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeAssignableTo<IEnumerable<ProdutoDTO>>()
            .And.NotBeNull();
    }

    [Fact]
    public async Task GetProdutos_Return_BadRequestResult()
    {
        var data = await _controller.Get();
        data.Result.Should().BeOfType<BadRequestResult>();
    }
}
