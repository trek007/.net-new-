using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DoneRightAddressAPI.DAL.Data_Models
{
    public class ClsUsers
    {
        public static IConfigurationRoot Configuration { get; set; }
        public static string GetDBConnection()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true)
               .AddEnvironmentVariables();
            Configuration = builder.Build();
            return Configuration.GetSection("ConnectionString").Value;
        }
        public DataSet AddressMatcher_Suffixs(string suffix)
        {

            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(GetDBConnection());
            SqlCommand cmd = new SqlCommand("AddressMatcher_Suffixs");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@suffix", SqlDbType.VarChar, 50).Value = suffix;
            cmd.Connection = con;
            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
            {
                sda.Fill(ds);

            }
            return ds;



        }
        public DataSet Addressmatcher_Unitvalue()
        {
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(GetDBConnection());
            SqlCommand cmd = new SqlCommand("select unitvalue from Unitvalues");
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;
            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
            {
                sda.Fill(ds);

            }
            return ds;
        }
        public DataTable Get_DataFromDatabaseInDT(string sql)
        {
            SqlDataAdapter adp = new SqlDataAdapter(sql, GetDBConnection());
            DataSet ds = new System.Data.DataSet();
            adp.Fill(ds);
            DataTable dt = new System.Data.DataTable();
            dt = ds.Tables[0];
            return dt;
        }
    }
}
