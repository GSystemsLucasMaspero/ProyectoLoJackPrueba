using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace ProyectoLojackABM.Models
{
    public class CodeDB
    {
        // Conexión de variables
        protected SqlConnection connection;

        // Abrir conexión
        public bool Open(string Connection = "DefaultConnection")
        {
            connection = new SqlConnection(@WebConfigurationManager.ConnectionStrings[Connection].ToString());
            try
            {
                if (connection.State.ToString() != "Open")
                    connection.Open();
                return true;
            }
            catch(SqlException ex)
            {
                return false;
            }
        }

        // Cerrar conexión
        public bool Close()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (SqlException ex)
            {
                return false;
            }
        }

        public int ToInt(object s)
        {
            try
            {
                return Int32.Parse(s.ToString());
            } catch {
                return 0;
            }
        }

        public int DataInsert(string sql)
        {
            int LastID = 0;
            string query = sql + ";SELECT @@Identity;";

            try
            {
                if (connection.State.ToString() == "Open")
                {
                    SqlCommand cmd = new SqlCommand(query, connection);
                    LastID = cmd.ExecuteNonQuery();
                }
                return this.ToInt(LastID);
            }
            catch
            {
                return 0;
            }
        }


    }
}