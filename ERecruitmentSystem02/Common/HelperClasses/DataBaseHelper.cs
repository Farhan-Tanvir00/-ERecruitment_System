using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace ERecruitmentSystem02.Common.HelperClasses
{
    public sealed class DataBaseHelper
    {
        private readonly string _connectionString;

        public DataBaseHelper(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("OracleDbConnection");
        }

        public async Task<OracleConnection> GetConnectionAsync()
        {
            var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();
            return conn;
        }

        public async Task<OracleCommand> CreateStoredProcedureCommandAsync(string procedureName, OracleConnection conn, OracleTransaction transaction)
        {
            var cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = procedureName;
            cmd.Transaction = transaction;

            return cmd;
        }

        public async Task<OracleCommand> CreateStoredProcedureCommandAsync(string procedureName)
        {
            var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = procedureName;

            return cmd;
        }

        public void AddInputParameter(OracleCommand cmd, string name, OracleDbType type, object value)
        {
            var param = cmd.Parameters.Add(name, type);
            param.Value = value ?? DBNull.Value;
        }

        public void AddOutputParameter(OracleCommand cmd, string name, OracleDbType type, int size = 2000)
        {
            var param = cmd.Parameters.Add(name, type, size);
            param.Direction = ParameterDirection.Output;
        }

        public void AddRefCursor(OracleCommand cmd, string name)
        {
            var param = cmd.Parameters.Add(name, OracleDbType.RefCursor);
            param.Direction = ParameterDirection.Output;
        }

        public async Task ExecuteNonQueryAsync(OracleCommand cmd)
        {
            await cmd.ExecuteNonQueryAsync();
            await cmd.Connection.CloseAsync();
        }


        public async Task NccExecuteNonQueryAsync(OracleCommand cmd)
        {
            await cmd.ExecuteNonQueryAsync();
        }
        public async Task<OracleDataReader> ExecuteReaderAsync(OracleCommand cmd)
        {
            var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
            return reader;
        }
    }
}
