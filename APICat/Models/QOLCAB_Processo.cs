using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICat.Models;

public class ProcessoQOL // Table para update e verificação
{
    [Key]
    public String NumSerieProduto { get; set; }
    public long CorCodigo { get; set; }
}
