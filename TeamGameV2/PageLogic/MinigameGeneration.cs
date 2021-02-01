using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamGameV2.DatabaseConnection;

namespace TeamGameV2.PageLogic
{
    public static class MinigameGeneration
    {
        //this will take in what game it is and return an array of the proper variables for the game
        //will return in the form [var1,var2,var3,var4] depending on the game

        public static int[] GenerateVariables(int whatgame)
        {
            Random rndm = new Random();
            //if whatgame==1
            if (true)
            {
                //sixninegame
                //just needs to generate 2 random numbers between 0-31 which will be where the 6s are
                int[] temp = new int[4];
                for (int i = 0; i < 2; i++)
                {
                    temp[i] = rndm.Next(0, 32);
                }
                
                return temp;
            }
        }

        public static async Task PlayerFinishedGame(int player, bool won, int lobby)
        {
            //need to start new game here
            switch (player)
            {
                case 1: WriteDB.ChangeInGame(lobby, player, 0); break;
                case 2: WriteDB.ChangeInGame(lobby, player, 0); break;
                case 3: WriteDB.ChangeInGame(lobby, player, 0); break;
                case 4: WriteDB.ChangeInGame(lobby, player, 0); break;
                default: break;
            }
            //if (won) { } else { }
            GenerateNewGame(player, lobby);
            

            await Task.Delay(7000);

            switch (player)
            {
                case 1: WriteDB.ChangeInGame(lobby, player, 1); break;
                case 2: WriteDB.ChangeInGame(lobby, player, 1); break;
                case 3: WriteDB.ChangeInGame(lobby, player, 1); break;
                case 4: WriteDB.ChangeInGame(lobby, player, 1); break;
                default: break;
            }



        }
        public static void GenerateNewGame(int player, int lobby)
        {
            //making it work only for sixninegame right now
            int[] tempvars = new int[4];
            tempvars = GenerateVariables(1);
            WriteDB.UpdateGameVars(lobby, player, tempvars);
        }
        

    }


}

