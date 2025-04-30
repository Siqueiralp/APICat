using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
namespace APICat.Models;

public class ProdutoQOL
{
    public String Chassis { get; set; }
    [Key]
    public String? NumSerie { get; set; }
}
