using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace FluentBuild.Database
{
    public class MsSqlDatabase
    {
        private readonly SqlConnectionStringBuilder _connectionString = new SqlConnectionStringBuilder();
        SqlConnectionStringBuilder _masterDatabaseConnectionString;
            
        private string _createScriptPath;
        private string _upgradScriptPath;
        private string _versionTableName = "Version";

        public static MsSqlDatabase CreateOrUpgradeDatabase()
        {
            return new MsSqlDatabase();
        }

        public MsSqlDatabase CreateScript(string path)
        {
            _createScriptPath = path;
            return this;
        }

        public MsSqlDatabase UpdateScripts(string path)
        {
            _upgradScriptPath = path;
            return this;
        }

        public MsSqlDatabase ConnectionString(string connectionString)
        {
            _connectionString.Clear();
            _connectionString.ConnectionString = connectionString;
            _masterDatabaseConnectionString = new SqlConnectionStringBuilder(_connectionString.ConnectionString);
            _masterDatabaseConnectionString.InitialCatalog = "master";
            return this;
        }

        public MsSqlDatabase VersionTable(string tableName)
        {
            _versionTableName = tableName;
            return this;
        }


        public bool DoesDatabaseAlreadyExist()
        {
            //create a copy and set the initial DB to be master so we can check if the DB exists
            using (IDbConnection con = new SqlConnection(_masterDatabaseConnectionString.ConnectionString))
            {
                var parameters = new NameValueCollection();
                parameters.Add("databaseName", _connectionString.InitialCatalog);
                using (IDbCommand command = CreateTextCommand(con, "SELECT Count(*) FROM master.sys.databases WHERE [name]=@databaseName", parameters))
                {
                    command.Connection.Open();
                    if ((int) command.ExecuteScalar() == 0)
                        return false;
                    return true;
                }
            }
        }

        private void ExecuteNonQueryCommandAgainstDatabase(SqlConnectionStringBuilder connection, string commandText, NameValueCollection parameters)
        {
            using (IDbConnection con = new SqlConnection(connection.ConnectionString))
            {
                con.Open();
                string separator = "GO" + Environment.NewLine;
                string[] commands = commandText.Split(new[] {separator}, StringSplitOptions.RemoveEmptyEntries);
               
                if (commands.Length == 0) //it is only one statement that does not have a "GO" at the end
                    commands = new[] {commandText};

                //execute each command in the array
                foreach (var individualCommand in commands)
                {
                    using (IDbCommand command = CreateTextCommand(con, individualCommand, parameters))
                    {
                        MessageLogger.WriteDebugMessage("Executing:" + command.CommandText);
                        command.ExecuteNonQuery();
                    }    
                }
                
            }
        }

        private IDbCommand CreateTextCommand(IDbConnection connection, string commandText, NameValueCollection parameters)
        {
            IDbCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = commandText;
            
            if(parameters == null)
                return command;

            foreach (var key in parameters.Keys)
            {
                IDbDataParameter tableNameParameter = command.CreateParameter();
                tableNameParameter.ParameterName = key.ToString();
                tableNameParameter.Value = parameters[key.ToString()];
                command.Parameters.Add(tableNameParameter);    
            }
            return command;
        }

        public void Execute()
        {
            if (!DoesDatabaseAlreadyExist())
            {
                CreateDatabase();
            }

            UpdateDatabase();
        }

        private void UpdateDatabase()
        {
            MessageLogger.WriteDebugMessage("Executing database updates");
            //determine current version
            int currentVersion=0;
                 using (IDbConnection con = new SqlConnection(_connectionString.ConnectionString))
                 {
                     con.Open();
                     IDbCommand command = CreateTextCommand(con, "select version from " + _versionTableName, null);
                     currentVersion = (int)command.ExecuteScalar();
                 }

            //execute updates higher than that version
            foreach (var upgradeFile in Directory.GetFiles(_upgradScriptPath))
            {
                string fileName = Path.GetFileName(upgradeFile);
                var fileVersion = int.Parse(fileName.Substring(0, fileName.IndexOf("_")));
                if (fileVersion > currentVersion)
                {
                    MessageLogger.WriteDebugMessage("Changes found. Updating to version " + fileVersion);
                    using(var x = new StreamReader(upgradeFile))
                    {
                        ExecuteNonQueryCommandAgainstDatabase(_connectionString, x.ReadToEnd(), null);
                    }

                    ExecuteNonQueryCommandAgainstDatabase(_connectionString, "update " + _versionTableName + " set version=" + fileVersion, null);
                    
                }
            }
            
            
        }

        private void CreateDatabase()
        {
            MessageLogger.WriteDebugMessage("Database does not exist, creating it");
            //create database
            MessageLogger.WriteDebugMessage("creating database " + _connectionString.InitialCatalog);
            ExecuteNonQueryCommandAgainstDatabase(_masterDatabaseConnectionString, "CREATE DATABASE " + _connectionString.InitialCatalog, null);
                
            //execute create script
            MessageLogger.WriteDebugMessage("Executing create script");
            using (var x = new StreamReader(_createScriptPath))
            {
                ExecuteNonQueryCommandAgainstDatabase(_connectionString, x.ReadToEnd(), null);
            }

            //create version table
            MessageLogger.WriteDebugMessage("Creating version table");
            ExecuteNonQueryCommandAgainstDatabase(_connectionString, "create table " + _versionTableName + " (version int)", null);

            //insert version 0
            ExecuteNonQueryCommandAgainstDatabase(_connectionString, "insert into " + _versionTableName + " values (0)", null);
        }
    }
}