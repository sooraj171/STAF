# STAF - Simple Test Automation Framework

> **C# Selenium** | **.NET Selenium Framework** | **MCP Server for Selenium C#** | **Selenium WebDriver C#** | **Test Automation .NET** | **UI Testing C#** | **API Testing .NET** | **MSTest Selenium**

[![NuGet](https://img.shields.io/nuget/v/STAF.UI.API.svg?style=flat-square)](https://www.nuget.org/packages/STAF.UI.API)
[![.NET 10](https://img.shields.io/badge/.NET-10-blue.svg?style=flat-square)](https://dotnet.microsoft.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg?style=flat-square)](LICENSE)

**STAF** is a production-ready .NET test automation framework for Selenium-based UI testing, API testing, and Excel validation. It provides base classes, HTML reporting, parallel execution, and optional database and accessibility support. The framework is distributed as the **STAF.UI.API** NuGet package and targets **.NET 10** with **MSTest**.

> **Note:** This release targets .NET 10. Projects consuming STAF.UI.API must use **.NET 10 or above**.

---

## Sample Project & MCP Agent

**[STAF.Selenium.Tests](https://github.com/sooraj171/STAF.Selenium.Tests)** is the official template and reference implementation for STAF. It includes:

- **Working samples** for every major STAF feature (UI, API, Excel, Database, Accessibility)
- **MCP Agent** - Model Context Protocol server for Selenium + STAF. Use with **Cursor**, **VS Code**, or **Visual Studio** to:
  - **Control browsers** - Start Chrome/Edge/Firefox, navigate, click, type, take screenshots
  - **Generate STAF code** - Produce C# Selenium tests (Page Object Model, ReportResult, TestBaseClass) from natural language

Get started with the framework quickly by cloning the template:

```bash
git clone https://github.com/sooraj171/STAF.Selenium.Tests
cd STAF.Selenium.Tests
dotnet restore
dotnet build
```

See the [STAF.Selenium.Tests repository](https://github.com/sooraj171/STAF.Selenium.Tests) for setup, MCP configuration, and sample tests.

---

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Key Components](#key-components)
- [Getting Started](#getting-started)
- [Configuration](#configuration)
- [Reporting](#reporting)
- [Usage Guide](#usage-guide)
- [License](#license)

---

## Overview

STAF streamlines automated testing for web applications and APIs using Selenium WebDriver and C#. It supports:

| Area | Capability |
|------|------------|
| **UI automation** | Page Object Model-style tests with Selenium (Chrome, Edge, local or remote) |
| **API automation** | REST-style tests with the same reporting and lifecycle as UI tests |
| **HTML reporting** | Per-test and assembly-level HTML reports with pass/fail summary and optional screenshots |
| **Parallel execution** | Thread-safe reporting and MSTest parallelization (method-level, configurable workers) |
| **Excel** | Compare workbooks/sheets and read/write cell data via ClosedXML |
| **Database** | SQL Server helpers (connection, query, scalar, non-query) using configuration |
| **Accessibility** | Axe-core (Deque.AxeCore.Selenium) integration for accessibility scans and HTML reports |

---

## Features

| Feature | Description |
|--------|-------------|
| **Base classes** | `TestBaseClass` (UI), `TestBaseAPI` (API), `PageBaseClass` (elements and waits) |
| **Browser support** | Chrome, Edge; local or remote WebDriver. Overridable options and driver creation |
| **HTML reporting** | In-test step reporting (Pass/Fail/Warn/Info) and assembly summary (`ResultTemplateFinal.html`) |
| **Parallel execution** | Parallel-safe result accumulation; MSTest `Parallelize` (e.g. 4 workers, method scope) |
| **Excel** | Compare two workbooks/sheets; get/set cell data; row/column counts |
| **Database** | `DbHelper`: connection strings from config, execute query/scalar/non-query |
| **Configuration** | `appsettings.json` and run settings (browser, driver path, URL, test parameters) |
| **Email** | Optional email of test results (SMTP via config or TestContext) |
| **Accessibility** | Axe-core page/element scans and styled HTML reports |
| **Report generator** | Programmatic HTML reports via `TestReportGenerator` and `TestResultData` |

---

## Key Components

### Test and Page Base Classes

| Class | Purpose |
|-------|---------|
| **TestBaseClass** | Inherit for UI tests. Initializes WebDriver (from TestContext/run settings), sets up per-test HTML result file, and cleans up (including reporting to the assembly summary) |
| **TestBaseAPI** | Inherit for API tests. Same startup/cleanup and reporting as UI tests, without a browser |
| **PageBaseClass** | Wraps element location with wait: `FindAppElement(By)`, `FindAppElement(By, description)`, `FindAppElement(parent, By, description)` |

### Browser and Driver

| Class | Purpose |
|-------|---------|
| **BrowserDriver** | Creates `IWebDriver` for Chrome or Edge, local or remote. Override `SetChromeOptions()` / `SetEdgeOptions()` or `GetBrowserDriverObject()` to customize |

### Reporting

| Class | Purpose |
|-------|---------|
| **ReportResult** | Log steps for UI tests: `ReportResultPass/Fail/Warn/Info(driver, TestContext, moduleName, description[, exception])` |
| **ReportResultAPI** | Same for API tests: `ReportResultPass/Fail/Warn/Info(TestContext, moduleName, description[, exception])` |
| **HtmlResult** | Creates and appends to per-test HTML files. Used by report classes; supports screenshots on fail |
| **ReportElement** (extensions) | Assert and report in one call: `ReportElementExists`, `ReportElementIsDisplayed`, `ReportElementIsEnabled` (with optional proceed-on-fail flag) |

### WebDriver Extensions

| Method | Purpose |
|--------|---------|
| **CloseAllTabsExceptCurrent** | Close all browser tabs except the current one |
| **getTotalTabsCount** | Return number of open tabs |
| **waitForFindElement** | Find element with explicit timeout |
| **waitForElementExist** / **waitForElementNotExist** | Wait for element presence/absence |
| **WaitForElementDisapper** | Wait until element is no longer present (By) |
| **WaitForDocumentReady** | Wait for document ready and (if present) jQuery idle |

### Excel, Database & Accessibility

| Class | Purpose |
|-------|---------|
| **ExcelDriver** | `CompareFiles`, `GetExcelWorkbook`, `GetExcelCellData`, `SetExcelCellData`, `GetExcelRowCount`, `GetExcelColumnCount` |
| **DbHelper** | Connection strings from `AppConfig`. `OpenConnection`, `VerifyConnection`, `ExecuteQuery`, `ExecuteScalar`, `ExecuteNonQuery` |
| **AxeAccessibility** | Deque Axe-core: `AnalyzePage()`, `AnalyzePageAndSaveHtml()`, `AnalyzeCssSelector()`, `AnalyzeElement()`, `AnalyzeWithConfigurator()` |

---

## Getting Started

### Prerequisites

- **.NET 10 SDK**
- **Visual Studio 2022** (or later) or **VS Code** with C# extension
- **Chrome** or **Edge** (for UI tests)
- **MSTest** (included via package reference)

### Install

1. **Add the NuGet package** to your test project:
   ```bash
   dotnet add package STAF.UI.API
   ```
   Or use the [sample project template](https://github.com/sooraj171/STAF.Selenium.Tests) for a complete setup with examples.

2. Configure run settings: **Test** > **Configure Run Settings** > **Select Solution Wide runsettings File** > choose `testrunsetting.runsettings`.

3. Build and run tests from **Test Explorer** or CLI: `dotnet test`.

### Sample UI Test

```csharp
[TestMethod]
public void TestLogin()
{
    var element = FindAppElement(By.Id("loginButton"), "Login button");
    ReportResult.ReportResultPass(driver, TestContext, "Login", "Login button is visible");
}
```

### Sample API Test

```csharp
[TestMethod]
public void TestAPIStatus()
{
    ReportResultAPI.ReportResultPass(TestContext, "API", "Status check passed");
}
```

---

## Configuration

### Run settings (`testrunsetting.runsettings`)

- **TestRunParameters**: `browser` (e.g. `chrome`), `driverPath`, `url`, optional email/smtp settings
- **MSTest**: `Parallelize` (e.g. `Workers=4`, `Scope=MethodLevel`)
- **ResultsDirectory**: e.g. `.\TestResults`

### appsettings.json

- **ConnectionStrings**: e.g. `DefaultConnection` for `DbHelper`
- **Email**: SmtpHost, SmtpPort, UseDefaultCred, Username, Password (optional)

### Overriding browser options

```csharp
protected override ChromeOptions SetChromeOptions()
{
    var options = new ChromeOptions();
    options.AddArguments("start-maximized");
    return options;
}
```

---

## Reporting

### In-test step reporting

```csharp
ReportResult.ReportResultPass(driver, TestContext, "ModuleName", "Description");
ReportResult.ReportResultFail(driver, TestContext, "ModuleName", "Description", "exception text");

// API tests (no driver)
ReportResultAPI.ReportResultPass(TestContext, "ModuleName", "Description");
```

### Element assertions with reporting

```csharp
element.ReportElementExists(driver, TestContext, testName, "Element exists", ProdceedFlag: true);
element.ReportElementIsDisplayed(driver, TestContext, testName, "Element is displayed", ProdceedFlag: true);
```

### Assembly summary

After all tests, **AssemblyCleanup** writes the combined result body to **ResultTemplate.html** and copies it to **ResultTemplateFinal.html**. Reporting is parallel-safe (file-based accumulator).

---

## Usage Guide

### Excel comparison

```csharp
var excel = new ExcelDriver();
ExcelCompareStatus res = excel.CompareFiles("path/Excel1.xlsx", "path/Excel2.xlsx", sheetIndex1: 1, sheetIndex2: 1);
if (res.IsMatching) return true;
```

### Accessibility (Axe)

```csharp
var axe = new AxeAccessibility(driver);
axe.AnalyzePage();                                    // Full page scan
axe.AnalyzePageAndSaveHtml("report.html");            // Run and save HTML report
axe.AnalyzeCssSelector("#main");                      // Scoped scan
axe.AnalyzeWithConfigurator(b => b.Include(".modal")); // Custom configuration
```

### Database (DbHelper)

Configure connection in `appsettings.json` under `ConnectionStrings:DefaultConnection`, then use:

```csharp
DbHelper.VerifyConnection();
var value = DbHelper.ExecuteScalar<int>("SELECT COUNT(*) FROM Users");
```

---

## License

This project is licensed under the **MIT License**.

**Author:** Sooraj Ramachandran  
**Copyright (c) 2026 Sooraj Ramachandran. All rights reserved.**

---

| Links |
|-------|
| **NuGet:** [STAF.UI.API](https://www.nuget.org/packages/STAF.UI.API) |
| **Sample project & MCP Agent:** [STAF.Selenium.Tests](https://github.com/sooraj171/STAF.Selenium.Tests) |

*This software is provided "as is", without warranty of any kind. See the [license](LICENSE) for full terms.*
