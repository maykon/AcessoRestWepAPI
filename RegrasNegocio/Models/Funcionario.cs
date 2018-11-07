using Castle.Components.DictionaryAdapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegrasNegocio
{
  public class Funcionario
  {
    public Funcionario()
    {
      dependentes = new List<Dependente>();
    }

    [Key("id")]
    public int id { get; set; }
    public string nome { get; set; }
    public string email { get; set; }
    public DateTime nascimento { get; set; }
    public DateTime inicioContrato { get; set; }
    public char ativo { get; set; }
    public char sexo { get; set; }
    public decimal salario { get; set; }
    public List<Dependente> dependentes { get; set; }
  }

  public class Dependente
  {
    [Key("id")]
    public int id { get; set; }
    public int  pessoa { get; set; }
    public string nome { get; set; }
    public DateTime nascimento { get; set; }
    public char sexo { get; set; }
  }
}
