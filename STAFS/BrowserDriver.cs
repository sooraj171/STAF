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
            driver = GetWebDriver(brwType, AppDomain.CurrentDomain.BaseDirectory,false);
            return driver;
        }

        public IWebDriver BrowserDriverObject(string driverPath,ChromeOptions options)
        {
            driver = new ChromeDriver(driverPath, options);
            return driver;
        }

        public IWebDriver BrowserDriverObject(string brwType, string driverPath="")
        {
            driverPath = driverPath.Trim() == "" ? AppDomain.CurrentDomain.BaseDirectory : driverPath;
            driver = GetWebDriver(brwType,driverPath, false);
            return driver;
        }

        public IWebDriver BrowserDriverObject(string brwType, bool isRemote=false, string driverPath = "")
        {
            driverPath = driverPath.Trim() == "" ? AppDomain.CurrentDomain.BaseDirectory : driverPath;
            driver = GetWebDriver(brwType, driverPath,isRemote);
            return driver;
        }

        public IWebDriver GetWebDriver(string brwType, string driverPath, bool isRemote)
        {
            switch (brwType.ToLower())
            {
                case "chrome":
                    if (isRemote)
                    {
                        driver = new RemoteWebDriver(new Uri(driverPath), GetChromeOptions()); 
                    }
                    else driver = new ChromeDriver(driverPath, GetChromeOptions());
                    break;

                case "edge":
                    if (isRemote)
                    {
                        driver = new RemoteWebDriver(new Uri(driverPath), GetEdgeOptions());
                    }
                    else driver = new EdgeDriver(driverPath, GetEdgeOptions());
                    break;

                default:
                    driver = new ChromeDriver(driverPath, GetChromeOptions());
                    break;

            }
            return driver;
        }

        private ChromeOptions GetChromeOptions()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("start-maximized");
            return options;
        }
        private EdgeOptions GetEdgeOptions()
        {
            EdgeOptions options = new EdgeOptions();
            options.AddArguments("start-maximized");
            return options;
        }
    }
}
