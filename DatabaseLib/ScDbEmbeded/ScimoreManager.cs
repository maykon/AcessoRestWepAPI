namespace DatabaseLib
{
  using System;
  using System.Data.Common;
  using System.IO;
  using Dapper;
  using Scimore.Data.ScimoreClient;

  public class ScimoreManager : DbManager
  {
    public ScimoreManager(ConnectionInfo info) : base(info)
    {
    }

    protected ScimoreEmbedded Embedded { get; set; }

    public override bool Initialize()
    {
      if (Initialized)
        return false;
      Embedded = new ScimoreEmbedded();
      Embedded.MaxConnections = 16;
      Embedded.MemoryPages = 3000;
      Embedded.MaxLocks = 1000000;
      var exits = Directory.Exists(Path);
      if(!exits)
        Embedded.Create(Path);
      Embedded.Open(Path);

      Initialized = true;
      using (var conn = GetConnection(true))
      {
        if(!exits)
          conn.Execute($"CREATE DATABASE {DatabaseName}");
        conn.Execute($"USE {DatabaseName}");
      }
      
      return !exits;
    }

    public override void Dispose()
    {
      Embedded?.Dispose();
      Embedded = null;
    }

    public override DbConnection GetConnection(bool openConnection)
    {
      if (!Initialized)
        throw new ApplicationException("Not initialized");
      var conn = Embedded.CreateConnection($"Database={DatabaseName};");
      if (openConnection)
        conn.Open();
      return conn;
    }

    public override IDbAccess GetAccess()
    {
      return new ScimoreDbAccess(GetConnection(true));
    }
  }
}
