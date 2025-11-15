using System;
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
