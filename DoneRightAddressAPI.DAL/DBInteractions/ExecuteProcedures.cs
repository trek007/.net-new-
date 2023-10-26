using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoneRightAddressAPI.DAL.DBInteractions
{
    /// <summary>
    /// Class To Execute Procedures With Diffrent Return Type. 
    /// </summary>
    public class ExecuteProcedures
    {
        #region Fields
        SqlDataAdapter sqlDataAdapter = null;
        Connection Connection = null;
        SqlCommand sqlCommand = null;
        #endregion
        #region Methods
        /// <summary>
        /// Login Stored Procedure Called
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameterNames"></param>
        /// <param name="parameterValues"></param>
        public async Task<DataTable> Get_DataTableAsync(string storedProcedureName, string[] parameterNames, string[] parameterValues)
        {
            try
            {
                Task<DataTable> task1 = Task<DataTable>.Factory.StartNew(() =>
                {

                    DataTable dataTable = new DataTable();
                    sqlDataAdapter = new SqlDataAdapter(storedProcedureName, Connection.OpenConnection());
                    sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    if (parameterNames != null && parameterNames.Length > 0)
                    {
                        for (int i = 0; i < parameterNames.Length; i++)
                        {
                            if (parameterValues[i] == null)
                            {
                                sqlDataAdapter.SelectCommand.Parameters.AddWithValue(parameterNames[i], DBNull.Value);
                            }
                            else
                            {
                                sqlDataAdapter.SelectCommand.Parameters.AddWithValue(parameterNames[i], parameterValues[i]);
                            }
                        }
                    }

                    sqlDataAdapter.SelectCommand.CommandTimeout = 300;
                    sqlDataAdapter.Fill(dataTable);
                    sqlDataAdapter.Dispose();
                    Connection.CloseConnection();
                    return dataTable;
                });
                return await task1.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Get Datatable From Stored Procedure.
        /// </summary>
        /// <param name="storedProcedureName">Stored Procedure Name.</param>
        /// <param name="parameterNames">Names of Parameter in String Array.</param>
        /// <param name="parameterValues">Values of Parameter in String Array.</param>
        /// <returns>Returns Datatable.</returns>
        public DataTable ReturnDataTable(string storedProcedureName, string[] parameterNames, string[] parameterValues)
        {
            var Connection = new Connection();
            using (sqlDataAdapter = new SqlDataAdapter(storedProcedureName, Connection.OpenConnection()))
            {
                sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDataAdapter.SelectCommand.CommandTimeout = 300;
                if (parameterNames != null && parameterNames.Length > 0)
                {
                    for (int i = 0; i < parameterNames.Length; i++)
                    {
                        sqlDataAdapter.SelectCommand.Parameters.AddWithValue(parameterNames[i], parameterValues[i]);
                    }
                }
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                Connection.CloseConnection();
                Connection = null;
                sqlDataAdapter.Dispose();
                return dataTable;
            }
        }

        /// <summary>
        /// Execute Procedure With No Return Value. eg: Insert, Update, Delete 
        /// </summary>
        /// <param name="storedProcedureName">Stored Procedure Name.</param>
        /// <param name="parameterNames">Names of Parameter in String Array.</param>
        /// <param name="parameterValues">Values of Parameter in String Array.</param>
        public void ReturnEmpty(string storedProcedureName, string[] parameterNames, string[] parameterValues)
        {
            Connection = new Connection();
            sqlCommand = new SqlCommand(storedProcedureName, Connection.OpenConnection());
            sqlCommand.CommandType = CommandType.StoredProcedure;
            if (parameterNames != null && parameterNames.Length > 0)
            {
                for (int i = 0; i < parameterNames.Length; i++)
                {
                    if (parameterValues[i] == null)
                    {
                        sqlCommand.Parameters.AddWithValue(parameterNames[i], DBNull.Value);
                    }
                    else
                    {
                        sqlCommand.Parameters.AddWithValue(parameterNames[i], parameterValues[i]);
                    }
                }
            }
            sqlCommand.ExecuteNonQuery();
            sqlCommand.Dispose();
            Connection.CloseConnection();
            Connection = null;
        }

        /// <summary>
        /// Get Dataset From Stored Procedure.
        /// </summary>
        /// <param name="storedProcedureName">Stored Procedure Name.</param>
        /// <param name="parameterNames">Names of Parameter in String Array.</param>
        /// <param name="parameterValues">Values of Parameter in String Array.</param>
        /// <returns>Return Dataset.</returns>
        public DataSet ReturnDataSet(string storedProcedureName, string[] parameterNames, string[] parameterValues)
        {
            Connection = new Connection();
            DataSet dataSet = new DataSet();
            using (sqlDataAdapter = new SqlDataAdapter(storedProcedureName, Connection.OpenConnection()))
            {
                sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                if (parameterNames != null && parameterNames.Length > 0)
                {
                    for (int i = 0; i < parameterNames.Length; i++)
                    {
                        sqlDataAdapter.SelectCommand.Parameters.AddWithValue(parameterNames[i], parameterValues[i]);
                    }
                }

                //sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@" + parameterName, parameterValue);
                sqlDataAdapter.Fill(dataSet);
                sqlDataAdapter.Dispose();
                Connection.CloseConnection();
                Connection = null;
                return dataSet;
            }

        }

        public string ReturnStringFromDataSet(string storedProcedureName, string[] parameterNames, string[] parameterValues)
        {
            Connection = new Connection();
            DataSet dataSet = new DataSet();
            using (sqlDataAdapter = new SqlDataAdapter(storedProcedureName, Connection.OpenConnection()))
            {
                sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                if (parameterNames != null && parameterNames.Length > 0)
                {
                    for (int i = 0; i < parameterNames.Length; i++)
                    {
                        sqlDataAdapter.SelectCommand.Parameters.AddWithValue(parameterNames[i], parameterValues[i]);
                    }
                }

                //sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@" + parameterName, parameterValue);
                sqlDataAdapter.Fill(dataSet);
                sqlDataAdapter.Dispose();
                Connection.CloseConnection();
                Connection = null;
                return dataSet.ToString();
            }

        }

        public object Return_SingleValue_in_Object(string storedProcedureName, string[] parameterNames, string[] parameterValues)
        {
            Connection = new Connection();
            object scalarValue = "";
            sqlCommand = new SqlCommand(storedProcedureName, Connection.OpenConnection());
            sqlCommand.CommandType = CommandType.StoredProcedure;
            if (parameterNames != null && parameterNames.Length > 0)
            {
                for (int i = 0; i < parameterNames.Length; i++)
                {
                    sqlCommand.Parameters.AddWithValue(parameterNames[i], parameterValues[i]);
                }
            }
            scalarValue = sqlCommand.ExecuteScalar();
            sqlCommand.Dispose();
            Connection.CloseConnection();
            Connection = null;
            return scalarValue;
        }

        public DataTable Get_DataFromDatabaseInDT(string sql)
        {
            SqlDataAdapter adp = new SqlDataAdapter(sql, Connection.OpenConnection());
            DataSet ds = new System.Data.DataSet();
            adp.Fill(ds);
            DataTable dt = new System.Data.DataTable();
            dt = ds.Tables[0];
            return dt;
        }
        #endregion
    }
}
