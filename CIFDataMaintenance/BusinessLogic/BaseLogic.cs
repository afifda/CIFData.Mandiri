using CIFDataMaintenance.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using UtilityLibrary;

namespace CIFDataMaintenance.BusinessLogic
{
    class BaseLogic
    {
        public virtual int Save<T>(T entity)
        {
            return new BaseDataAccess().SaveEntity<T>(entity);
        }

        public virtual int SaveBulk<T>(List<T> entityList)
        {
            return new BaseDataAccess().SaveBulkEntity<T>(entityList);
        }

        public virtual int Update<T>(T entity)
        {
            return new BaseDataAccess().UpdateEntity<T>(entity);
        }

        public virtual int Delete<T>(T entity)
        {
            return new BaseDataAccess().DeleteEntity<T>(entity);
        }
        public virtual int Delete<T>(string key)
        {
            return new BaseDataAccess().DeleteEntity<T>(key);
        }

        public virtual List<T> Read<T>(T entity) where T : new()
        {
            return new BaseDataAccess().ReadEntity<T>(entity);
        }

        public virtual DataTable Read<T>(string key, string condition = "") where T : new()
        {
            return new BaseDataAccess().ReadEntityToDataTable<T>(key, condition);
        }

        public T ReadScalar<T>(string sql)
        {
            return new BaseDataAccess().ReadScalar<T>(sql);
        }

        public int DeleteTable(string table)
        {
            return new BaseDataAccess().DeleteTable(table);
        }

        public virtual T1 ReadWithDetails<T1, T2>(T1 entity)
            where T1 : new()
            where T2 : new()
        {
            return new BaseDataAccess().ReadWithDetailsEntity<T1, T2>(entity);
        }

        public void SetReadOption(bool isReadAll, bool filteredByPK = true)
        {
            new BaseDataAccess().ReadOption(isReadAll, filteredByPK);
        }

        public DataTable LoadExcelFile<T>(string FilePath, string Extension) where T : new()
        {
            BaseDataAccess dataAccess = null;
            switch (Extension)
            {
                case ".xls": //Excel 97-03
                    dataAccess = new BaseDataAccess(ConnectionStringOptions.Excel03ConString);
                    break;

                case ".xlsx": //Excel 07
                    dataAccess = new BaseDataAccess(ConnectionStringOptions.Excel07ConString);
                    break;
            }
            string conStr = String.Format(dataAccess.Connection.ConnectionString, FilePath, "YES");
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            DataTable dt = new DataTable();
            cmdExcel.Connection = connExcel;

            //Get the name of First Sheet
            connExcel.Open();
            DataTable dtExcelSchema;

            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            //string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
            connExcel.Close();

            //Read Data from First Sheet

            connExcel.Open();
            T entity = new T();
            SqlCommand commandSql = dataAccess.Read<T>(entity);
            cmdExcel.CommandText = commandSql.CommandText;//"SELECT a.Request_No, a.Category, a.Sub_Category From [Advance_Payment_Info$] a"; 
            oda.SelectCommand = cmdExcel;
            oda.Fill(dt);
            connExcel.Close();
            //DataTable result = dt.Clone();
            //SetDataTableColumnDataType<T>(result);
            //result.Load(dt.CreateDataReader());

            return dt;
        }

        protected void SetDataTableColumnDataType<T>(DataTable dt)
        {
            System.Reflection.PropertyInfo[] _props = typeof(T).GetProperties();

            foreach (System.Reflection.PropertyInfo propInfo in _props)
            {
                var attributes = propInfo.GetCustomAttributes(false);
                var columnMapping = attributes.FirstOrDefault(a => a.GetType() == typeof(ColumnAttribute));
                if (columnMapping != null)
                {
                    ColumnAttribute mapsto = columnMapping as ColumnAttribute;
                    if (!mapsto.ToBeRead) continue;
                    string colIndex = mapsto.UseAlias ? propInfo.Name : mapsto.ColumnName;
                    dt.Columns[colIndex].DataType = propInfo.PropertyType;
                }
            }
        }

        public void BulkCopy<T>(DataTable dt, string tableName, int batch = 10, string log = "", bool isMapWithProperties = true)
        {
            BaseDataAccess dataAccess = new BaseDataAccess();
            dataAccess.SetTransactionOperation(true);
            dataAccess.Connection.OpenConnection();
            dataAccess.Connection.BeginTransaction();
            dt.TableName = tableName;
            bool isValidperation = true;

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(dataAccess.Connection.DBConnection, SqlBulkCopyOptions.CheckConstraints, dataAccess.Connection.DBTransaction))
            {
                bulkCopy.DestinationTableName = tableName;
                bulkCopy.BatchSize = batch;

                // Add your column mappings here
                if (isMapWithProperties)
                {
                    System.Reflection.PropertyInfo[] _props = typeof(T).GetProperties();
                    foreach (System.Reflection.PropertyInfo propInfo in _props)
                    {
                        if (dt.Columns.IndexOf(propInfo.Name) > -1)
                            bulkCopy.ColumnMappings.Add(propInfo.Name, propInfo.Name);
                    }
                }
                try
                {
                    bulkCopy.WriteToServer(dt);
                    LogMessageToFile(dt.Rows.Count.ToString() + " row(s) have been successsfully migrated to " + tableName + ".", log);
                }
                catch (Exception ex)
                {
                    LogMessageToFile("Migration to " + tableName + " failed. Please delete the row(s) inside the table and try again.", log);
                    isValidperation = false;
                    throw ex;
                }
                finally
                {
                    if (isValidperation)
                    {
                        dataAccess.Connection.CommitTransaction();
                    }
                    else
                    {
                        dataAccess.Connection.RollbackTransaction();
                    }
                    dataAccess.Connection.CloseConnection();
                }
            }

        }

        public DataSet BulkCopyToStagingTable<T>(string file, string ext, string stagingTable, string log = "") where T : new()
        {
            DataSet ds = new DataSet();
            DataTable dt = LoadExcelFile<T>(file, ext);
            dt.TableName = stagingTable;
            DeleteTable(stagingTable);
            BulkCopy<T>(dt, stagingTable, 100, log);
            ds.Tables.Add(dt);
            return ds;
        }

        public void LogMessageToFile(string msg, string logPath)
        {
            using (System.IO.FileStream fs = new System.IO.FileStream(logPath, System.IO.FileMode.Append))
            {
                string logLine = System.String.Format("[{0:G}]{2}{1}.{3}", System.DateTime.Now, msg, "\t", Environment.NewLine);
                System.IO.StreamWriter sw = new System.IO.StreamWriter(fs);
                sw.WriteLine(logLine);

                sw.Flush();
                sw.Close();
            }

        }

        public DataTable CreateDataTableFromEntity<T>()
        {
            DataTable dt = new DataTable();
            Type type = typeof(T);
            object[] attributes = type.GetCustomAttributes(typeof(TableAttribute), true);
            var tabAttribute = attributes.FirstOrDefault(a => a.GetType() == typeof(TableAttribute));
            if (tabAttribute != null)
            {
                TableAttribute mapsto = tabAttribute as TableAttribute;
                dt.TableName = mapsto.Name;
            }
            System.Reflection.PropertyInfo[] _props = type.GetProperties();

            foreach (System.Reflection.PropertyInfo propInfo in _props)
            {
                dt.Columns.Add(propInfo.Name);
            }
            return dt;
        }

    }
}
