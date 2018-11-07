namespace RegrasNegocio
{
  using System;
  using System.Collections.Generic;
  using System.Data;
  using System.Linq;

  public class CalculaAumentoFuncionario : IDisposable
  {
    public List<Funcionario> Funcionarios;
    public IPersistenciaFuncionario Persistencia { get; protected set; }

    protected ILogWriter InternalLog;

    protected ILogWriter InternalErrorLog;

    public List<Funcionario> Calculados => Funcionarios;

    public string Log => InternalLog.LogContent;

    public string ErrorLog => InternalErrorLog.LogContent;

    public decimal PercentualCalculado { get; protected set; }

    public CalculaAumentoFuncionario(IPersistenciaFuncionario access, ILogWriter logExecucao, ILogWriter logErros)
    {
      Persistencia = access;
      InternalLog = logExecucao;
      InternalErrorLog = logErros;
    }

    protected void WriteLog(string line)
    {
      InternalLog.WriteLog(line);
    }

    protected void WriteError(string line)
    {
      InternalErrorLog.WriteLog(line);
    }

    public List<Funcionario> GetFuncionarios()
    {
      return Persistencia.RetornaFuncionarios();
    }

    public void Calcula()
    {
      WriteLog($"****Processo de Cálculo de Aumento Salarial - Iniciado em {DateTime.Now}");
      WriteLog(string.Empty);
      Funcionarios = GetFuncionarios();
      var cont = 0;
      TimeSpan duracao;
      var inicio = DateTime.Now;
      long acumulado = 0;
      foreach (Funcionario funcionario in Funcionarios)
      {
        try
        {
          var novoPercentual = Calcula(funcionario, out duracao);
          if (novoPercentual > 1)
          {
            var salarioAntigo = RetornaSalario(funcionario);
            var novoSalario = salarioAntigo * (novoPercentual);
            
            funcionario.salario = novoSalario;
            WriteLog($"    Salário Antigo:[R$ {salarioAntigo}]");
            WriteLog($"    Novo Salário  :[R$ {novoSalario}] = ({salarioAntigo} * {novoPercentual})");
            GravaDados(funcionario);
            WriteLog(string.Empty);
            acumulado += duracao.Ticks;
            cont++;
          }
        }
        catch(Exception ex)
        {
          WriteError($"Erro ao processar funcionário {funcionario.id}/{funcionario.nome}:{ex}");
        }
      }
      var total = DateTime.Now - inicio;
      WriteLog($"    Tempo total do Cálculo                : [{total}]");
      WriteLog($"    Quantidade de Funcionários calculados : [{cont}]");
      if (acumulado > 0)
      {
        var item = new TimeSpan(acumulado / cont);
        WriteLog($"    Tempo médio de Calculo por funcionário: [{item}]");
        WriteLog($"    Tempo total de Calculo por funcionário: [{new TimeSpan(acumulado)}]");
        WriteLog($"    Tempo com outras operações            : [{ total - (new TimeSpan(acumulado))}]");
      }
    }

    protected void GravaDados(Funcionario funcionario)
    {
      WriteLog("    Gravando dados do funcionario");
      var inicioGrava = DateTime.Now;
      var alteracoes = Persistencia.PersisteFuncionario(funcionario);
      if (alteracoes == 1)
      {
        WriteLog($"    Novo salário calculado com sucesso");
        WriteLog($"    Tempo de Gravação no banco         : {DateTime.Now - inicioGrava}");
        WriteLog(Persistencia.LogContent);
        Persistencia.ClearLog();
      }
    }

    protected virtual decimal Calcula(Funcionario funcionario, out TimeSpan duracao)
    {
      var incio = DateTime.Now;
      decimal percentual = 1M;
      if (Ativo(funcionario) && AtendeFaixaSalarial(funcionario))
      {
        WriteLog($">   Processando dados do funcionário {funcionario.id}:{funcionario.nome}");
        percentual += 0.1M;
        WriteLog($"    Percentual Base: {percentual - 1}");
        var dependentesMulheres = RetornaDependentesValidosParaCalculo(funcionario);
        WriteLog($"    Percentual Ajustado para dependentes: ({percentual-1}) + ({dependentesMulheres * 0.01M}) = [{percentual-1 + dependentesMulheres * 0.01M}]");
        var percentualDep = (0.01M * dependentesMulheres);

        var anosTrabalhados = RetornaAnosTrabalhados(funcionario);
        WriteLog($"    Anos trabalhados (Limitado a 3): {anosTrabalhados}");
        var percentualAnos = (0.02M * RetornaAnosTrabalhados(funcionario));

        PercentualCalculado = percentual + percentualAnos + percentualDep;
        WriteLog($"    Percentual Final = ({percentual-1}) + ({percentualAnos}) + {percentualDep} = [{(PercentualCalculado-1) * 100}%]  ");
        percentual += percentualAnos + percentualDep;
      }
      duracao = DateTime.Now - incio;
      return percentual;
    }

    protected decimal RetornaSalario(Funcionario funcionario)
    {
      var result = funcionario.salario;
      if (result < 0)
        throw new ApplicationException("Cálculo bloqueado para salários negativos!");
      return result;
    }

    protected bool AtendeFaixaSalarial(Funcionario funcionario)
    {
      var salario = RetornaSalario(funcionario);
      return (salario >= 1000) && (salario < 5000);
    }

    protected bool Ativo(Funcionario funcionario)
    {
      return string.Compare(Convert.ToString(funcionario.ativo), "S") == 0;
    }

    public static readonly string _relation = "PESSOA_DEPEND";
    protected int RetornaDependentesValidosParaCalculo(Funcionario funcionario)
    {
      var dependentesMulheres = funcionario.dependentes
        .Where(x => x.sexo == 'F' && x.nascimento.Month >= 7).Count();

      WriteLog($"    Dependentes do Sexo Feminino nascidas após mês de Julho: {dependentesMulheres}");
      var result = Tops(3, dependentesMulheres);
      WriteLog($"    Aplicando limite máximo de dependentes: {result}");
      return result;
    }

    protected int RetornaAnosTrabalhados(Funcionario funcionario)
    {
      var anosTrabalhados = (((DateTime.Now - funcionario.inicioContrato)).Days / 365);
      anosTrabalhados = Tops(3, anosTrabalhados);
      return anosTrabalhados;
    }

    public static int Tops(int maxValue, int currentValue)
    {
      return currentValue <= maxValue ? currentValue : maxValue;
    }

    public void Dispose()
    {
      Persistencia?.Dispose();
      Persistencia = null;
    }
  }
}
