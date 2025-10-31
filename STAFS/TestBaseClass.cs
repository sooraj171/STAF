using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;
using System.Diagnostics;
using System.Linq;

namespace STAF.CF
{
    [TestClass]
    public class TestBaseClass: BrowserDriver
    {
        protected IWebDriver driver;
        private TestContext testContext;

        public TestContext TestContext
        {
            get { return testContext; }
            set { testContext = value; }
        }

        public string currTestName;
        public string currResultFile;
        public static int exeCnt = 0;
        private Stopwatch stopwatch;

        [TestInitialize]
        public void TestInitialize()
        {
            stopwatch = Stopwatch.StartNew();
            currTestName = TestContext.TestName;
            currResultFile = CommonAction.setStartUpValues(TestContext);

            // Safer retrieval of TestContext properties
            string brwType = TestContext.Properties.ToString().Contains("browser") ? TestContext.Properties["browser"].ToString() : "chrome";
            string driverPath = TestContext.Properties.ToString().Contains("driverPath") ? TestContext.Properties["driverPath"].ToString() : "";

            driver = GetBrowserDriverObject(brwType, driverPath);
        }


        [TestCleanup]
        public void TestCleanup()
        {
            currTestName = TestContext.TestName;
            stopwatch.Stop();
            string totTime = stopwatch.Elapsed.ToString(@"mm\:ss") + " Min:Sec";

            try
            {
                CommonAction.setCleanUpValues(currResultFile, TestContext, totTime);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during cleanup: {ex.Message}");
            }

            driver.Quit();

            // Null-safe check for environment variable
            if (string.Equals(Environment.GetEnvironmentVariable("failFlag"), "yes", StringComparison.OrdinalIgnoreCase))
            {
                Assert.Fail();
            }
        }
    }
}
