using MySql.Data.MySqlClient;

namespace PagamentoAPI.Repository
{
    public class CartaoRepository
    {
        private readonly MySqlDbContext _dbContext;
        private readonly ILogger<CartaoRepository> _logger;

        public CartaoRepository(MySqlDbContext dbContext, ILogger<CartaoRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public bool VerificarCartaoValido(string numeroCartao)
        {
            bool valido = false;

            try
            {
                using (MySqlCommand cmd = _dbContext.GetConnection().CreateCommand())
                {
                    cmd.CommandText = @"
                    SELECT Validade 
                    FROM Cartao 
                    WHERE Numero = @numeroCartao 
                    LIMIT 1";

                    cmd.Parameters.AddWithValue("@numeroCartao", numeroCartao);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            DateTime validade = reader.GetDateTime("Validade");

                            // Comparar com a data atual
                            if (validade > DateTime.Now)
                            {
                                valido = true;
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                _logger.LogError(ex, "Erro ao verificar validade do cartão");
                throw;
            }

            return valido;
        }
    }
}
