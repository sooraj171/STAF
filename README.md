# STAF - Simple Test Automation Framework

> **C# Selenium** | **.NET Selenium Framework** | **MCP Server for Selenium C#** | **Selenium WebDriver C#** | **Test Automation .NET** | **UI Testing C#** | **API Testing .NET** | **MSTest Selenium**

[![NuGet](https://img.shields.io/nuget/v/STAF.UI.API.svg?style=flat-square)](https://www.nuget.org/packages/STAF.UI.API)
[![.NET 10](https://img.shields.io/badge/.NET-10-blue.svg?style=flat-square)](https://dotnet.microsoft.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg?style=flat-square)](LICENSE)

**STAF** is a production-ready .NET test automation framework for Selenium-based UI testing, API testing, and Excel validation. It provides base classes, HTML reporting, parallel execution, and optional database and accessibility support. The framework is distributed as the **STAF.UI.API** NuGet package and targets **.NET 10** with **MSTest**.

> **Note:** This release targets .NET 10. Projects consuming STAF.UI.API must use **.NET 10 or above**.

---

## Table of Contents

- [AI-assisted development and MCP (implementation repo)](#ai-assisted-development-and-mcp-implementation-repo)
- [Overview](#overview)
- [Features](#features)
- [Key Components](#key-components)
- [Getting Started](#getting-started)
- [Configuration](#configuration)
- [Reporting](#reporting)
- [Parallel execution and thread safety](#parallel-execution-and-thread-safety)
- [Upgrading and migration](#upgrading-and-migration)
- [Usage Guide](#usage-guide)
- [License](#license)

---

## AI-assisted development and MCP (implementation repo)

**This repository** contains the **STAF.UI.API** framework source and ships the NuGet package. It does **not** include the MCP server executable or editor-specific AI configuration—those live in the official **implementation / sample** repository so you can pair the package with real projects and tooling.

**Use this together with the implementation project:**

| Piece | Where it lives |
|-------|----------------|
| **STAF.UI.API** NuGet package | This repo / [NuGet.org](https://www.nuget.org/packages/STAF.UI.API) — add it to any .NET 10+ test project |
| **Sample solution, MCP server, and AI workflows** | **[STAF.Selenium.Tests](https://github.com/sooraj171/STAF.Selenium.Tests)** on GitHub |

**[STAF.Selenium.Tests](https://github.com/sooraj171/STAF.Selenium.Tests)** is the official template that **references STAF.UI.API** and adds:

- **MCP (Model Context Protocol) server** for Selenium + STAF — a self-contained agent you can attach to **Cursor**, **VS Code**, or **Visual Studio** (Copilot agent mode) to drive browsers and generate tests from natural language.
- **Editor rules** — e.g. `.cursor/rules`, `.github/copilot-instructions.md`, and repo-level `.mcp.json` so assistants follow STAF patterns (`TestBaseClass`, `FindAppElement`, `ReportResult`, etc.).
- **Working samples** for UI, API, Excel, database, reporting, accessibility, and parallel runs.

Clone the implementation repo to run MCP, copy its config into your own solution, or start new suites from the template while keeping **STAF.UI.API** as the shared framework dependency:

```bash
git clone https://github.com/sooraj171/STAF.Selenium.Tests
cd STAF.Selenium.Tests
dotnet restore
dotnet build
```

For MCP setup (paths to `MCPAgent/publish/...`, tool list, troubleshooting), see **MCPAgent/README.md** and the main README in [STAF.Selenium.Tests](https://github.com/sooraj171/STAF.Selenium.Tests).

---

## Overview

STAF streamlines automated testing for web applications and APIs using Selenium WebDriver and C#. It supports:

| Area | Capability |
|------|------------|
| **UI automation** | Page Object Model-style tests with Selenium (Chrome, Edge, local or remote) |
| **API automation** | REST-style tests with the same reporting and lifecycle as UI tests |
| **HTML reporting** | Per-test and assembly-level HTML reports with pass/fail summary and optional screenshots |
| **Parallel execution** | Per-test `AsyncLocal` state, per-report-file locking, file-based assembly accumulator; MSTest parallelization (method-level, configurable workers) |
| **Excel** | Compare workbooks/sheets and read/write cell data via ClosedXML |
| **Database** | SQL Server helpers (connection, query, scalar, non-query) using configuration |
| **Accessibility** | Axe-core (Deque.AxeCore.Selenium) integration for accessibility scans and HTML reports |
| **AI / MCP (optional)** | Not in this repo — use **[STAF.Selenium.Tests](https://github.com/sooraj171/STAF.Selenium.Tests)** with this NuGet package for an MCP server and Cursor / Copilot integration |

---

## Features

| Feature | Description |
|--------|-------------|
| **Base classes** | `TestBaseClass` (UI), `TestBaseAPI` (API), `PageBaseClass` (elements and waits) |
| **Browser support** | Chrome, Edge; local or remote WebDriver. Overridable options and driver creation |
| **HTML reporting** | In-test step reporting (Pass/Fail/Warn/Info) and assembly summary (`ResultTemplateFinal.html`) |
| **Parallel execution** | Parallel-safe paths and fail flags (`AsyncLocal`); per-file HTML locks; MSTest `Parallelize` (e.g. 4 workers, method scope) |
| **Excel** | Compare two workbooks/sheets; get/set cell data; row/column counts |
| **Database** | `DbHelper`: connection strings from config, execute query/scalar/non-query |
| **Configuration** | `appsettings.json` (resolved from base directory, assembly folder, or cwd) and run settings (browser, headless, driver path, URL, test parameters) |
| **Email** | Optional email of test results (SMTP via config or TestContext) |
| **Accessibility** | Axe-core page/element scans and styled HTML reports |
| **Report generator** | `TestReportGenerator.GenerateTestReport(filePath, TestResultData)` for programmatic HTML (plus sidecar CSS/JS) |
| **AI-assisted authoring** | Implemented in **[STAF.Selenium.Tests](https://github.com/sooraj171/STAF.Selenium.Tests)** via MCP + editor rules; consumes **STAF.UI.API** like any other test project |

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
| **BrowserDriver** | Creates `IWebDriver` for Chrome or Edge, local or remote. Override parameterless `SetChromeOptions()` / `SetEdgeOptions()` for custom arguments; optional `headless` runsetting applies headless flags after your options |

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
| **WaitForDocumentReady** | Wait for document ready and (if present) jQuery idle (default timeout 30 seconds; optional `timeoutSeconds` parameter) |

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
   Or use the [STAF.Selenium.Tests](https://github.com/sooraj171/STAF.Selenium.Tests) implementation project for samples, runsettings, and optional **MCP / AI** tooling (see [AI-assisted development and MCP](#ai-assisted-development-and-mcp-implementation-repo)).

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
- **`headless`**: set to `true` for CI/agents (Chrome/Edge headless is applied after your `SetChromeOptions` / `SetEdgeOptions` overrides)
- **`killOrphanChromedrivers`**: default `false`. Set to `true` only on **isolated** agents if you still need assembly-level cleanup that terminates **all** `chromedriver` processes (avoid on shared build machines)
- **MSTest**: `Parallelize` (e.g. `Workers=4`, `Scope=MethodLevel`)
- **ResultsDirectory**: e.g. `.\TestResults`

### appsettings.json

`AppConfig.GetConfig()` looks for `appsettings.json` in this order: **`AppContext.BaseDirectory`**, the **executing assembly directory**, then **`Directory.GetCurrentDirectory()`**. The first match wins.

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

Use runsetting **`headless=true`** for pipelines; the framework adds headless arguments **after** this method runs, so existing overrides stay valid.

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

After all tests, **AssemblyCleanup** merges per-test HTML fragments using a **file-based accumulator**, appends them to **ResultTemplate.html**, and copies the outcome to **ResultTemplateFinal.html** next to the test run output. Those files are created under **`TestContext.TestRunDirectory`** when the test host provides it (typical under **TestResults**); otherwise the framework falls back to the test assembly base directory.

Fail screenshots are written next to the **per-test HTML report** with unique names; the report still embeds them as **base64** for the same in-browser experience as before.

### Programmatic HTML (`TestReportGenerator`)

```csharp
TestReportGenerator.GenerateTestReport(@"C:\out\my_report.html", myTestResultData);
```

Use a **unique `filePath`** when multiple processes or tools might generate reports at the same time.

---

## Parallel execution and thread safety

- **Per-test state** (result HTML path, failure flag) uses **`AsyncLocal`** so parallel MSTest workers do not overwrite each other. The framework still mirrors **`failFlag`** and per-test path keys to **environment variables** for backward compatibility in sequential runs.
- **HTML append** uses a **lock per report file path**, not one global lock, so different tests can write their own reports concurrently.
- **`ReportResult` / `ReportResultAPI`** resolve the report file via the active test context first, then fall back to **`Environment.GetEnvironmentVariable(TestContext.TestName)`** if you use a custom startup path.

---

## Upgrading and migration

When updating from older STAF.UI.API builds, check the following:

| Topic | What changed |
|--------|----------------|
| **ChromeDriver cleanup** | Killing every `chromedriver` process at assembly start/end is **off by default**. Set **`killOrphanChromedrivers=true`** in runsettings only if you need the old behavior on a dedicated agent. |
| **Aggregate report location** | **ResultTemplate.html**, **ResultBodyAccumulator.txt**, and the merged **ResultTemplateFinal.html** path are rooted in **`TestRunDirectory`** when available. Pipelines or scripts that assumed **`bin\Debug\...`** only should publish **`TestResults`** (or your configured **ResultsDirectory**) instead. |
| **Headless CI** | Prefer **`headless=true`** in runsettings rather than only custom options. |
| **`TestReportGenerator`** | Use the public **`GenerateTestReport(string filePath, TestResultData results)`** API; avoid hard-coding a shared filename like `test_report.html` when running in parallel. |
| **Public API** | Existing entry points (**`TestBaseClass`**, **`TestBaseAPI`**, **`CommonAction.setStartUpValues` / `setCleanUpValues`**, **`ReportResult`**, **`HtmlResult`**, parameterless **`SetChromeOptions` / `SetEdgeOptions`**) are preserved for consuming projects. |

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
| **Implementation project, samples & MCP (AI):** [STAF.Selenium.Tests](https://github.com/sooraj171/STAF.Selenium.Tests) |

*This software is provided "as is", without warranty of any kind. See the [license](LICENSE) for full terms.*
