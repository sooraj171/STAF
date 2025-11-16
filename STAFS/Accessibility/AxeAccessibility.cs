using System;
using System.IO;
using System.Text.Json;
using OpenQA.Selenium;
using Deque.AxeCore.Selenium;

namespace SATF.Accessibility
{
    /// <summary>
    /// Wrapper around Deque.AxeCore.Selenium to provide simple accessibility analysis helpers.
    /// Each method returns the raw result object from the AxeBuilder.Analyze call (typed as object to avoid
    /// direct dependency on specific result types in callers).
    /// </summary>
    public class AxeAccessibility
    {
        private readonly IWebDriver driver;

        public AxeAccessibility(IWebDriver webDriver)
        {
            driver = webDriver ?? throw new ArgumentNullException(nameof(webDriver));
        }

        /// <summary>
        /// Analyze the whole page and return the accessibility result object.
        /// </summary>
        public object AnalyzePage()
        {
            var builder = new AxeBuilder(driver);
            var result = builder.Analyze();
            return result;
        }

        /// <summary>
        /// Analyze the page and save the result as an HTML file at the provided path. Returns the path written.
        /// </summary>
        public string AnalyzePageAndSaveHtml(string filePath)
        {
            var result = AnalyzePage();
            SaveResultAsHtml(filePath, result);
            return filePath;
        }

        /// <summary>
        /// Save the raw axe result object as a readable HTML file (JSON wrapped in a &lt;pre&gt; block).
        /// Uses System.Text.Json for pretty-printing.
        /// </summary>
        public void SaveResultAsHtml(string filePath, object result)
        {
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("filePath must be provided", nameof(filePath));
            if (result == null) throw new ArgumentNullException(nameof(result));

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            string json = JsonSerializer.Serialize(result, options);

            // Build a simple HTML wrapper for readability
            string html = $"<!doctype html>\n<html><head><meta charset=\"utf-8\"/>" +
                          "<title>Axe Accessibility Result</title>" +
                          "<style>body{font-family:Segoe UI,Helvetica,Arial; padding:16px;} pre{white-space:pre-wrap; word-wrap:break-word; background:#f6f8fa; padding:12px; border-radius:6px; border:1px solid #ddd;}</style></head>" +
                          "<body>" +
                          $"<h2>Axe Accessibility Result - {DateTime.Now:yyyy-MM-dd HH:mm:ss}</h2>" +
                          (driver != null ? $"<p>URL: <a href=\"{driver.Url}\">{System.Net.WebUtility.HtmlEncode(driver.Url)}</a></p>" : string.Empty) +
                          "<pre>" + System.Net.WebUtility.HtmlEncode(json) + "</pre>" +
                          "</body></html>";

            // Ensure directory exists
            var dir = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);

            File.WriteAllText(filePath, html);
        }

        /// <summary>
        /// Analyze the page including only the provided CSS selector scope.
        /// Example selector: "#main", ".content" or "div[role=main]"
        /// </summary>
        public object AnalyzeCssSelector(string cssSelector)
        {
            if (string.IsNullOrWhiteSpace(cssSelector))
                throw new ArgumentException("cssSelector must be provided", nameof(cssSelector));

            var builder = new AxeBuilder(driver).Include(cssSelector);
            var result = builder.Analyze();
            return result;
        }

        /// <summary>
        /// Analyze a specific IWebElement scope and return the accessibility result object.
        /// The element must have an id attribute; the wrapper will use that id to scope the analysis.
        /// </summary>
        public object AnalyzeElement(IWebElement element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));

            var id = element.GetAttribute("id");
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("The provided element must have an 'id' attribute. Use AnalyzeCssSelector to pass a selector if element has no id.", nameof(element));
            }

            var selector = "#" + id;
            var builder = new AxeBuilder(driver).Include(selector);
            var result = builder.Analyze();
            return result;
        }

        /// <summary>
        /// Analyze using a custom configuration action on the AxeBuilder.
        /// Allows callers to customize rules, tags, includes/excludes etc. Returns the raw result object.
        /// </summary>
        public object AnalyzeWithConfigurator(Action<AxeBuilder> configure)
        {
            var builder = new AxeBuilder(driver);
            configure?.Invoke(builder);
            var result = builder.Analyze();
            return result;
        }
    }
}
