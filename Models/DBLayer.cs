using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ClassLateManage.Models
{
    
    public class DBLayer
    {
         SqlConnection connection=new SqlConnection("Data Source=HAPPY\\SQLEXPRESS;Initial Catalog=LateManager;Integrated Security=True;");
        //    SqlConnection connection = new SqlConnection(
        //"Server=sql6034.site4now.net;" +
        //"Database=db_a2fc24_viveksir;" +
        //"User Id=db_a2fc24_viveksir_admin;" +
        //"Password=hgtbsed12@$6fgfD;" +
        //"Encrypt=True;" +
        //"TrustServerCertificate=True;" +
        //"Connection Timeout=30;"



        //SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        public DataTable LoginUser(string proc,params SqlParameter[] parameters)
        {
            SqlCommand command = new SqlCommand(proc,connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dt=new DataTable();
            adapter.Fill(dt);
            return dt;
        }
        public DataTable ExecuteSelect(string proc) 
        {
            SqlCommand command = new SqlCommand(proc, connection);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public int ExecuteInsert(string proc,params SqlParameter[] parameters)
        {
            try
            {
                SqlCommand command = new SqlCommand(proc, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddRange(parameters);
                connection.Open();
                int result = command.ExecuteNonQuery();
                connection.Close();
                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }


        }
    }
}