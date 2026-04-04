using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;
using System.Collections.Concurrent;
using System.IO;

namespace STAF.CF
{
    public class HtmlResult
    {
        private static readonly ConcurrentDictionary<string, object> FileLocks = new ConcurrentDictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        private static object GetLockForPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return FileLocks.GetOrAdd("_empty_", _ => new object());
            try
            {
                string full = Path.GetFullPath(path);
                return FileLocks.GetOrAdd(full, _ => new object());
            }
            catch
            {
                return FileLocks.GetOrAdd(path, _ => new object());
            }
        }

        public static void TC_ResultStartTime(string strProject, string strFileName, string RelativePath)
        {
            string htmlPath = Path.Combine(RelativePath, strFileName + ".html");
            lock (GetLockForPath(htmlPath))
            {
                using (StreamWriter streamWriter = new StreamWriter((Stream)File.Open(htmlPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite)))
                {
                    BuildHtmlString(streamWriter, strProject);
                    streamWriter.Flush();
                }
            }
        }

        public static void TC_ResultCreation(IWebDriver driver, string strFilename, string strModuleName, string strDesc, string strResult, string strLinkFile)
        {
            string htmlReportPath = strFilename;
            lock (GetLockForPath(htmlReportPath))
            {
                using (StreamWriter streamWriter = new StreamWriter((Stream)File.Open(htmlReportPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite)))
                {
                    int num1 = 0;
                    if (strResult == "")
                        strResult = "Not Executed";
                    if (strModuleName != "")
                    {
                        int num2 = num1 + 1;
                        streamWriter.WriteLine("<tr class='result' bgcolor = #80D8FF >");
                        streamWriter.WriteLine("<td width=200>");
                        streamWriter.WriteLine("<p align=center><font face=Verdana size=2>" + strModuleName + "</td>");
                        streamWriter.WriteLine("<td width=400nowrap>");
                        streamWriter.WriteLine("<p align=center><font face=Verdana size=2>" + strDesc + "</td>");
                        streamWriter.WriteLine("<td width=200>");
                        if (strResult.ToLower() == "pass")
                            streamWriter.WriteLine("<p align=center><font face=Verdana size=2> As Expected </td>");
                        else if (strResult.ToLower() == "fail")
                            streamWriter.WriteLine("<p align=center><font face=Verdana size=2>Not As Expected </td>");
                        else if (strResult.ToLower() == "warning")
                            streamWriter.WriteLine("<p align=center><font face=Verdana size=2>Warning </td>");
                        else if (strResult.ToLower() == "info")
                            streamWriter.WriteLine("<p align=center><font face=Verdana size=2>Information </td>");
                        else
                            streamWriter.WriteLine("<p align=center><font face=Verdana size=2>Not Run</td>");
                        streamWriter.WriteLine("<td height=23width=200>");
                        if (strResult.ToLower() == "pass")
                            streamWriter.WriteLine("<p align=center><a href=\"" + strLinkFile + "\"><b><font face=Verdana size=2 color=#008000>" + strResult + "</font></b></a></td>");
                        else if (strResult.ToLower() == "fail")
                        {
                            Screenshot image = ((ITakesScreenshot)driver).GetScreenshot();
                            TestRunState.SetFailed();
                            string screenshotDir = Path.GetDirectoryName(htmlReportPath);
                            if (string.IsNullOrEmpty(screenshotDir))
                                screenshotDir = Directory.GetCurrentDirectory();
                            string moduleSafe = strModuleName.Replace(":", "").Replace(Path.DirectorySeparatorChar, '_').Replace(Path.AltDirectorySeparatorChar, '_');
                            string pngName = Path.GetFileNameWithoutExtension(htmlReportPath) + "_" + moduleSafe + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + Guid.NewGuid().ToString("N")[..8] + ".png";
                            strLinkFile = Path.Combine(screenshotDir, pngName);
                            image.SaveAsFile(strLinkFile);
                            strLinkFile = "data:image/png;base64," + Convert.ToBase64String(File.ReadAllBytes(strLinkFile));
                            streamWriter.WriteLine("<p align=center><img class=\"screenshot\" src=\"" + strLinkFile + "\" class=\"img-circle\" width=\"304\" height=\"236\"/><b><font face=Verdana size=2 color=#FF0000>" + strResult + "</font></b></td>");
                        }
                        else if (strResult.ToLower() == "warning")
                        {
                            streamWriter.WriteLine("<p align=center><b><font face=Verdana size=2 color=#ff9933>" + strResult + "</font></b></td>");
                        }
                        else if (strResult.ToLower() == "info")
                        {
                            streamWriter.WriteLine("<p align=center><b><font face=Verdana size=2 color=#ffffff>" + strResult + "</font></b></td>");
                        }
                        else
                            streamWriter.WriteLine("<p align=center><b><font face=Verdana size=2 color=#FF0888>" + strResult + "</font></b></td>");
                        streamWriter.WriteLine("</tr>");
                    }
                    streamWriter.Flush();
                }
            }
        }


