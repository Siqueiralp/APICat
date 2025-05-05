using APICat.Context;
using APICat.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace APICat.Controllers
{
    [Route("findSnByChassi")]
    [ApiController]
    public class ProdutoQOLController : ControllerBase
    {
        private readonly AppDbContext _context; // insere a referencia classe do contexto no documento
        public ProdutoQOLController(AppDbContext context) // Cria uma instância de contexto de conexão à db.
        {
            _context = context;
        }

        [HttpGet("{Chassi}", Name ="ObterNumSerie")]
        public ActionResult<ProdutoQOL> Get(String Chassi)
        {
            var cabine = _context.PRODUTOQOL.FirstOrDefault(p => p.Chassis == Chassi);
            if (cabine != null)
            {
                var _cabine = _context.QOLCAB_Processo.FirstOrDefault(q => q.NumSerieProduto == cabine.NumSerie);
                return Ok(_cabine);
            }
            var cor = _context.PRODUTOQOL.FirstOrDefault(p => p.Chassis == Chassi);
            if (cabine is null) return NotFound("Chassi não encontrado.");

            return Ok(cabine);
        }

        [HttpPut("{NSerie},{Chassi}")]
        public ActionResult Put(String NSerie, ProcessoQOL categoria)
        {
 
            if (NSerie != categoria.NumSerieProduto)// inserir no PUT a lógica para verificar se o chassi corresponde ao ID.
            {
                return BadRequest();
            }
            _context.Entry(categoria).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return Ok(categoria);
        }

    }
}

