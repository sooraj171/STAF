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

        private IWebElement _currElmPara;
        /// <summary>
        /// Find An Element with wait by Passing By Expression and passing Element report string
        /// </summary>
        /// <param name="parentElement"></param>
        /// <param name="Expression"></param>
        /// <param name="strObjectDetails"></param>
        /// <returns></returns>
        public IWebElement FindAppElement(IWebElement parentElement, By Expression, string strObjectDetails)
        {
            try
            {
                _currElmPara = waitforelement(parentElement, Expression);
                ReportResult.ReportResultPass(Driver, context, strObjectDetails, "Element is present");
            }
            catch
            {
                _currElmPara = null;
                ReportResult.ReportResultFail(Driver, context, strObjectDetails, "Element is not present");
            }
            return _currElmPara;
        }

        private IWebElement waitforelement(IWebElement parentElement, By by)
        {
            int cnt = 10 * 5;
            for (int i = 0; i < cnt; i++)
            {
                System.Threading.Thread.Sleep(200);
                if (ElementExists(parentElement, by))
                {
                    return parentElement.FindElement(by);
                }
                else
                {
                    return null;
                }

            }
            return null;
        }

        private bool ElementExists(IWebElement parentElement, By by)
        {
            try
            {
                parentElement.FindElement(by);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

}
