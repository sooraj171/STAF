using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STAF.CF
{
    public static class WebDriverExtensions
    {
        //public static IWebDriver CloseAllTabsExceptCurrent(this IWebDriver driver)
        //{
        //    try
        //    {
        //        int tabCnt = GetTotalTabsCount(driver);

        //        if (tabCnt > 1)
        //        {
        //            var mainWin = driver.CurrentWindowHandle;

        //            driver.WindowHandles.Where(w => w != mainWin).ToList()
        //                .ForEach(w =>
        //                {
        //                    driver.SwitchTo().Window(w);
        //                    driver.Close();
        //                });
        //        }
        //    }
        //    catch
        //    {
        //        //ReportResult.ReportResultFail("CloseAllTabsExceptCurrent", "Failed to Close all open tabs. ");
        //    }
        //    return driver;
        //}

        /// <summary>
        /// Closes all tabs except the main tab in the current window.
        /// </summary>
        public static IWebDriver CloseAllTabsExceptCurrent(this IWebDriver driver)
        {
            try
            {
                int tabCount = driver.WindowHandles.Count;

                if (tabCount > 1)
                {
                    string mainWindowHandle = driver.CurrentWindowHandle;

                    foreach (string handle in driver.WindowHandles)
                    {
                        if (handle != mainWindowHandle)
                        {
                            driver.SwitchTo().Window(handle);
                            driver.Close();
                        }
                    }

                    driver.SwitchTo().Window(mainWindowHandle);
                }
            }
            catch (NoSuchWindowException ex)
            {
                throw new Exception("Failed to close all tabs except main tab.", ex);
            }

            return driver;
        }


        //public static int GetTotalTabsCount(this IWebDriver driver)
        //{
        //    int cnt = 0;

        //    try
        //    {
        //        List<string> windowLs = new List<string>(driver.WindowHandles);
        //        cnt = windowLs.Count;
        //    }
        //    catch
        //    {
        //        //CommonAction.ReportResultFail("getTotalTabsCount", "Failed to get list of all tabs");
        //        return 0;
        //    }
        //    return cnt;
        //}

        /// <summary>
        /// Gets the total number of tabs in the current window.
        /// </summary>
        public static int GetTotalTabsCount(this IWebDriver driver)
        {
            int count;

            try
            {
                count = driver.WindowHandles.Count;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get total tab count.", ex);
            }

            return count;
        }

        //public static IWebElement WaitForFindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        //{
        //    if (timeoutInSeconds > 0)
        //    {
        //        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
        //        return wait.Until(drv => drv.FindElement(by));
        //    }
        //    return driver.FindElement(by);
        //}

        /// <summary>
        /// Finds a web element using the specified locator and waits for a certain amount of time if necessary.
        /// </summary>
        /// <param name="driver">The WebDriver instance to use.</param>
        /// <param name="locator">The locator used to find the web element.</param>
        /// <param name="timeoutInSeconds">The maximum amount of time to wait for the web element to be found.</param>
        /// <returns>The web element if found, or null if not found.</returns>
        public static IWebElement WaitForFindElement(this IWebDriver driver, By locator, int timeoutInSeconds = 10)
        {
            try
            {
                if (timeoutInSeconds > 0)
                {
                    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                    return wait.Until(drv => drv.FindElement(locator));
                }

                return driver.FindElement(locator);
            }
            catch (Exception ex)
            {
                // Handle the exception here, e.g. log it or throw a more specific exception.
                throw new Exception($"Failed to find element with locator {locator} after {timeoutInSeconds} seconds.", ex);
            }
        }


        //public static bool WaitForElementExist(this IWebDriver driver, IWebElement elmObject, int timeoutInSeconds)
        //{
        //    bool res = false;
        //    if (timeoutInSeconds > 0)
        //    {
        //        WebDriverWait wttest = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
        //        var tbObj = wttest.Until(d =>
        //        {
        //            if (elmObject != null)
        //            {
        //                return true;
        //            }
        //            else { return false; }
        //        }
        //          );
        //    }
        //    return res;
        //}

        /// <summary>
        /// Waits for an element to exist for a certain amount of time.
        /// </summary>
        /// <param name="driver">The WebDriver instance to use.</param>
        /// <param name="element">The web element to wait for.</param>
        /// <param name="timeoutInSeconds">The maximum amount of time to wait for the element to exist.</param>
        /// <returns>True if the element exists, false if it does not.</returns>
        public static bool WaitForElementExist(this IWebDriver driver, IWebElement element, int timeoutInSeconds)
        {
            try
            {
                if (timeoutInSeconds > 0)
                {
                    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                    wait.Until(drv => element != null);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        /// <summary>
        /// Waits for an element to disappear for a certain amount of time.
        /// </summary>
        /// <param name="driver">The WebDriver instance to use.</param>
        /// <param name="element">The web element to wait for.</param>
        /// <param name="timeoutInSeconds">The maximum amount of time to wait for the element to disappear.</param>
        /// <returns>True if the element disappears, false if it is still present after the timeout.</returns>
        public static bool WaitForElementNotExist(this IWebDriver driver, IWebElement element, int timeoutInSeconds)
        {
            try
            {
                if (timeoutInSeconds > 0)
                {
                    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                    wait.Until(drv => element == null || !element.Displayed);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static bool WaitForElementDisapper(IWebDriver driver, By element, int timeoutInSeconds)
        {
            try
            {
                int tempTO = 0;
                string flgFind = "n";
                while (timeoutInSeconds >= tempTO)
                {
                    try
                    {
                        //WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
                        //IWebElement temp = wait.Until<IWebElement>(d => d.FindElement(By.XPath("//div[@class='subline inc']")));
                        IWebElement temp = driver.FindElement(element);

                        if (temp != null)
                        {
                            tempTO = tempTO++;
                        }
                    }
                    catch (NoSuchElementException)
                    {
                        flgFind = "y";
                        return true;
                    }
                    catch (StaleElementReferenceException)
                    {
                        flgFind = "y";
                        return true;
                    }
                }

                if (timeoutInSeconds == tempTO && flgFind == "n")
                {
                    return false;
                }

                return false;

            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Wait for browser ready state and Jquery Ready State
        /// </summary>
        /// <param name="driver"></param>
        /// <returns>true</returns>
        //public static bool WaitForDocumentReady(this IWebDriver driver)
        //{
        //    IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
        //    try
        //    {
        //        var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(10));

        //        //int cnt = 0;
        //        bool retFlag = false;
        //        if ((bool)jse.ExecuteScript("return window.jQuery != undefined"))
        //        {

        //            wait.Until(d => (bool)jse.ExecuteScript("return jQuery.active == 0"));

        //            wait.Until(d => (bool)jse.ExecuteScript("return document.readyState == 'complete'"));
        //            retFlag = true;
        //        }
        //        else
        //        {
        //            wait.Until(d => (bool)jse.ExecuteScript("return document.readyState == 'complete'"));
        //            retFlag = true;
        //        }

        //        return retFlag;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

        /// <summary>
        /// Waits for the document to be fully loaded and ready for interaction.
        /// </summary>
        /// <param name="driver">The WebDriver instance to use.</param>
        /// <returns>True if the document is ready, false otherwise.</returns>
        public static bool WaitForDocumentReady(this IWebDriver driver)
        {
            if (driver == null || driver is not IJavaScriptExecutor jse)
            {
                return false;
            }

            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(10));

                if (jse.ExecuteScript("return window.jQuery != undefined") as bool? == true)
                {
                    wait.Until(d => jse.ExecuteScript("return jQuery.active == 0") as bool? == true);
                }

                wait.Until(d => jse.ExecuteScript("return document.readyState")?.ToString() == "complete");

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


    }

}
