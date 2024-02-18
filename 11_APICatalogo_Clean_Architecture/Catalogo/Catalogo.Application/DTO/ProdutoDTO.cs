using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catalogo.Application.DTOs;

public class ProdutoDTO
{
    public int Id { get; set; }

    [MinLength(3)]
    [MaxLength(100)]
    [Required(ErrorMessage = "O nome é obrigatório")]
    public string Nome { get; set; }

    [MinLength(5)]
    [MaxLength(200)]
    [Required(ErrorMessage = "A descrição é obrigatória")]
    public string Descricao { get; set; }

    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18,2)")]
    [DisplayFormat(DataFormatString = "{0:C2}")]
    [Required(ErrorMessage = "Informe o preço")]
    public decimal Preco { get; set; }

    [MaxLength(250)]
    public string ImagemUrl { get; set; }

    [Range(1, 9999)]
    [Required(ErrorMessage = "O estoque é obrigatório")]
    public int Estoque { get; set; }

    [Required(ErrorMessage = "Informe a data do cadastro")]
    public DateTime DataCadastro { get; set; }

    public int CategoriaId { get; set; }
}
