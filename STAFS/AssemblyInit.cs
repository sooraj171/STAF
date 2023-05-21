using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Linq;
using System.IO;
using STAF.CF;

namespace STAF.CF
{
    [TestClass]
    public class AssemblyInit
    {
        private static string resTestDir = "";
        private static string SentEmail = "false";
        //private static string MailTo = "";
        //private static string MailFrom = "donotreply@test.com";
        private static TestContext _testContext;

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext tc)
        {
            try
            {
                //Console.WriteLine("Before all tests");
                var driverProcess = Process.GetProcesses().Where(pr => pr.ProcessName == "chromedriver");
                _testContext = tc;
                foreach (var process in driverProcess)
                {
                    process.Kill();
                }
                resTestDir = tc.TestDir;
                string locPath = tc.DeploymentDirectory;
                MakeAfile(DirectoryUtils.BaseDirectory + "\\ResultTemplate.html");
                Environment.SetEnvironmentVariable("OverallFailFlag", "No");
                Environment.SetEnvironmentVariable("resultbodyfinal", "");
                SentEmail=tc.Properties["useemail"]==null?"": tc.Properties["useemail"].ToString().ToLower();
                
            }
            catch { }
        }

        [AssemblyCleanup]
        public static void AssemblyCleanUp()
        {
            try
            {
                closeAllBrowser();

                StreamWriter writer;
                string overallResult = DirectoryUtils.BaseDirectory + "\\ResultTemplate.html";
                writer = new StreamWriter(File.Open(overallResult, FileMode.Append, FileAccess.Write, FileShare.Write));
                writer.WriteLine(Environment.GetEnvironmentVariable("resultbodyfinal"));
                writer.Flush();
                writer.Close();

                File.Copy(DirectoryUtils.BaseDirectory + "\\ResultTemplate.html", Directory.GetParent(resTestDir) + @"\ResultTemplateFinal.html",true);
                //if (SentEmail == "true")
                //{
                //    try
                //    {
                //        MailFrom= _testContext.Properties["mailfrom"] == null ? "" : _testContext.Properties["mailfrom"].ToString().ToLower();
                //        MailTo = _testContext.Properties["mailto"] == null ? "" : _testContext.Properties["mailto"].ToString().ToLower();
                //        CommonAction.SendEmail(MailFrom, MailTo, "Automation Test Result", Environment.GetEnvironmentVariable("resultbody"), Directory.GetParent(resTestDir) + @"\ResultTemplateFinal.html",_testContext);
                //    }
                //    catch { Console.WriteLine("Error in sending email."); }
                //}
                if (Environment.GetEnvironmentVariable("OverallFailFlag").ToLower() == "yes")
                {
                    Assert.Fail("Some Test Cases failed in execution");
                }
            }
            catch { }
        }

        public static void closeAllBrowser()
        {
            var driverP = Process.GetProcesses().Where(pr => pr.ProcessName == "chromedriver");
            foreach (var process in driverP)
            {
                process.Kill();
            }
        }

