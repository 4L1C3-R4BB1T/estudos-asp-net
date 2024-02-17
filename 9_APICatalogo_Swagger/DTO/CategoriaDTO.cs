using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTO;

public class CategoriaDTO
{
    public int CategoriaId { get; set; }

    [Required]
    [StringLength(80)]
    public string? Nome { get; set; }

    [Required]
    [StringLength(80)]
    public string? ImagemUrl { get; set; }
}
