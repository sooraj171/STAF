using STAF.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace STAF.Utilities
{
    public class TRXParser
    {
        public static List<Dictionary<string, string>> GetUnitTestResults(string xml)
        {
            List<Dictionary<string, string>> results = new List<Dictionary<string, string>>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmlDoc.NameTable);
            namespaceManager.AddNamespace("ns", "http://microsoft.com/schemas/VisualStudio/TeamTest/2010");


            XmlNodeList unitTestResultNodes = xmlDoc.SelectNodes("/ns:TestRun/ns:Results/ns:UnitTestResult", namespaceManager);

            foreach (XmlNode unitTestResultNode in unitTestResultNodes)
            {
                Dictionary<string, string> result = new Dictionary<string, string>();

                result["testName"] = unitTestResultNode.Attributes["testName"]?.Value;
                result["duration"] = unitTestResultNode.Attributes["duration"]?.Value;
                result["startTime"] = unitTestResultNode.Attributes["startTime"]?.Value;
                result["endTime"] = unitTestResultNode.Attributes["endTime"]?.Value;
                result["outcome"] = unitTestResultNode.Attributes["outcome"]?.Value;

                results.Add(result);
            }

            return results;
        }

        public static string ReadTRXFile(string filePath)
        {
            try
            {
                // Read the .trx file as text
                string fileContents = File.ReadAllText(filePath);

                // Return the file data as a string
                return fileContents;
            }
            catch (FileNotFoundException)
            {
                // Handle file not found error
                return "File not found.";
            }
            catch (IOException)
            {
                // Handle IO error
                return "Error reading the file.";
            }
        }

        public static void SaveResultsToDB(List<Dictionary<string, string>> TestResultList)
        {
            string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=TestResults;User Id=TRUser;Password=TestResult123;";
            DBConnector connector = new (connectionString);
           

            foreach (Dictionary<string, string> result in TestResultList)
            {
                Console.WriteLine("Test Name: " + result["testName"]);
                Console.WriteLine("Duration: " + result["duration"]);
                Console.WriteLine("Start Time: " + result["startTime"]);
                Console.WriteLine("End Time: " + result["endTime"]);
                Console.WriteLine("Outcome: " + result["outcome"]);
                Console.WriteLine();
                connector.InsertExecutionResult("AppName", result["testName"], result["duration"], result["startTime"], result["endTime"], result["outcome"]);
            }
           
        }

        public static void StoreExecutionResults(string StrFile)
        {
            SaveResultsToDB(GetUnitTestResults(ReadTRXFile(StrFile)));
        }
    }
}
