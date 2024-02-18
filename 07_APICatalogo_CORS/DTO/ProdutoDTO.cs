using System.ComponentModel.DataAnnotations;
using APICatalogo.Validations;

namespace APICatalogo.DTO;

public class ProdutoDTO
{
    public int ProdutoId { get; set; }

    [PascalCase]
    [StringLength(80)]
    [Required(ErrorMessage = "O nome é obrigatório.")]
    public string? Nome { get; set; }

    [StringLength(300)]
    [Required(ErrorMessage = "A descrição é obrigatória.")]
    public string? Descricao { get; set; }

    [Required(ErrorMessage = "O preço é obrigatório.")]
    public decimal Preco { get; set; }

    [StringLength(300)]
    [Required(ErrorMessage = "A url é obrigatória.")]
    public string? ImagemUrl { get; set; }

    public int CategoriaId { get; set; }
}
