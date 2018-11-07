using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegrasNegocio
{
    public class AtualizarStatusFuncionario : IDisposable
    {
        public Funcionario Funcionario;
        public IPersistenciaFuncionario Persistencia { get; protected set; }
        protected ILogWriter InternalLog;
        protected ILogWriter InternalErrorLog;
        public string Log => InternalLog.LogContent;

        public AtualizarStatusFuncionario(IPersistenciaFuncionario access)
        {
            Persistencia = access;
            Funcionario = new Funcionario();
            InternalLog = new LogWriter();
            InternalErrorLog = new LogWriter();
        }

        public void DefinirId(int id)
        {
            Funcionario.id = id;
        }

        public void DefinirStatus(char status)
        {
            Funcionario.ativo = status;
        }

        public int AtualizarStatus()
        {
            int atualizado = 0;
            if (Funcionario.id < 0)
            {
                throw new ArgumentException("Id do funcionário deve ser maior que zero.");
            }
            if (char.IsNumber(Funcionario.ativo))
            {
                throw new ArgumentException("Status deve conter apenas letras.");
            }
            if(!(Funcionario.ativo.Equals('N') || Funcionario.ativo.Equals('S'))){
                throw new ArgumentException("Status permite apenas os valores 'N' ou 'S'.");
            }

            var inicio = DateTime.Now;
            try
            {
                WriteLog($"****Processo de Atualização do status do funcionario - Iniciado em {inicio}");
                WriteLog(string.Empty);
                atualizado = Persistencia.PersisteStatusFuncionario(Funcionario);
                WriteLog(string.Empty);
                WriteLog(Persistencia.LogContent);
            }
            catch (Exception ex)
            {
                WriteError($"Erro ao processar funcionário {Funcionario.id}/{Funcionario.ativo}:{ex}");
            }
            var total = DateTime.Now - inicio;
            WriteLog($"    Tempo total do Cálculo                : [{total}]");
            return atualizado;
        }

        protected void WriteLog(string line)
        {
            InternalLog.WriteLog(line);
        }

        protected void WriteError(string line)
        {
            InternalErrorLog.WriteLog(line);
        }

        public void Dispose()
        {
            Persistencia?.Dispose();
            Persistencia = null;
        }
    }
}
