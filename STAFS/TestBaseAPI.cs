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
    public class TestBaseAPI
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
            //string endPoint = TestContext.Properties["browser"].ToString();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            currTestName = TestContext.TestName;
            stopwatch.Stop();
            string totTime = stopwatch.Elapsed.ToString(@"mm\:ss") + " Min:Sec";

            CommonAction.setCleanUpValues(currResultFile, TestContext, totTime);

            if (string.Equals(Environment.GetEnvironmentVariable("failFlag"), "yes", StringComparison.OrdinalIgnoreCase))
            {
                Assert.Fail();
            }
        }
    }
}
