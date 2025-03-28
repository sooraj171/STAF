using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Remote;
using System;

namespace STAF.CF
{
    public class BrowserDriver
    {
        private IWebDriver driver;

        // Create a new driver instance based on the browser type (with automatic driver management in Selenium 4)
        public IWebDriver CreateDriverInstance(string brwType)
        {
            driver = GetWebDriver(brwType, string.Empty, false);
            return driver;
        }

        // Overloaded method with specific options for Chrome
        public IWebDriver CreateDriverInstance(string driverPath, ChromeOptions options)
        {
            driver = new ChromeDriver(driverPath, options);
            return driver;
        }
        public IWebDriver BrowserDriverObject(string brwType)
        {
            driver = GetWebDriver(brwType, AppDomain.CurrentDomain.BaseDirectory,false);
            return driver;
        }

        public IWebDriver BrowserDriverObject(string driverPath,ChromeOptions options)
        {
            driver = new ChromeDriver(driverPath, options);
            return driver;
        }
        /// <summary>
        /// Getting the Web driver object for the test run
        /// </summary>
        /// <param name="brwType">Which browser you want to use Chrome/Edge</param>
        /// <param name="driverPath">Driver Path if running locally; Remote Hub Url if using Hub</param>
        /// <param name="isRemote">To set thre remote driver for Hub set it to true; default is false</param>
        /// <returns></returns>
        public virtual IWebDriver GetBrowserDriverObject(string brwType, string driverPath="", bool isRemote = false)
        {
            driverPath = driverPath.Trim() == "" ? AppDomain.CurrentDomain.BaseDirectory : driverPath;
            driver = GetWebDriver(brwType,driverPath, isRemote);
            return driver;
        }


        //private IWebDriver GetWebDriver(string brwType, string driverPath, bool isRemote)
        //{
        //    switch (brwType.ToLower())
        //    {
        //        case "chrome":
        //            if (isRemote)
        //            {
        //                driver = new RemoteWebDriver(new Uri(driverPath), SetChromeOptions()); 
        //            }
        //            else driver = new ChromeDriver(driverPath, SetChromeOptions());
        //            break;

        //        case "edge":
        //            if (isRemote)
        //            {
        //                driver = new RemoteWebDriver(new Uri(driverPath), SetEdgeOptions());
        //            }
        //            else driver = new EdgeDriver(driverPath, SetEdgeOptions());
        //            break;

        //        default:
        //            driver = new ChromeDriver(driverPath, SetChromeOptions());
        //            break;

        //    }
        //    return driver;
        //}

        /// <summary>
        /// Getting the WebDriver object for the test run. 
        /// Supports local and remote drivers based on the isRemote flag.
        /// </summary>
        private IWebDriver GetWebDriver(string brwType, string driverPath = "", bool isRemote = false)
        {
            if (string.IsNullOrWhiteSpace(driverPath))
            {
                driverPath = AppDomain.CurrentDomain.BaseDirectory;
            }

            brwType = brwType.ToLower();
            switch (brwType)
            {
                case "chrome":
                    driver = isRemote
                        ? new RemoteWebDriver(new Uri(driverPath), SetChromeOptions())
                        : new ChromeDriver(SetChromeOptions());
                    break;

                case "edge":
                    driver = isRemote
                        ? new RemoteWebDriver(new Uri(driverPath), SetEdgeOptions())
                        : new EdgeDriver(SetEdgeOptions());
                    break;

                default:
                    throw new ArgumentException($"Unsupported browser type: {brwType}", nameof(brwType));
            }

            return driver;
        }

        protected virtual ChromeOptions SetChromeOptions()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("start-maximized");
            return options;
        }
        protected virtual EdgeOptions SetEdgeOptions()
        {
            EdgeOptions options = new EdgeOptions();
            options.AddArguments("start-maximized");
            return options;
        }
    }
}
