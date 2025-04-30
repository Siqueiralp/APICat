using Microsoft.AspNetCore.Mvc;
using APICat.Context;
using APICat.Models;
using Microsoft.EntityFrameworkCore;
namespace APICat.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:long}", Name="ObterCor")]
        public ActionResult<ProcessoQOL> Get(long id)
        {
            var produto = _context.QOLCAB_Processo.AsNoTracking().FirstOrDefault(p => p.NumSerieProduto == id);
            if (produto is null)
            {
                return NotFound("Numero de série não encontrado.");
            }
            return produto;
        }
        
    }
}