        public static void TC_ResultCreation(string strFilename, string strModuleName, string strDesc, string strResult, string strLinkFile)
        {
            string htmlReportPath = strFilename;
            lock (GetLockForPath(htmlReportPath))
            {
                using (StreamWriter streamWriter = new StreamWriter((Stream)File.Open(htmlReportPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite)))
                {
                    int num1 = 0;
                    if (strResult == "")
                        strResult = "Not Executed";
                    if (strModuleName != "")
                    {
                        int num2 = num1 + 1;
                        streamWriter.WriteLine("<tr class='result' bgcolor = #80D8FF >");
                        streamWriter.WriteLine("<td width=200>");
                        streamWriter.WriteLine("<p align=center><font face=Verdana size=2>" + strModuleName + "</td>");
                        streamWriter.WriteLine("<td width=400nowrap>");
                        streamWriter.WriteLine("<p align=center><font face=Verdana size=2>" + strDesc + "</td>");
                        streamWriter.WriteLine("<td width=200>");
                        if (strResult.ToLower() == "pass")
                            streamWriter.WriteLine("<p align=center><font face=Verdana size=2> As Expected </td>");
                        else if (strResult.ToLower() == "fail")
                            streamWriter.WriteLine("<p align=center><font face=Verdana size=2>Not As Expected </td>");
                        else if (strResult.ToLower() == "warning")
                            streamWriter.WriteLine("<p align=center><font face=Verdana size=2>Warning </td>");
                        else if (strResult.ToLower() == "info")
                            streamWriter.WriteLine("<p align=center><font face=Verdana size=2>Information </td>");
                        else
                            streamWriter.WriteLine("<p align=center><font face=Verdana size=2>Not Run</td>");
                        streamWriter.WriteLine("<td height=23width=200>");
                        if (strResult.ToLower() == "pass")
                            streamWriter.WriteLine("<p align=center><a href=\"" + strLinkFile + "\"><b><font face=Verdana size=2 color=#008000>" + strResult + "</font></b></a></td>");
                        else if (strResult.ToLower() == "fail")
                        {
                            TestRunState.SetFailed();
                            streamWriter.WriteLine("<p align=center><b><font face=Verdana size=2 color=#FF0000>" + strResult + "</font></b></td>");
                        }
                        else if (strResult.ToLower() == "warning")
                        {
                            streamWriter.WriteLine("<p align=center><b><font face=Verdana size=2 color=#ff9933>" + strResult + "</font></b></td>");
                        }
                        else if (strResult.ToLower() == "info")
                        {
                            streamWriter.WriteLine("<p align=center><b><font face=Verdana size=2 color=#ffffff>" + strResult + "</font></b></td>");
                        }
                        else
                            streamWriter.WriteLine("<p align=center><b><font face=Verdana size=2 color=#FF0888>" + strResult + "</font></b></td>");
                        streamWriter.WriteLine("</tr>");
                    }
                    streamWriter.Flush();
                }
            }
        }

