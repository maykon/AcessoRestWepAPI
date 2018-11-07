namespace DatabaseLib
{
  using System;
  using System.Data.Common;

  public abstract class DbManager : IDbManager
  {

    protected ConnectionInfo Info { get; set; }

    public DbManager(ConnectionInfo info)
    {
      Info = info;
    }

    public virtual string DatabaseName => Info.DatabaseName;

    public virtual string Path => Info.Path;

    public virtual bool Initialized { get; protected set; }

    public string Server => Info.ServerName;

    public string UserName => Info.UserName;

    public virtual void Dispose()
    {
      throw new NotImplementedException();
    }

    public virtual IDbAccess GetAccess()
    {
      return new DbAccess(GetConnection(true));
    }

    public abstract DbConnection GetConnection(bool openConnection);

    public abstract bool Initialize();
  }
}
