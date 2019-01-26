using si.birokrat.next.common.database;
using System;
using System.Data;

namespace DbConnectable
{
    class Program
    {
        static void Main(string[] args)
        {
            string connString = ConnectionString.Format("localhost", "biromaster", "turizem", "q");
            Connection conn = new Connection(connString);
            IDbCommand comm = conn.GenerateCommand();
            comm.CommandType = CommandType.Text;
            comm.CommandText = "SELECT TOP 3 * FROM plugin_cache";
            DataTable dt = conn.ExecuteDataTable(comm);
            
        }
    }
}
