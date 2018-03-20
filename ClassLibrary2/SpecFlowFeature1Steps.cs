using System.Diagnostics;
using BestPracticeFunctionApp;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Moq;
using TechTalk.SpecFlow;

namespace ClassLibrary2
{
    [Binding]
    public class SpecFlowFeature1Steps
    {

        [Given(@"an empty user repository")]
        public void GivenAnEmptyUserRepository()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I add a user with email '(.*)' and password '(.*)'")]
        public void WhenIAddAUserWithEmailAndPassword(string p0, string p1)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"user repository contains (.*) user")]
        public void ThenUserRepositoryContainsUser(int p0)
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I have entered (.*) into the calculator")]
        public void GivenIHaveEnteredIntoTheCalculator(int p0)
        {
            
        }
        
        [When(@"I press add")]
        public void WhenIPressAdd()
        {
            var log = new Mock<TraceWriter>(TraceLevel.Info);

            var req = new Mock<HttpRequest>();

            var rv = RootMe.Run(req.Object, log.Object);
            //ScenarioContext.Current.Pending();
        }
        
        [Then(@"the result should be (.*) on the screen")]
        public void ThenTheResultShouldBeOnTheScreen(int p0)
        {
            //ScenarioContext.Current.Pending();
        }
    }
}
