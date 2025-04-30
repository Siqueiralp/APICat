using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
namespace APICat.Models;

public class Categoria
{
    public ProdutoQOL()
    {
        Cabines = new Collection<QOLCAB_Processo>();
    }
    [Key]
    public int CategoriaId { get; set; }

    [Required]
    [StringLength(80)]
    public string? Nome { get; set; } // prop + tab tab cria uma property nova
    [Required]
    [StringLength(300)]
    public string? ImgUrl { get; set; }

    public ICollection<Produto>? Produtos { get; set; }
}
