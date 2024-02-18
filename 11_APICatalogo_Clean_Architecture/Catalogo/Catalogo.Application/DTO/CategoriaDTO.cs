using System.ComponentModel.DataAnnotations;

namespace Catalogo.Application.DTOs;

public class CategoriaDTO
{
    public int Id { get; set; }

    [MinLength(3)]
    [MaxLength(100)]
    [Required(ErrorMessage = "Informe o nome da categoria")]
    public string Nome { get; set; }

    [MinLength(5)]
    [MaxLength(250)]
    [Required(ErrorMessage = "Informe o nome da imagem")]
    public string ImagemUrl { get; set; }
}
