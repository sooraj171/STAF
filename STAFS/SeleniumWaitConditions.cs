using OpenQA.Selenium;
using System;

namespace STAF.CF
{
    /// <summary>
    /// Selenium 4 wait predicates for use with <see cref="OpenQA.Selenium.Support.UI.WebDriverWait"/>.
    /// Replaces <c>DotNetSeleniumExtras.WaitHelpers.ExpectedConditions</c> (which targets Selenium 3.x).
    /// </summary>
    public static class SeleniumWaitConditions
    {
        /// <summary>
        /// Waits until at least one element matching <paramref name="locator"/> exists in the DOM.
        /// Returns <see langword="null"/> while not found so <see cref="OpenQA.Selenium.Support.UI.WebDriverWait"/> keeps polling.
        /// Behavior matches legacy <c>ExpectedConditions.ElementExists</c>.
        /// </summary>
        public static Func<IWebDriver, IWebElement> ElementExists(By locator) =>
            driver =>
            {
                try
                {
                    return driver.FindElement(locator);
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
            };

        /// <summary>
        /// Waits until a child element matching <paramref name="locator"/> exists under <paramref name="parent"/>.
        /// </summary>
        public static Func<IWebDriver, IWebElement> ElementExists(IWebElement parent, By locator) =>
            _ =>
            {
                try
                {
                    return parent.FindElement(locator);
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            };
    }
}
