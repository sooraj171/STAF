using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;

namespace STAF.CF
{
    public class ReportResult
    {
        public static void ReportResultStatus(IWebDriver driver, TestContext context, string moduleName, string description, string result, string exception = "")
        {
            string testName = Environment.GetEnvironmentVariable(context.TestName) ?? string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(testName))
                {
                    HtmlResult.TC_ResultCreation(driver, testName, moduleName, description, result, "");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in {moduleName} {description} {result}. Exception: {ex.Message}");
            }

            Console.WriteLine($"{moduleName} {description} {result} {exception}");
        }

        public static void ReportResultPass(IWebDriver driver, TestContext context, string moduleName, string description, string exception = "")
            => ReportResultStatus(driver, context, moduleName, description, "PASS", exception);

        public static void ReportResultFail(IWebDriver driver, TestContext context, string moduleName, string description, string exception = "")
            => ReportResultStatus(driver, context, moduleName, description, "FAIL", exception);

        public static void ReportResultWarn(IWebDriver driver, TestContext context, string moduleName, string description, string exception = "")
            => ReportResultStatus(driver, context, moduleName, description, "WARNING", exception);

        public static void ReportResultInfo(IWebDriver driver, TestContext context, string moduleName, string description, string exception = "")
            => ReportResultStatus(driver, context, moduleName, description, "INFO", exception);
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

        public static void ReportResultWarn(TestContext context, string moduleName, string description, string exception = "")
        {
            string result = "WARNING";

            if (Environment.GetEnvironmentVariable(context.TestName) != null)
            {
                try
                {
                    HtmlResult.TC_ResultCreation(Environment.GetEnvironmentVariable(context.TestName), moduleName, description, result, "");
                }
                catch (Exception)
                {
                    Console.WriteLine(moduleName + " " + description + " " + result + " " + exception);
                }
            }
            Console.WriteLine(moduleName + " " + description + " " + result + " " + exception);
        }
        public static void ReportResultInfo(TestContext context, string moduleName, string description, string exception = "")
        {
            string result = "INFO";

            if (Environment.GetEnvironmentVariable(context.TestName) != null)
            {
                try
                {
                    HtmlResult.TC_ResultCreation(Environment.GetEnvironmentVariable(context.TestName), moduleName, description, result, "");
                }
                catch (Exception)
                {
                    Console.WriteLine(moduleName + " " + description + " " + result + " " + exception);
                }
            }
            Console.WriteLine(moduleName + " " + description + " " + result + " " + exception);
        }
    }
}
