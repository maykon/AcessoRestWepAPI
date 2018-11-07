namespace DatabaseLib
{
  using System.Data;
  using System.Data.Common;
  using Dapper;
  using System.Linq;
  using Scimore.Data.ScimoreClient;

  public class ScimoreDbAccess : DbAccess
  {
    public ScimoreDbAccess(DbConnection connection) : base(connection)
    {
    }

    protected override void LoadReader(DataTable table, IDataReader reader)
    {
      ScimoreDataReader newReader = (ScimoreDataReader)((IWrappedDataReader)reader).Reader;
      var pkNames = newReader.Fields.Where(x => x.IsPrimaryKey).Select(x => x.FieldName).ToArray();
      foreach(var field in newReader.Fields)
      {
        if (!table.Columns.Contains(field.FieldName))
        {
          table.Columns.Add(field.FieldName, field.FieldType);
          table.Columns[field.FieldName].AllowDBNull = field.IsNullable;
        }
      }

      while (newReader.Read())
      {
        var row = table.NewRow();
        row.BeginEdit();
        foreach (var field in newReader.Fields)
          row[field.FieldName] = field.Value;
        row.EndEdit();
        table.Rows.Add(row);
      }

      if (pkNames.Length > 0)
        table.PrimaryKey = pkNames.Select(x => table.Columns[x]).ToArray();
    }


    public void FillErrorEventHandler(object sender, FillErrorEventArgs e)
    {
      e.Continue = true;
    }
  }
}
