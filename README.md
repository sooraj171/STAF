## **About:**  

- STAF stands for Simple Test Automation Framework.
- STAF.UI.API nuget for Page Object Model implementation of Selenium Framework.

**Overview**
This repository contains an automation test framework built using Selenium WebDriver and C#. It is designed to streamline the creation and execution of automated tests for web applications and APIs, allowing for efficient reporting and error handling.

The framework consists of the following key components:
- PageBaseClass: A base class to interact with web elements on the page with wait functionality.
- TestBaseClass: A base test class for initializing and cleaning up WebDriver and running tests with Selenium.
- TestBaseAPI: A class for running API tests with similar initialization and cleanup functionalities.
- ReportResult: A utility class to handle reporting of test results including pass, fail, warn, and info statuses.

## **What it has**

- STAF is a test automation framework for c# projects using selenium.
- STAF helps to automate API and UI Tests using REST and Selenium.
- STAF has all base class written so you can inherit and use those.
- STAF has a good HTML reporting as well for test result viewing.
- Parallel Execution using native machine threads.
- Excel Comparison and easy usage.
- STAF framework is maintained by https://github.com/sooraj171


## **How to implement**

From your Visual Studio IDE Navigate to file menu:
- Extension -> Manage Extension -> Online Templates -> Search "STAFTemplate".
- Install the template successfully.
-   Create a new project from Visual Studio
- Search for the template "STAFSample" and select it and create project
- From the Build menu, Click Build Project
- From Test Explorer, select the test and Click run.
- A sample test will get executed.

## **How to Override the driver and driver options**

To override BrowserOptions use SetChromeOptions / SetEdgeOptions
    Usage:
        protected override ChromeOptions SetChromeOptions()
        { return ChromeOptions;}

To override BrowserDriver
    Usage:
        public override IWebDriver GetBrowserDriverObject("Chrome", "", false)
        { return WebDriver;}

## **How to Use the Reporting options**
    
    Usage:
        ReportResult.ReportResultPass(Driver, context, testName, "Details.");
        ReportResultAPI.ReportResultPass(TestContext, "ModuleName/FunctionName", "Details");
    Other Options:  
        ReportResultFail,ReportResultWarn,ReportResultInfo

## **How to Use Excel Options**
    ExcelDriver excel= new ExcelDriver();
            ExcelCompareStatus res = excel.CompareFiles("\Excel1.xlsx", "\Excel2.xlsx",SheetIndexFile1,SheetIndexFile2);
            if (res.IsMatching)
            {
               return true;
            }
            else
            {
                return false;
            }
            
## Key Classes

### 1. **PageBaseClass**

-   **Purpose**: Handles interactions with web elements by locating them using various `By` expressions and waiting for them to be visible before performing actions.
-   **Main Methods**:
    -   `FindAppElement`: Locates an element on the page with wait functionality.
    -   `FindAppElement(IWebElement parentElement)`: Locates an element within a specific parent element with wait functionality.

### 2. **TestBaseClass**

-   **Purpose**: A base test class that initializes the WebDriver and starts/stops tests, including performance monitoring.
-   **Main Features**:
    -   Initializes the WebDriver for the browser type specified in the TestContext.
    -   Records the total time for each test execution.
    -   Handles test setup and cleanup with logging of start and stop times.

### 3. **TestBaseAPI**

-   **Purpose**: Similar to `TestBaseClass` but specifically used for API testing.
-   **Main Features**:
    -   Handles API test initialization and cleanup.
    -   Allows reporting of test execution status (Pass/Fail) for API-related tests.

### 4. **ReportResult & ReportResultAPI**

-   **Purpose**: These classes handle reporting the results of test executions.
-   **Main Features**:
    -   Report test results with various statuses: Pass, Fail, Warn, Info.
    -   Uses `HtmlResult.TC_ResultCreation` to generate HTML reports.
    -   Can be used for both WebDriver-based tests (`ReportResult`) and API-based tests (`ReportResultAPI`).

## Setup & Configuration

### Prerequisites

-   **.NET Framework** or **.NET Core**: The framework is compatible with both.
-   **Selenium WebDriver**: Used for interacting with the browser.
-   **TestContext**: Required for passing test metadata (e.g., test name, test properties).

### Environment Variables

-   `browser`: The type of browser to use (e.g., "Chrome", "Firefox").
-   `driverPath`: Path to the browser driver (e.g., ChromeDriver for Chrome).
-   **Test Report Location**: Ensure that a valid directory for reports is available.

### API Test Setup

For API tests, the framework requires an **endpoint** to be set in the TestContext. This will be used during the test initialization.

## Running Tests

### Web Tests

1.  **Initialize WebDriver**: The framework will automatically pick up the browser type and driver path from the TestContext.
2.  **Write Test**: Create test methods using the `TestBaseClass`. Example:
```csharp
[TestMethod] public  void  TestLogin()
{ // Test code here ReportResult.ReportResultPass(driver, TestContext, "Login", "Login button is visible");
}
```
### API Tests
-   **API Test Initialization**: Set the endpoint in the TestContext properties.
-   **Write API Test**: Create test methods using `TestBaseAPI`. 
Example:
```csharp
[TestMethod]
public void TestAPIStatus()
{
    // API test code here
    ReportResultAPI.ReportResultPass(TestContext, "API", "Status check passed");
}
```
### Accessibility Tests Setup

This project includes a lightweight wrapper around Deque.AxeCore.Selenium to run accessibility scans: the `SATF.Accessibility.AxeAccessibility` class (file: `STAFS/Accessibility/AxeAccessibility.cs`).

Quick overview
- Purpose: run axe-core scans from tests and produce a readable HTML report.
- Key methods:
  - `AnalyzePage()` — runs an accessibility scan of the current page and returns the raw result object.
  - `AnalyzePageAndSaveHtml(string filePath)` — runs the scan and writes a styled HTML report (summary + details + raw JSON).
  - `SaveResultAsHtml(string filePath, object result)` — save a previously obtained result to HTML.
  - `AnalyzeCssSelector(string cssSelector)` — run scan scoped to a CSS selector.
  - `AnalyzeElement(IWebElement element)` — run scan for an element that has an `id` attribute (uses `#id`).
  - `AnalyzeWithConfigurator(Action<AxeBuilder> configure)` — pass a lambda to customize the `AxeBuilder` before analysis.

Usage examples

- Simple run and save report (recommended inside a test cleanup or explicit step):

````````

## **License:**  
     This project is licensed under the MIT License.
 
© 2025 Sooraj Ramachandran. All Rights Reserved.

THIS SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.