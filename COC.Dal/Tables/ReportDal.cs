using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Data.EntityClient;

namespace COC.Dal.Tables
{
    public class ReportDal
    {
        public static int Report_SP_RPT_WORKDETAIL_Exist(string datefrom, string dateto)
        {
            SqlConnection conn = new SqlConnection();
            SqlDataAdapter da = null;
            SqlCommand command = new SqlCommand();
            DataTable dt = new DataTable();
            SLMDBEntities slmdb = new SLMDBEntities();

            try
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();

                conn.ConnectionString = ((EntityConnection)slmdb.Connection).StoreConnection.ConnectionString;
                conn.Open();

                command.Connection = conn;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_RPT_WORKDETAIL";
                command.Parameters.Add(new SqlParameter("@STARTDATE", datefrom));
                command.Parameters.Add(new SqlParameter("@ENDDATE", dateto));

                da = new SqlDataAdapter(command);
                da.Fill(dt);
                return dt.Rows.Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (da != null) da.Dispose();
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        public static int Report_SP_RPT_STAFF_WORKING_Exist(string datefrom, string dateto)
        {
            SqlConnection conn = new SqlConnection();
            SqlDataAdapter da = null;
            SqlCommand command = new SqlCommand();
            DataTable dt = new DataTable();
            SLMDBEntities slmdb = new SLMDBEntities();

            try
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();

                conn.ConnectionString = ((EntityConnection)slmdb.Connection).StoreConnection.ConnectionString;
                conn.Open();

                command.Connection = conn;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_RPT_STAFF_WORKING";
                command.Parameters.Add(new SqlParameter("@STARTDATE", datefrom));
                command.Parameters.Add(new SqlParameter("@ENDDATE", dateto));

                da = new SqlDataAdapter(command);
                da.Fill(dt);
                return dt.Rows.Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (da != null) da.Dispose();
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        public static int Report_SP_RPT_SNAP_MONITORING_Exist(string datefrom, string dateto)
        {
            SqlConnection conn = new SqlConnection();
            SqlDataAdapter da = null;
            SqlCommand command = new SqlCommand();
            DataTable dt = new DataTable();
            SLMDBEntities slmdb = new SLMDBEntities();

            try
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();

                conn.ConnectionString = ((EntityConnection)slmdb.Connection).StoreConnection.ConnectionString;
                conn.Open();

                command.Connection = conn;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_RPT_SNAP_MONITORING";
                command.Parameters.Add(new SqlParameter("@STARTDATE", datefrom));
                command.Parameters.Add(new SqlParameter("@ENDDATE", dateto));

                da = new SqlDataAdapter(command);
                da.Fill(dt);
                return dt.Rows.Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (da != null) da.Dispose();
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }
    }
}
