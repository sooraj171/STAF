using System;
using System.Reflection;
using System.Net.Mail;
using System.Xml;
using System.Text;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using SATF;
using Microsoft.Extensions.Configuration;

namespace STAF.CF
{
    public class CommonAction
    {
        public static IWebDriver driver;

        // Enter mailfrom mailto mailSubject mailBody as html and Attachmentpath if available: Sooraj
        public static void SendEmail(string mailFrom, string mailTo, string mSubject, string mhtmlBody, string mAttachmentPath,TestContext context=null)
        {

            mailTo = mailTo == null ? "" : mailTo;
            mailFrom = mailFrom == null ? "" : mailFrom;
            if (mailTo.Trim() != "" && mailFrom.Trim() != "")
            {

                string[] to = mailTo.Split(';');


                MailAddress AddressFrom = new MailAddress(mailFrom);
                MailMessage MyMail = new MailMessage();
                MyMail.From = AddressFrom;
                foreach (var mId in to)
                {
                    MyMail.To.Add(mId);
                }

                MyMail.IsBodyHtml = true;
                MyMail.Subject = mSubject;
                MyMail.Body = mhtmlBody;
                if (mAttachmentPath.Trim() != "")
                {
                    string[] attach = mAttachmentPath.Split(';');

                    foreach (var att in attach)
                    {
                        MyMail.Attachments.Add(new Attachment(att));
                    }
                }
                string StrSmtpHost = "";
                int IntSmtpPort = 0;
                bool boolDefMailCred = true;

                if (AppConfig.GetConfig() != null)
                {
                     StrSmtpHost = AppConfig.GetConfig().GetSection("Email:SmtpHost").Value;
                     IntSmtpPort = Convert.ToInt32(AppConfig.GetConfig().GetSection("Email:SmtpPort").Value);
                     boolDefMailCred = Convert.ToBoolean(AppConfig.GetConfig().GetSection("Email:UseDefaultCred").Value);

                }
                else 
                {
                     StrSmtpHost = context.Properties["smtphost"] == null ? "" : context.Properties["smtphost"].ToString().ToLower();
                     IntSmtpPort = Convert.ToInt32(context.Properties["smtpport"] == null ? 0 : context.Properties["smtpport"].ToString().ToLower());
                     boolDefMailCred = true;
                }

                SmtpClient MySmtpClient = SetMailServer(StrSmtpHost, boolDefMailCred, IntSmtpPort);

                try
                {
                    MySmtpClient.Send(MyMail);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception caught in Sending Email(): {0}",
                    ex.ToString());
                }
            }

        }

        public static string EmailBody(string path)
        {
            //string strfulltext = System.IO.File.ReadAllText(path);
            string[] lines = System.IO.File.ReadAllLines(path);

            StringBuilder msgBody = new StringBuilder();
            foreach (string line in lines)
            {
                msgBody.AppendLine(line);
            }
            msgBody.Replace("</body>", "");
            msgBody.Replace("</html>", "");
            msgBody.AppendLine("</body>");
            msgBody.AppendLine("</html>");

            return msgBody.ToString();
        }

        public static string zipMailFiles(string path)
        {

            string zipPath = path + @"\detailresult.zip";
            //ZipFile.CreateFromDirectory(path + @"\Detailed", zipPath);
            return zipPath;

        }

        public static void LoadAppEnvVariables(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlNodeList parent = doc.GetElementsByTagName("appData");
            foreach (XmlNode node in parent[0].ChildNodes)
            {
                Environment.SetEnvironmentVariable(node.Name, node.InnerText.ToString());
            }

        }

        public static void updateEnvVal(string TagName, string value)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(DirectoryUtils.BaseDirectory + "\\EnvVal.xml");
                var resVal = doc.GetElementsByTagName(TagName);

                if (resVal.Count > 0)
                {
                    resVal.Item(0).InnerText = value;
                }
                else
                {
                    XmlNode root = doc.DocumentElement;
                    XmlElement elem = doc.CreateElement(TagName);
                    elem.InnerText = value;
                    root.AppendChild(elem);
                }

