using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace STAF.CF
{
    [TestClass]
    public class TestBaseClass
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
            currTestName = TestContext.TestName.ToString();
            currResultFile = CommonAction.setStartUpValues(TestContext);
            string brwType = TestContext.Properties["browser"].ToString(); 
            string driverPath = TestContext.Properties["driverPath"].ToString();
            driver = new BrowserDriver().BrowserDriverObject(brwType, driverPath);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            currTestName = this.TestContext.TestName;
            stopwatch.Stop();
            string totTime = stopwatch.Elapsed.ToString("mm") + ":" + stopwatch.Elapsed.ToString("ss") + "Min:Sec";

            CommonAction.setCleanUpValues(currResultFile,TestContext, totTime);

            driver.Quit();
           
            if (Environment.GetEnvironmentVariable("failFlag").ToString() == "yes")
            {
                Assert.Fail();
            }
        }
    }
}
