using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
namespace APICat.Models;

public class CorQOL
{
    [Key]
    public long Codigo { get; set; }
    public String? Nome { get; set; }

}
