using APICat.Models;
using Microsoft.EntityFrameworkCore; // permite a herança de Db:Context.

namespace APICat.Context
{
    public class AppDbContext : DbContext // herda de using.Microsoft.EntitiyFrameworkCore | Conecta com a DB.
    {
        /* a palavra chave (base) indica herença da classe na qual a classe está inclusa. No casso de AppDbContext, usa a classe 
         * DbContext definda acima como base para a estruturação da variável options.
         */
        public AppDbContext(DbContextOptions<AppDbContext> options) : base( options ) // Consultor | <armazena nome do contexto> options = variavel de armazenamento das informações de conexão
        {}

        public DbSet<ProdutoQOL>? PRODUTOQOL { get; set; }
        public DbSet<ProcessoQOL>? QOLCAB_Processo { get; set; }
        public DbSet<Categoria>? Categorias { get; set; }
        public DbSet<Produto>? Produtos { get; set; }
    }
}