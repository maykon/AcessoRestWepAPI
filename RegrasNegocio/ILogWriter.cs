namespace RegrasNegocio
{
  public interface ILogWriter
  {
    string LogContent { get; }
    void WriteLog(string content);
    void ClearLog();
  }
}
