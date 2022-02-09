using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;

namespace STAF.CF
{
    public class ReportResult
    {
        public static void ReportResultPass(IWebDriver driver, TestContext context, string moduleName, string description, string exception = "")
        {
            string result = "PASS";
            if (Environment.GetEnvironmentVariable(context.TestName) != null)
            {
                HtmlResult.TC_ResultCreation(driver, Environment.GetEnvironmentVariable(context.TestName), moduleName, description, result, "");
            }
            //TTPReporter.ReportEvent(result, moduleName, description+ " "+exception);
            Console.WriteLine(moduleName + " " + description + " " + result + " " + exception);
        }

        public static void ReportResultFail(IWebDriver driver, TestContext context, string moduleName, string description, string exception = "")
        {
            string result = "FAIL";

            if (Environment.GetEnvironmentVariable(context.TestName) != null)
            {
                try
                {
                    HtmlResult.TC_ResultCreation(driver, Environment.GetEnvironmentVariable(context.TestName), moduleName, description, result, "");
                }
                catch (Exception)
                {
                    Console.WriteLine(moduleName + " " + description + " " + result + " " + exception);
                    //HtmlResult.TC_ResultCreation(Environment.GetEnvironmentVariable("resultFile"), moduleName, description, result, "");
                }
            }
            //TTPReporter.ReportEvent(result, moduleName, description +" " +exception);
            Console.WriteLine(moduleName + " " + description + " " + result + " " + exception);
        }
    }

    public class ReportResultAPI
    {
        public static void ReportResultPass(TestContext context, string moduleName, string description, string exception = "")
        {
            string result = "PASS";
            if (Environment.GetEnvironmentVariable(context.TestName) != null)
            {
                HtmlResult.TC_ResultCreation(Environment.GetEnvironmentVariable(context.TestName), moduleName, description, result, "");
            }
            //TTPReporter.ReportEvent(result, moduleName, description+ " "+exception);
            Console.WriteLine(moduleName + " " + description + " " + result + " " + exception);
        }

        public static void ReportResultFail(TestContext context, string moduleName, string description, string exception = "")
        {
            string result = "FAIL";

            if (Environment.GetEnvironmentVariable(context.TestName) != null)
            {
                try
                {
                    HtmlResult.TC_ResultCreation(Environment.GetEnvironmentVariable(context.TestName), moduleName, description, result, "");
                }
                catch (Exception)
                {
                    Console.WriteLine(moduleName + " " + description + " " + result + " " + exception);
                    //HtmlResult.TC_ResultCreation(Environment.GetEnvironmentVariable("resultFile"), moduleName, description, result, "");
                }
            }
            //TTPReporter.ReportEvent(result, moduleName, description +" " +exception);
            Console.WriteLine(moduleName + " " + description + " " + result + " " + exception);
        }
    }
}
