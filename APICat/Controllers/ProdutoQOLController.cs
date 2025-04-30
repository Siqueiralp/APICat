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

        [HttpGet("{id}", Name ="ObterNumSerie")]
        public ActionResult<ProdutoQOL> Get(String id)
        {
            var cabine = _context.PRODUTOQOL.FirstOrDefault(p => p.Chassis == id);
            if (cabine != null)
            {
                var _cabine = _context.QOLCAB_Processo.FirstOrDefault(q => q.NumSerieProduto == cabine.NumSerie);
                return Ok(_cabine);
            }
            var cor = _context.PRODUTOQOL.FirstOrDefault(p => p.Chassis == id);
            if (cabine is null) return NotFound("Chassi não encontrado.");

            return Ok(cabine);
        }

        [HttpPut("{id}")]
        public ActionResult Put(String id, ProcessoQOL categoria)
        {
            if (id != categoria.NumSerieProduto)
            {
                return BadRequest();
            }
            _context.Entry(categoria).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return Ok(categoria);
        }

    }
}

