using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLib.SqlServer
{
  public class SqlServerManager : DbManager
  {
    public SqlServerManager(ConnectionInfo info) : base(info)
    {
    }


    public static string GetConnecionString(string server, string dbName, string user, string pass)
    {
      var builder = new SqlConnectionStringBuilder();
      builder.DataSource = server;
      builder.InitialCatalog = dbName;
      builder.UserID = user;
      builder.Password = pass;
      return builder.ConnectionString;
    }

    public override DbConnection GetConnection(bool openConnection)
    {
      var conn = new SqlConnection(GetConnecionString(Server,DatabaseName,UserName, Info.Password));
      if (openConnection)
        conn.Open();
      return conn;
    }

    public override bool Initialize()
    {
      return true;
    }
  }
}
