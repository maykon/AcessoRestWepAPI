namespace UnitTestsProject
{
  using System;
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using System.Data;
  using RegrasNegocio;
  using System.Collections.Generic;

  [TestClass]
  public class TestCalculaAumentoFuncionario : ClasseBaseTestesUnitarios
  {
    [TestMethod]
    [TestCategory("Calculo")]
    public void TestPessoaForaDaFaixaSalarioInferior()
    {
      var salario = 999.99M;
      var tablePessoa = CriaTabelaPessoa(salario, 'S', new DateTime(2008, 1, 1));

      var tableDependentes = CriatabelaDependentes(1, new DadosDependente[] {
        new DadosDependente{Sexo = 'F', Nascimento = new DateTime(2009,07,01)},
        new DadosDependente{Sexo = 'F', Nascimento = new DateTime(2009,06,01)}
      });

      var log = new LogWriter();
      var logErros = new LogWriter();
      var persistencia = StubPersistencia(tablePessoa, tableDependentes);
      var calcula = new CalculaAumentoFuncionario(persistencia, log, logErros);

      calcula.Calcula();
      var novoSalario = calcula.Calculados[0].salario;
      Assert.AreEqual(salario, novoSalario);
    }

    [TestMethod]
    [TestCategory("Calculo")]
    public void TestPessoaForaDaFaixaSalarioSuperior()
    {
      var salario = 9999.99M;
      var tablePessoa = CriaTabelaPessoa(salario, 'S', new DateTime(2008, 1, 1));

      var tableDependentes = CriatabelaDependentes(1, new DadosDependente[] {
        new DadosDependente{Sexo = 'F', Nascimento = new DateTime(2009,07,01)},
        new DadosDependente{Sexo = 'F', Nascimento = new DateTime(2009,06,01)}
      });

      var log = new LogWriter();
      var logErros = new LogWriter();
      var persistencia = StubPersistencia(tablePessoa, tableDependentes);
      var calcula = new CalculaAumentoFuncionario(persistencia, log, logErros);

      calcula.Calcula();
      var novoSalario = calcula.Calculados[0].salario;
      Assert.AreEqual(salario, novoSalario);
    }

    [TestMethod]
    [TestCategory("Calculo")]
    public void TestPessoaInativa()
    {
      var salario = 2000M;
      var tablePessoa = CriaTabelaPessoa(salario, 'N', new DateTime(2008, 1, 1));

      var tableDependentes = CriatabelaDependentes(1, new DadosDependente[] {
        new DadosDependente{Sexo = 'F', Nascimento = new DateTime(2009,07,01)},
        new DadosDependente{Sexo = 'F', Nascimento = new DateTime(2009,06,01)}
      });

      var log = new LogWriter();
      var logErros = new LogWriter();
      var persistencia = StubPersistencia(tablePessoa, tableDependentes);
      var calcula = new CalculaAumentoFuncionario(persistencia, log, logErros);

      calcula.Calcula();
      var novoSalario = calcula.Calculados[0].salario;
      Assert.AreEqual(salario, novoSalario);
    }

    [TestMethod]
    [TestCategory("Calculo")]
    public void TestPessoaAumentoBase()
    {
      var salario = 2000M;
      var tablePessoa = CriaTabelaPessoa(salario, 'S', DateTime.Now);
      var tableDependentes = new List<Dependente>();

      var log = new LogWriter();
      var logErros = new LogWriter();
      var persistencia = StubPersistencia(tablePessoa, tableDependentes);
      var calcula = new CalculaAumentoFuncionario(persistencia, log, logErros);

      calcula.Calcula();
      var novoSalario = calcula.Calculados[0].salario;
      Assert.AreEqual(salario * 1.10M, novoSalario);
    }

    [TestMethod]
    [TestCategory("Calculo")]
    public void TestTempodeServicoLimitadoem3()
    {
      var salario = 2000M;
      var tablePessoa = CriaTabelaPessoa(salario, 'S', new DateTime(1990,1,1));
      var tableDependentes = new List<Dependente>();

      var log = new LogWriter();
      var logErros = new LogWriter();
      var persistencia = StubPersistencia(tablePessoa, tableDependentes);
      var calcula = new CalculaAumentoFuncionario(persistencia, log, logErros);

      calcula.Calcula();
      var novoSalario = calcula.Calculados[0].salario;
      Assert.AreEqual(salario * (1.10M + (0.02M * 3)  ), novoSalario);
    }

    [TestMethod]
    [TestCategory("Calculo")]
    public void TestPessoaAumentoLimita3Dependentes()
    {
      var salario = 2000M;
      var tablePessoa = CriaTabelaPessoa(salario, 'S', DateTime.Now);
      var tableDependentes = CriatabelaDependentes(
        1, new DadosDependente[]
        {
          new DadosDependente( new DateTime(1990,7,1), 'F'),
          new DadosDependente( new DateTime(1991,7,2), 'F'),
          new DadosDependente( new DateTime(1992,7,3), 'F'),
          new DadosDependente( new DateTime(1993,7,4), 'F'),
          new DadosDependente( new DateTime(1994,7,5), 'F')
        }
        );
      var log = new LogWriter();
      var logErros = new LogWriter();
      var persistencia = StubPersistencia(tablePessoa, tableDependentes);
      var calcula = new CalculaAumentoFuncionario(persistencia, log, logErros);

      calcula.Calcula();
      var novoSalario = calcula.Calculados[0].salario;
      Assert.AreEqual(salario * (1.10M + (3 * 0.01M)), novoSalario);
    }

    [TestMethod]
    [TestCategory("Calculo")]
    public void TestPessoaLimtaDependentesPorSexoEMesNascimento()
    {
      var salario = 2000M;
      var tablePessoa = CriaTabelaPessoa(salario, 'S', DateTime.Now);
      var tableDependentes = CriatabelaDependentes(
        1, new DadosDependente[]
        {
          new DadosDependente( new DateTime(1990,7,1), 'F'),
          new DadosDependente( new DateTime(1991,7,2), 'M'),
          new DadosDependente( new DateTime(1992,12,3), 'F'),
          new DadosDependente( new DateTime(1993,1,4), 'F'),
          new DadosDependente( new DateTime(1994,12,5), 'M')
        }
        );
      var log = new LogWriter();
      var logErros = new LogWriter();
      var persistencia = StubPersistencia(tablePessoa, tableDependentes);
      var calcula = new CalculaAumentoFuncionario(persistencia, log, logErros);

      calcula.Calcula();
      var novoSalario = calcula.Calculados[0].salario;
      Assert.AreEqual(salario * (1.10M + (2 * 0.01M)), novoSalario);
    }
  }

}
