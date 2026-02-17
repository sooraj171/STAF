# STAF - Simple Test Automation Framework

**STAF** is a .NET test automation framework for Selenium-based UI testing, API testing, and Excel validation. It provides base classes, HTML reporting, parallel execution, and optional database and accessibility support. The framework is distributed as the **STAF.UI.API** NuGet package and is built for **.NET 10** with **MSTest**.

> **Note:** This release targets .NET 10. Projects consuming STAF.UI.API must use **.NET 10 or above**.

---

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Key Components](#key-components)
- [Getting Started](#getting-started)
- [Configuration](#configuration)
- [Usage Guide](#usage-guide)
- [Reporting](#reporting)
- [License](#license)

---

## Overview

STAF streamlines automated testing for web applications and APIs using Selenium WebDriver and C#. It supports:

- **UI automation** ? Page Object Model?style tests with Selenium (Chrome, Edge, local or remote).
- **API automation** ? REST-style tests with the same reporting and lifecycle as UI tests.
- **HTML reporting** ? Per-test and assembly-level HTML reports with pass/fail summary and optional screenshots.
- **Parallel execution** ? Thread-safe reporting and MSTest parallelization (method-level, configurable workers).
- **Excel** ? Compare workbooks/sheets and read/write cell data via ClosedXML.
- **Database** ? SQL Server helpers (connection, query, scalar, non-query) using configuration.
- **Accessibility** ? Axe-core (Deque.AxeCore.Selenium) integration for accessibility scans and HTML reports.

---

## Features

| Feature | Description |
|--------|-------------|
| **Base classes** | `TestBaseClass` (UI), `TestBaseAPI` (API), `PageBaseClass` (elements and waits). |
| **Browser support** | Chrome, Edge; local or remote WebDriver. Overridable options and driver creation. |
| **HTML reporting** | In-test step reporting (Pass/Fail/Warn/Info) and assembly summary (`ResultTemplateFinal.html`). |
| **Parallel execution** | Parallel-safe result accumulation; MSTest `Parallelize` (e.g. 4 workers, method scope). |
| **Excel** | Compare two workbooks/sheets; get/set cell data; row/column counts. |
| **Database** | `DbHelper`: connection strings from config, open connection, verify, execute query/scalar/non-query. |
| **Configuration** | `appsettings.json` and run settings (browser, driver path, URL, test parameters). |
| **Email** | Optional email of test results (SMTP via config or TestContext). |
| **Accessibility** | Axe-core page/element scans and styled HTML reports. |
| **Report generator** | Programmatic HTML reports via `TestReportGenerator` and `TestResultData`. |

---

## Key Components

### Test and Page Base Classes

| Class | Purpose |
|-------|--------|
| **TestBaseClass** | Inherit for UI tests. Initializes WebDriver (from TestContext/run settings), sets up per-test HTML result file, and cleans up (including reporting to the assembly summary). |
| **TestBaseAPI** | Inherit for API tests. Same startup/cleanup and reporting as UI tests, without a browser. |
| **PageBaseClass** | Wraps element location with wait: `FindAppElement(By)`, `FindAppElement(By, description)`, `FindAppElement(parent, By, description)`. |

### Browser and Driver

| Class | Purpose |
|-------|--------|
| **BrowserDriver** | Creates `IWebDriver` for Chrome or Edge, local or remote. Override `SetChromeOptions()` / `SetEdgeOptions()` or `GetBrowserDriverObject()` to customize. |

### Reporting

| Class | Purpose |
|-------|--------|
| **ReportResult** | Log steps for UI tests: `ReportResultPass/Fail/Warn/Info(driver, TestContext, moduleName, description[, exception])`. |
| **ReportResultAPI** | Same for API tests: `ReportResultPass/Fail/Warn/Info(TestContext, moduleName, description[, exception])`. |
| **HtmlResult** | Creates and appends to per-test HTML files (`TC_ResultStartTime`, `TC_ResultCreation`, `TC_EndTime`). Used by report classes; supports screenshots on fail. |
| **ReportElement** (extensions) | Assert and report in one call: `ReportElementExists`, `ReportElementIsDisplayed`, `ReportElementIsEnabled` (with optional proceed-on-fail flag). |

### Assembly and Configuration

| Class / File | Purpose |
|--------------|--------|
| **AssemblyInit** | `[AssemblyInitialize]` / `[AssemblyCleanup]`: creates `ResultTemplate.html`, initializes result accumulator, cleans up chromedriver; writes final summary to `ResultTemplateFinal.html` (parallel-safe). |
| **CommonAction** | Test lifecycle: `setStartUpValues(TestContext)`, `setCleanUpValues(resultFile, TestContext, totalTime)`. Email: `SendEmail(...)`, `SetMailServer(...)`. Env: `LoadAppEnvVariables(path)`, `updateEnvVal(name, value)`. |
| **AppConfig** | `AppConfig.GetConfig()` ? loads `appsettings.json` from current directory. |
| **DirectoryUtils** | `DirectoryUtils.BaseDirectory` ? assembly location (for reports and config paths). |

### WebDriver Extensions (CommonAction namespace)

| Method | Purpose |
|--------|--------|
| **CloseAllTabsExceptCurrent** | Close all browser tabs except the current one. |
| **getTotalTabsCount** | Return number of open tabs. |
| **waitForFindElement** | Find element with explicit timeout. |
| **waitForElementExist** / **waitForElementNotExist** | Wait for element presence/absence. |
| **WaitForElementDisapper** | Wait until element is no longer present (By). |
| **WaitForDocumentReady** | Wait for document ready and (if present) jQuery idle. |

### Excel

| Class | Purpose |
|-------|--------|
| **ExcelDriver** | `CompareFiles(file1, file2[, sheet1, sheet2])` ? `ExcelCompareStatus` (IsMatching, Messages). `GetExcelWorkbook(file)`, `GetExcelCellData` / `SetExcelCellData`, `GetExcelRowCount` / `GetExcelColumnCount`. |
| **ExcelCompareStatus** | `IsMatching`, `Messages` (list of difference or validation messages). |

### Database

| Class | Purpose |
|-------|--------|
| **DbHelper** | Connection strings from `AppConfig` (e.g. `DefaultConnection` in appsettings). `GetConnectionString(name)`, `OpenConnection(name)`, `VerifyConnection(name)`, `ExecuteQuery(sql[, connName, params])`, `ExecuteScalar`, `ExecuteNonQuery`, and helpers to read results. |

### Programmatic Reports and Accessibility

| Class | Purpose |
|-------|--------|
| **TestReportGenerator** | Build HTML/CSS/JS reports from `TestResultData` (test name, result, time, filters, groups, items, messages, screenshots). Use when you need custom report layout or data. |
| **AxeAccessibility** (STAFS/Accessibility) | Deque Axe-core: `AnalyzePage()`, `AnalyzePageAndSaveHtml(filePath)`, `SaveResultAsHtml(filePath, result)`, `AnalyzeCssSelector(cssSelector)`, `AnalyzeElement(IWebElement)`, `AnalyzeWithConfigurator(Action<AxeBuilder>)`. |

---

## Getting Started

### Prerequisites

- **.NET 10 SDK**
- **Visual Studio 2022** (or later) or **VS Code** with C# extension
- **Chrome** or **Edge** (for UI tests)
- **MSTest** (included via package reference)

### Install

1. Add the **STAF.UI.API** NuGet package to your test project, or clone the repository:
   ```bash
   git clone https://github.com/sooraj171/STAF.Selenium.Tests
   ```
2. Open the solution in Visual Studio.
3. **Test** ? **Configure Run Settings** ? **Select Solution Wide runsettings File** ? choose `testrunsetting.runsettings`.
4. **Build** ? **Build Solution**.
5. In **Test Explorer**, select one or more tests and run.

### Sample UI Test

```csharp
[TestMethod]
public void TestLogin()
{
    // driver and TestContext come from TestBaseClass
    var element = FindAppElement(By.Id("loginButton"), "Login button");
    ReportResult.ReportResultPass(driver, TestContext, "Login", "Login button is visible");
}
```

### Sample API Test

```csharp
[TestMethod]
public void TestAPIStatus()
{
    // Use TestBaseAPI; no browser
    ReportResultAPI.ReportResultPass(TestContext, "API", "Status check passed");
}
```

---

## Configuration

### Run settings (`testrunsetting.runsettings`)

- **TestRunParameters**: `browser` (e.g. `chrome`), `driverPath`, `url`, and optional email/smtp settings.
- **MSTest**: `Parallelize` (e.g. `Workers=4`, `Scope=MethodLevel`) for parallel execution.
- **ResultsDirectory**: e.g. `.\TestResults`.

### appsettings.json

- **ConnectionStrings**: e.g. `DefaultConnection` for `DbHelper`.
- **Email**: SmtpHost, SmtpPort, UseDefaultCred, Username, Password (optional).

### Overriding browser options or driver

```csharp
protected override ChromeOptions SetChromeOptions()
{
    var options = new ChromeOptions();
    options.AddArguments("start-maximized");
    return options;
}

public override IWebDriver GetBrowserDriverObject(string brwType, string driverPath = "", bool isRemote = false)
{
    return base.GetBrowserDriverObject(brwType, driverPath, isRemote);
}
```

---

## Reporting

### In-test step reporting

```csharp
ReportResult.ReportResultPass(driver, TestContext, "ModuleName", "Description");
ReportResult.ReportResultFail(driver, TestContext, "ModuleName", "Description", "exception text");
ReportResult.ReportResultWarn(driver, TestContext, "ModuleName", "Description");
ReportResult.ReportResultInfo(driver, TestContext, "ModuleName", "Description");

// API tests (no driver)
ReportResultAPI.ReportResultPass(TestContext, "ModuleName", "Description");
ReportResultAPI.ReportResultFail(TestContext, "ModuleName", "Description", "exception text");
```

### Element assertions with reporting

```csharp
element.ReportElementExists(driver, TestContext, testName, "Element exists", ProdceedFlag: true);
element.ReportElementIsDisplayed(driver, TestContext, testName, "Element is displayed", ProdceedFlag: true);
element.ReportElementIsEnabled(driver, TestContext, testName, "Element is enabled", ProdceedFlag: true);
```

### Assembly summary

- After all tests, **AssemblyCleanup** writes the combined result body to **ResultTemplate.html** and copies it to **ResultTemplateFinal.html** (e.g. under TestResults or parent folder). Reporting is parallel-safe (file-based accumulator).

---

## Usage Guide

### Excel comparison

```csharp
var excel = new ExcelDriver();
ExcelCompareStatus res = excel.CompareFiles("path/Excel1.xlsx", "path/Excel2.xlsx", sheetIndex1: 1, sheetIndex2: 1);
if (res.IsMatching)
    return true;
else
    return false;
```

### Accessibility (Axe)

- Use **AxeAccessibility** in a test (with a running browser and page loaded):
  - `AnalyzePage()` for full page scan.
  - `AnalyzePageAndSaveHtml(filePath)` to run and save a styled HTML report.
  - `AnalyzeCssSelector(selector)` or `AnalyzeElement(element)` for scoped scans.
  - `AnalyzeWithConfigurator(configure)` to customize `AxeBuilder` before analysis.

### Database (DbHelper)

- Configure connection in `appsettings.json` under `ConnectionStrings:DefaultConnection`.
- Use `DbHelper.OpenConnection()`, `DbHelper.ExecuteQuery(sql)`, `DbHelper.ExecuteScalar`, `DbHelper.ExecuteNonQuery`, etc., as needed in tests.

---

## License

This project is licensed under the **MIT License**.

**Author:** Sooraj Ramachandran  
**Copyright (c) 2026 Sooraj Ramachandran. All rights reserved.**

---

Framework maintained at: [github.com/sooraj171/STAF.Selenium.Tests](https://github.com/sooraj171/STAF.Selenium.Tests)

*THIS SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.*
