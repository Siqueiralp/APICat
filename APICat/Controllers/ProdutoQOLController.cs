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
        public ProdutoQOLController(AppDbContext context)
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

        [HttpPost("trocacor")]
        public ActionResult PostTrocaCor(
            [FromQuery] string NSerie,
            [FromQuery] string Chassi,
            [FromQuery] int CorErrada,
            [FromQuery] int CorCerta)
        {
            try
            {
                var _processo =  GetProcessoQOL(NSerie);
                var _produto = GetProdutoQol(NSerie);
                ValidateChassisOnQOLDataBase(Chassi, NSerie);

                if (CorErrada != _processo.CorCodigo) // Verifica se o chassi realmente possui a cor errada informada.
                {
                    return BadRequest($"A cabine de n. de chassi {Chassi} não possui a cor {CorErrada}, atualmente ela esta com a cor: {_processo.CorCodigo}.");
                }

                var _colorcheck = _context.QOLCAB_Cor.FirstOrDefault(cc => cc.Codigo == CorCerta);
                if (_colorcheck == null) // Verifica se a cor informada existe no banco de dados.
                {
                    return BadRequest($"O código da cor ({CorCerta}) a ser alterada não existe na tabela de Cores [dbo.QOLCAB_Cor].");
                }

                _processo.CorCodigo = CorCerta; // Altera a cor para o código informado.
                _context.Entry(_processo).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
                return Ok(_processo);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                //// Salvar no banco de dados o erro, enviar um teams pro Dev e abrir um INC
                //// Rollback
                return BadRequest($"Erro no sistema, valide por gentileza o erro na mensagem a seguir, Exception: {ex.Message}");
            }
            finally
            {
                /// Fechar a conexão do banco de dados
            }
        }

        /// <summary>
        /// Verifica se o Chassi informado corresponde ao número de série informado.
        /// </summary>
        /// <param name="chassi">Chassis que foi enviado pelo request.</param>
        /// <param name="nSerie">Numero de série enviado pelo request.</param>
        /// <param name="produto"></param>
        /// <exception cref="InvalidOperationException"></exception>
        private void ValidateChassisOnQOLDataBase(string chassi, string nSerie)
        {
            var _produto = _context.PRODUTOQOL.FirstOrDefault(p => p.NumSerie == nSerie); // Obtem os dados relativos ao n. de serie informado
            if (chassi != _produto.Chassis) // Verifica se o Chassi informado corresponde ao número de série informado.
            {
                throw new InvalidOperationException($"O chassis: {chassi} não corresponde ao numero de serie: {nSerie} informado pelo QOL, Chassis informado no QOL: {_produto.Chassis}.");
            }
        }

        /// <summary>
        /// Obtém o objeto do Produto da tabela QOL com base no Numero de Série.
        /// </summary>
        /// <param name="nSerie">Número de série enviado pelo request.</param>
        /// <returns>Objeto do produto QOL com base no número de série.</returns>
        /// <exception cref="InvalidOperationException">Caso não seja encontrado o objeto na tabela QOL ele retorna a mensagem de erro.</exception>
        private ProdutoQOL GetProdutoQol(string nSerie)
        {
            var _produto = _context.PRODUTOQOL.FirstOrDefault(p => p.NumSerie == nSerie);
            if (_produto == null)
            {
                throw new InvalidOperationException($"N. de série {nSerie} não encontrado na tabela dbo.PRODUTOQOL. Verifique a integração.");
            }

            return _produto;
        }

        /// <summary>
        /// Obtém o objeto do Processo da tabela QOL com base no Numero de Série.
        /// </summary>
        /// <param name="nSerie"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private ProcessoQOL GetProcessoQOL(string nSerie)
        {
            var _processo = _context.QOLCAB_Processo.FirstOrDefault(q => q.NumSerieProduto == nSerie);
            if (_processo == null)
            {
                throw new InvalidOperationException($"N. de série {nSerie} não encontrado na tabela dbo.QOLCAB_Processo. Verifique o app CTCAB.");
            }

            return _processo;
        }
    }
}

