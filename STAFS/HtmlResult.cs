using OpenQA.Selenium;
using System;
using System.IO;

namespace STAF.CF
{
    public class HtmlResult
    {
        public static void TC_ResultStartTime(string strProject, string strFileName, string RelativePath)
        {
            DateTime now = DateTime.Now;
            strFileName = RelativePath + "\\" + strFileName + ".html";
            StreamWriter streamWriter = new StreamWriter((Stream)File.Open(strFileName, FileMode.Create, FileAccess.Write));
            if (streamWriter == null)
                return;
            streamWriter.WriteLine("<html>");
            streamWriter.WriteLine("<head>");
            streamWriter.WriteLine("<meta http-equiv=Content-Language+content=en-us>");
            streamWriter.WriteLine("<meta http-equiv=Content-Typecontent=text/html; charset=windows-1252>");
            streamWriter.WriteLine("<title>" + strProject + "</title>");
            streamWriter.WriteLine("</head>");
            streamWriter.WriteLine("<body>");
            streamWriter.WriteLine("<blockquote>");
            streamWriter.WriteLine("<table border=2 bordercolor=#000000 id=table1 width=844 height=31 bordercolorlight=#000000>");
            streamWriter.WriteLine("<tr>");
            streamWriter.WriteLine("<td COLSPAN = 5 bgcolor = #FFF8C6>");
            streamWriter.WriteLine("<p align=center><font color=#000080 size=4 face= \"Copperplate Gothic Bold\">&nbsp; Automation Script - " + strProject + "</font><font face= \"Copperplate Gothic Bold\"></font> </p>");
            streamWriter.WriteLine("</td>");
            streamWriter.WriteLine("</tr>");
            streamWriter.WriteLine("<tr>");
            streamWriter.WriteLine("<td COLSPAN = 5 bgcolor = #FFF8C6>");
            streamWriter.WriteLine("<p align=justify><b><font color=#000080 size=2 face= Verdana>&nbsp;START TIME:&nbsp;&nbsp;" + DateTime.Now.ToString("MM / dd / yyyy T hh : mm : ss") + "&nbsp");
            streamWriter.WriteLine("</td>");
            streamWriter.WriteLine("</tr>");
            streamWriter.WriteLine("<tr bgcolor=#8A4117>");
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
            streamWriter.Flush();
            streamWriter.Close();
        }

        public static void TC_ResultCreation(IWebDriver driver, string strFilename, string strModuleName, string strDesc, string strResult, string strLinkFile)
        {
            StreamWriter streamWriter = new StreamWriter((Stream)File.Open(strFilename, FileMode.Append, FileAccess.Write));
            int num1 = 0;
            if (strResult == "")
                strResult = "Not Executed";
            if (strModuleName != "")
            {
                int num2 = num1 + 1;
                streamWriter.WriteLine("<tr bgcolor = #FDEEF4 >");
                streamWriter.WriteLine("<td width=200>");
                streamWriter.WriteLine("<p align=center><font face=Verdana size=2>" + strModuleName + "</td>");
                streamWriter.WriteLine("<td width=400nowrap>");
                streamWriter.WriteLine("<p align=center><font face=Verdana size=2>" + strDesc + "</td>");
                streamWriter.WriteLine("<td width=200>");
                if (strResult.ToLower() == "pass")
                    streamWriter.WriteLine("<p align=center><font face=Verdana size=2> As Expected </td>");
                else if (strResult.ToLower() == "fail")
                    streamWriter.WriteLine("<p align=center><font face=Verdana size=2>Not As Expected </td>");
                else
                    streamWriter.WriteLine("<p align=center><font face=Verdana size=2>Not Run</td>");
                streamWriter.WriteLine("<td height=23width=200>");
                if (strResult.ToLower() == "pass")
                    streamWriter.WriteLine("<p align=center><a href=\"" + strLinkFile + "\"><b><font face=Verdana size=2 color=#008000>" + strResult + "</font></b></a></td>");
                else if (strResult.ToLower() == "fail")
                {
                    Screenshot image = ((ITakesScreenshot)driver).GetScreenshot();
                    Environment.SetEnvironmentVariable("failFlag", "yes");
                    strFilename = Environment.GetEnvironmentVariable("currTestName") == null ? "currTestName" : Environment.GetEnvironmentVariable("currTestName");
                    strModuleName = strModuleName.Replace(":", "");
                    strLinkFile = strFilename + "_" + strModuleName + "_" + DateTime.Now.ToString("MMddyyyymmss") + ".png";
                    image.SaveAsFile(strLinkFile, ScreenshotImageFormat.Png);
                    strLinkFile = "data:image/png;base64," + Convert.ToBase64String(File.ReadAllBytes(strLinkFile));
                    streamWriter.WriteLine("<p align=center><img class=\"screenshot\" src=\"" + strLinkFile + "\" class=\"img-circle\" width=\"304\" height=\"236\"/><b><font face=Verdana size=2 color=#FF0000>" + strResult + "</font></b></td>");
                }
                else
                    streamWriter.WriteLine("<p align=center><b><font face=Verdana size=2 color=#FF0888>" + strResult + "</font></b></td>");
                streamWriter.WriteLine("</tr>");
            }
            streamWriter.Flush();
            streamWriter.Close();
        }

