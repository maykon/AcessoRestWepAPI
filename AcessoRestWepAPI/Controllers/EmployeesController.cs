using System;
using RegrasNegocio;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DatabaseLib;
using System.Data.SqlClient;

namespace AcessoRestWepAPI.Controllers
{
    public class EmployeesController : ApiController
    {
        // GET: api/Employees
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Employees/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Employees
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Employees/5
        public string Put(int id, [FromBody]char value)
        {
            string logCalculo = string.Empty;
            using (var persistencia = new PersistenciaFuncionario(CriaAccess(), new LogWriter()))
            {
                try
                {
                    var atualizaStatus = new AtualizarStatusFuncionario(persistencia);
                    atualizaStatus.DefinirId(id);
                    atualizaStatus.DefinirStatus(value);
                    atualizaStatus.AtualizarStatus();
                    logCalculo = atualizaStatus.Log;
                }catch(Exception ex)
                {
                    logCalculo = ex.Message;
                }
            }
            return logCalculo;
        }

        // DELETE: api/Employees/5
        public void Delete(int id)
        {
        }

        private IDbAccess CriaAccess()
        {
            return new DbAccess(new SqlConnection("Data Source=localhost\\SQLEXPRESS;Initial Catalog=testeunitario;User ID=rm;Password=rm;Trusted_Connection=True;"));
        }
    }
}
