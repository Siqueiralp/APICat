using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICat.Models;

public class ProcessoQOL // Table para update e verificação
{
    public int NumSerieProduto { get; set; }
    public int CorCodigo { get; set; }
}
