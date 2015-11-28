using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using UtilityLibrary;

namespace CIFDataService.DataAccess
{
    class BaseDataAccess : DoubleASqlTextCommand
    {
        public DoubleASqlConnection Connection { get; set; }
        bool IsTransOperation;

        public void SetTransactionOperation(bool isTransaction = true)
        {
            this.IsTransOperation = isTransaction;
        }
        public BaseDataAccess()
        {
            Connection = new DoubleASqlConnection();
            Connection.ConnectionString = ConfigurationManager.ConnectionStrings[ConnectionStringOptions.SMLConnection.ToString()].ConnectionString;
        }
        public BaseDataAccess(ConnectionStringOptions connectionString)
        {
            Connection = new DoubleASqlConnection();
            Connection.ConnectionString = ConfigurationManager.ConnectionStrings[connectionString.ToString()].ConnectionString;
        }

        public virtual int SaveEntity<T>(T entity)
        {
            int rowsAffected = 0;
            try
            {
                SqlCommand command = base.Save<T>(entity);
                OpenConnection(command, this.Connection);
                rowsAffected = command.ExecuteNonQuery();
                CloseCommitConnection();
            }
            catch (Exception ex)
            {
                CloseRollbackConnection();
                throw ex;
            }
            return rowsAffected;
        }

        public virtual int SaveBulkEntity<T>(List<T> entityList)
        {
            int rowsAffected = 0;
            try
            {
                SqlCommand command = base.SaveBulk<T>(entityList);
                OpenConnection(command, this.Connection);
                rowsAffected = command.ExecuteNonQuery();
                CloseCommitConnection();
            }
            catch (Exception ex)
            {
                CloseRollbackConnection();
                throw ex;
            }
            return rowsAffected;
        }

        public virtual int UpdateEntity<T>(T entity)
        {
            int rowsAffected = 0;
            try
            {
                SqlCommand command = base.Update<T>(entity);
                OpenConnection(command, this.Connection);
                rowsAffected = command.ExecuteNonQuery();
                CloseCommitConnection();
            }
            catch (Exception ex)
            {
                CloseRollbackConnection();
                throw ex;
            }
            return rowsAffected;
        }

        public virtual int DeleteEntity<T>(T entity)
        {
            int rowsAffected = 0;
            try
            {
                SqlCommand command = base.Delete<T>(entity);
                OpenConnection(command, this.Connection);
                rowsAffected = command.ExecuteNonQuery();
                CloseCommitConnection();
            }
            catch (Exception ex)
            {
                CloseRollbackConnection();
                throw ex;
            }
            return rowsAffected;
        }