        private static void MakeAfile(string StrFileName)
        {
            StreamWriter sw = new ((Stream)File.Open(StrFileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite));

            sw.WriteLine("<html>");
            sw.WriteLine("<head>");
            sw.WriteLine("<title>Test Result By Sooraj</title>");
            sw.WriteLine("<meta name=\"author\" content=\"Sooraj Ramachandran\">");
            sw.WriteLine("<meta name=\"ResultTestLog\"/>");
            sw.WriteLine("<script type=\"text/javascript\" src=\"http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.6.2.min.js\"></script>");

            sw.WriteLine("<style type=\"text/css\">");
            sw.WriteLine(".ui-icon { width: 16px; height: 16px; background-repeat:no-repeat; }");
            sw.WriteLine(".ui-icon-triangle-1-e { background-position: 7px 3px; }");
            sw.WriteLine(".ui-icon-triangle-1-s { background-position: 7px 6px; }");
            sw.WriteLine(".ui-icon-circle-arrow-e { background-position: 0px 0px; }");
            sw.WriteLine(".ui-icon-check { background-position: 0px 0px; }");
            sw.WriteLine(".ui-icon-alert { background-position: 0px 0px; }");
            sw.WriteLine(".ui-icon-info { background-position: 0px 0px; }");
            sw.WriteLine(".ui-icon-circle-close { background-position: 0px 0px; }");
            sw.WriteLine(".ui-icon-star { background-position: 0px 0px; }");
            sw.WriteLine("</style>");


            sw.WriteLine("<script type=\"text/javascript\">");

            sw.WriteLine("$(function () {");

            // Hide all group contents by default
            sw.WriteLine("$(\".log .g-c\").hide();");

            //Update the total time for the test
            sw.WriteLine("$(\".total-time\").append($('input#totaltime').val());");

            // Add error and fail symbols to group headers
            sw.WriteLine("$(\".g-h.failed\").prepend($(\"<span/>\").addClass(\"ui-icon ui-icon-circle-close marker-1-e\"));");
            sw.WriteLine("$(\".g-h.warning\").prepend($(\"<span />\").addClass(\"ui-icon ui-icon-alert marker-1-e\"));");
            sw.WriteLine("$(\".g-h.passed\").prepend($(\"<span />\").addClass(\"ui-icon ui-icon-check marker-1-e\"));");

            // Make each group header accordion like - selectable look, triangle icon, hover
            // and click events.

            sw.WriteLine("$(\".log .g-h\")");

            sw.WriteLine(".prepend($(\"<span />\").addClass(\"ui-icon ui-icon-triangle-1-e\"))");
            sw.WriteLine(".click(function () { toggleGroup($(this)); })");
            sw.WriteLine(".hover(function () { $(this).addClass(\"hoverColor\"); }, function () { $(this).removeClass(\"hoverColor\"); });");

            // Pre-expand all failing groups

            //expandInstant($(".log .g-h.failed"));

            // Make all screenshot clicks open a full-size version, another click closes it

            sw.WriteLine("$(\".screenshot\").click(function () {");

            sw.WriteLine("$(\".ui-background\").show();");
            sw.WriteLine("$(this)");
            sw.WriteLine(".clone()");
            sw.WriteLine(".appendTo($('body'))");
            sw.WriteLine(".addClass(\"full\");");
            sw.WriteLine("});");



            // remove the full-size screenshot on clicking the close button
            sw.WriteLine("$(\".ui-background a.button\").click(function () { removeFullSizeScreenshot(); });");
            sw.WriteLine("$(\".ui-background.transparent\").click(function () { removeFullSizeScreenshot(); });");


            // Add icons to each message
            sw.WriteLine("$(\".m.debug\").prepend($(\"<span />\").addClass(\"ui-icon ui-icon-circle-triangle-e marker\"));");
            sw.WriteLine("$(\".m.info\").prepend($(\"<span />\").addClass(\"ui-icon ui-icon-info marker\"));");
            sw.WriteLine("$(\".m.warn\").prepend($(\"<span />\").addClass(\"ui-icon ui-icon-alert marker\"));");
            sw.WriteLine("$(\".m.fail\").prepend($(\"<span />\").addClass(\"ui-icon ui-icon-circle-close marker\"));");
            sw.WriteLine("$(\".m.custom\").prepend($(\"<span />\").addClass(\"ui-icon ui-icon-star marker\"));");
            sw.WriteLine("$(\".m.pass\").prepend($(\"<span />\").addClass(\"ui-icon ui-icon-check marker\"));");

            // Hook up expand-all and collapse-all buttons
            sw.WriteLine("$(\"#expand-all\").click(function () { expandInstant($(\".log .g-h\")); });");
            sw.WriteLine("$(\"#collapse-all\").click(function () { collapseInstant($(\".log .g-h\")); });");


            sw.WriteLine("});");

            sw.WriteLine("function toggleGroup(e) {");
            sw.WriteLine("e.find(\"span\").toggleClass(\"ui-icon-triangle-1-e ui-icon-triangle-1-s\");");
            sw.WriteLine("e.next().slideToggle();");
            sw.WriteLine("}");


            sw.WriteLine("function expandInstant(e) {");
            sw.WriteLine("e.find(\"span\").removeClass(\"ui-icon-triangle-1-e\").addClass(\"ui-icon-triangle-1-s\");");
            sw.WriteLine("e.next().show();");
            sw.WriteLine("}");

            sw.WriteLine("function collapseInstant(e) {");
            sw.WriteLine("e.find(\"span\").removeClass(\"ui-icon-triangle-1-s\").addClass(\"ui-icon-triangle-1-e\");");
            sw.WriteLine("e.next().hide();");
            sw.WriteLine("if (!e.find(\"div\").is(\":visible\")) { e.find(\"div\").slideDown(); }");
            sw.WriteLine("}");


            sw.WriteLine("function removeFullSizeScreenshot() {");
            sw.WriteLine("$(\".ui-background\").hide();");
            sw.WriteLine("$(\".screenshot.full\").remove();");
            sw.WriteLine("}");

            // hide full-size screenshot on escape
            sw.WriteLine("document.onkeyup = checkKey;");

            sw.WriteLine("function checkKey(e) {");
            sw.WriteLine("var keyId = (window.event) ? event.keyCode : e.keyCode;");

            sw.WriteLine("switch (keyId) {");

            sw.WriteLine("case 27: // if escape was pressed");
            sw.WriteLine("removeFullSizeScreenshot();");
            sw.WriteLine("break;");
            sw.WriteLine("}");
            sw.WriteLine("}");

            sw.WriteLine("function filterErrors() {");

            sw.WriteLine("$(\".g-h.failed\").each(function () {");

            sw.WriteLine("if ($(this).parent(\".g\").parent(\".gs\").length != 0) {");



            sw.WriteLine("if ($(this).is(\":visible\")) {");

            sw.WriteLine("collapseInstant($(this));");
            sw.WriteLine("}");
            sw.WriteLine("$(this).slideToggle();");
            sw.WriteLine("}");
            sw.WriteLine("});");
            sw.WriteLine("}");


            sw.WriteLine("function filterWarnings() { // only warning primary groups and all its contents are to be removed");

            sw.WriteLine("$(\".g-h.warning\").each(function () {");

            sw.WriteLine("if ($(this).parent(\".g\").parent(\".gs\").length != 0) {");


            sw.WriteLine("if ($(this).is(\":visible\")) {");

            sw.WriteLine("collapseInstant($(this));");
            sw.WriteLine("}");
            sw.WriteLine("$(this).slideToggle();");
            sw.WriteLine("}");
            sw.WriteLine("});");
            sw.WriteLine("}");



            sw.WriteLine("function filterPass() {");

            sw.WriteLine("$(\".g-h.passed\").each(function () {");

            sw.WriteLine("if ($(this).parent(\".g\").parent(\".gs\").length != 0) {");

            sw.WriteLine("if ($(this).is(\":visible\")) {");

            sw.WriteLine("collapseInstant($(this));");
            sw.WriteLine("}");
            sw.WriteLine("$(this).slideToggle();");
            sw.WriteLine("}");
            sw.WriteLine("});");

            sw.WriteLine("}");
            sw.WriteLine("</script>");


            sw.WriteLine("<style type=\"text/css\">");
            sw.WriteLine(".log { color:#373A3D ; font-size: 13pt;}");
            sw.WriteLine(".log .s { clear:both; }");
            sw.WriteLine(".log .s a {color:#373A3D ; text-decoration: none; }");
            sw.WriteLine(".log .s a:hover { text-decoration: underline; }");
            sw.WriteLine(".log .s .test-name { color:#373A3D ; float: left; font-family:'Segoe UI'; font-size:1em; padding-bottom:0.3em; }");
            sw.WriteLine(".log .s .result-overall { font-family:'Segoe UI'; font-size:10pt; font-variant:small-caps; clear: both; padding-bottom:0.3em; }");
            sw.WriteLine(".log .s .total-time { font-family:'Segoe UI'; font-size:10pt; font-variant:small-caps; float:right; padding-bottom: 0.3em; padding-right: 13px;  }");
            sw.WriteLine(".log .s #expand-all { float: left; margin-top: 0.4em; margin-left: 0.25em; }");
            sw.WriteLine(".log .s #collapse-all { float: left; margin-top: 0.4em; }");
            sw.WriteLine(".log .gs { clear: both;  font-size:10pt; font-family:'Segoe UI'; padding-top:0.8em;}");
            sw.WriteLine(".log .g { clear: both; }");
            sw.WriteLine(".log .g-h { border-top: 1px solid #E4E4E4  ; padding: 1px; cursor: pointer;}");
            sw.WriteLine(".log .g-h span.right { padding: 0px 10px 0px 10px; float: right; }");
            sw.WriteLine(".log .g-h .status { font-size: 1em; color:#747474; padding: 0px 10px 0px 10px; }");
            sw.WriteLine(".log .g-h .ui-icon { float: left; margin: 0.1em 0.4em 0px 0px; }");
            sw.WriteLine(".log .g-c { padding: 5px 0px 5px 20px; }");
            sw.WriteLine(".log .g-c .p-g { float: left; padding: 5px; margin: 3px; margin-bottom: 8px; border: 1px Dotted Grey; }");
            sw.WriteLine(".log .g-c .p-g-n { font-size: 0.55em; font-weight: bold; }");
            sw.WriteLine(".log .g-c td { font-size: 0.45em; padding: 1px 4px 1px 4px; border-top: 1px Solid LightGrey; border-collapse: collapse; }");
            sw.WriteLine(".log .g-c td:first-child { font-weight: bold; }");
            sw.WriteLine(".m { padding-top:3px; padding-bottom:3px; position: relative; margin-left:41px; color:#747474;}");
            sw.WriteLine(".m .m { margin-left: 25px; }");
            sw.WriteLine(".m.fail .e-t { font-weight: bold; }");
            sw.WriteLine(".m.fail .e-m { color:#747474; }");
            sw.WriteLine(".m.fail .e-s { color:#747474; font-size: 1em; padding: 0em 1.8em 0.5em; }");
            sw.WriteLine(".m.fail .e-s .type { color: #747474; font-weight: bold; }");
            sw.WriteLine(".m.fail .e-s .method { font-weight: bold; color:#747474;}");
            sw.WriteLine(".m.fail .e-s .file { padding-left: 1em; font-size: 1em; }");
            sw.WriteLine(".hoverColor { background-color : #EFEFEF; }");
            sw.WriteLine(".ui-background {  width:100%; height:100%; z-index:500; display: none;}");
            sw.WriteLine(".ui-background .transparent{  width:inherit; height:inherit; opacity: 0.8; background:#000;");
            sw.WriteLine("position:fixed; top:0; left:0; z-index:500; }");
            sw.WriteLine(".ui-background a.button {");
            sw.WriteLine("margin:9px 0 0 95.5%; text-align:center;  background-color: #ffffff ; display:block; width:30px; height:15px; padding: 6px 10px 6px; color: #000000; text-decoration: none;");
            sw.WriteLine("font-family:'Segoe UI'; line-height: 1; font-size:10pt;  z-index : 600; position: fixed;");
            sw.WriteLine("}");
            sw.WriteLine(".ui-background a.button:hover { background-color: #007acc; color: #ffffff; cursor:pointer; }                             ");
            sw.WriteLine(".log .s .filter { font-family:'Segoe UI Semibold'; font-size:0.67em; font-variant:small-caps; display:inline;   margin-left:12.5%; }                                                       ");
            sw.WriteLine(".m .diff { color: #373A3D ; }");
            sw.WriteLine(".m .old { background-color: #ffffff; }");
            sw.WriteLine(".m .new { background-color: #ffffff; }");
            sw.WriteLine(".screenshot { width: 240px; cursor: pointer; float: right; padding-bottom:0.8em; }");
            sw.WriteLine(".screenshot.full { position: fixed;  width:88%; height:96%; left: 6%; top: 2%; bottom:2%; right: 6%; z-index: 550; opacity :0.91;  }");
            sw.WriteLine(".passed { }");
            sw.WriteLine(".failed { }");
            sw.WriteLine(".warning { }");
            sw.WriteLine(".marker { position: absolute; top: 0.4em; left: -20px; }");
            sw.WriteLine(".marker-1-e { position:relative;  left: 3px; }");
            sw.WriteLine(".m.debug { color: #9f9f9f;}");

            sw.WriteLine(".m.debug .marker { background-image: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyJpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMy1jMDExIDY2LjE0NTY2MSwgMjAxMi8wMi8wNi0xNDo1NjoyNyAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNiAoV2luZG93cykiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6QTRFMjhDMUU3NUI4MTFFMkI0RkNBMjlFN0E5QjlEMEYiIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6QTRFMjhDMUY3NUI4MTFFMkI0RkNBMjlFN0E5QjlEMEYiPiA8eG1wTU06RGVyaXZlZEZyb20gc3RSZWY6aW5zdGFuY2VJRD0ieG1wLmlpZDpBNEUyOEMxQzc1QjgxMUUyQjRGQ0EyOUU3QTlCOUQwRiIgc3RSZWY6ZG9jdW1lbnRJRD0ieG1wLmRpZDpBNEUyOEMxRDc1QjgxMUUyQjRGQ0EyOUU3QTlCOUQwRiIvPiA8L3JkZjpEZXNjcmlwdGlvbj4gPC9yZGY6UkRGPiA8L3g6eG1wbWV0YT4gPD94cGFja2V0IGVuZD0iciI/Pot/rYoAAADgSURBVHjaYmDAAf7//+/xHwECGEgFQE0n/t0J/Q/CQHAeiBlJ0ezx/+uF///OcoAxiE2SK+C2Qw0gyRXotpPsCpjtHw5x/VeWVwBjfK5gQred4dtFc4aPmzFNBol9u2gAZPnjNAAIGv4/b8XtOohcPbIrmIiyHY8rmIi1HZcrGJFs3/7/hgVR0cyocYKBgUs/kJGRcQMTLts/fWViUA2SAGN8rmAiyu94woIRFO//78eYM7xfS1pmEQxmYFRccgHkhQOMYvkMDMx8xGsGqgXrYWDYCYvCjv+kgw6QXoAAAwCTkwe6Kh00kQAAAABJRU5ErkJggg==); }");

            sw.WriteLine(".m.fail .marker { background-image: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyBpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMC1jMDYwIDYxLjEzNDc3NywgMjAxMC8wMi8xMi0xNzozMjowMCAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNSBXaW5kb3dzIiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOjE1Qzc5N0QyMTZCQzExRTI4NzFFRjAwQzY4ODNDNDUwIiB4bXBNTTpEb2N1bWVudElEPSJ4bXAuZGlkOjE1Qzc5N0QzMTZCQzExRTI4NzFFRjAwQzY4ODNDNDUwIj4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6MTVDNzk3RDAxNkJDMTFFMjg3MUVGMDBDNjg4M0M0NTAiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6MTVDNzk3RDExNkJDMTFFMjg3MUVGMDBDNjg4M0M0NTAiLz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+IDw/eHBhY2tldCBlbmQ9InIiPz6etqryAAABBklEQVR42pxTMQrCQBBcgq1okTdYCemsU6XOC8ROAhbp/YRVglWeEJ+QxgcIfsFCCCRiJahn5siGzV0aMzBw2b2d3b3sklKKDAYNc2Ujb329+/Jj1TB7nQtVrkN1c6lH2OBrkDRcmAIwZNVuYwWarPdxT4QFEjhw4e57mmagtAsRLRCgNL70qStNKTJkb9sJHCKKnscDSTizObmngiZLTxNn2CTamAgVWKUiE8BZ+Wy2Bjhk4H29UBn69H3UOiuIM2zwmbAE/oUlIHtGZq6E38QCJowHx+yff93QOyAGsaggnW7jnqjsWb6JRBuT8iB1UzhmkLo94GkcM8q8D8nYZRq9zj8BBgA4BylhQeHHRQAAAABJRU5ErkJggg==); }");

            sw.WriteLine(".m.info .marker { background-image: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyBpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMC1jMDYwIDYxLjEzNDc3NywgMjAxMC8wMi8xMi0xNzozMjowMCAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNSBXaW5kb3dzIiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOjRBRUQzQjY3MTZCQzExRTJBNzdFOEFDNTlFQkQzQjcwIiB4bXBNTTpEb2N1bWVudElEPSJ4bXAuZGlkOjRBRUQzQjY4MTZCQzExRTJBNzdFOEFDNTlFQkQzQjcwIj4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6NEFFRDNCNjUxNkJDMTFFMkE3N0U4QUM1OUVCRDNCNzAiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6NEFFRDNCNjYxNkJDMTFFMkE3N0U4QUM1OUVCRDNCNzAiLz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+IDw/eHBhY2tldCBlbmQ9InIiPz4oQZsOAAACGElEQVR42oxTTWsTURQ9M8nYJpOkaJq2tlYrBEQEPxZSBDctKl1KQTfiVhgprlxUQdx0oRRXSgr+ga7cKpQupHRVLW2h2kLBWGpKMc3QxGkzk8zM894X08bAhB44A+/Nvefdc999EEKggS+I2yIYmaZ4hHGECaJh2u6pT1kLS79tmGUPPm0OdGgYTSfQFQ0bPbpMeVxPUliFMMnJMz8tffJrHtuWi0ZwRMUTuH8hgQcXT+JSsm2KtsaIvkqfDPHph42i/mx+BzlKLrs+7tKJiw/TeDvci146VaGg6bUiXi/ksVmqGrR8R0ywgJEtVvDmyy5sV8jAIEQ1FbObFqbX92DaHosY0lBm2YRVFYeBbSEV87kDPJrJoVTxUbA9aGpNWieRuV8HuJqKYOR8DFwBflAFAkcCjufjZl8U7+/04cm1JJLtIVT92n8W+l6w0a2HePlKVrBuOjguFCqErX7e2udbqVXQGQm39N4Mj26O+8F5UuBKqh2qcjwJl6yciWsY6tepV0pNYPisDupbQxeC4dA8DJ6OSBuEcRaY4jsfGYjLBrUS4UZ2k+/RdAcuU9UM9d9Y7r+80YXb52I4QSIK2cmXXSzulLGx58hTXfLNnp8PpnCL4vhg5n+jzNf8MfuHBqWIFXoLnOQT41oI92iMh/pjuN4TqSfLUUbQa9wqVcTqri0c6tpawQ58jc0CdY4Rv7V6xnX+FWAAqixWxIF64pQAAAAASUVORK5CYII=); }");

            sw.WriteLine(".m.warn .marker { background-image: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyJpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMy1jMDExIDY2LjE0NTY2MSwgMjAxMi8wMi8wNi0xNDo1NjoyNyAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNiAoV2luZG93cykiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6QTRFMjhDMUU3NUI4MTFFMkI0RkNBMjlFN0E5QjlEMEYiIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6QTRFMjhDMUY3NUI4MTFFMkI0RkNBMjlFN0E5QjlEMEYiPiA8eG1wTU06RGVyaXZlZEZyb20gc3RSZWY6aW5zdGFuY2VJRD0ieG1wLmlpZDpBNEUyOEMxQzc1QjgxMUUyQjRGQ0EyOUU3QTlCOUQwRiIgc3RSZWY6ZG9jdW1lbnRJRD0ieG1wLmRpZDpBNEUyOEMxRDc1QjgxMUUyQjRGQ0EyOUU3QTlCOUQwRiIvPiA8L3JkZjpEZXNjcmlwdGlvbj4gPC9yZGY6UkRGPiA8L3g6eG1wbWV0YT4gPD94cGFja2V0IGVuZD0iciI/Pot/rYoAAADgSURBVHjaYmDAAf7//+/xHwECGEgFQE0n/t0J/Q/CQHAeiBlJ0ezx/+uF///OcoAxiE2SK+C2Qw0gyRXotpPsCpjtHw5x/VeWVwBjfK5gQred4dtFc4aPmzFNBol9u2gAZPnjNAAIGv4/b8XtOohcPbIrmIiyHY8rmIi1HZcrGJFs3/7/hgVR0cyocYKBgUs/kJGRcQMTLts/fWViUA2SAGN8rmAiyu94woIRFO//78eYM7xfS1pmEQxmYFRccgHkhQOMYvkMDMx8xGsGqgXrYWDYCYvCjv+kgw6QXoAAAwCTkwe6Kh00kQAAAABJRU5ErkJggg==); }");

            /* custom and debug are not used in the code - here debug is given the alert symbol and custom the pass symbol */

            sw.WriteLine(".m.custom .marker { background-image: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyBpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMC1jMDYwIDYxLjEzNDc3NywgMjAxMC8wMi8xMi0xNzozMjowMCAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNSBXaW5kb3dzIiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOjE1Qzc5N0QyMTZCQzExRTI4NzFFRjAwQzY4ODNDNDUwIiB4bXBNTTpEb2N1bWVudElEPSJ4bXAuZGlkOjE1Qzc5N0QzMTZCQzExRTI4NzFFRjAwQzY4ODNDNDUwIj4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6MTVDNzk3RDAxNkJDMTFFMjg3MUVGMDBDNjg4M0M0NTAiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6MTVDNzk3RDExNkJDMTFFMjg3MUVGMDBDNjg4M0M0NTAiLz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+IDw/eHBhY2tldCBlbmQ9InIiPz6etqryAAABBklEQVR42pxTMQrCQBBcgq1okTdYCemsU6XOC8ROAhbp/YRVglWeEJ+QxgcIfsFCCCRiJahn5siGzV0aMzBw2b2d3b3sklKKDAYNc2Ujb329+/Jj1TB7nQtVrkN1c6lH2OBrkDRcmAIwZNVuYwWarPdxT4QFEjhw4e57mmagtAsRLRCgNL70qStNKTJkb9sJHCKKnscDSTizObmngiZLTxNn2CTamAgVWKUiE8BZ+Wy2Bjhk4H29UBn69H3UOiuIM2zwmbAE/oUlIHtGZq6E38QCJowHx+yff93QOyAGsaggnW7jnqjsWb6JRBuT8iB1UzhmkLo94GkcM8q8D8nYZRq9zj8BBgA4BylhQeHHRQAAAABJRU5ErkJggg==); }");

            /* Verify aboe and below - both are errors*/

            sw.WriteLine(".g-h.failed .marker-1-e { background-image: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyBpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMC1jMDYwIDYxLjEzNDc3NywgMjAxMC8wMi8xMi0xNzozMjowMCAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNSBXaW5kb3dzIiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOjE1Qzc5N0QyMTZCQzExRTI4NzFFRjAwQzY4ODNDNDUwIiB4bXBNTTpEb2N1bWVudElEPSJ4bXAuZGlkOjE1Qzc5N0QzMTZCQzExRTI4NzFFRjAwQzY4ODNDNDUwIj4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6MTVDNzk3RDAxNkJDMTFFMjg3MUVGMDBDNjg4M0M0NTAiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6MTVDNzk3RDExNkJDMTFFMjg3MUVGMDBDNjg4M0M0NTAiLz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+IDw/eHBhY2tldCBlbmQ9InIiPz6etqryAAABBklEQVR42pxTMQrCQBBcgq1okTdYCemsU6XOC8ROAhbp/YRVglWeEJ+QxgcIfsFCCCRiJahn5siGzV0aMzBw2b2d3b3sklKKDAYNc2Ujb329+/Jj1TB7nQtVrkN1c6lH2OBrkDRcmAIwZNVuYwWarPdxT4QFEjhw4e57mmagtAsRLRCgNL70qStNKTJkb9sJHCKKnscDSTizObmngiZLTxNn2CTamAgVWKUiE8BZ+Wy2Bjhk4H29UBn69H3UOiuIM2zwmbAE/oUlIHtGZq6E38QCJowHx+yff93QOyAGsaggnW7jnqjsWb6JRBuT8iB1UzhmkLo94GkcM8q8D8nYZRq9zj8BBgA4BylhQeHHRQAAAABJRU5ErkJggg==); }");

            /* warning primary group with a pass symbol */

            sw.WriteLine(".gs .g .g-h.warning .marker-1-e { background-image: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyBpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMC1jMDYwIDYxLjEzNDc3NywgMjAxMC8wMi8xMi0xNzozMjowMCAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNSBXaW5kb3dzIiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOjAxRjAxRTc2MTZCQzExRTJBODlEQTc5NTcyRENDODI0IiB4bXBNTTpEb2N1bWVudElEPSJ4bXAuZGlkOjAxRjAxRTc3MTZCQzExRTJBODlEQTc5NTcyRENDODI0Ij4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6MDFGMDFFNzQxNkJDMTFFMkE4OURBNzk1NzJEQ0M4MjQiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6MDFGMDFFNzUxNkJDMTFFMkE4OURBNzk1NzJEQ0M4MjQiLz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+IDw/eHBhY2tldCBlbmQ9InIiPz6kSXFAAAABNklEQVR42mJgQAPfv39P+Pr16/5v3779R8YgMZAcunpGJI0K////X3/+5XmD1ddXMxx+fBhFoa2sLUOoZiiDobjhBUZGxkBOTs4HcAOgms+3HWsT2H53OwM+ADIkzyTvA9AQQ5AhTCBBkM2TzkwiqBkEQK4DqQXpAfGZQP6CORsX8FXzZTiTdoZhc+RmuCEgPSC9TP/+/YvHpxkE0ozTwPSss7NQXALSywT0iwN6gKHbLskryfD883OGzbc2w8VBekB6mQj5GZvtyIAFXQDkT5CNjQcawXxstiMDDBfAbALZTMh2sAHA6DgASiRwFwBtAtkIshmf7SA9IL1MQLAQlDiwuQKf7SA9IL3glAhM66BUaEBMQkJKjRe4uLgMwWEASttVVlUf0F1CICkHUiUzMVCanQECDACQbuk8AyijcAAAAABJRU5ErkJggg==); }");

            /* warning secondary groups with an alert symbol */

            sw.WriteLine(".g-c .g .g-h.warning .marker-1-e { background-image: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyJpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMy1jMDExIDY2LjE0NTY2MSwgMjAxMi8wMi8wNi0xNDo1NjoyNyAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNiAoV2luZG93cykiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6QTRFMjhDMUU3NUI4MTFFMkI0RkNBMjlFN0E5QjlEMEYiIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6QTRFMjhDMUY3NUI4MTFFMkI0RkNBMjlFN0E5QjlEMEYiPiA8eG1wTU06RGVyaXZlZEZyb20gc3RSZWY6aW5zdGFuY2VJRD0ieG1wLmlpZDpBNEUyOEMxQzc1QjgxMUUyQjRGQ0EyOUU3QTlCOUQwRiIgc3RSZWY6ZG9jdW1lbnRJRD0ieG1wLmRpZDpBNEUyOEMxRDc1QjgxMUUyQjRGQ0EyOUU3QTlCOUQwRiIvPiA8L3JkZjpEZXNjcmlwdGlvbj4gPC9yZGY6UkRGPiA8L3g6eG1wbWV0YT4gPD94cGFja2V0IGVuZD0iciI/Pot/rYoAAADgSURBVHjaYmDAAf7//+/xHwECGEgFQE0n/t0J/Q/CQHAeiBlJ0ezx/+uF///OcoAxiE2SK+C2Qw0gyRXotpPsCpjtHw5x/VeWVwBjfK5gQred4dtFc4aPmzFNBol9u2gAZPnjNAAIGv4/b8XtOohcPbIrmIiyHY8rmIi1HZcrGJFs3/7/hgVR0cyocYKBgUs/kJGRcQMTLts/fWViUA2SAGN8rmAiyu94woIRFO//78eYM7xfS1pmEQxmYFRccgHkhQOMYvkMDMx8xGsGqgXrYWDYCYvCjv+kgw6QXoAAAwCTkwe6Kh00kQAAAABJRU5ErkJggg==); }");

            sw.WriteLine(".g-h.passed .marker-1-e { background-image: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyBpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMC1jMDYwIDYxLjEzNDc3NywgMjAxMC8wMi8xMi0xNzozMjowMCAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNSBXaW5kb3dzIiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOjAxRjAxRTc2MTZCQzExRTJBODlEQTc5NTcyRENDODI0IiB4bXBNTTpEb2N1bWVudElEPSJ4bXAuZGlkOjAxRjAxRTc3MTZCQzExRTJBODlEQTc5NTcyRENDODI0Ij4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6MDFGMDFFNzQxNkJDMTFFMkE4OURBNzk1NzJEQ0M4MjQiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6MDFGMDFFNzUxNkJDMTFFMkE4OURBNzk1NzJEQ0M4MjQiLz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+IDw/eHBhY2tldCBlbmQ9InIiPz6kSXFAAAABNklEQVR42mJgQAPfv39P+Pr16/5v3779R8YgMZAcunpGJI0K////X3/+5XmD1ddXMxx+fBhFoa2sLUOoZiiDobjhBUZGxkBOTs4HcAOgms+3HWsT2H53OwM+ADIkzyTvA9AQQ5AhTCBBkM2TzkwiqBkEQK4DqQXpAfGZQP6CORsX8FXzZTiTdoZhc+RmuCEgPSC9TP/+/YvHpxkE0ozTwPSss7NQXALSywT0iwN6gKHbLskryfD883OGzbc2w8VBekB6mQj5GZvtyIAFXQDkT5CNjQcawXxstiMDDBfAbALZTMh2sAHA6DgASiRwFwBtAtkIshmf7SA9IL1MQLAQlDiwuQKf7SA9IL3glAhM66BUaEBMQkJKjRe4uLgMwWEASttVVlUf0F1CICkHUiUzMVCanQECDACQbuk8AyijcAAAAABJRU5ErkJggg==); }");

            sw.WriteLine(".ui-icon.ui-icon-triangle-1-s { background-image: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAoAAAAFCAYAAAB8ZH1oAAAAIGNIUk0AAHolAACAgwAA+f8AAIDpAAB1MAAA6mAAADqYAAAXb5JfxUYAAAAJcEhZcwAACxIAAAsSAdLdfvwAAAAySURBVBhXYwgNDWWOjIxcCsT/ceBFIDUMIIBHMUIRWCV2xZiKsCjGrQhZMdxNMEEgDQB9sjAB1ndmhAAAAABJRU5ErkJggg==);}");

            sw.WriteLine(".ui-icon.ui-icon-triangle-1-e { background-image: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAUAAAAKCAYAAAB8OZQwAAAAIGNIUk0AAHolAACAgwAA+f8AAIDpAAB1MAAA6mAAADqYAAAXb5JfxUYAAAAJcEhZcwAACxEAAAsRAX9kX5EAAAA5SURBVBhXYwgNDWVmQAeRkZGLMCSAgv8xJKCCqBJIgggJNMGlYPORBCECIAAVRAhABVEFQILYHA8AQxIwARAEcssAAAAASUVORK5CYII=); }");

            sw.WriteLine("</style>");

            sw.WriteLine("</head>");
            sw.WriteLine("<body>");
            sw.WriteLine("<div class=\"log\"><div class=\"s\">");
            sw.WriteLine("<div class=\"test-name\">Result Test Log</div>");
            sw.WriteLine("</div>");
            sw.WriteLine("<div class=\"ui-background\">");
            sw.WriteLine("<div class=\"transparent\"> </div>");
            sw.WriteLine("<a class=\"button\">Close</a>");
            sw.WriteLine("</div>");

            sw.Flush();
            sw.Close();
        }
    }
}
