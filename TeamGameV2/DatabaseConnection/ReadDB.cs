using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace TeamGameV2.DatabaseConnection
{
    public class ReadDB
    {

        public static DatabaseModel GetVars(int lobby)
        {

            var cs = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\source\ServerSideBlazor\DataAccessLibrary\Database1.mdf;Integrated Security=True;Connect Timeout=30";

            using var con = new SqlConnection(cs);

            con.Open();


            DatabaseModel DM = con.QueryFirst<DatabaseModel>(@"SELECT * FROM CursorPos WHERE LobbyNumber=" + lobby + ";");
            // Create the command
            // Add the parameters.
            con.Dispose();
            return DM;
            // use the connection here
        }

    }
}