        public virtual List<T> ReadEntity<T>(T entity) where T : new()
        {
            DoubleASqlMapper mapData = new DoubleASqlMapper();
            List<T> entityList = null;
            IDataReader reader = null;
            try
            {
                SqlCommand command = base.Read<T>(entity);
                OpenConnection(command, this.Connection);
                reader = command.ExecuteReader();
                entityList = mapData.MapDataToListWithAttributes<T>(reader);
                CloseCommitConnection();
            }
            catch (Exception ex)
            {
                CloseRollbackConnection();
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return entityList;
        }

        public virtual List<T> ReadEntity<T>(string key) where T : new()
        {
            DoubleASqlMapper mapData = new DoubleASqlMapper();
            List<T> entityList = null;
            IDataReader reader = null;
            try
            {
                SqlCommand command = base.Read<T>(key);
                OpenConnection(command, this.Connection);
                reader = command.ExecuteReader();
                entityList = mapData.MapDataToListWithAttributes<T>(reader);
                CloseCommitConnection();
            }
            catch (Exception ex)
            {
                CloseRollbackConnection();
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return entityList;
        }

        public virtual int DeleteEntity<T>(string key)
        {
            int rowsAffected = 0;
            try
            {
                SqlCommand command = base.Delete<T>(key);
                OpenConnection(command, this.Connection);
                rowsAffected = command.ExecuteNonQuery();
                CloseCommitConnection();
            }
            catch (Exception ex)
            {
                CloseRollbackConnection();
                throw ex;
            }
            return rowsAffected;
        }

        public T ReadScalar<T>(string sql)
        {
            T data;
            try
            {
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = sql;
                OpenConnection(command, this.Connection);
                data = (T)command.ExecuteScalar();
                CloseCommitConnection();
            }
            catch (Exception ex)
            {
                CloseRollbackConnection();
                throw ex;
            }
            finally
            {

            }
            return data;
        }

        public int DeleteTable(string table)
        {
            int data;
            string sql = string.Format("DELETE {0}", table);
            try
            {
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = sql;
                OpenConnection(command, this.Connection);
                data = command.ExecuteNonQuery();
                CloseCommitConnection();
            }
            catch (Exception ex)
            {
                CloseRollbackConnection();
                throw ex;
            }
            finally
            {

            }
            return data;
        }

        public DataTable ReadEntityToDataTable<T>(string key, string condition = "") where T : new()
        {
            DoubleASqlMapper mapData = new DoubleASqlMapper();
            DataTable dt = new DataTable();
            IDataReader reader = null;
            try
            {
                SqlCommand command = base.Read<T>(key);
                command.CommandText += string.IsNullOrEmpty(condition) ? string.Empty : " " + condition;
                Type type = typeof(T);
                object[] attributes = type.GetCustomAttributes(typeof(TableAttribute), true);
                var tabAttribute = attributes.FirstOrDefault(a => a.GetType() == typeof(TableAttribute));
                if (tabAttribute != null)
                {
                    TableAttribute mapsto = tabAttribute as TableAttribute;
                    dt.TableName = mapsto.Name;
                }
                OpenConnection(command, this.Connection);
                reader = command.ExecuteReader();
                dt.Load(reader);
                CloseCommitConnection();
            }
            catch (Exception ex)
            {
                CloseRollbackConnection();
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return dt;
        }


        public virtual T1 ReadWithDetailsEntity<T1, T2>(T1 entity)
            where T1 : new()
            where T2 : new()
        {
            DoubleASqlMapper mapData = new DoubleASqlMapper();
            T1 outEntity;
            IDataReader reader = null;
            try
            {
                SqlCommand command = base.Read<T1>(entity);
                OpenConnection(command, this.Connection);
                reader = command.ExecuteReader();
                outEntity = mapData.MapDataToEntityWithDetailsWithAttributes<T1, T2>(reader);
                CloseCommitConnection();
            }
            catch (Exception ex)
            {
                CloseRollbackConnection();
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

            }
            return outEntity;
        }

        public void ReadOption(bool isReadAll, bool filteredByPK = true)
        {
            SetReadOption(isReadAll, filteredByPK);
        }

        public void OpenConnection(SqlCommand command, DoubleASqlConnection conn)
        {
            Connection.OpenConnection();
            command.Connection = this.Connection.DBConnection;
            if (IsTransOperation)
            {
                Connection.BeginTransaction();
                command.Transaction = this.Connection.DBTransaction;
            }
        }

        public void CloseCommitConnection()
        {
            if (IsTransOperation)
            {
                Connection.CommitTransaction();
            }
            Connection.CloseConnection();
        }

        public void CloseRollbackConnection()
        {
            if (IsTransOperation)
            {
                Connection.RollbackTransaction();
            }
            Connection.CloseConnection();
        }

        public void CallUpdateList()
        {

        }

        public DataTable ReadEntityWithExternalFileToDataTable<T>(string key, string condition = "", string columnName = "Col1", string columnPath = "colPath") where T : new()
        {
            DoubleASqlMapper mapData = new DoubleASqlMapper();
            DataTable dt = new DataTable();
            IDataReader reader = null;
            try
            {
                SqlCommand command = base.Read<T>(key);
                command.CommandText += string.IsNullOrEmpty(condition) ? string.Empty : " " + condition;
                Type type = typeof(T);
                object[] attributes = type.GetCustomAttributes(typeof(TableAttribute), true);
                var tabAttribute = attributes.FirstOrDefault(a => a.GetType() == typeof(TableAttribute));
                if (tabAttribute != null)
                {
                    TableAttribute mapsto = tabAttribute as TableAttribute;
                    dt.TableName = mapsto.Name;
                }
                OpenConnection(command, this.Connection);
                reader = command.ExecuteReader();
                dt.Load(reader);
                dt.Columns[columnName].ReadOnly = false;
                dt.Columns[columnName].MaxLength = -1;
                foreach (DataRow item in dt.Rows)
                {
                    item[columnName] = ReadExternalFile(item[columnPath].ToString());
                }
                dt.Columns.Remove(columnPath);
                CloseCommitConnection();
            }
            catch (Exception ex)
            {
                CloseRollbackConnection();
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return dt;
        }

        public string ReadExternalFile(string FilePath)
        {
            string contents = string.Empty;
            using (System.IO.FileStream fs = new System.IO.FileStream(FilePath, System.IO.FileMode.Open))
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(fs);
                contents = sr.ReadToEnd();

                sr.Close();
                sr.Dispose();
            }
            return contents;
        }

    }
}
