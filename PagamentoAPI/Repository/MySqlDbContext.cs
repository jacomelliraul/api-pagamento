﻿using MySql.Data.MySqlClient;

namespace PagamentoAPI.Repository
{
    public class MySqlDbContext :IDisposable
    {
        private readonly MySqlConnection _conexao;
        private readonly AppSettings _appSettings;
        public MySqlDbContext(AppSettings appSettings)
        {
            
            _appSettings = appSettings;
            _conexao = new MySqlConnection(_appSettings.StringConexao);
        }

        public MySqlConnection GetConnection() {

            if (_conexao.State == System.Data.ConnectionState.Closed)
                _conexao.Open();

            return _conexao;    
        }

        public void Dispose()
        {
            _conexao.Close();

            _conexao.Dispose();
        }


    }
}
