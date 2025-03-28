using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STAF
{
    public class TestReportGenerator
    {
        private static void GenerateTestReport(string filePath, TestResultData results)
        {
            try
            {
                // Create CSS and JS file paths relative to the HTML file
                string cssFilePath = Path.ChangeExtension(filePath, "css");
                string jsFilePath = Path.ChangeExtension(filePath, "js");

                // Generate CSS and JS files
                GenerateCssFile(cssFilePath);
                GenerateJsFile(jsFilePath);

                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.Write(GenerateHtmlReport(results, cssFilePath, jsFilePath));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating report: {ex.Message}");
            }
        }

        private static void GenerateCssFile(string filePath)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(filePath))
                {
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
                    sw.WriteLine(".ui-background a.button:hover { background-color: #007acc; color: #ffffff; cursor:pointer; }");
                    sw.WriteLine(".log .s .filter { font-family:'Segoe UI Semibold'; font-size:0.67em; font-variant:small-caps; display:inline;   margin-left:12.5%; }");
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
                    sw.WriteLine(".m.debug .marker { background-image: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyJpVFh0WE1MOmNvbS5hZG9iZS54bWAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMy1jMDExIDY2LjE0NTY2MSwgMjAxMi8wMi8wNi0xNDo1NjoyNyAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNiAoV2luZG93cykiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6QTRFMjhDMUU3NUI4MTFFMkI0RkNBMjlFN0E5QjlEMEYiIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6QTRFMjhDMUY3NUI4MTFFMkI0RkNBMjlFN0E5QjlEMEYiPiA8eG1wTU06RGVyaXZlZEZyb20gc3RSZWY6aW5zdGFuY2VJRD0ieG1wLmlpZDpBNEUyOEMxQzc1QjgxMUUyQjRGQ0EyOUU3QTlCOUQwRiIgc3RSZWY6ZG9jdW1lbnRJRD0ieG1wLmRpZDpBNEUyOEMxRDc1QjgxMUUyQjRGQ0EyOUU3QTlCOUQwRiIvPiA8L3JkZjpEZXNjcmlwdGlvbj4gPC9yZGY6UkRGPiA8L3g6eG1wbWV0YT4gPD94cGFja2V0IGVuZD0iciI/Pot/rYoAAADgSURBVHjaYmDAAf7//+/xHwECGEgFQE0n/t0J/Q/CQHAeiBlJ0ezx/+uF///OcoAxiE2SK+C2Qw0gyRXotpPsCpjtHw5x/VeWVwBjfK5gQred4dtFc4aPmzFNBol9u2gAZPnjNAAIGv4/b8XtOohcPbIrmIiyHY8rmIi1HZcrGJFs3/7/hgVR0cyocYKBgUs/kJGRcQMTLts/fWViUA2SAGN8rmAiyu94woIRFO//78eYM7xfS1pmEQxmYFRccgHkhQOMYvkMDMx8xGsGqgXrYWDYCYvCjv+kgw6QXoAAAwCTkwe6Kh00kQAAAABJRU5ErkJggg==); }");
                    sw.WriteLine(".m.fail .marker { background-image: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyBpVFh0WE1MOmNvbS5hZG9iZS54bWAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMC1jMDYwIDYxLjEzNDc3NywgMjAxMC8wMi8xMi0xNzozMjowMCAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNSBXaW5kb3dzIiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOjE1Qzc5N0QyMTZCQzExRTI4NzFFRjAwQzY4ODNDNDUwIiB4bXBNTTpEb2N1bWVudElEPSJ4bXAuZGlkOjE1Qzc5N0QzMTZCQzExRTI4NzFFRjAwQzY4ODNDNDUwIj4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6MTVDNzk3RDAxNkJDMTFFMjg3MUVGMDBDNjg4M0M0NTAiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6MTVDNzk3RDExNkJDMTFFMjg3MUVGMDBDNjg4M0M0NTAiLz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+IDw/eHBhY2tldCBlbmQ9InIiPz6etqryAAABBklEQVR42pxTMQrCQBBcgq1okTdYCemsU6XOC8ROAhbp/YRVglWeEJ+QxgcIfsFCCCRiJahn5siGzV0aMzBw2b2d3b3sklKKDAYNc2Ujb329+/Jj1TB7nQtVrkN1c6lH2OBrkDRcmAIwZNVuYwWarPdxT4QFEjhw4e57mmagtAsRLRCgNL70qStNKTJkb9sJHCKKnscDSTizObmngiZLTxNn2CTamAgVWKUiE8BZ+Wy2Bjhk4H29UBn69H3UOiuIM2zwmbAE/oUlIHtGZq6E38QCJowHx+yff93QOyAGsaggnW7jnqjsWb6JRBuT8iB1UzhmkLo94GkcM8q8D8nYZRq9zj8BBgA4BylhQeHHRQAAAABJRU5ErkJggg==); }");
                    sw.WriteLine(".m.info .marker { background-image: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyBpVFh0WE1MOmNvbS5hZG9iZS54bWAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMC1jMDYwIDYxLjEzNDc3NywgMjAxMC8wMi8xMi0xNzozMjowMCAgICAgICAgIj4gPHRkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHRkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNSBXaW5kb3dzIiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOjRBRUQzQjY3MTZCQzExRTJBNzdFOEFDNTlFQkQzQjcwIiB4bXBNTTpEb2N1bWVudElEPSJ4bXAuZGlkOjRBRUQzQjY4MTZCQzExRTJBNzdFOEFDNTlFQkQzQjcwIj4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6NEFFRDNCNjUxNkJDMTFFMkE3N0U4QUM1OUVCRDNCNzAiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6NEFFRDNCNjYxNkJDMTFFMkE3N0U4QUM1OUVCRDNCNzAiLz4gPC9yZGY6UkRGPiA8L3g6eG1wbWV0YT4gPD94cGFja2V0IGVuZD0iciI/Pot/rYoAAACGElEQVR42oxTTWsTURQ9M8nYJpOkaJq2tlYrBEQEPxZSBDctKl1KQTfiVhgprlxUQdx0oRRXSgr+ga7cKpQupHRVLW2h2kLBWGpKMc3QxGkzk8zM894X08bAhB44A+/Nvefdc999EEKggS+I2yIYmaZ4hHGECaJh2u6pT1kLS79tmGUPPm0OdGgYTSfQFQ0bPbpMeVxPUliFMMnJMz8tffJrHtuWi0ZwRMUTuH8hgQcXT+JSsm2KtsaIvkqfDPHph42i/mx+BzlKLrs+7tKJiw/TeDvci146VaGg6bUiXi/ksVmqGrR8R0ywgJEtVvDmyy5sV8jAIEQ1FbObFqbX92DaHosY0lBm2YRVFYeBbSEV87kDPJrJoVTxUbA9aGpNWieRuV8HuJqKYOR8DFwBflAFAkcCjufjZl8U7+/04cm1JJLtIVT92n8W+l6w0a2HePlKVrBuOjguFCqErX7e2udbqVXQGQm39N4Mj26O+8F5UuBKqh2qcjwJl6yciWsY6tepV0pNYPisDupbQxeC4dA8DJ6OSBuEcRaY4jsfGYjLBrUS4UZ2k+/RdAcuU9UM9d9Y7r+80YXb52I4QSIK2cmXXSzulLGx58hTXfLNnp8PpnCL4vhg5n+jzNf8MfuHBqWIFXoLnOQT41oI92iMh/pjuN4TqSfLUUbQa9wqVcTqri0c6tpawQ58jc0CdY4Rv7V6xnX+FWAAqixWxIF64pQAAAAASUVORK5CYII=); }");
                    sw.WriteLine(".m.warn .marker { background-image: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyJpVFh0WE1MOmNvbS5hZG9iZS54bWAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMy1jMDExIDY2LjE0NTY2MSwgMjAxMi8wMi8wNi0xNDo1NjoyNyAgICAgICAgIj4gPHRkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHRkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNiAoV2luZG93cykiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6QTRFMjhDMUU3NUI4MTFFMkI0RkNBMjlFN0E5QjlEMEYiIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6QTRFMjhDMUY3NUI4MTFFMkI0RkNBMjlFN0E5QjlEMEYiPiA8eG1wTU06RGVyaXZlZEZyb20gc3RSZWY6aW5zdGFuY2VJRD0ieG1wLmlpZDpBNEUyOEMxQzc1QjgxMUUyQjRGQ0EyOUU3QTlCOUQwRiIgc3RSZWY6ZG9jdW1lbnRJRD0ieG1wLmRpZDpBNEUyOEMxRDc1QjgxMUUyQjRGQ0EyOUU3QTlCOUQwRiIvPiA8L3JkZjpEZXNjcmlwdGlvbj4gPC9yZGY6UkRGPiA8L3g6eG1wbWV0YT4gPD94cGFja2V0IGVuZD0iciI/Pot/rYoAAADgSURBVHjaYmDAAf7//+/xHwECGEgFQE0n/t0J/Q/CQHAeiBlJ0ezx/+uF///OcoAxiE2SK+C2Qw0gyRXotpPsCpjtHw5x/VeWVwBjfK5gQred4dtFc4aPmzFNBol9u2gAZPnjNAAIGv4/b8XtOohcPbIrmIiyHY8rmIi1HZcrGJFs3/7/hgVR0cyocYKBgUs/kJGRcQMTLts/fWViUA2SAGN8rmAiyu94woIRFO//78eYM7xfS1pmEQxmYFRccgHkhQOMYvkMDMx8xGsGqgXrYWDYCYvCjv+kgw6QXoAAAwCTkwe6Kh00kQAAAABJRU5ErkJggg==); }");
                    sw.WriteLine(".m.custom .marker { background-image: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyBpVFh0WE1MOmNvbS5hZG9iZS54bWAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMC1jMDYwIDYxLjEzNDc3NywgMjAxMC8wMi8xMi0xNzozMjowMCAgICAgICAgIj4gPHRkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHRkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNSBXaW5kb3dzIiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOjE1Qzc5N0QyMTZCQzExRTI4NzFFRjAwQzY4ODNDNDUwIiB4bXBNTTpEb2N1bWVudElEPSJ4bXAuZGlkOjE1Qzc5N0QzMTZCQzExRTI4NzFFRjAwQzY4ODNDNDUwIj4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6MTVDNzk3RDAxNkJDMTFFMjg3MUVGMDBDNjg4M0M0NTAiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6MTVDNzk3RDExNkJDMTFFMjg3MUVGMDBDNjg4M0M0NTAiLz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+IDw/eHBhY2tldCBlbmQ9InIiPz6etqryAAABBklEQVR42pxTMQrCQBBcgq1okTdYCemsU6XOC8ROAhbp/YRVglWeEJ+QxgcIfsFCCCRiJahn5siGzV0aMzBw2b2d3b3sklKKDAYNc2Ujb329+/Jj1TB7nQtVrkN1c6lH2OBrkDRcmAIwZNVuYwWarPdxT4QFEjhw4e57mmagtAsRLRCgNL70qStNKTJkb9sJHCKKnscDSTizObmngiZLTxNn2CTamAgVWKUiE8BZ+Wy2Bjhk4H29UBn69H3UOiuIM2zwmbAE/oUlIHtGZq6E38QCJowHx+yff93QOyAGsaggnW7jnqjsWb6JRBuT8iB1UzhmkLo94GkcM8q8D8nYZRq9zj8BBgA4BylhQeHHRQAAAABJRU5ErkJggg==); }");
                    sw.WriteLine(".g-h.failed .marker-1-e { background-image: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyBpVFh0WE1MOmNvbS5hZG9iZS54bWAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMC1jMDYwIDYxLjEzNDc3NywgMjAxMC8wMi8xMi0xNzozMjowMCAgICAgICAgIj4gPHRkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHRkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNSBXaW5kb3dzIiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOjE1Qzc5N0QyMTZCQzExRTI4NzFFRjAwQzY4ODNDNDUwIiB4bXBNTTpEb2N1bWVudElEPSJ4bXAuZGlkOjE1Qzc5N0QzMTZCQzExRTI4NzFFRjAwQzY4ODNDNDUwIj4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6MTVDNzk3RDAxNkJDMTFFMjg3MUVGMDBDNjg4M0M0NTAiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6MTVDNzk3RDExNkJDMTFFMjg3MUVGMDBDNjg4M0M0NTAiLz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+IDw/eHBhY2tldCBlbmQ9InIiPz6etqryAAABBklEQVR42pxTMQrCQBBcgq1okTdYCemsU6XOC8ROAhbp/YRVglWeEJ+QxgcIfsFCCCRiJahn5siGzV0aMzBw2b2d3b3sklKKDAYNc2Ujb329+/Jj1TB7nQtVrkN1c6lH2OBrkDRcmAIwZNVuYwWarPdxT4QFEjhw4e57mmagtAsRLRCgNL70qStNKTJkb9sJHCKKnscDSTizObmngiZLTxNn2CTamAgVWKUiE8BZ+Wy2Bjhk4H29UBn69H3UOiuIM2zwmbAE/oUlIHtGZq6E38QCJowHx+yff93QOyAGsaggnW7jnqjsWb6JRBuT8iB1UzhmkLo94GkcM8q8D8nYZRq9zj8BBgA4BylhQeHHRQAAAABJRU5ErkJggg==); }");
                    sw.WriteLine(".gs .g .g-h.warning .marker-1-e { background-image: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyBpVFh0WE1MOmNvbS5hZG9iZS54bWAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMC1jMDYwIDYxLjEzNDc3NywgMjAxMC8wMi8xMi0xNzozMjowMCAgICAgICAgIj4gPHRkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHRkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNSBXaW5kb3dzIiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOjAxRjAxRTc2MTZCQzExRTJBODlEQTc5NTcyRENDODI0IiB4bXBNTTpEb2N1bWVudElEPSJ4bXAuZGlkOjAxRjAxRTc3MTZCQzExRTJBODlEQTc5NTcyRENDODI0Ij4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6MDFGMDFFNzQxNkJDMTFFMkE4OURBNzk1NzJEQ0M4MjQiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6MDFGMDFFNzUxNkJDMTFFMkE4OURBNzk1NzJEQ0M4MjQiLz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+IDw/eHBhY2tldCBlbmQ9InIiPz6kSXFAAAABNklEQVR42mJgQAPfv39P+Pr16/5v3779R8YgMZAcunpGJI0K////X3/+5XmD1ddXMxx+fBhFoa2sLUOoZiiDobjhBUZGxkBOTs4HcAOgms+3HWsT2H53OwM+ADIkzyTvA9AQQ5AhTCBBkM2TzkwiqBkEQK4DqQXpAfGZQP6CORsX8FXzZTiTdoZhc+RmuCEgPSC9TP/+/YvHpxkE0ozTwPSss7NQXALSywT0iwN6gKHbLskryfD883OGzbc2w8VBekB6mQj5GZvtyIAFXQDkT5CNjQcawXxstiMDDBfAbALZTMh2sAHA6DgASiRwFwBtAtkIshmf7SA9IL1MQLAQlDiwuQKf7SA9IL3glAhM66BUaEBMQkJKjRe4uLgMwWEASttVVlUf0F1CICkHUiUzMVCanQECDACQbuk8AyijcAAAAABJRU5ErkJggg==); }");
                    sw.WriteLine(".g-c .g .g-h.warning .marker-1-e { background-image: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyJpVFh0WE1MOmNvbS5hZG9iZS54bWAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMy1jMDExIDY2LjE0NTY2MSwgMjAxMi8wMi8wNi0xNDo1NjoyNyAgICAgICAgIj4gPHRkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHRkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNiAoV2luZG93cykiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6QTRFMjhDMUU3NUI4MTFFMkI0RkNBMjlFN0E5QjlEMEYiIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6QTRFMjhDMUY3NUI4MTFFMkI0RkNBMjlFN0E5QjlEMEYiPiA8eG1wTU06RGVyaXZlZEZyb20gc3RSZWY6aW5zdGFuY2VJRD0ieG1wLmlpZDpBNEUyOEMxQzc1QjgxMUUyQjRGQ0EyOUU3QTlCOUQwRiIgc3RSZWY6ZG9jdW1lbnRJRD0ieG1wLmRpZDpBNEUyOEMxRDc1QjgxMUUyQjRGQ0EyOUU3QTlCOUQwRiIvPiA8L3JkZjpEZXNjcmlwdGlvbj4gPC9yZGY6UkRGPiA8L3g6eG1wbWV0YT4gPD94cGFja2V0IGVuZD0iciI/Pot/rYoAAADgSURBVHjaYmDAAf7//+/xHwECGEgFQE0n/t0J/Q/CQHAeiBlJ0ezx/+uF///OcoAxiE2SK+C2Qw0gyRXotpPsCpjtHw5x/VeWVwBjfK5gQred4dtFc4aPmzFNBol9u2gAZPnjNAAIGv4/b8XtOohcPbIrmIiyHY8rmIi1HZcrGJFs3/7/hgVR0cyocYKBgUs/kJGRcQMTLts/fWViUA2SAGN8rmAiyu94woIRFO//78eYM7xfS1pmEQxmYFRccgHkhQOMYvkMDMx8xGsGqgXrYWDYCYvCjv+kgw6QXoAAAwCTkwe6Kh00kQAAAABJRU5ErkJggg==); }");
                    sw.WriteLine(".g-h.passed .marker-1-e { background-image: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyBpVFh0WE1MOmNvbS5hZG9iZS54bWAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMC1jMDYwIDYxLjEzNDc3NywgMjAxMC8wMi8xMi0xNzozMjowMCAgICAgICAgIj4gPHRkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHRkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNSBXaW5kb3dzIiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOjAxRjAxRTc2MTZCQzExRTJBODlEQTc5NTcyRENDODI0IiB4bXBNTTpEb2N1bWVudElEPSJ4bXAuZGlkOjAxRjAxRTc3MTZCQzExRTJBODlEQTc5NTcyRENDODI0Ij4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6MDFGMDFFNzQxNkJDMTFFMkE4OURBNzk1NzJEQ0M4MjQiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6MDFGMDFFNzUxNkJDMTFFMkE4OURBNzk1NzJEQ0M4MjQiLz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+IDw/eHBhY2tldCBlbmQ9InIiPz6kSXFAAAABNklEQVR42mJgQAPfv39P+Pr16/5v3779R8YgMZAcunpGJI0K////X3/+5XmD1ddXMxx+fBhFoa2sLUOoZiiDobjhBUZGxkBOTs4HcAOgms+3HWsT2H53OwM+ADIkzyTvA9AQQ5AhTCBBkM2TzkwiqBkEQK4DqQXpAfGZQP6CORsX8FXzZTiTdoZhc+RmuCEgPSC9TP/+/YvHpxkE0ozTwPSss7NQXALSywT0iwN6gKHbLskryfD883OGzbc2w8VBekB6mQj5GZvtyIAFXQDkT5CNjQcawXxstiMDDBfAbALZTMh2sAHA6DgASiRwFwBtAtkIshmf7SA9IL1MQLAQlDiwuQKf7SA9IL3glAhM66BUaEBMQkJKjRe4uLgMwWEASttVVlUf0F1CICkHUiUzMVCanQECDACQbuk8AyijcAAAAABJRU5ErkJggg==); }");
                    sw.WriteLine(".ui-icon.ui-icon-triangle-1-s { background-image: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAoAAAAFCAYAAAB8ZH1oAAAAIGNIUk0AAHolAACAgwAA+f8AAIDpAAB1MAAA6mAAADqYAAAXb5JfxUYAAAAJcEhZcwAACxIAAAsSAdLdfvwAAAAySURBVBhXYwgNDWWOjIxcCsT/ceBFIDUMIIBHMUIRWCV2xZiKsCjGrQhZMdxNMEEgDQB9sjAB1ndmhAAAAABJRU5ErkJggg==);}");
                    sw.WriteLine(".ui-icon.ui-icon-triangle-1-e { background-image: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAUAAAAKCAYAAAB8OZQwAAAAIGNIUk0AAHolAACAgwAA+f8AAIDpAAB1MAAA6mAAADqYAAAXb5JfxUYAAAAJcEhZcwAACxEAAAsRAX9kX5EAAAA5SURBVBhXYwgNDWVmQAeRkZGLMCSAgv8xJKCCqBwBwJVjYF+gJYwAAAABJRU5ErkJggg==); }");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating CSS file: {ex.Message}");
            }
        }

        private static void GenerateJsFile(string filePath)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.WriteLine("$(document).ready(function () {");
                    sw.WriteLine("    $('.g-h').click(function () {");
                    sw.WriteLine("        $(this).next('.g-c').toggle('slow');");
                    sw.WriteLine("        $(this).find('.ui-icon').toggleClass('ui-icon-triangle-1-s ui-icon-triangle-1-e');");
                    sw.WriteLine("    });");
                    sw.WriteLine("    $('#expand-all').click(function () {");
                    sw.WriteLine("        $('.g-c').show('slow');");
                    sw.WriteLine("        $('.g-h .ui-icon').removeClass('ui-icon-triangle-1-e').addClass('ui-icon-triangle-1-s');");
                    sw.WriteLine("    });");
                    sw.WriteLine("    $('#collapse-all').click(function () {");
                    sw.WriteLine("        $('.g-c').hide('slow');");
                    sw.WriteLine("        $('.g-h .ui-icon').removeClass('ui-icon-triangle-1-s').addClass('ui-icon-triangle-1-e');");
                    sw.WriteLine("    });");
                    sw.WriteLine("    $('.screenshot').click(function () {");
                    sw.WriteLine("        $(this).toggleClass('full');");
                    sw.WriteLine("        if ($('.ui-background').is(':visible')) {");
                    sw.WriteLine("            $('.ui-background').hide();");
                    sw.WriteLine("        } else {");
                    sw.WriteLine("            $('.ui-background').show();");
                    sw.WriteLine("        }");
                    sw.WriteLine("    });");
                    sw.WriteLine("    $('.ui-background a.button').click(function () {");
                    sw.WriteLine("        $('.ui-background').hide();");
                    sw.WriteLine("        $('.screenshot').removeClass('full');");
                    sw.WriteLine("    });");
                    sw.WriteLine("    $('.g-h').hover(");
                    sw.WriteLine("       function () { $(this).addClass('hoverColor'); },");
                    sw.WriteLine("       function () { $(this).removeClass('hoverColor'); }");
                    sw.WriteLine("     );");
                    sw.WriteLine("});");
                    sw.WriteLine("function showDiv(divId) {");
                    sw.WriteLine("    $('#' + divId).show('slow');");
                    sw.WriteLine("}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating JS file: {ex.Message}");
            }
        }

        private static string GenerateHtmlReport(TestResultData results, string cssFilePath, string jsFilePath)
        {
            StringBuilder html = new StringBuilder();
            html.AppendLine("<!DOCTYPE html>");
            html.AppendLine("<html lang=\"en\">");
            html.AppendLine("<head>");
            html.AppendLine("    <meta charset=\"UTF-8\">");
            html.AppendLine("    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
            html.AppendLine("    <title>Test Report</title>");
            // Link CSS and JS files
            html.AppendLine($"    <link rel=\"stylesheet\" href=\"{Path.GetFileName(cssFilePath)}\">");
            html.AppendLine("    <link rel=\"stylesheet\" href=\"//code.jquery.com/ui/1.13.2/themes/base/jquery-ui.css\">");
            html.AppendLine("    <script src=\"https://code.jquery.com/jquery-3.6.0.js\"></script>");
            html.AppendLine("    <script src=\"https://code.jquery.com/ui/1.13.2/jquery-ui.js\"></script>");
            html.AppendLine($"    <script src=\"{Path.GetFileName(jsFilePath)}\"></script>");
            html.AppendLine("</head>");
            html.AppendLine("<body>");
            html.AppendLine("<div class=\"log\">");
            html.AppendLine("<div class=\"ui-background\">");
            html.AppendLine("    <div class=\"transparent\"></div>");
            html.AppendLine("    <a class=\"button\" href=\"#\">Close</a>");
            html.AppendLine("</div>");

            // Summary
            html.AppendLine("    <div class=\"s\">");
            html.AppendLine($"        <a href=\"#\" id=\"expand-all\">Expand All</a> <a href=\"#\" id=\"collapse-all\">Collapse All</a>");
            html.AppendLine($"        <span class=\"test-name\">Test Name: {results.TestName}</span>");
            html.AppendLine($"        <span class=\"result-overall\">Result: {results.Result}</span>");
            html.AppendLine($"        <span class=\"total-time\">Total Time: {results.TotalTime}</span>");
            if (results.Filters != null && results.Filters.Length > 0)
            {
                html.AppendLine($"    <span class=\"filter\"> Filters : {string.Join(", ", results.Filters)} </span>");
            }
            html.AppendLine("    </div>");

            // Groups
            html.AppendLine("    <div class=\"gs\">Groups</div>");
            foreach (TestGroup group in results.Groups)
            {
                html.AppendLine("    <div class=\"g\">");
                html.AppendLine($"        <div class=\"g-h {group.Result.ToLower()} \">");
                html.AppendLine($"            <span class=\"ui-icon ui-icon-triangle-1-e\"></span>");
                html.AppendLine($"            <span class=\"right\">{group.EndTime}</span>");
                html.AppendLine($"             <span class=\"status\">{group.Result}</span>");
                html.AppendLine($"            <span>{group.Name} ({group.StartTime})</span>");
                html.AppendLine("        </div>");
                html.AppendLine("        <div class=\"g-c\" style=\"display:none;\">");
                if (group.Items != null)
                {
                    foreach (TestItem item in group.Items)
                    {
                        html.AppendLine("            <div class=\"p-g\">");
                        html.AppendLine($"                <div class=\"p-g-n\">{item.Name}</div>");
                        html.AppendLine("                <table>");
                        html.AppendLine("                    <tr>");
                        html.AppendLine("                        <td>Start Time</td>");
                        html.AppendLine($"                        <td>{item.StartTime}</td>");
                        html.AppendLine("                    </tr>");
                        html.AppendLine("                    <tr>");
                        html.AppendLine("                        <td>End Time</td>");
                        html.AppendLine($"                        <td>{item.EndTime}</td>");
                        html.AppendLine("                    </tr>");
                        html.AppendLine("                    <tr>");
                        html.AppendLine("                        <td>Result</td>");
                        html.AppendLine($"                        <td class=\"{item.Result.ToLower()}\">{item.Result}</td>");
                        html.AppendLine("                    </tr>");
                        if (item.Messages != null)
                        {
                            foreach (TestMessage message in item.Messages)
                            {
                                html.AppendLine("                    <tr>");
                                html.AppendLine("                        <td>Message</td>");
                                html.AppendLine($"                        <td><div class=\"m {message.Type.ToLower()}\"><span class=\"marker marker-1-e\"></span>{message.Text}</div></td>");
                                html.AppendLine("                    </tr>");
                            }
                        }
                        if (item.ScreenShots != null)
                        {
                            foreach (string screenshot in item.ScreenShots)
                            {
                                html.AppendLine("                    <tr>");
                                html.AppendLine("                        <td>ScreenShot</td>");
                                html.AppendLine($"                        <td><img class=\"screenshot\" src=\"{screenshot}\" alt=\"Screenshot\" /></td>");
                                html.AppendLine("                    </tr>");
                            }
                        }
                        html.AppendLine("                </table>");
                        html.AppendLine("            </div>");
                    }
                }
                html.AppendLine("        </div>");
                html.AppendLine("    </div>");
            }
            html.AppendLine("</div>");
            html.AppendLine("</body>");
            html.AppendLine("</html>");
            return html.ToString();
        }

        // Example usage
        public static void Main(string[] args)
        {
            // Create sample data
            TestResultData results = new TestResultData
            {
                TestName = "MyTest",
                Result = "Passed",
                TotalTime = "00:00:01",
                Filters = new string[] { "Filter1", "Filter2" },
                Groups = new TestGroup[]
                {
                new TestGroup
                {
                    Name = "Group1",
                    StartTime = "2024-07-24 10:00:00",
                    EndTime = "2024-07-24 10:00:01",
                    Result = "Passed",
                    Items = new TestItem[]
                    {
                        new TestItem
                        {
                            Name = "Item1",
                            StartTime = "2024-07-24 10:00:00",
                            EndTime = "2024-07-24 10:00:00",
                            Result = "Passed",
                            Messages = new TestMessage[]
                            {
                                new TestMessage { Text = "Message 1", Type = "Info" },
                                new TestMessage { Text = "Message 2", Type = "Debug" }
                            },
                            ScreenShots = new string[] { "screenshot1.png", "screenshot2.png" }
                        },
                        new TestItem
                        {
                            Name = "Item2",
                            StartTime = "2024-07-24 10:00:00",
                            EndTime = "2024-07-24 10:00:01",
                            Result = "Failed",
                            Messages = new TestMessage[]
                            {
                                new TestMessage { Text = "Error Message", Type = "Fail" },
                                 new TestMessage { Text = "Warning Message", Type = "Warn" }
                            },
                            ScreenShots = new string[] { "screenshot3.png" }
                        }
                    }
                },
                new TestGroup
                {
                    Name = "Group2",
                    StartTime = "2024-07-24 10:00:01",
                    EndTime = "2024-07-24 10:00:01",
                    Result = "Passed",
                     Items = new TestItem[]
                    {
                        new TestItem
                        {
                            Name = "Item3",
                            StartTime = "2024-07-24 10:00:00",
                            EndTime = "2024-07-24 10:00:00",
                            Result = "Passed",
                            Messages = new TestMessage[]
                            {
                                new TestMessage { Text = "Message 1", Type = "Info" },
                                new TestMessage { Text = "Message 2", Type = "Debug" }
                            },
                            ScreenShots = new string[] { "screenshot1.png", "screenshot2.png" }
                        },
                    }
                }
                }
            };

            // Generate the report
            string reportFilePath = "test_report.html";
            GenerateTestReport(reportFilePath, results);
            Console.WriteLine($"Report generated successfully at {reportFilePath}");
        }
    }

    // Data classes for the test report
    public class TestResultData
    {
        public string TestName { get; set; }
        public string Result { get; set; }
        public string TotalTime { get; set; }
        public string[] Filters { get; set; }
        public TestGroup[] Groups { get; set; }
    }

    public class TestGroup
    {
        public string Name { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Result { get; set; }
        public TestItem[] Items { get; set; }
    }

    public class TestItem
    {
        public string Name { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Result { get; set; }
        public TestMessage[] Messages { get; set; }
        public string[] ScreenShots { get; set; }
    }

    public class TestMessage
    {
        public string Text { get; set; }
        public string Type { get; set; }
    }

}
