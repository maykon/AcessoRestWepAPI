namespace DatabaseLib
{
  using System.Collections.Generic;
  using System.Data.Common;
  using Dapper;
  using System.Data;
  using System.Linq;
  using System;

  public class DbAccess : IDbAccess
  {

    public DbAccess(IDbConnection connection)
    {
      Connection = connection;
    }
    public IDbConnection Connection { get; protected set; }

    public void Dispose()
    {
      Connection?.Dispose();
      Connection = null;
    }

    public int Execute(string sql, object param=null, IDbTransaction trans=null, int? timeout=null, CommandType? commandType=null)
    {
      return Connection.Execute(sql, param, trans, timeout, commandType);
    }

    public IEnumerable<T> Query<T>(string sql, object param = null, IDbTransaction trans = null, bool buffered = true, int? timeout = null, CommandType? commandType = null)
    {
      return Connection.Query<T>(sql, param, trans, buffered, timeout, commandType);
    }


    public T ExecuteScalar<T>(string sql, object param = null, IDbTransaction trans = null, bool buffered = true, int? timeout = null, CommandType? commandType = null)
    {
      return Connection.ExecuteScalar<T>(sql, param, trans, timeout, commandType);
    }

    public DataTable QuerySelect(string tableName,string sql, object param = null, IDbTransaction trans = null, bool buffered = true, int? timeout = null, CommandType? commandType = null)
    {
      using (var reader = Connection.ExecuteReader(sql, param, trans, timeout, commandType))
      {
        return LoadReader(tableName, reader);
      }
    }


    public delegate void QueryFillDelegate(DataSet dataSet, string tableName, string sql, object param = null, IDbTransaction trans = null, bool buffered = true, int? timeout = null, CommandType? commandType = null);

    public void QueryFill(DataSet dataSet, string tableName, string sql, object param = null, IDbTransaction trans = null, bool buffered = true, int? timeout = null, CommandType? commandType = null)
    {
      using (var reader = Connection.ExecuteReader(sql, param, trans, timeout, commandType))
      {
        var table = dataSet.Tables.Contains(tableName) ? dataSet.Tables[tableName] : new DataTable(tableName);
        LoadReader(table, reader);
        if (!dataSet.Tables.Contains(tableName))
          dataSet.Tables.Add(table);
      }
    }


    protected DbDataAdapter CreateAdapter(string tableName)
    {
      var types = Connection.GetType().Assembly.GetTypes();
      var adapterType = types.Where(x => x.GetInterfaces().Any(y => y == typeof(IDbDataAdapter))).FirstOrDefault();
      if (adapterType != null)
      {

        var selectCommand = Connection.CreateCommand();
        selectCommand.CommandText = $"SELECT * FROM {tableName} WHERE 0=1";
        var adapter = (DbDataAdapter)Activator.CreateInstance(adapterType, selectCommand);
        var ds = new DataSet();
        adapter.Fill(ds);
        PrepareAdapter(adapter);
        return adapter;
      }
      else
        throw new ApplicationException("IDbDataAdapterClass not found!");

    }



    public int QueryUpdate(DataTable table)
    {
      using (var adapter = CreateAdapter(table.TableName))
      {
        var result = adapter.Update(table);
        table.AcceptChanges();
        return result;
      }
    }

    public int QueryUpdate(string tableName, DataSet dataSet)
    {
      using (var adapter = CreateAdapter(tableName))
      {
        var result = adapter.Update(dataSet);
        dataSet.AcceptChanges();
        return result;
      }
    }

    public int QueryUpdate(DataRow row)
    {
      using (var adapter = CreateAdapter(row.Table.TableName))
      {
        var result = adapter.Update(new DataRow[] { row });
        row.AcceptChanges();
        return result;
      }
    }



    protected virtual void PrepareAdapter(DbDataAdapter adapter)
    {
      var types = Connection.GetType().Assembly.GetTypes();
      var adapterType = types.Where(x => x.BaseType == typeof(DbCommandBuilder)).FirstOrDefault();
      if (adapterType != null)
      {
        var builder = (DbCommandBuilder)Activator.CreateInstance(adapterType, adapter);
        adapter.InsertCommand = builder.GetInsertCommand();
        adapter.UpdateCommand = builder.GetUpdateCommand();
        adapter.DeleteCommand = builder.GetDeleteCommand();
      }
    }


    protected virtual void LoadReader(DataTable table, IDataReader reader)
    {
      table.Load(reader);
    }

    protected DataTable LoadReader(string tableName,IDataReader reader)
    {
      var table = new DataTable(tableName);
      LoadReader(table, reader);
      return table;
    }
  }
}
