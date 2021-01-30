using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamGameV2.DatabaseConnection;
using TeamGameV2.Pages;
using TeamGameV2.Components.GamePageComponents;
using Microsoft.AspNetCore.SignalR;
using TeamGameV2.Javascript;
using TeamGameV2.PageLogic;


namespace TeamGameV2.PageLogic
{
    public partial class MainGamePageLogic : ComponentBase
    {

        //all of these are instead of doing the at inject
        [Inject]
        protected NavigationManager navManager { get; set; }
        //for the js
        [Inject]
        protected BrowserService Service { get; set; }

        [Parameter]
        public int lobbynumber { get; set; }
        [Parameter] 
        public int playernumber { get; set; }

        protected DatabaseModel DM = new DatabaseModel();
        protected MyValues MV = new MyValues();

        public string MyCursorStyle;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            DM = ReadDB.GetVars(lobbynumber);
            
            //js the three below
            var dimension = await Service.GetDimensions();
            MV.Height = dimension.Height;
            MV.Width = dimension.Width;
            MV.PlayerIAm = playernumber;
            MV.MyLobby = lobbynumber;
            MyCursorStyle = "p" + MV.PlayerIAm;
            WriteDB.UpdateMyMouseCoords(MV);
            StateHasChanged();

        }
        //todo fix doubling player number when double joining on loading
        //oninitizlized async is called twice with server and component render
        //onafter is only called once afterwards so i used it to update the db and not get doubles
        protected override void OnInitialized()
        {

        }

        public bool P1Add = false;
        public bool P1Subtract = false;
        public bool P2Add = false;
        public bool P2Subtract = false;
        public bool P3Add = false;
        public bool P3Subtract = false;
        public bool P4Add = false;
        public bool P4Subtract = false;

        public async Task PlayerLostHealth(int player, int healthchange)
        {
            bool update = true;
            switch (player)
            {
                case 1:
                    if (DM.P1Health + healthchange > 0) { P1Subtract = true; }
                    else { update = false; PlayerDied(player); } 
                    break;
                case 2:
                    if (DM.P2Health + healthchange > 0) { P2Subtract = true; }
                    else { update = false; PlayerDied(player); }
                    break;
                case 3:
                    if (DM.P3Health + healthchange > 0) { P3Subtract = true; }
                    else { update = false; PlayerDied(player); }
                    break;
                case 4:
                    if (DM.P4Health + healthchange > 0) { P4Subtract = true; }
                    else { update = false; PlayerDied(player); }
                    break;
                default: break;
            }

            await Task.Delay(700);
            if (update)
            {
                WriteDB.UpdatePlayerHealth(MV.MyLobby, player, DM, healthchange);
            }
            switch (player)
            {
                case 1: P1Subtract = false; break;
                case 2: P2Subtract = false; break;
                case 3: P3Subtract = false; break;
                case 4: P4Subtract = false; break;
                default: break;
            }
        }
        public async Task PlayerGainedHealth(int player, int healthchange)
        {
            bool update = true;
            switch (player)
            {
                case 1:
                    if (DM.P1Health + healthchange <= 12) { P1Add = true; }
                    else { update = false; P1Add = true; WriteDB.UpdatePlayerHealthMax(MV.MyLobby, player); }
                    break;
                case 2:
                    if (DM.P2Health + healthchange <= 12) { P1Add = true; }
                    else { update = false; P1Add = true; WriteDB.UpdatePlayerHealthMax(MV.MyLobby, player); }
                    break;
                case 3:
                    if (DM.P3Health + healthchange <= 12) { P1Add = true; }
                    else { update = false; P1Add = true; WriteDB.UpdatePlayerHealthMax(MV.MyLobby, player); }
                    break;
                case 4:
                    if (DM.P4Health + healthchange <= 12) { P1Add = true; }
                    else { update = false; P1Add = true; WriteDB.UpdatePlayerHealthMax(MV.MyLobby, player); }
                    break;
                default: break;
            }
            await Task.Delay(300);
            if (update)
            {
                WriteDB.UpdatePlayerHealth(MV.MyLobby, player, DM, healthchange);

            }
            switch (player)
            {
                case 1: P1Add = false; break;
                case 2: P2Add = false; break;
                case 3: P3Add = false; break;
                case 4: P4Add = false; break;
                default: break;
            }
        }
        public async Task PlayerFinishedGame(int player, bool won, int lobby)
        {
            //need to start new game here



        }
        public static void GenerateNewGame()
        {




        }
        public static string dead;
        public static void PlayerDied(int player)
        {

            dead = dead + " " + player.ToString();
            
        }

    }
    
}
