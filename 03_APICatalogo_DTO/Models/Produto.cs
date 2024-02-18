using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using APICatalogo.Validations;

namespace APICatalogo.Models;

[Table("Produtos")]
public class Produto
{
    [Key]
    public int ProdutoId { get; set; }

    [PascalCase]
    [StringLength(80)]
    [Required(ErrorMessage = "O nome é obrigatório.")]
    public string? Nome { get; set; }

    [StringLength(300)]
    [Required(ErrorMessage = "A descrição é obrigatória.")]
    public string? Descricao { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    [Required(ErrorMessage = "O preço é obrigatório.")]
    public decimal Preco { get; set; }

    [StringLength(300)]
    [Required(ErrorMessage = "A url é obrigatória.")]
    public string? ImagemUrl { get; set; }

    public float Estoque { get; set; }

    public DateTime DataCadastro { get; set; }

    public int CategoriaId { get; set; }

    [JsonIgnore]
    public Categoria? Categoria { get; set; }
}
