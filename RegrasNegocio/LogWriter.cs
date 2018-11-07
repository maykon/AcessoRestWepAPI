namespace RegrasNegocio
{
  using System.Text;

  public class LogWriter : ILogWriter
  {

    protected StringBuilder Content = new StringBuilder();

    public string LogContent => Content.ToString();

    public void ClearLog()
    {
      Content.Clear();
    }

    public void WriteLog(string content)
    {
      Content.AppendLine(content);
    }
  }
}
