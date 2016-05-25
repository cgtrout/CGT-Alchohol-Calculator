using System;
using System.Data.Common;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace BloodAlcoholCalculator.Data
{
     /// <summary>
     /// Base database - other databases extend off this
     /// </summary>
     public abstract class Database : IDisposable
     {
          protected SQLiteConnection connection;
          protected DataContext context;

          //string of last sql command - for debugging
          public string LastCommandText { get; set; }

          /// <summary>
          /// Connect to data
          /// </summary>
          /// <param name="connectionString">Connection string containing sqlite parameters</param>
          protected void Connect(string connectionString)
          {
               connection = new SQLiteConnection(connectionString);
               connection.Open();
          }

          /// <summary>
          /// Close the connection
          /// </summary>
          public void Close()
          {
               connection.Close();
          }

          /// <summary>
          /// Creates all tables for application
          /// </summary>
          public abstract void CreateTables();

          /// <summary>
          /// Initialize context for subclass
          /// </summary>
          public abstract void InitializeContext();

          /// <summary>
          /// Route sqlite logs to debug output
          /// </summary>
          public void SetDebugText()
          {
#if DEBUG
               context.Log = new DebugTextWriter();
#endif
          }

          protected void SetLastCommandText<T>(IQueryable<T> e)
          {
               DbCommand dc = context.GetCommand(e);
               LastCommandText = dc.CommandText;
          }

          /// <summary>
          /// Create Tables from Defined attributes
          /// </summary>
          /// <param name="classPropertyInfo">PropertyInfo array of defined type</param>
          public void CreateTable(PropertyInfo[] classPropertyInfo, string tableName)
          {
               string sql = String.Format("create table if not exists {0} (", tableName);

               //use defined attributes to create table structure
               foreach (var p in classPropertyInfo) {
                    var attributes = p.GetCustomAttributes(true);
                    ColumnAttribute columnAttribute = null;
                    SqliteAttribute sqliteAttribute = null;
                    foreach (object attribute in attributes) {
                         var columnAttributeTest = attribute as ColumnAttribute;
                         var sqliteAttributeTest = attribute as SqliteAttribute;
                         if (columnAttributeTest != null) {
                              columnAttribute = columnAttributeTest;
                         } else if (sqliteAttributeTest != null) {
                              sqliteAttribute = sqliteAttributeTest;
                         }
                    }
                    if (columnAttribute != null && sqliteAttribute != null) {
                         sql += columnAttribute.Name + " " + sqliteAttribute.DataType;

                         if (sqliteAttribute.IsPrimaryKey) {
                              sql += " " + "PRIMARY KEY";
                         }

                         if (sqliteAttribute.IsAutoIncrement) {
                              sql += " " + "AUTOINCREMENT";
                         }

                         if (sqliteAttribute.IsUnique) {
                              sql += " " + "UNIQUE";
                         }

                         sql += ",";
                    }
               }

               //remove final comma
               sql = sql.Substring(0, sql.Length - 1);

               sql += ")";

               try {
                    var command = new SQLiteCommand(sql, connection);
                    command.ExecuteNonQuery();
               }
               catch (SQLiteException e) {
                    Debug.Write("Error writing table in CreateTable: " + e.Message);
               }
          }

          /// <summary>
          /// Deletes all entries from a table
          /// </summary>
          /// <param name="tablename">Table to clear</param>
          public void ClearTable(string tablename)
          {
               ExecuteSQL(String.Format("delete from {0}", tablename));
          }

          /// <summary>
          /// Runs sql string
          /// </summary>
          /// <param name="sql">sql string to execute</param>
          private void ExecuteSQL(string sql)
          {
               var command = new SQLiteCommand(sql, connection);
               command.ExecuteNonQuery();
          }

          public void Dispose()
          {
               Dispose(true);
               GC.SuppressFinalize(this);
          }

          private bool disposed = false;

          protected virtual void Dispose(bool disposing)
          {
               if (disposed) return;

               if (disposing) {
               }
               //if(connection.State==
               connection.Close();
               disposed = true;
          }
     }

     internal class DebugTextWriter : System.IO.TextWriter
     {
          public override void Write(char[] buffer, int index, int count)
          {
               System.Diagnostics.Debug.Write(new string(buffer, index, count));
          }

          public override void Write(string value)
          {
               System.Diagnostics.Debug.Write(value);
          }

          public override System.Text.Encoding Encoding
          {
               get {
                    return System.Text.Encoding.Default;
               }
          }
     }
}