                doc.Save(DirectoryUtils.BaseDirectory + "\\EnvVal.xml");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Updating Environment Value Failed: " + ex.Message);
            }

        }

        public static string setStartUpValues(TestContext currTestName)
        {
            string resultFile = "";
            try
            {
                Environment.SetEnvironmentVariable("currTestName", currTestName.TestName);
                resultFile = currTestName.TestRunDirectory + "\\" + currTestName.TestName + ".html"; // Updated to use TestRunDirectory
                Environment.SetEnvironmentVariable(currTestName.TestName, resultFile);
                Environment.SetEnvironmentVariable("failFlag", "no");
                string StrProject = currTestName.Properties["project"] == null ? "TestSteps" : "TestSteps:" + currTestName.Properties["project"].ToString();
                HtmlResult.TC_ResultStartTime(StrProject, currTestName.TestName, currTestName.TestRunDirectory); // Updated to use TestRunDirectory
                // System.IO.File.Copy(DirectoryUtils.BaseDirectory + "\\ResultTemplate.html", currTestName.TestRunDirectory + @"\ResultTemplate.html"); // Updated to use TestRunDirectory
            }
            catch (Exception)
            {
                return resultFile;
            }
            return resultFile;
        }

        private static readonly object _resultLock = new object();

        public static void setCleanUpValues(string currresultFile, TestContext currTestContext, string totTime)
        {
            string currTestName = currTestContext.TestName;

            HtmlResult.TC_EndTime(currresultFile);
            currTestContext.AddResultFile(currresultFile);
            string contents = File.ReadAllText(currresultFile);

            lock (_resultLock)
            {
                string strBody = Environment.GetEnvironmentVariable("resultbody") ?? "";
                StringBuilder mailBody = new StringBuilder("");

                if (string.IsNullOrEmpty(strBody))
                {
                    mailBody.AppendLine("<html>");
                    mailBody.AppendLine("<body>");
                    mailBody.AppendLine("<p><font color=#000080 size=4>Automation Test Result Status :</font></p>");
                    Environment.SetEnvironmentVariable("resultbody", mailBody.ToString());
                }
                else
                {
                    mailBody.AppendLine(strBody);
                }

                StringBuilder resBody = new StringBuilder();
                bool failed = string.Equals(Environment.GetEnvironmentVariable("failFlag"), "yes", StringComparison.OrdinalIgnoreCase);

                if (failed)
                {
                    resBody.AppendLine("<div class=\"gs\">");
                    resBody.AppendLine("<div class=\"g\">");
                    resBody.AppendLine("<div class=\"g-h failed\">" + currTestName + "</div>");
                    resBody.AppendLine("<div class=\"g-c\">");
                    resBody.AppendLine("<div class=\"g\">");
                    resBody.AppendLine(contents + "</div></div>");
                    mailBody.AppendLine(@"<p><font face=Verdana size=2>" + currTestName + ": <b><font face=Verdana size=2 color=#FF0000>FAIL</font></b></p>");
                    Environment.SetEnvironmentVariable("resultbody", mailBody.ToString());
                    Environment.SetEnvironmentVariable("OverallFailFlag", "yes");
                }
                else
                {
                    resBody.AppendLine("<div class=\"gs\">");
                    resBody.AppendLine("<div class=\"g\">");
                    resBody.AppendLine("<div class=\"g-h passed\">" + currTestName + "</div>");
                    resBody.AppendLine("<div class=\"g-c\">");
                    resBody.AppendLine("<div class=\"g\">");
                    resBody.AppendLine(contents + "</div></div>");
                    mailBody.AppendLine(@"<p><font face=Verdana size=2>" + currTestName + ": <b><font face=Verdana size=2 color=#008000>PASS</font></b></p>");
                    Environment.SetEnvironmentVariable("resultbody", mailBody.ToString());
                }

                Environment.SetEnvironmentVariable("resultbodyfinal", (Environment.GetEnvironmentVariable("resultbodyfinal") ?? "") + resBody.ToString());

                string accumulatorPath = Environment.GetEnvironmentVariable("resultAccumulatorPath")
                    ?? Path.Combine(DirectoryUtils.BaseDirectory, "ResultBodyAccumulator.txt");
                try
                {
                    File.AppendAllText(accumulatorPath, resBody.ToString(), Encoding.UTF8);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("setCleanUpValues: Could not append to accumulator: " + ex.Message);
                }
            }
        }

        public static SmtpClient SetMailServer(string StrSMTPHost, bool UseDefaultCred, int SMTPPort=0,string UserName="",string Password="")
        {


            SmtpClient smtpClient = new SmtpClient(StrSMTPHost);
            smtpClient.EnableSsl = true;
            if (SMTPPort!=0)
                smtpClient.Port = SMTPPort;
            if (UseDefaultCred)
            {
                smtpClient.UseDefaultCredentials = UseDefaultCred;
            }
            else 
            {
                UserName = AppConfig.GetConfig().GetSection("Email:Username").Value;
                Password = AppConfig.GetConfig().GetSection("Email:Password").Value;
                smtpClient.Credentials = new NetworkCredential(UserName, Password);
            }
            return smtpClient;
        }
        


    }

    public static class WebDriverExtensions
    {
        public static IWebDriver CloseAllTabsExceptCurrent(IWebDriver driver)
        {
            try
            {
                int tabCnt = getTotalTabsCount(driver);

                if (tabCnt > 1)
                {
                    var mainWin = driver.CurrentWindowHandle;

                    driver.WindowHandles.Where(w => w != mainWin).ToList()
                        .ForEach(w =>
                       {
                           driver.SwitchTo().Window(w);
                           driver.Close();
                       });
                }
            }
            catch
            {
                //ReportResult.ReportResultFail("CloseAllTabsExceptCurrent", "Failed to Close all open tabs. ");
            }
            return driver;
        }

        public static int getTotalTabsCount(IWebDriver driver)
        {
            int cnt = 0;

            try
            {
                List<string> windowLs = new List<string>(driver.WindowHandles);
                cnt = windowLs.Count;
            }
            catch
            {
                //CommonAction.ReportResultFail("getTotalTabsCount", "Failed to get list of all tabs");
                return 0;
            }
            return cnt;
        }
        public static IWebElement waitForFindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => drv.FindElement(by));
            }
            return driver.FindElement(by);
        }

        public static bool waitForElementExist(this IWebDriver driver, IWebElement elmObject, int timeoutInSeconds)
        {
            bool res = false;
            if (timeoutInSeconds > 0)
            {
                WebDriverWait wttest = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                var tbObj = wttest.Until(d =>
                {
                    if (elmObject != null)
                    {
                        return true;
                    }
                    else { return false; }
                }
                  );
            }
            return res;
        }

        public static bool waitForElementNotExist(this IWebDriver driver, IWebElement elmObject, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                WebDriverWait wttest = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));

                var tbObj = wttest.Until(d =>
                {
                    if (elmObject != null)
                    {
                        return true;
                    }
                    else { return false; }
                }
                  );
            }
            return true;
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
                            tempTO = tempTO + 1;
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
        public static bool WaitForDocumentReady(this IWebDriver driver)
        {
            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(10));

                //int cnt = 0;
                bool retFlag = false;
                if ((bool)jse.ExecuteScript("return window.jQuery != undefined"))
                {

                    wait.Until(d => (bool)jse.ExecuteScript("return jQuery.active == 0"));

                    wait.Until(d => (bool)jse.ExecuteScript("return document.readyState == 'complete'"));
                    retFlag = true;
                }
                else
                {
                    wait.Until(d => (bool)jse.ExecuteScript("return document.readyState == 'complete'"));
                    retFlag = true;
                }

                return retFlag;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }


   
    public static class DirectoryUtils
    {
        public static string BaseDirectory
        {
            get
            {
                string name = Assembly.GetExecutingAssembly().Location;
                name = Path.GetDirectoryName(name);
                //
                // Support for unit testing on the desktop. CodeBase returns a path with "file:\" at the start
                // under windows, but not under Windows CE. The code that uses this property expects
                // the path without the file:\ at the start.
                //
                if (System.Environment.OSVersion.Platform != PlatformID.WinCE)
                {
                    if (name.StartsWith(@"file:\"))
                        name = name.Remove(0, 6);
                }
                return name;
            }
        }
    }
}
