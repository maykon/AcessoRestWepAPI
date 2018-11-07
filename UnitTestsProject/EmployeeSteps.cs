using System;
using TechTalk.SpecFlow;
using RegrasNegocio;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestsProject
{
    [Binding]
    public class EmployeeSteps : ClasseBaseTestesUnitarios
    {
        private AtualizarStatusFuncionario atualizaStatus;
        private IPersistenciaFuncionario persistencia;

        [Given(@"I have a empty employee")]
        public void GivenIHaveAEmptyEmployee()
        {
            persistencia = StubPersistenciaFunc(0);
            atualizaStatus = new AtualizarStatusFuncionario(persistencia);
            atualizaStatus.DefinirId(-999);
        }
        
        [Given(@"I have a employee ID")]
        public void GivenIHaveAEmployeeID()
        {
            persistencia = StubPersistenciaFunc(1);
            atualizaStatus = new AtualizarStatusFuncionario(persistencia);
            atualizaStatus.DefinirId(1);
        }
        
        [Given(@"I try change status to number char")]
        public void GivenITryChangeStatusToEmpty()
        {
            atualizaStatus.DefinirStatus('0');
        }
        
        [Given(@"I try change status to 'S'")]
        public void GivenITryChangeStatusToActiveInative()
        {
            atualizaStatus.DefinirStatus('S');
        }
        
        [When(@"I press change status")]
        public void WhenIPressChangeStatus()
        {
            try
            {
                var changed = atualizaStatus.AtualizarStatus();
                ScenarioContext.Current.Set<int>(changed, "Changed");
            }
            catch (Exception err)
            {
                ScenarioContext.Current.Set<String>(err.Message, "Error");
            }
        }
        
        [Then(@"the result should be a message 'Id do funcionário deve ser maior que zero.'")]
        public void ThenTheResultShouldBeAMessageId()
        {
            Assert.IsTrue(ScenarioContext.Current.ContainsKey("Error"));
            Assert.AreEqual(ScenarioContext.Current.Get<String>("Error"), "Id do funcionário deve ser maior que zero.");
        }

        [Then(@"the result should be a message 'Status deve conter apenas letras.'")]
        public void ThenTheResultShouldBeAMessageStatus()
        {
            Assert.IsTrue(ScenarioContext.Current.ContainsKey("Error"));
            Assert.AreEqual(ScenarioContext.Current.Get<String>("Error"), "Status deve conter apenas letras.");
        }

        [Then(@"the result should be a status changed")]
        public void ThenTheResultShouldBeAStatusChanged()
        {
            int changed = ScenarioContext.Current.Get<int>("Changed");
            Assert.AreEqual<int>(changed, 1);
        }
    }
}
