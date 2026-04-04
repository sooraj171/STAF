using System;
using System.Collections;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace STAF.CF
{
    /// <summary>
    /// Per-logical-execution state for the current test. Uses AsyncLocal so parallel MSTest workers
    /// do not overwrite each other's result paths or failure flags. Still mirrors key values to
    /// environment variables where legacy consumers expect them (sequential / same-thread use).
    /// </summary>
    internal static class TestRunState
    {
        private static readonly AsyncLocal<string> ResultHtmlPath = new();
        private static readonly AsyncLocal<bool?> Failed = new();

        public static void BeginTest(TestContext context, string resultHtmlPath)
        {
            ResultHtmlPath.Value = resultHtmlPath;
            Failed.Value = false;

            Environment.SetEnvironmentVariable("currTestName", context.TestName);
            Environment.SetEnvironmentVariable(context.TestName, resultHtmlPath);
            Environment.SetEnvironmentVariable("failFlag", "no");
        }

        public static void Clear()
        {
            ResultHtmlPath.Value = null;
            Failed.Value = null;
        }

        /// <summary>Resolves the HTML report file path for the active test.</summary>
        public static string GetResultHtmlPath(TestContext context)
        {
            var path = ResultHtmlPath.Value;
            if (!string.IsNullOrEmpty(path))
                return path;

            if (context != null)
            {
                var legacy = Environment.GetEnvironmentVariable(context.TestName);
                if (!string.IsNullOrEmpty(legacy))
                    return legacy;
            }

            return string.Empty;
        }

        public static void SetFailed()
        {
            Failed.Value = true;
            Environment.SetEnvironmentVariable("failFlag", "yes");
        }

        public static bool IsFailed()
        {
            if (Failed.Value == true)
                return true;
            return string.Equals(Environment.GetEnvironmentVariable("failFlag"), "yes", StringComparison.OrdinalIgnoreCase);
        }
    }

    /// <summary>Safe reads for MSTest runsettings / TestContext.Properties (non-generic IDictionary).</summary>
    internal static class TestContextPropertyHelper
    {
        public static string GetString(TestContext context, string propertyName, string defaultValue = "")
        {
            if (context?.Properties == null || string.IsNullOrEmpty(propertyName))
                return defaultValue;

            try
            {
                var direct = context.Properties[propertyName];
                if (direct != null)
                    return direct.ToString() ?? defaultValue;
            }
            catch
            {
                // indexer may throw for missing keys depending on adapter version
            }

            try
            {
                if (context.Properties is IDictionary dict)
                {
                    foreach (DictionaryEntry entry in dict)
                    {
                        if (entry.Key != null
                            && string.Equals(entry.Key.ToString(), propertyName, StringComparison.OrdinalIgnoreCase))
                            return entry.Value?.ToString() ?? defaultValue;
                    }
                }
            }
            catch
            {
                // ignored
            }

            return defaultValue;
        }

        public static bool GetBool(TestContext context, string propertyName, bool defaultValue = false)
        {
            var s = GetString(context, propertyName, null);
            if (string.IsNullOrEmpty(s))
                return defaultValue;
            return string.Equals(s, "true", StringComparison.OrdinalIgnoreCase)
                   || string.Equals(s, "1", StringComparison.OrdinalIgnoreCase)
                   || string.Equals(s, "yes", StringComparison.OrdinalIgnoreCase);
        }
    }
}
