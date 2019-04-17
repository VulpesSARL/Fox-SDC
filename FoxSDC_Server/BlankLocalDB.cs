using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server
{
    class BlankLocalDB
    {
        public static void CreateBlankLocalDB()
        {
            if (Settings.Default.DBType.ToLower() == "localdb")
            {
                if (Settings.Default.DBLocalPath == "")
                {
                    Console.WriteLine("LocalDB path missing");
                    return;
                }
                if (File.Exists(Settings.Default.DBLocalPath) == true)
                {
                    Console.WriteLine("Exists already LocalDB");
                    return;
                }
                try
                {
                    string p = Path.GetDirectoryName(Settings.Default.DBLocalPath);
                    Directory.CreateDirectory(p);
                }
                catch
                {

                }
                SQLLib sql = new SQLLib();
                sql.ApplicationName = "Fox SDC Server [new LocalDB]";
                sql.ConnectionPooling = false;
                if (sql.ConnectLocalDatabaseBlank() == false)
                {
                    Console.WriteLine("Cannot start LocalDB");
                    return;
                }

                string LogFile = Path.GetDirectoryName(Settings.Default.DBLocalPath);
                if (LogFile.EndsWith("\\") == false)
                    LogFile += "\\";
                LogFile += Path.GetFileNameWithoutExtension(Settings.Default.DBLocalPath) + ".ldf";

                if (sql.ExecSQL("DECLARE @sql NVARCHAR(2000) SELECT @sql = 'CREATE DATABASE [FoxSDCDBnew] ON PRIMARY (NAME=FoxSDCDBnew_DATA, FILENAME = '+quotename(@fn)+') LOG ON (NAME=FoxSDCDBnew_LOG, FILENAME = '+quotename(@ln)+')' EXEC (@sql)",
                    new SQLParam("@ln", LogFile),
                    new SQLParam("@fn", Settings.Default.DBLocalPath)) == false)
                {
                    sql.CloseConnection();
                    Console.WriteLine("Canoot create DB");
                    return;
                }

                if (sql.ExecSQL("USE [FoxSDCDBnew]") == false)
                {
                    sql.CloseConnection();
                    Console.WriteLine("Canoot switch DB");
                    return;
                }

                sql.ExecSQL(@"CREATE TABLE [dbo].[Config]( 
                        [Key] [nvarchar](100) NOT NULL, 
                        [Value] [nvarchar](1000) NOT NULL, 
                    CONSTRAINT [PK_Config] PRIMARY KEY CLUSTERED  
                    ( 
                       [Key] ASC 
                    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] 
                    ) ON [PRIMARY]");
                sql.ExecSQL(@"INSERT [dbo].[Config] ([Key], [Value]) VALUES (N'ID', N'FOXSDCv1') 
                    INSERT [dbo].[Config] ([Key], [Value]) VALUES (N'Version', N'0')");

                sql.ExecSQL("USE MASTER");

                if (sql.ExecSQLSP("sp_detach_db",
                    new SQLParam("@dbname", "FoxSDCDBnew")) == false)
                {
                    sql.CloseConnection();
                    Console.WriteLine("Canoot detach DB");
                    return;
                }

                sql.CloseConnection();
                Console.WriteLine("Success");
            }
            else
            {
                Console.WriteLine("Not setup for LocalDB usage");
            }
        }
    }
}
