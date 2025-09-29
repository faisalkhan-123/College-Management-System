using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Configuration;
using System.Web;
using System.Data;

namespace College_Management_System.Models
{
    public class DBLAYER
    {
        SqlConnection ConnectObj;
        public DBLAYER()
        {
            SqlConnection ConnectObj = new SqlConnection("Data Source=DESKTOP-6HR46AF\\SQLEXPRESS;Initial Catalog=CollegeMS;Integrated Security=True;Encrypt=False");
        }
        public int ExecuteUID(string procname, SqlParameter[] parameter)
        {
            SqlCommand cmd = new SqlCommand(procname, ConnectObj);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddRange(parameter);
            if(ConnectObj.State==ConnectionState.Closed)
            {
                ConnectObj.Open();
            }
            int result = cmd.ExecuteNonQuery();
            if(ConnectObj.State==ConnectionState.Open)
            {
                ConnectObj.Close();
            }
            return result;
        }
        public object ExecuteScalar(string procename, SqlParameter[] parameters)
        {
            SqlCommand cmd = new SqlCommand(procename, ConnectObj);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(parameters);
            if(ConnectObj.State==ConnectionState.Open)
            {
                ConnectObj.Close();
            }
            object result = cmd.ExecuteScalar();
            if(ConnectObj.State==ConnectionState.Closed)
            {
                ConnectObj.Open();
            }
            return result;
        }
        public object ExecuteScalar(string procename)
        {
            SqlCommand cmd = new SqlCommand(procename, ConnectObj);
            cmd.CommandType = CommandType.StoredProcedure;
            if(ConnectObj.State==ConnectionState.Open)
            {
                ConnectObj.Close();
            }
            object result = cmd.ExecuteScalar();
            if(ConnectObj.State==ConnectionState.Closed)
            {
                ConnectObj.Open();
            }
            return result;
        }
        public DataTable ExeuteSelect(string procname)
        {
            SqlCommand cmd = new SqlCommand(procname,ConnectObj);
            cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);
            return dt;
        }
        public DataTable ExeuteSelect(string procname, SqlParameter[] parameter)
        {
            SqlCommand cmd = new SqlCommand(procname,ConnectObj);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(parameter);
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);
            return dt;
        }

    }
}