using APICatalogo.Context;
using APICatalogo.DTO.Mappings;
using APICatalogo.Repositories.Interfaces;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace APICatalogoxUnitTests.UnitTests;

public class ProdutosUnitTestController
{
    public IUnitOfWork repository;
    public IMapper mapper;
    public static DbContextOptions<AppDbContext> dbContextOptions { get; }
    public static string connectionString = "Server=localhost;DataBase=CatalogoDB;Uid=root;Pwd=";

    static ProdutosUnitTestController()
    {
        dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            .Options;
    }

    public ProdutosUnitTestController()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile(new ProdutoDTOMappingProfile()));
        mapper = config.CreateMapper();
        var context = new AppDbContext(dbContextOptions);
        repository = new UnitOfWork(context);
    }
}
