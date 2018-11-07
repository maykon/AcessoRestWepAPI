namespace UnitTestsProject
{
  using DatabaseLib;
  using RegrasNegocio;
  using Rhino.Mocks;
  using System;
  using System.Collections.Generic;
  using System.Data;


  public class ClasseBaseTestesUnitarios
  {
    #region metodos auxiliares


    protected Funcionario CriaTabelaPessoa(decimal salario, char situacao, DateTime admissao)
    {
      var funcionario = new Funcionario()
      {
        id = 1,
        ativo = situacao,
        nome = "NomeFuncionário",
        email = "email@email.com",
        nascimento = new DateTime(1990,1,1),
        inicioContrato = admissao,
        sexo = 'M',
        salario = salario
      };

      return funcionario;
    }

    protected Funcionario CriaTabelaFuncionario(int id, char status)
    {
        var funcionario = new Funcionario()
        {
            id = id,
            ativo = status,
            nome = "NomeFuncionário",
            email = "email@email.com",
            nascimento = new DateTime(1990, 1, 1),
            inicioContrato = DateTime.Now,
            sexo = 'M',
            salario = 1000
        };

        return funcionario;
    }

    protected List<Dependente> CriatabelaDependentes(int idDependente, params DadosDependente[] dependentes)
    {
      var listResult = new List<Dependente>();
      var cont = 0;
      foreach (DadosDependente dependente in dependentes)
      {
        var result = new Dependente();
        result.id = idDependente;
        result.nascimento = dependente.Nascimento;
        result.sexo = dependente.Sexo;
        listResult.Add(result);
      }
      return listResult;
    }

    protected IDbAccess StubDbAccess(DataTable pessoa, DataTable dependente)
    {
      var dba = MockRepository.GenerateStub<IDbAccess>();
      dba.Stub(x => x.QueryFill(Arg<DataSet>.Is.Anything, Arg<string>.Is.Equal("PESSOA"),
       Arg<string>.Is.Anything,
       Arg<object>.Is.Anything,
       Arg<IDbTransaction>.Is.Anything,
       Arg<bool>.Is.Anything,
       Arg<int?>.Is.Anything,
       Arg<CommandType?>.Is.Anything
       )).Do(new DbAccess.QueryFillDelegate((dSet, tableName, c, d, e, f, g, h) => { dSet.Tables.Add(pessoa); }));

      dba.Stub(x => x.QueryFill(Arg<DataSet>.Is.Anything, Arg<string>.Is.Equal("DEPENDENTE"),
       Arg<string>.Is.Anything,
       Arg<object>.Is.Anything,
       Arg<IDbTransaction>.Is.Anything,
       Arg<bool>.Is.Anything,
       Arg<int?>.Is.Anything,
       Arg<CommandType?>.Is.Anything
       )).Do(new DbAccess.QueryFillDelegate((dSet, tableName, c, d, e, f, g, h) => { dSet.Tables.Add(dependente); }));
      return dba;
    }

    protected IDbAccess StubDbAccessFuncionario(DataTable pessoa)
    {
        var dba = MockRepository.GenerateStub<IDbAccess>();
        dba.Stub(x => x.QueryFill(Arg<DataSet>.Is.Anything, Arg<string>.Is.Equal("PESSOA"),
            Arg<string>.Is.Anything,
            Arg<object>.Is.Anything,
            Arg<IDbTransaction>.Is.Anything,
            Arg<bool>.Is.Anything,
            Arg<int?>.Is.Anything,
            Arg<CommandType?>.Is.Anything
            )).Do(new DbAccess.QueryFillDelegate((dSet, tableName, c, d, e, f, g, h) => { dSet.Tables.Add(pessoa); }));
        return dba;
    }

    protected IPersistenciaFuncionario StubPersistenciaFunc(int resultInt = 1)
    {
        var result = MockRepository.GenerateStub<IPersistenciaFuncionario>();
        result.Stub(x => x.PersisteStatusFuncionario(Arg<Funcionario>.Is.Anything)).Return(resultInt);
        return result;
    }

    protected IPersistenciaFuncionario StubPersistencia(Funcionario pessoa, List<Dependente> dependentes, int resultInt = 1)
    {
      var result = MockRepository.GenerateStub<IPersistenciaFuncionario>();
      result.Stub(x => x.RetornaFuncionarios())
        .Return(CriaRetornoFuncionarios(pessoa, dependentes));
      result.Stub(x => x.PersisteFuncionario(Arg<Funcionario>.Is.Anything)).Return(resultInt);
      return result;
    }


    protected List<Funcionario> CriaRetornoFuncionarios(Funcionario pessoa, List<Dependente> dependentes)
    {
      List<Funcionario> listFunionario = new List<Funcionario>();
      pessoa.dependentes = dependentes;
      listFunionario.Add(pessoa);
      return listFunionario;
    }
 

    #endregion

  }
  public struct DadosDependente
  {
    public DadosDependente(DateTime nascimento, char sexo)
    {
      Nascimento = nascimento;
      Sexo = sexo;
    }
    public DateTime Nascimento;
    public char Sexo;
  }


}
