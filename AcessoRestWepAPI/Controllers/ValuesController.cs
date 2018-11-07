using DatabaseLib;
using Newtonsoft.Json;
using RegrasNegocio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AcessoRestWepAPI.Controllers
{
  public class ValuesController : ApiController
  {
    // GET api/values
    public IEnumerable<string> Get()
    {
      return new string[] { "value1", "value2" };
    }

    // GET api/values/5
    public string Get(int id)
    {
      return "value";
    }

    // POST api/values
    public string Post()
    {
      string logCalculo = string.Empty;
      using (var persistencia = new PersistenciaFuncionario(CriaAccess(), new LogWriter()))
      {
        var calculoAumento = new CalculaAumentoFuncionario(persistencia, new LogWriter(), new LogWriter());
        calculoAumento.Calcula();
        logCalculo = calculoAumento.Log;
      }
      return logCalculo;
    }

    // PUT api/values/5
    public void Put(int id, [FromBody]string value)
    {
    }

    // DELETE api/values/5
    public void Delete(int id)
    {
    }

    private IDbAccess CriaAccess()
    {
      return new DbAccess(new SqlConnection("Data Source=localhost\\SQLEXPRESS;Initial Catalog=testeunitario;User ID=rm;Password=rm;Trusted_Connection=True;"));
    }
  }
}
