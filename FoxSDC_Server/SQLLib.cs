using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FoxSDC_Server
{
    public partial class SQLLib : IDisposable
    {
        private SqlConnection Connection = null;
        private SqlTransaction trans = null;
        public bool SEHError = false;
        public bool ConnectionPooling = true;
        private bool SQLTransaction = false;
        public string ApplicationName
        {
            get { return (appname); }
            set
            {
                appname = value.Trim();
            }
        }
        private string appname = "";
        public bool isSQLTransactional
        {
            get
            {
                return (SQLTransaction);
            }
        }

        public void Dispose()
        {
            try
            {
                if (Connection != null)
                {
                    Connection.Close();
                    Connection.Dispose();
                }
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
            }
        }


        public int SqlCommandTimeout = 600;

        public void CloseConnection()
        {
            Debug.WriteLine("SQLCONNET: CLOSE ===============");
            try
            {
                Connection.Close();
            }
            catch
            {
            }
        }

        public SqlConnection connection
        {
            get
            {
                return (Connection);
            }
        }

        public void BeginTransaction()
        {
            trans = Connection.BeginTransaction();
            SQLTransaction = true;
        }

        public void CommitTransaction()
        {
            if (SQLTransaction == true)
                trans.Commit();
            SQLTransaction = false;
        }

        public void RollBackTransaction()
        {
            if (SQLTransaction == true)
                trans.Rollback();
            SQLTransaction = false;
        }


        public bool ConnectLocalDatabaseBlank()
        {
            System.Diagnostics.Debug.WriteLine("SQLCONNECT (LOCAL blank)");
#if !DEBUGDB
            try
            {
#endif
                SqlConnectionStringBuilder conn = new SqlConnectionStringBuilder();
                if (appname != "")
                    conn["Application Name"] = appname;
                conn["Connection Timeout"] = 180;
                conn["Data Source"] = "(LocalDB)\\v11.0";
                conn["Integrated Security"] = "SSPI";
                conn["Pooling"] = ConnectionPooling == true ? "true" : "false";
                Connection = new SqlConnection(conn.ConnectionString);
                Connection.Open();
#if !DEBUGDB
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                if (SEHError == true)
                    throw;
                return (false);
            }
#endif
            return (true);
        }
        public bool ConnectLocalDatabase(string Filename)
        {
            System.Diagnostics.Debug.WriteLine("SQLCONNECT (LOCAL): " + Filename);
#if !DEBUGDB
            try
            {
#endif
                if (Filename == "")
                    throw new Exception("Filename missing");
                SqlConnectionStringBuilder conn = new SqlConnectionStringBuilder();
                if (appname != "")
                    conn["Application Name"] = appname;
                conn["Connection Timeout"] = 180;
                conn["Data Source"] = "(LocalDB)\\v11.0";
                conn["AttachDbFileName"] = Filename;
                conn["Integrated Security"] = "SSPI";
                conn["Pooling"] = ConnectionPooling == true ? "true" : "false";
                Connection = new SqlConnection(conn.ConnectionString);
                Connection.Open();
#if !DEBUGDB
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                if (SEHError == true)
                    throw;
                return (false);
            }
#endif
            return (true);
        }

        public bool ConnectDatabase(string server, string database)
        {
            return (ConnectDatabase(server, database, "", ""));
        }

        public bool ConnectDatabase(string DirectConnectionString)
        {
            System.Diagnostics.Debug.WriteLine("SQLCONNECT: " + DirectConnectionString);
#if !DEBUGDB
            try
            {
#endif
                Connection = new SqlConnection(DirectConnectionString);
                Connection.Open();
#if !DEBUGDB
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                if (SEHError == true)
                    throw;
                return (false);
            }
#endif
            return (true);
        }

        public bool ConnectDatabase(string server, string database, bool UseWinAuth)
        {
            if (UseWinAuth == false)
                return (ConnectDatabase(server, database, "", ""));
            System.Diagnostics.Debug.WriteLine("SQLCONNECT: " + database + "@" + server);
#if !DEBUGDB
            try
            {
#endif
                if (server == "")
                    server = "localhost";
                SqlConnectionStringBuilder conn = new SqlConnectionStringBuilder();
                if (appname != "")
                    conn["Application Name"] = appname;
                conn["Connection Timeout"] = 180;
                conn["Server"] = server;
                conn["Database"] = database;
                conn["Integrated Security"] = "SSPI";
                conn["Max Pool Size"] = 102400;
                conn["Pooling"] = ConnectionPooling == true ? "true" : "false";
                Connection = new SqlConnection(conn.ConnectionString);
                Connection.Open();
#if !DEBUGDB
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                if (SEHError == true)
                    throw ee;
                return (false);
            }
#endif
            return (true);
        }

        public bool ConnectDatabase(string server, string database, string Username, string Password)
        {
            System.Diagnostics.Debug.WriteLine("SQLCONNECT: " + database + "@" + server);
#if !DEBUGDB
            try
            {
#endif
                if (server == "")
                    server = "localhost";
                SqlConnectionStringBuilder conn = new SqlConnectionStringBuilder();
                if (appname != "")
                    conn["Application Name"] = appname;
                conn["Connection Timeout"] = 180;
                conn["Server"] = server;
                conn["Database"] = database;
                conn["Max Pool Size"] = 102400;
                conn["Pooling"] = ConnectionPooling == true ? "true" : "false";
                if (Username.Trim() != "")
                    conn["User ID"] = Username;
                if (Password != "")
                    conn["Password"] = Password;
                Connection = new SqlConnection(conn.ConnectionString);
                Connection.Open();
#if !DEBUGDB
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                if (SEHError == true)
                    throw;
                return (false);
            }
#endif
            return (true);
        }

        public bool ExecSQL(string Query, params SQLParam[] Parameters)
        {
            if (Connection == null)
                return (false);
            System.Diagnostics.Debug.WriteLine("SQL--: " + Query);
#if !DEBUGDB
            try
            {
#endif
                SqlCommand command = new SqlCommand(Query);
                command.CommandTimeout = SqlCommandTimeout;
                command.Connection = Connection;
                command.CommandType = System.Data.CommandType.Text;
                if (SQLTransaction == true)
                    command.Transaction = trans;
                foreach (SQLParam p in Parameters)
                {
                    command.Parameters.Add(new SqlParameter(p.Variable, p.Content));
                }
                command.ExecuteNonQuery();
                return (true);
#if !DEBUGDB
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                if (SEHError == true)
                    throw;
                return (false);
            }
#endif
        }

        public static DateTime GetDTUTC(object dr)
        {
            DateTime dt = Convert.ToDateTime(dr);
            dt = DateTime.SpecifyKind(dt, DateTimeKind.Utc);
            return (dt);
        }

        public int ExecSQLNQ(string Query, params SQLParam[] Parameters)
        {
            if (Connection == null)
                return (0);
            System.Diagnostics.Debug.WriteLine("SQLNQ: " + Query);
#if !DEBUGDB
            try
            {
#endif
                SqlCommand command = new SqlCommand(Query);
                command.CommandTimeout = SqlCommandTimeout;
                command.Connection = Connection;
                command.CommandType = System.Data.CommandType.Text;
                if (SQLTransaction == true)
                    command.Transaction = trans;
                foreach (SQLParam p in Parameters)
                {
                    command.Parameters.Add(new SqlParameter(p.Variable, p.Content));
                }
                return (command.ExecuteNonQuery());
#if !DEBUGDB
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                if (SEHError == true)
                    throw;
                return (0);
            }
#endif
        }

        public object ExecSQLScalar(string Query, params SQLParam[] Parameters)
        {
            if (Connection == null)
                return (null);
            System.Diagnostics.Debug.WriteLine("SQLSC: " + Query);
#if !DEBUGDB
            try
            {
#endif
                SqlCommand command = new SqlCommand(Query);
                command.CommandTimeout = SqlCommandTimeout;
                command.Connection = Connection;
                command.CommandType = System.Data.CommandType.Text;
                if (SQLTransaction == true)
                    command.Transaction = trans;
                foreach (SQLParam p in Parameters)
                {
                    command.Parameters.Add(new SqlParameter(p.Variable, p.Content));
                }
                return (command.ExecuteScalar());
#if !DEBUGDB
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                if (SEHError == true)
                    throw;
                return (null);
            }
#endif
        }

        public SqlDataReader ExecSQLReader(string Query, params SQLParam[] Parameters)
        {
            if (Connection == null)
                return (null);
            System.Diagnostics.Debug.WriteLine("SQLDR: " + Query);
#if !DEBUGDB
            try
            {
#endif
                SqlCommand command = new SqlCommand(Query);
                command.CommandTimeout = SqlCommandTimeout;
                command.Connection = Connection;
                command.CommandType = System.Data.CommandType.Text;
                if (SQLTransaction == true)
                    command.Transaction = trans;
                foreach (SQLParam p in Parameters)
                {
                    command.Parameters.Add(new SqlParameter(p.Variable, p.Content));
                }
                return (command.ExecuteReader());
#if !DEBUGDB
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                if (SEHError == true)
                    throw;
                return (null);
            }
#endif
        }

        public DataSet ExecSQLDataSet(string Query, params SQLParam[] Parameters)
        {
            if (Connection == null)
                return (null);
            System.Diagnostics.Debug.WriteLine("SQLDS: " + Query);
#if !DEBUGDB
            try
            {
#endif
                DataSet ds = new DataSet();
                SqlDataAdapter ad = new SqlDataAdapter();
                SqlCommand command = new SqlCommand(Query);
                command.CommandTimeout = SqlCommandTimeout;
                command.Connection = Connection;
                command.CommandType = System.Data.CommandType.Text;
                if (SQLTransaction == true)
                    command.Transaction = trans;
                foreach (SQLParam p in Parameters)
                {
                    command.Parameters.Add(new SqlParameter(p.Variable, p.Content));
                }
                ad.SelectCommand = command;
                ad.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                ad.Fill(ds);
                return (ds);
#if !DEBUGDB
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                if (SEHError == true)
                    throw;
                return (null);
            }
#endif
        }

        public DataTable ExecSQLDataTable(string Table, params SQLParam[] Parameters)
        {
            if (Connection == null)
                return (null);
#if !DEBUGDB
            try
            {
#endif
                SqlDataAdapter ad = new SqlDataAdapter();
                SqlCommand command = new SqlCommand("SELECT * FROM " + Table);
                command.CommandTimeout = SqlCommandTimeout;
                command.Connection = Connection;
                command.CommandType = System.Data.CommandType.Text;
                if (SQLTransaction == true)
                    command.Transaction = trans;
                foreach (SQLParam p in Parameters)
                {
                    command.Parameters.Add(new SqlParameter(p.Variable, p.Content));
                }
                DataTable dt = new DataTable(Table);
                ad.SelectCommand = command;
                ad.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                ad.Fill(dt);
                return (dt);
#if !DEBUGDB
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                if (SEHError == true)
                    throw;
                return (null);
            }
#endif
        }

        public bool ExistTable(string database, string Table)
        {
            if (Connection == null)
                return (false);
#if !DEBUGDB
            if (SQLTransaction == true)
                throw new Exception("Cannot execute in transaction-mode");
            try
            {
#endif
                object result = ExecSQLScalar("IF EXISTS(SELECT name FROM " + database + ".dbo.sysobjects WHERE id = OBJECT_ID(@table)) SELECT 1 AS Result ELSE SELECT 0 AS Result",
                    new SQLParam("@table", Table));
                if (Convert.ToInt32(result) == 1)
                    return (true);
                else
                    return (false);
#if !DEBUGDB
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                if (SEHError == true)
                    throw;
                return (false);
            }
#endif
        }

        public bool ExistDatabase(string Database)
        {
            if (Connection == null)
                return (false);
#if !DEBUGDB
            if (SQLTransaction == true)
                throw new Exception("Cannot execute in transaction-mode");
            try
            {
#endif
                object result = ExecSQLScalar("IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = @database) SELECT 1 AS Result ELSE SELECT 0 AS Result",
                    new SQLParam("@database", Database));
                if (Convert.ToInt32(result) == 1)
                    return (true);
                else
                    return (false);
#if !DEBUGDB
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                if (SEHError == true)
                    throw;
                return (false);
            }
#endif
        }

        public bool InsertMultiData(string Table, params SQLData[] data)
        {
            List<SQLData> l = new List<SQLData>();
            foreach (SQLData p in data)
                l.Add(p);
            return (InsertMultiData(Table, l));
        }

        public bool InsertMultiData(string Table, List<SQLData> data)
        {
            if (connection == null)
                return (false);
            if (data.Count == 0)
            {
                if (SEHError == true)
                    throw new Exception("data.Count is zero!");
                return (false);
            }
            int Count = 1;
            string Query = "INSERT INTO " + (Table.EndsWith("]") == true && Table.StartsWith("[") == true ? Table : "[" + Table + "]") + " (";
            string Variables = "";
            foreach (SQLData d in data)
            {
                Query += "[" + d.Column + "],";
                d.VariableName = "@variable" + Count.ToString();
                d.Count = Count;
                Variables += d.VariableName + ",";
                Count++;
            }

            if (Query.EndsWith(",") == true)
                Query = Query.Substring(0, Query.Length - 1);
            if (Variables.EndsWith(",") == true)
                Variables = Variables.Substring(0, Variables.Length - 1);

            Query += ") VALUES (" + Variables + ")";

#if !DEBUGDB
            try
            {
#endif
                SqlCommand command = new SqlCommand(Query);
                command.CommandTimeout = SqlCommandTimeout;
                command.Connection = Connection;
                command.CommandType = System.Data.CommandType.Text;
                Debug.WriteLine("SQLIM: " + Query);
                if (SQLTransaction == true)
                    command.Transaction = trans;
                foreach (SQLData d in data)
                {
                    command.Parameters.Add(new SqlParameter(d.VariableName, d.Data));
                }
                command.ExecuteNonQuery();
                return (true);
#if !DEBUGDB
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                if (SEHError == true)
                    throw;
                return (false);
            }
#endif
        }

        public Int64? InsertMultiDataID(string Table, params SQLData[] data)
        {
            List<SQLData> l = new List<SQLData>();
            foreach (SQLData p in data)
                l.Add(p);
            return (InsertMultiDataID(Table, l));
        }

        public Int64? InsertMultiDataID(string Table, List<SQLData> data)
        {
            if (connection == null)
                return (null);
            if (data.Count == 0)
            {
                if (SEHError == true)
                    throw new Exception("data.Count is zero!");
                return (null);
            }
            int Count = 1;
            string Query = "DECLARE @tabl table(ID bigint); ";
            Query += "INSERT INTO " + (Table.EndsWith("]") == true && Table.StartsWith("[") == true ? Table : "[" + Table + "]") + " (";
            string Variables = "";
            foreach (SQLData d in data)
            {
                Query += "[" + d.Column + "],";
                d.VariableName = "@variable" + Count.ToString();
                d.Count = Count;
                Variables += d.VariableName + ",";
                Count++;
            }

            if (Query.EndsWith(",") == true)
                Query = Query.Substring(0, Query.Length - 1);
            if (Variables.EndsWith(",") == true)
                Variables = Variables.Substring(0, Variables.Length - 1);

            Query += ") OUTPUT Inserted.ID INTO @tabl VALUES (" + Variables + "); SELECT * FROM @tabl";

#if !DEBUGDB
            try
            {
#endif
                SqlCommand command = new SqlCommand(Query);
                command.CommandTimeout = SqlCommandTimeout;
                command.Connection = Connection;
                command.CommandType = System.Data.CommandType.Text;
                Debug.WriteLine("SQLIMID: " + Query);
                if (SQLTransaction == true)
                    command.Transaction = trans;
                foreach (SQLData d in data)
                {
                    command.Parameters.Add(new SqlParameter(d.VariableName, d.Data));
                }
                return (Convert.ToInt64(command.ExecuteScalar()));
#if !DEBUGDB
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                if (SEHError == true)
                    throw;
                return (null);
            }
#endif
        }

        public bool ExecSQLSP(string Query, params SQLParam[] Parameters)
        {
            if (Connection == null)
                return (false);
#if !DEBUGDB
            try
            {
#endif
                SqlCommand command = new SqlCommand(Query);
                command.CommandTimeout = SqlCommandTimeout;
                command.Connection = Connection;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                if (SQLTransaction == true)
                    command.Transaction = trans;
                foreach (SQLParam p in Parameters)
                {
                    command.Parameters.Add(new SqlParameter(p.Variable, p.Content));
                }
                command.ExecuteNonQuery();
                return (true);
#if !DEBUGDB
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                if (SEHError == true)
                    throw;
                return (false);
            }
#endif
        }

        public bool LoadIntoClass(SqlDataReader dr, object obj)
        {
            FieldInfo[] fields = obj.GetType().GetFields();
            try
            {
                foreach (FieldInfo f in fields)
                {
                    string column = f.Name;
                    IEnumerable<Attribute> attrs = f.GetCustomAttributes();
                    foreach (Attribute a in attrs)
                    {
                        if (a is SQLLibColumn)
                        {
                            SQLLibColumn cl = (SQLLibColumn)a;
                            column = cl.col;
                            break;
                        }
                    }

                    if (column.Trim() == "")
                        continue;

                    bool found = false;
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        if (column.ToLower() == dr.GetName(i).ToLower())
                        {
                            found = true;
                            break;
                        }
                    }
                    if (found == false)
                        continue;

                    if (dr[column] is DBNull)
                    {
                        f.SetValue(obj, null);
                    }
                    else
                    {
                        if (f.FieldType.IsGenericType && f.FieldType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            f.SetValue(obj, Convert.ChangeType(dr[column], f.FieldType.GetGenericArguments()[0]));
                        }
                        else
                        {
                            if (f.FieldType.IsEnum == true)
                                f.SetValue(obj, Convert.ChangeType(dr[column], f.FieldType.GetEnumUnderlyingType()));
                            else
                                f.SetValue(obj, Convert.ChangeType(dr[column], f.FieldType));
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                if (SEHError == true)
                    throw;
                return (false);
            }
            return (true);
        }

        public List<SQLData> InsertFromClassPrep(object obj)
        {
            List<SQLData> Data = new List<SQLData>();
            FieldInfo[] fields = obj.GetType().GetFields();
            try
            {
                foreach (FieldInfo f in fields)
                {
                    string column = f.Name;
                    IEnumerable<Attribute> attrs = f.GetCustomAttributes();
                    foreach (Attribute a in attrs)
                    {
                        if (a is SQLLibColumn)
                        {
                            SQLLibColumn cl = (SQLLibColumn)a;
                            column = cl.col;
                            break;
                        }
                    }

                    if (column.Trim() == "")
                        continue;

                    SQLData sqldata = new SQLData(column, f.GetValue(obj));
                    sqldata.DataType = f.FieldType;
                    Data.Add(sqldata);
                }
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                if (SEHError == true)
                    throw;
                return (null);
            }

            return (Data);
        }

        public bool InsertFromClass(string Table, object obj)
        {
            List<SQLData> Data = InsertFromClassPrep(obj);
            if (Data == null)
                return (false);
            return (InsertMultiData(Table, Data));
        }

        public string GetConstraint(string Table, string Column)
        {
            return (Convert.ToString(this.ExecSQLScalar(@"SELECT df.name as CN, t.name as TN, c.NAME as CN
                FROM sys.default_constraints as df
                INNER JOIN sys.tables as t ON df.parent_object_id = t.object_id
                INNER JOIN sys.columns as c ON df.parent_object_id = c.object_id AND df.parent_column_id = c.column_id
                where t.name=@table and c.name=@column",
                    new SQLParam("@table", Table),
                    new SQLParam("@column", Column))));
        }

        public bool BulkInsertMultiData(string Table, List<List<SQLData>> datas)
        {
            try
            {
                if (SQLTransaction == true)
                    throw new Exception("Cannot run in Transaction mode");

                if (datas.Count == 0)
                    return (true);

                DataTable table = new DataTable(Table);

                foreach (SQLData d in datas[0])
                {
                    DataColumn col = new DataColumn();
                    col.ColumnName = d.Column;

                    if (d.DataType.IsGenericType && d.DataType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        col.DataType = d.DataType.GetGenericArguments()[0];
                    else
                        col.DataType = d.DataType;

                    table.Columns.Add(col);
                }

                foreach (List<SQLData> data in datas)
                {
                    DataRow row = table.NewRow();

                    foreach (SQLData d in data)
                    {
                        row[d.Column] = d.Data == null ? (object)DBNull.Value : d.Data;
                    }

                    table.Rows.Add(row);
                }
                table.AcceptChanges();

                SqlBulkCopy bulk = new SqlBulkCopy(connection);
                bulk.DestinationTableName = Table;
                bulk.BulkCopyTimeout = SqlCommandTimeout;
                Debug.WriteLine("SQLBULKI: " + Table);
                bulk.WriteToServer(table);
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                if (SEHError == true)
                    throw;
                return (false);
            }

            return (true);
        }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class SQLLibColumn : Attribute
    {
        public string col;
        public SQLLibColumn(string Column)
        {
            col = Column;
        }
    }

    public class SQLParam
    {
        public string Variable;
        public object Content;

        public SQLParam(string Var, object Cont)
        {
            this.Variable = Var;
            this.Content = Cont == null ? DBNull.Value : Cont;
        }
    }

    public class SQLData
    {
        public string Column;
        public object Data;

        public string VariableName;
        public int Count;

        public Type DataType;

        public SQLData(string Column, object data)
        {
            this.Column = Column;
            this.Data = data == null ? DBNull.Value : data;
        }
    }
}