        public static void TC_ResultCreation(string strFilename, string strModuleName, string strDesc, string strResult, string strLinkFile)
        {
            StreamWriter streamWriter = new StreamWriter((Stream)File.Open(strFilename, FileMode.Append, FileAccess.Write));
            int num1 = 0;
            if (strResult == "")
                strResult = "Not Executed";
            if (strModuleName != "")
            {
                int num2 = num1 + 1;
                streamWriter.WriteLine("<tr bgcolor = #FDEEF4 >");
                streamWriter.WriteLine("<td width=200>");
                streamWriter.WriteLine("<p align=center><font face=Verdana size=2>" + strModuleName + "</td>");
                streamWriter.WriteLine("<td width=400nowrap>");
                streamWriter.WriteLine("<p align=center><font face=Verdana size=2>" + strDesc + "</td>");
                streamWriter.WriteLine("<td width=200>");
                if (strResult.ToLower() == "pass")
                    streamWriter.WriteLine("<p align=center><font face=Verdana size=2> As Expected </td>");
                else if (strResult.ToLower() == "fail")
                    streamWriter.WriteLine("<p align=center><font face=Verdana size=2>Not As Expected </td>");
                else
                    streamWriter.WriteLine("<p align=center><font face=Verdana size=2>Not Run</td>");
                streamWriter.WriteLine("<td height=23width=200>");
                if (strResult.ToLower() == "pass")
                    streamWriter.WriteLine("<p align=center><a href=\"" + strLinkFile + "\"><b><font face=Verdana size=2 color=#008000>" + strResult + "</font></b></a></td>");
                else if (strResult.ToLower() == "fail")
                {
                    Environment.SetEnvironmentVariable("failFlag", "yes");
                    strFilename = Environment.GetEnvironmentVariable("currTestName") == null ? "currTestName" : Environment.GetEnvironmentVariable("currTestName");
                    streamWriter.WriteLine("<p align=center><b><font face=Verdana size=2 color=#FF0000>" + strResult + "</font></b></td>");
                }
                else
                    streamWriter.WriteLine("<p align=center><b><font face=Verdana size=2 color=#FF0888>" + strResult + "</font></b></td>");
                streamWriter.WriteLine("</tr>");
            }
            streamWriter.Flush();
            streamWriter.Close();
        }

        public static void TC_EndTime(string strFilename)
        {
            StreamWriter streamWriter = new StreamWriter((Stream)File.Open(strFilename, FileMode.Append, FileAccess.Write));
            streamWriter.WriteLine("<tr>");
            streamWriter.WriteLine("<td COLSPAN = 5 bgcolor = #FFF8C6>");
            streamWriter.WriteLine("<p align=justify><b><font color=#000080 size=2 face= Verdana>&nbsp;END TIME :&nbsp;&nbsp;" + DateTime.Now.ToString("MM / dd / yyyy T hh : mm : ss") + "&nbsp");
            streamWriter.WriteLine("</td>");
            streamWriter.WriteLine("</tr>");
            streamWriter.WriteLine("</table>");
            streamWriter.WriteLine("</body>");
            streamWriter.WriteLine("</html>");
            streamWriter.Flush();
            streamWriter.Close();
        }
    }

}
