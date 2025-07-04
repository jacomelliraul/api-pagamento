using MySql.Data.MySqlClient;
using PagamentoAPI.Model.Entities;
using PagamentoAPI.Model.Enum;
using PagamentoAPI.Repository;

public class PagamentoRepository
{
    private readonly MySqlDbContext _dbContext;
    private readonly ILogger<PagamentoRepository> _logger;

    public PagamentoRepository(MySqlDbContext dbContext, ILogger<PagamentoRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public int Adicionar(Pagamento pagamento)
    {
        int novoId = 0;

        try
        {
            using var cmd = _dbContext.GetConnection().CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Transacao (Valor, Cartao, CVV, Parcelas, Situacao)
                VALUES (@valor, @cartao, @cvv, @parcelas, @situacao);
                SELECT LAST_INSERT_ID();";

            cmd.Parameters.AddWithValue("@valor", pagamento.valor);
            cmd.Parameters.AddWithValue("@cartao", pagamento.cartao);
            cmd.Parameters.AddWithValue("@cvv", pagamento.CVV);
            cmd.Parameters.AddWithValue("@parcelas", pagamento.parcelas);
            cmd.Parameters.AddWithValue("@situacao", (int)pagamento.situacao);

            novoId = Convert.ToInt32(cmd.ExecuteScalar());
        }
        catch (MySqlException ex)
        {
            _logger.LogError(ex, "Erro ao inserir transação");
            throw;
        }

        return novoId;
    }

    public StatusPagamento? ObterSituacaoPorId(int id)
    {
        try
        {
            using var cmd = _dbContext.GetConnection().CreateCommand();
            cmd.CommandText = "SELECT Situacao FROM Transacao WHERE TransacaoId = @id";
            cmd.Parameters.AddWithValue("@id", id);

            var result = cmd.ExecuteScalar();

            if (result != null && int.TryParse(result.ToString(), out int situacaoInt))
            {
                return (StatusPagamento)situacaoInt;
            }

            return null;
        }
        catch (MySqlException ex)
        {
            _logger.LogError(ex, "Erro ao consultar situação do pagamento");
            throw;
        }
    }

    public bool AtualizarSituacao(int id, StatusPagamento novaSituacao)
    {
        try
        {
            using var cmd = _dbContext.GetConnection().CreateCommand();
            cmd.CommandText = @"
            UPDATE Transacao 
            SET Situacao = @novaSituacao 
            WHERE TransacaoId = @id";

            cmd.Parameters.AddWithValue("@novaSituacao", (int)novaSituacao);
            cmd.Parameters.AddWithValue("@id", id);

            int linhasAfetadas = cmd.ExecuteNonQuery();
            return linhasAfetadas > 0;
        }
        catch (MySqlException ex)
        {
            _logger.LogError(ex, "Erro ao atualizar situação do pagamento");
            throw;
        }
    }


}
