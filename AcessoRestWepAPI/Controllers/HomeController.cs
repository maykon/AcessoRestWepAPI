using DatabaseLib;
using RegrasNegocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace AcessoRestWepAPI.Controllers
{
  public class HomeController : Controller
  {
    public ActionResult Index()
    {
      ViewBag.Title = "Home Page";

      return View();
    }

    public string CalcularAumento()
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

    private IDbAccess CriaAccess()
    {
      return new DbAccess(new SqlConnection("Data Source=localhost\\SQLEXPRESS;Initial Catalog=testeunitario;User ID=rm;Password=rm;Trusted_Connection=True;"));
    }
  }
}
