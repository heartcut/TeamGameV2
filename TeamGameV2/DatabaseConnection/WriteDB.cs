using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TeamGameV2.PageLogic;

namespace TeamGameV2.DatabaseConnection
{
    public class WriteDB
    {
        public static void IJoined(int lobbynum)
        {
            
            //ill use this to update the vars that i need
            var cs = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\source\ServerSideBlazor\DataAccessLibrary\Database1.mdf;Integrated Security=True;Connect Timeout=30";
            using var con = new SqlConnection(cs);
            con.Open();
            int players = con.QueryFirst<int>(@"SELECT PlayersInLobby FROM CursorPos WHERE LobbyNumber=" + lobbynum);
            players = players + 1;
            con.Query<DatabaseModel>("UPDATE dbo.CursorPos SET PlayersInLobby = "+players+ " WHERE LobbyNumber = " + lobbynum);

            con.Dispose();

        }
        public static void ILeft(int lobnumber,int playernum)
        {
            var cs = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\source\ServerSideBlazor\DataAccessLibrary\Database1.mdf;Integrated Security=True;Connect Timeout=30";

            using var con = new SqlConnection(cs);
            con.Open();
            int players = con.QueryFirst<int>(@"SELECT PlayersInLobby FROM CursorPos WHERE LobbyNumber=" + lobnumber);
            players=players-1;
            con.Execute("UPDATE CursorPos SET PlayersInLobby = " + players + ", P"+playernum+"Present = 0 WHERE LobbyNumber = " + lobnumber + "; ");

            con.Dispose();

        }
        public static void PlayerPresent(int lobnumber,int player)
        {
            var cs = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\source\ServerSideBlazor\DataAccessLibrary\Database1.mdf;Integrated Security=True;Connect Timeout=30";

            using var con = new SqlConnection(cs);
            con.Open();
            
            con.Execute("UPDATE CursorPos SET P"+player+"Present = 1 WHERE LobbyNumber = " + lobnumber + "; ");

            con.Dispose();

        }
        public static void UpdateMyMouseCoords(MyValues MV)
        {
            var cs = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\source\ServerSideBlazor\DataAccessLibrary\Database1.mdf;Integrated Security=True;Connect Timeout=30";
            using var con = new SqlConnection(cs);
            con.Open();
            con.Execute("UPDATE dbo.CursorPos SET P" + MV.PlayerIAm + "Xcords =" + MV.mycursx + ", P" + MV.PlayerIAm +
                "Ycords = " + MV.mycursy + " WHERE LobbyNumber =" + MV.MyLobby + ";");


            con.Dispose();

        }
        public static void UpdatePlayerHealth(int lobby, int player,  DatabaseModel DM, int healthchange)
        {
            var newhealth = player switch
            {
                1 => DM.P1Health + healthchange,
                2 => DM.P2Health + healthchange,
                3 => DM.P3Health + healthchange,
                4 => DM.P4Health + healthchange,
                _ => DM.P4Health + healthchange,
            };
            var cs = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\source\ServerSideBlazor\DataAccessLibrary\Database1.mdf;Integrated Security=True;Connect Timeout=30";

            using var con = new SqlConnection(cs);
            con.Open();

            con.Execute("UPDATE CursorPos SET P" + player + "Health = " + newhealth + "WHERE LobbyNumber = " + lobby + "; ");

            con.Dispose();



        }
    }
}
