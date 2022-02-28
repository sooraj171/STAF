using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using SeleniumExtras.WaitHelpers;

namespace STAF.CF
{
    public class PageBaseClass
    {
        //https://stackoverflow.com/questions/9801624/get-name-of-a-variable-or-parameter
        private IWebDriver Driver;
        private TestContext context;
        public PageBaseClass(IWebDriver _driver, TestContext testContext)
        {
            Driver = _driver;
            context = testContext;
        }


        private IWebElement _currElm;
        /// <summary>
        /// Find An Element with wait by Passing By Expression
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public IWebElement FindAppElement(By Expression)
        {
            try
            {
                _currElm = new WebDriverWait(Driver, TimeSpan.FromSeconds(10)).Until(ExpectedConditions.ElementExists(Expression));
                ReportResult.ReportResultPass(Driver, context, Expression.ToString(), "Element is present");
            }
            catch
            {
                _currElm = null;
                ReportResult.ReportResultFail(Driver, context, Expression.ToString(), "Element is not present");
            }
            return _currElm;
        }


        private IWebElement _currElmParan;
        /// <summary>
        /// Find An Element with wait by Passing By Expression and passing Element report string
        /// </summary>
        /// <param name="Expression"></param>
        /// <param name="strObjectDetails"></param>
        /// <returns>Web Element</returns>
        public IWebElement FindAppElement(By Expression, string strObjectDetails)
        {
            try
            {
                _currElmParan = new WebDriverWait(Driver, TimeSpan.FromSeconds(10)).Until(ExpectedConditions.ElementExists(Expression));
                ReportResult.ReportResultPass(Driver, context, strObjectDetails, "Element is present");
            }
            catch
            {
                _currElmParan = null;
                ReportResult.ReportResultFail(Driver, context, strObjectDetails, "Element is not present");
            }
            return _currElmParan;
        }
    }

}
