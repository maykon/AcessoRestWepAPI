namespace DatabaseLib
{
  using System;
  using System.Data.Common;

  public interface IDbManager : IDisposable
  {
    string DatabaseName { get; }
    string Path { get; }

    string Server { get; }

    string UserName { get; }
    bool Initialized { get;}
    DbConnection GetConnection(bool openConnection);
    bool Initialize();


    IDbAccess GetAccess();
  }
}
