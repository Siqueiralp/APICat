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

        [HttpPut("{NSerie},{Chassi},{CorErrada},{CorCerta}")]
        public ActionResult Put(String NSerie, int CorErrada, String Chassi, int CorCerta)
        {
            var _processo = _context.QOLCAB_Processo.FirstOrDefault(q => q.NumSerieProduto == NSerie); // Obtem os dados relativos ao n. de serie informado
            var _produto = _context.PRODUTOQOL.FirstOrDefault(p => p.NumSerie == NSerie); // Obtem os dados relativos ao n. de serie informado
            if(_processo == null || _produto == null) // Verifica se o n. de serie informado existe na base de dados.
            {
                return NotFound("N. de série não encontrado.");
            }
            if (Chassi != _produto.Chassis)// inserir no PUT a lógica para verificar se o chassi corresponde ao ID.
            {
                return BadRequest("O chassi não corresponde ao numero de serie informado.");
            }
            if (CorErrada != _processo.CorCodigo)// inserir no PUT a lógica para verificar se o chassi corresponde ao ID.
            {
                return BadRequest("A cabine encontrada não possui a cor errada informada.");
            }
            var _colorcheck = _context.QOLCAB_Cor.FirstOrDefault(cc => cc.Codigo == CorCerta);
            if (_colorcheck == null)
            {
                return BadRequest("O código da cor a ser alterada não existe.");
            }
            _processo.CorCodigo = CorCerta; // Altera a cor para o código informado.
            _context.Entry(_processo).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return Ok(_processo);
        }

    }
}

