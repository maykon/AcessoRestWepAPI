namespace RegrasNegocio
{
  using DatabaseLib;
  using DatabaseLib.SqlServer;
  using System;
  using System.Collections.Generic;
  using System.Data;
  using System.Data.SqlClient;
  using System.Diagnostics.Contracts;

  public class PersistenciaFuncionario : IPersistenciaFuncionario
  {
    private IDbAccess Access { get; set; }

    public ILogWriter Log { get; protected set; }

    public string LogContent => Log.LogContent;

    public PersistenciaFuncionario(IDbAccess access, ILogWriter logWriter)
    {
      //Access = DiContainer.Resolve<IDbAccess>();
      Access = access;
      Log = logWriter;
    }

    public PersistenciaFuncionario()
    {
     
    }

    public List<Funcionario> RetornaFuncionarios()
    {
      List<Funcionario> funcionarios = (List<Funcionario>) Access.Query<Funcionario>("SELECT * FROM PESSOA");

      foreach(Funcionario funcionario in funcionarios)
      {
        List<Dependente> dependentes = (List<Dependente>)Access.Query<Dependente>($@"SELECT * FROM DEPENDENTE 
                                                                                     LEFT JOIN PESSOA
                                                                                       ON PESSOA.ID = DEPENDENTE.PESSOA
                                                                                     WHERE DEPENDENTE.PESSOA = {funcionario.id}");
        funcionario.dependentes = dependentes;
      }

      /*
      List<Funcionario> funcionarios = new List<Funcionario>();

      Access.Query<Funcionario, Dependente>(@"SELECT * FROM PESSOA
                                             LEFT JOIN DEPENDENTE
                                              ON DEPENDENTE.PESSOA = PESSOA.ID",
          (funcionario, dependente) =>
          {
            funcionarios.Add(funcionario);
            if (funcionario != null)
            {
              funcionarios[0].dependentes.Add(dependente);
            }
            return funcionarios.FirstOrDefault();
          }); */

      return funcionarios;
    }

    public int PersisteFuncionario(Funcionario funcionario)
    {
      if (funcionario == null)
        throw new ArgumentNullException("Funcionario não pode ser nulo!");

      Log.WriteLog("    Gravando dados do funcionario");
      var inicioGrava = DateTime.Now;

      var result = Access.Execute(@"UPDATE PESSOA SET SALARIO = @SALARIO
                                    WHERE ID = @ID", new { funcionario.salario, funcionario.id });


      Log.WriteLog($"    Novo salário calculado com sucesso");
      Log.WriteLog($"    Tempo de Gravação no banco         : {DateTime.Now - inicioGrava}");
      return result;
    }

    public int PersisteStatusFuncionario(Funcionario funcionario)
    {
        if (funcionario == null)
            throw new ArgumentNullException("Funcionario não pode ser nulo!");

        Log.WriteLog("    Gravando dados do funcionario");
        var inicioGrava = DateTime.Now;

        var result = Access.Execute(@"UPDATE PESSOA SET ATIVO = @ATIVO
                                WHERE ID = @ID", new { funcionario.ativo, funcionario.id });


        Log.WriteLog($"    Novo status alterado com sucesso");
        Log.WriteLog($"    Tempo de Gravação no banco         : {DateTime.Now - inicioGrava}");
        return result;
    }

    public int PersisteFuncionario(List<Funcionario> funcionarios)
    {
      if (funcionarios == null)
        throw new ArgumentNullException("Funcionarios não pode ser nula!");

      Log.WriteLog("    Gravando dados do funcionario");
      var inicioGrava = DateTime.Now;

      int result = 0;
      foreach (Funcionario funcionario in funcionarios)
      {
        result = Access.Execute(@"UPDATE PESSOA SET SALARIO = @SALARIO
                                    WHERE ID = @ID", new { funcionario.salario, funcionario.id });

      }

      Log.WriteLog($"    Novo salário calculado com sucesso");
      Log.WriteLog($"    Tempo de Gravação no banco         : {DateTime.Now - inicioGrava}");
      return result;
    }

    /* public int PersisteFuncionario(DataSet dataSet)
     {
       if (dataSet == null)
         throw new ArgumentNullException("DataSet não pode ser nula!");

       Log.WriteLog("    Gravando dados do funcionario");
       var inicioGrava = DateTime.Now;
       var result = Access.QueryUpdate("PESSOA", dataSet);
       Log.WriteLog($"    Novo salário calculado com sucesso");
       Log.WriteLog($"    Tempo de Gravação no banco         : {DateTime.Now - inicioGrava}");
       return result;
     } */

    public DataSet RetornaFuncionariosDataSet()
    {
      var dsMain = new DataSet();
      Access.QueryFill(dsMain, "PESSOA", "SELECT * FROM PESSOA ");
      Access.QueryFill(dsMain, "DEPENDENTE", "SELECT * FROM DEPENDENTE ");

      dsMain.Relations.Add(new DataRelation(
        "PESSOA_DEPEND", dsMain.Tables["PESSOA"].Columns["ID"],
        dsMain.Tables["DEPENDENTE"].Columns["PESSOA"],
        true));
      return dsMain;
    }

    public void ClearLog()
    {
      Log.ClearLog();
    }

    public void Dispose()
    {
      Access?.Dispose();
      Access = null;
    }
  }
}
