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

        public IWebDriver BrowserDriverObject(string brwType)
        {
            driver = GetWebDriver(brwType);
            return driver;
        }

        public IWebDriver BrowserDriverObject(string driverPath,ChromeOptions options)
        {
            driver = new ChromeDriver(ChromeDriverService.CreateDefaultService(driverPath), options);
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
            driver = GetWebDriver(brwType,isRemote);
            return driver;
        }


        private IWebDriver GetWebDriver(string brwType, string driverPath, bool isRemote)
        {
            switch (brwType.ToLower())
            {
                case "chrome":
                    if (isRemote)
                    {
                        driver = new RemoteWebDriver(new Uri(driverPath), SetChromeOptions()); 
                    }
                    else driver = new ChromeDriver(driverPath, SetChromeOptions());
                    break;

                case "edge":
                    if (isRemote)
                    {
                        driver = new RemoteWebDriver(new Uri(driverPath), SetEdgeOptions());
                    }
                    else driver = new EdgeDriver(driverPath, SetEdgeOptions());
                    break;

                default:
                    driver = new ChromeDriver(driverPath, SetChromeOptions());
                    break;

            }
            return driver;
        }

        private IWebDriver GetWebDriver(string brwType, bool isRemote=false,string RemoteDriverUrl="")
        {
            switch (brwType.ToLower())
            {
                case "chrome":
                    if (isRemote)
                    {
                        driver = new RemoteWebDriver(new Uri(RemoteDriverUrl), SetChromeOptions());
                    }
                    else driver = new ChromeDriver(ChromeDriverService.CreateDefaultService(), SetChromeOptions());
                    break;

                case "edge":
                    if (isRemote)
                    {
                        driver = new RemoteWebDriver(new Uri(RemoteDriverUrl), SetEdgeOptions());
                    }
                    else driver = new EdgeDriver(EdgeDriverService.CreateDefaultService(), SetEdgeOptions());
                    break;

                default:
                    driver = new ChromeDriver(ChromeDriverService.CreateDefaultService(), SetChromeOptions());
                    break;

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
