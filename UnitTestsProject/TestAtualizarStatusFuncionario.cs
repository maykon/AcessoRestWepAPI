using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RegrasNegocio;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestsProject
{
    [TestClass]
    public class TestAtualizarStatusFuncionario : ClasseBaseTestesUnitarios
    {
        [TestMethod]
        [TestCategory("AtualizarStatus")]
        public void TestAtualizarStatusComFuncionarioInvalido()
        {
           var persistencia = StubPersistenciaFunc(0);
           var atualizaStatus = new AtualizarStatusFuncionario(persistencia);
           atualizaStatus.DefinirId(-999);
            try
            {
                atualizaStatus.AtualizarStatus();
            }
            catch(Exception ex)
            {
                Assert.AreEqual(ex.Message, "Id do funcionário deve ser maior que zero.");
            }

        }

        [TestMethod]
        [TestCategory("AtualizarStatus")]
        public void TestAtualizarStatusComFuncionarioStatusInvalido()
        {
            var persistencia = StubPersistenciaFunc(0);
            var atualizaStatus = new AtualizarStatusFuncionario(persistencia);
            atualizaStatus.DefinirId(0);
            atualizaStatus.DefinirStatus('0');
            try
            {
                atualizaStatus.AtualizarStatus();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "Status deve conter apenas letras.");
            }
        }

        [TestMethod]
        [TestCategory("AtualizarStatus")]
        public void TestAtualizarStatusComFuncionarioStatusValido()
        {
            var persistencia = StubPersistenciaFunc(1);
            var atualizaStatus = new AtualizarStatusFuncionario(persistencia);
            atualizaStatus.DefinirId(0);
            atualizaStatus.DefinirStatus('S');
            int changed = atualizaStatus.AtualizarStatus();
            Assert.AreEqual<int>(changed, 1);
        }

        [TestMethod]
        [TestCategory("AtualizarStatus")]
        public void TestAtualizarStatusComFuncionarioStatusIncorreto()
        {
            var persistencia = StubPersistenciaFunc(0);
            var atualizaStatus = new AtualizarStatusFuncionario(persistencia);
            atualizaStatus.DefinirId(0);
            atualizaStatus.DefinirStatus('W');
            try
            {
                atualizaStatus.AtualizarStatus();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "Status permite apenas os valores 'N' ou 'S'.");
            }
        }
    }
}
