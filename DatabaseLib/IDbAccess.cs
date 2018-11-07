namespace DatabaseLib
{
  using System;
  using System.Collections.Generic;
  using System.Data;
  using System.Data.Common;


  public interface IDbAccess : IDisposable
  {
    int QueryUpdate(DataTable table);

    int QueryUpdate(DataRow row);

    int QueryUpdate(string tableName, DataSet dataSet);
    IDbConnection Connection { get; }
    void QueryFill( DataSet dataSet, string tableName, string sql, object param = null, IDbTransaction trans = null, bool buffered = true, int? timeout = null, CommandType? commandType = null);
    DataTable QuerySelect(string tableName, string sql, object param = null, IDbTransaction trans = null, bool buffered = true, int? timeout = null, CommandType? commandType = null);
    IEnumerable<T> Query<T>(string sql, object param = null, IDbTransaction trans = null, bool buffered = true, int? timeout = null, CommandType? commandType = null);
    int Execute(string sql, object param = null, IDbTransaction trans = null, int? timeout = null, CommandType? commandType = null);

    T ExecuteScalar<T>(string sql, object param = null, IDbTransaction trans = null, bool buffered = true, int? timeout = null, CommandType? commandType = null);
  }
}
