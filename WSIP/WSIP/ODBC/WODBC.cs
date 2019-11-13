using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSIP
{
    public class WODBC
    {
        public bool IsConnected = false;

        string connectionString = "";
        SqlConnection connODBC = null;

        private string _LastError = "";
        public string LastError
        {
            get
            {
                return _LastError;
            }
            set
            {
                _LastError = value;

                if (_LastError.StartsWith("ERROR"))
                    Console.WriteLine(_LastError);
            }
        }


        public WODBC(string _connectString)
        {
            connectionString = _connectString;

            try
            {
                connODBC = new SqlConnection(connectionString);
                connODBC.Open();
                IsConnected = true;
            }
            catch (Exception)
            {
                connODBC = null;
                IsConnected = false;
            }
        }


        public int SQLQuery(string sqlstr, out string xmlResult)
        {
            LastError = "";
            xmlResult = "";

            if (connODBC is null)
            {
                LastError = "ERROR: Null connection.";
                return -1;
            }

            using (SqlCommand command = new SqlCommand(sqlstr, connODBC))
            {
                SqlDataAdapter sda = new SqlDataAdapter(command);
                DataTable dt = new DataTable("Query");
                sda.Fill(dt);
                MemoryStream ms = new MemoryStream();
                dt.WriteXml(ms);
                ms.Seek(0, SeekOrigin.Begin);
                StreamReader sr = new StreamReader(ms);
                xmlResult = sr.ReadToEnd();

                return dt.Rows.Count;
            }
        }

    }
}
