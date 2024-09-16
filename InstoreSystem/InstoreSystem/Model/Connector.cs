using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace InstoreSystem.Model
{
    internal static class Connector
    {
        static string cs = @"Server=localhost; Port=3307; Database=storedb; User Id=root;";

        public static MySqlConnection getConnection()
        {
            return new MySqlConnection(cs);
        }
    }
}
