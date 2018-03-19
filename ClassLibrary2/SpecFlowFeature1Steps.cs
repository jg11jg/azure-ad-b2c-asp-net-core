using System.Diagnostics;
using FunctionApp2;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Moq;
using TechTalk.SpecFlow;

namespace ClassLibrary2
{
    [Binding]
    public class SpecFlowFeature1Steps
    {
        [Given(@"I have entered (.*) into the calculator")]
        public void GivenIHaveEnteredIntoTheCalculator(int p0)
        {
            
        }
        
        [When(@"I press add")]
        public void WhenIPressAdd()
        {
            var log = new Mock<TraceWriter>(TraceLevel.Info);

            var req = new Mock<HttpRequest>();

            var rv = Function1.Run(req.Object, log.Object);
            //ScenarioContext.Current.Pending();
        }
        
        [Then(@"the result should be (.*) on the screen")]
        public void ThenTheResultShouldBeOnTheScreen(int p0)
        {
            //ScenarioContext.Current.Pending();
        }
    }
}
