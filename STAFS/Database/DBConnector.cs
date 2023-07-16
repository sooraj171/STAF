using DocumentFormat.OpenXml.Drawing.Diagrams;
using System;
using System.Data;
using System.Data.SqlClient;

namespace STAF.Database
{
    public class DBConnector
    {
        private string connectionString;
        private IDbConnection connection;

        public DBConnector(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IDbConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public void InsertExecutionResult(string appName, string testName, string duration, string startTime, string endTime, string outcome)
        {
            string query = "INSERT INTO [ExecutionResults] ([AppName], [TestName], [Duration], [StartTime], [EndTime], [Outcome]) " +
                           "VALUES (@AppName, @TestName, @Duration, @StartTime, @EndTime, @Outcome)";
            try
            {
                connection = GetConnection();
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;

                    // Add parameters
                    IDbDataParameter appNameParam = command.CreateParameter();
                    appNameParam.ParameterName = "@AppName";
                    appNameParam.Value = appName;
                    command.Parameters.Add(appNameParam);

                    IDbDataParameter testNameParam = command.CreateParameter();
                    testNameParam.ParameterName = "@TestName";
                    testNameParam.Value = testName;
                    command.Parameters.Add(testNameParam);

                    IDbDataParameter durationParam = command.CreateParameter();
                    durationParam.ParameterName = "@Duration";
                    durationParam.Value = duration;
                    command.Parameters.Add(durationParam);

                    IDbDataParameter startTimeParam = command.CreateParameter();
                    startTimeParam.ParameterName = "@StartTime";
                    startTimeParam.Value = startTime;
                    command.Parameters.Add(startTimeParam);

                    IDbDataParameter endTimeParam = command.CreateParameter();
                    endTimeParam.ParameterName = "@EndTime";
                    endTimeParam.Value = endTime;
                    command.Parameters.Add(endTimeParam);

                    IDbDataParameter outcomeParam = command.CreateParameter();
                    outcomeParam.ParameterName = "@Outcome";
                    outcomeParam.Value = outcome;
                    command.Parameters.Add(outcomeParam);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex) { Console.WriteLine("Error writing results to DB: "+ex.ToString()); }
        }
    }
}
