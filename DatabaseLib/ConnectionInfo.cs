namespace DatabaseLib
{
  public struct ConnectionInfo
  {
    public ConnectionInfo(string path, string databaseName, string serverName = null, string userName = null, string password=null )
    {
      DatabaseName = databaseName;
      ServerName = serverName;
      UserName = userName;
      Password = password;
      Path = path;
    }
    public string DatabaseName { get; set; }
    public string ServerName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Path { get; set; }
  }
}
