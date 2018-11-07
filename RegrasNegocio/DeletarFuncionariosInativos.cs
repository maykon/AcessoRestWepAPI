using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegrasNegocio
{
    public class DeletarFuncionariosInativos : IDisposable
    {

        public Funcionario Funcionario;
        public IPersistenciaFuncionario Persistencia { get; protected set; }
        protected ILogWriter InternalLog;
        protected ILogWriter InternalErrorLog;
        public string Log => InternalLog.LogContent;

        public DeletarFuncionariosInativos(IPersistenciaFuncionario access)
        {
            Persistencia = access;
            Funcionario = new Funcionario();
            InternalLog = new LogWriter();
            InternalErrorLog = new LogWriter();
        }
        
        public int deletarInativos()
        {
            int totalDeletados = 0;
            var inicio = DateTime.Now;
            try
            {
                WriteLog($"****Processo de deletar funcionários inativos - Iniciado em {inicio}");
                WriteLog(string.Empty);
                totalDeletados = Persistencia.PersisteDeletarFuncionariosInativos();
                WriteLog(string.Empty);
                WriteLog(Persistencia.LogContent);
            }
            catch (Exception ex)
            {
                WriteLog($"Erro ao deletar os funcionários inativos: {ex}");
            }
            var total = DateTime.Now - inicio;
            WriteLog($"    Tempo total                : [{total}]");
            return totalDeletados;
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

