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

            string brwType = TestContextPropertyHelper.GetString(TestContext, "browser", "chrome");
            string driverPath = TestContextPropertyHelper.GetString(TestContext, "driverPath", "");
            bool headless = TestContextPropertyHelper.GetBool(TestContext, "headless", false);

            driver = GetBrowserDriverObject(brwType, driverPath, false, headless);
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
            finally
            {
                try
                {
                    driver?.Quit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error quitting WebDriver: {ex.Message}");
                }
            }

            if (TestRunState.IsFailed())
            {
                Assert.Fail();
            }
        }
    }
}