        public static void TC_EndTime(string strFilename)
        {
            lock (GetLockForPath(strFilename))
            {
                using (StreamWriter streamWriter = new StreamWriter((Stream)File.Open(strFilename, FileMode.Append, FileAccess.Write, FileShare.ReadWrite)))
                {
                    streamWriter.WriteLine("<tr>");
                    streamWriter.WriteLine("<td class='headBk' COLSPAN = 4>");
                    streamWriter.WriteLine("<p align=justify><b><font color=white size=2 face= Verdana>&nbsp;END TIME :&nbsp;&nbsp;" + DateTime.Now.ToString("MM / dd / yyyy T hh : mm : ss") + "&nbsp");
                    streamWriter.WriteLine("</td>");
                    streamWriter.WriteLine("</tr>");
                    streamWriter.WriteLine("</table>");
                    streamWriter.WriteLine("</body>");
                    streamWriter.WriteLine("</html>");
                    streamWriter.Flush();
                }
            }
        }

        private static void BuildHtmlString(StreamWriter streamWriter, string strProject)
        {
            streamWriter.WriteLine("<html>");
            streamWriter.WriteLine("<head>");
            streamWriter.WriteLine("<meta http-equiv=Content-Language+content=en-us>");
            streamWriter.WriteLine("<meta http-equiv=Content-Typecontent=text/html; charset=windows-1252>");
            streamWriter.WriteLine("<title>" + strProject + "</title>");
            streamWriter.WriteLine("<style>");
            streamWriter.WriteLine(".result:hover { background-color: #FFF8C6; font-weight: bold;}");
            streamWriter.WriteLine(".headBk { background-color: #2962FF;} </style>");
            streamWriter.WriteLine("</head>");
            streamWriter.WriteLine("<body>");
            streamWriter.WriteLine("<blockquote>");
            streamWriter.WriteLine("<table border=2 bordercolor=#000000 id=table1 width=844 height=31 bordercolorlight=#000000");
            streamWriter.WriteLine("<tr>");
            streamWriter.WriteLine("<td COLSPAN = 4 class = 'headBk'>");
            streamWriter.WriteLine("<p align=center><font color=yellow size=4 face= \"Copperplate Gothic Bold\">&nbsp; Automation Script - " + strProject + "</font><font face= \"Copperplate Gothic Bold\"></font> </p>");
            streamWriter.WriteLine("</td>");
            streamWriter.WriteLine("</tr>");
            streamWriter.WriteLine("<tr>");
            streamWriter.WriteLine("<td COLSPAN = 4  class = 'headBk'>");
            streamWriter.WriteLine("<p align=justify><b><font color=white size=2 face= Verdana>&nbsp;START TIME:&nbsp;&nbsp;" + DateTime.Now.ToString("MM / dd / yyyy T hh : mm : ss") + "&nbsp");
            streamWriter.WriteLine("</td>");
            streamWriter.WriteLine("</tr>");
            streamWriter.WriteLine("<tr bgcolor=#448AFF>");
            streamWriter.WriteLine("<td width=200");
            streamWriter.WriteLine("<p align=center><b><font color = white face=Arial Narrow size=2>Module Name</b></td>");
            streamWriter.WriteLine("<td width=400>");
            streamWriter.WriteLine("<p align=center><b><font color = white face=Arial Narrow size=2>Description</b></td>");
            streamWriter.WriteLine("<td width=200>");
            streamWriter.WriteLine("<p align=center><b><font color = white face=Arial Narrow size=2>Actual Result</b></td>");
            streamWriter.WriteLine("<td width=200>");
            streamWriter.WriteLine("<p align=center><b><font color = white face=Arial Narrow size=2>Execution Status</b></td>");
            streamWriter.WriteLine("</tr>");
            streamWriter.WriteLine("</blockquote>");
            streamWriter.WriteLine("</body>");
            streamWriter.WriteLine("</html>");
        }
    }

    public static class ReportElement
    {
        /// <summary>
        /// Verify Element is not null(Exists) and Report status to HTML result.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="Driver"></param>
        /// <param name="context"></param>
        /// <param name="testName"></param>
        /// <param name="desc"></param>
        /// <param name="ProdceedFlag">if false it will stop test execution. Deafult is true</param>
        public static void ReportElementExists(this IWebElement element, IWebDriver Driver, TestContext context, string testName, string desc, bool ProdceedFlag = true)
        {
            try
            {

                if (element != null)
                {
                    ReportResult.ReportResultPass(Driver, context, testName, desc + " true");
                }
                else
                {
                    if (ProdceedFlag)
                    {
                        ReportResult.ReportResultFail(Driver, context, testName, desc + " false");
                    }
                    else
                    {
                        Assert.Fail();
                    }
                }
            }
            catch (Exception)
            {
                ReportResult.ReportResultFail(Driver, context, testName, desc + " false");
                if (ProdceedFlag == false)
                {
                    Assert.Fail(testName + " : " + desc + " false");
                }
            }
        }

        /// <summary>
        /// Verify Element is Displayed and Report status to HTML result.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="Driver"></param>
        /// <param name="context"></param>
        /// <param name="testName"></param>
        /// <param name="desc"></param>
        /// <param name="ProdceedFlag"></param>
        public static void ReportElementIsDisplayed(this IWebElement element, IWebDriver Driver, TestContext context, string testName, string desc, bool ProdceedFlag = true)
        {
            try
            {

                if (element.Displayed)
                {
                    ReportResult.ReportResultPass(Driver, context, testName, desc + " true");
                }
                else
                {
                    if (ProdceedFlag)
                    {
                        ReportResult.ReportResultFail(Driver, context, testName, desc + " false");
                    }
                    else
                    {
                        Assert.Fail();
                    }
                }
            }
            catch (Exception)
            {
                ReportResult.ReportResultFail(Driver, context, testName, desc + " false");
                if (ProdceedFlag == false)
                {
                    Assert.Fail(testName + " : " + desc + " false");
                }
            }
        }

        /// <summary>
        /// Verify Element is Enabled and Report status to HTML result.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="Driver"></param>
        /// <param name="context"></param>
        /// <param name="testName"></param>
        /// <param name="desc"></param>
        /// <param name="ProdceedFlag"></param>
        public static void ReportElementIsEnabled(this IWebElement element, IWebDriver Driver, TestContext context, string testName, string desc, bool ProdceedFlag = true)
        {
            try
            {

                if (element.Enabled)
                {
                    ReportResult.ReportResultPass(Driver, context, testName, desc + " true");
                }
                else
                {
                    if (ProdceedFlag)
                    {
                        ReportResult.ReportResultFail(Driver, context, testName, desc + " false");
                    }
                    else
                    {
                        Assert.Fail();
                    }
                }
            }
            catch (Exception)
            {
                ReportResult.ReportResultFail(Driver, context, testName, desc + " false");
                if (ProdceedFlag == false)
                {
                    Assert.Fail(testName + " : " + desc + " false");
                }
            }
        }

        public static IWebDriver GetWebDriverFromElement(IWebElement element)
        {
            IWebDriver driver = null;

            if (element.GetType().ToString() == "OpenQA.Selenium.Support.PageObjects.WebElementProxy")
            {
                driver = ((IWrapsDriver)element
                                 .GetType().GetProperty("WrappedElement")
                                 .GetValue(element, null)).WrappedDriver;
            }
            else
            {
                driver = ((IWrapsDriver)element).WrappedDriver;
            }
            return driver;
        }
    }
}
