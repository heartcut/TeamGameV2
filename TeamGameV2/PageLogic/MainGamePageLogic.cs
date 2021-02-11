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


        public static async Task PlayerLostHealth(int player, int healthchange, int lobb)
        {
            bool update = true;
            DatabaseModel HealthVars = ReadDB.GetVars(lobb);
            switch (player)
            {
                
                case 1:
                    if (HealthVars.P1Health + healthchange > 0) { WriteDB.PlayerHealthAnimationChange(lobb, 1, -1); }
                    else { update = false; WriteDB.ChangeInGame(lobb, player, -1); }//playerdied
                    break;
                case 2:
                    if (HealthVars.P2Health + healthchange > 0) { WriteDB.PlayerHealthAnimationChange(lobb, 2, -1); }
                    else { update = false; WriteDB.ChangeInGame(lobb, player, -1); }  //playerdied
                    break;
                case 3:
                    if (HealthVars.P3Health + healthchange > 0) { WriteDB.PlayerHealthAnimationChange(lobb, 3, -1); }
                    else { update = false; WriteDB.ChangeInGame(lobb, player, -1); } //playerdied
                    break;
                case 4:
                    if (HealthVars.P4Health + healthchange > 0) { WriteDB.PlayerHealthAnimationChange(lobb, 4, -1); }
                    else { update = false; WriteDB.ChangeInGame(lobb, player, -1); } //playerdied
                    break;
                default: break;
            }

            await Task.Delay(700);
            if (update)
            {
                switch (player)
                {
                    case 1: WriteDB.UpdatePlayerHealth(lobb, player, HealthVars.P1Health, healthchange); break;
                    case 2: WriteDB.UpdatePlayerHealth(lobb, player, HealthVars.P2Health, healthchange); break;
                    case 3: WriteDB.UpdatePlayerHealth(lobb, player, HealthVars.P3Health, healthchange); break;
                    case 4: WriteDB.UpdatePlayerHealth(lobb, player, HealthVars.P4Health, healthchange); break;
                    default: break;
                }
            }
            switch (player)
            {
                case 1: WriteDB.PlayerHealthAnimationChange(lobb, 1, 0); break;
                case 2: WriteDB.PlayerHealthAnimationChange(lobb, 2, 0); break;
                case 3: WriteDB.PlayerHealthAnimationChange(lobb, 3, 0); break;
                case 4: WriteDB.PlayerHealthAnimationChange(lobb, 4, 0); break;
                default: break;
            }
        }
        public static async Task PlayerGainedHealth(int player, int healthchange, int lobb)
        {
            bool update = true;
            DatabaseModel HealthVars = ReadDB.GetVars(lobb);
            switch (player)
            {
                case 1:
                    if (HealthVars.P1Health + healthchange <= 12) { WriteDB.PlayerHealthAnimationChange(lobb, 1, 1); }
                    else { update = false; WriteDB.PlayerHealthAnimationChange(lobb, 1, 1); WriteDB.UpdatePlayerHealthMax(lobb, player); }
                    break;
                case 2:
                    if (HealthVars.P2Health + healthchange <= 12) { WriteDB.PlayerHealthAnimationChange(lobb, 2, 1); }
                    else { update = false; WriteDB.PlayerHealthAnimationChange(lobb, 2, 1); WriteDB.UpdatePlayerHealthMax(lobb, player); }
                    break;
                case 3:
                    if (HealthVars.P3Health + healthchange <= 12) { WriteDB.PlayerHealthAnimationChange(lobb, 3, 1); }
                    else { update = false; WriteDB.PlayerHealthAnimationChange(lobb, 3, 1); WriteDB.UpdatePlayerHealthMax(lobb, player); }
                    break;
                case 4:
                    if (HealthVars.P4Health + healthchange <= 12) { WriteDB.PlayerHealthAnimationChange(lobb, 4, 1); }
                    else { update = false; WriteDB.PlayerHealthAnimationChange(lobb, 4, 1); WriteDB.UpdatePlayerHealthMax(lobb, player); }
                    break;
                default: break;
            }
            await Task.Delay(300);
            if (update)
            {
                
                switch (player)
                {
                    case 1: WriteDB.UpdatePlayerHealth(lobb, player, HealthVars.P1Health, healthchange); break;
                    case 2: WriteDB.UpdatePlayerHealth(lobb, player, HealthVars.P2Health, healthchange); break;
                    case 3: WriteDB.UpdatePlayerHealth(lobb, player, HealthVars.P3Health, healthchange); break;
                    case 4: WriteDB.UpdatePlayerHealth(lobb, player, HealthVars.P4Health, healthchange); break;
                    default: break;
                }
            }
            switch (player)
            {
                case 1: WriteDB.PlayerHealthAnimationChange(lobb, 1, 0); break;
                case 2: WriteDB.PlayerHealthAnimationChange(lobb, 2, 0); break;
                case 3: WriteDB.PlayerHealthAnimationChange(lobb, 3, 0); break;
                case 4: WriteDB.PlayerHealthAnimationChange(lobb, 4, 0); break;
                default: break;
            }
        }

        //public string dead;
        //public void PlayerDied(int player)
        //{

        //    dead = dead + " " + player.ToString();

        //}
        public static async Task PlayerFinishedGame(int player, bool won, int lobby)
        {
            //need to start new game here
            WriteDB.ChangeInGame(lobby, player, 0);
            if (won)
            {
                switch (player)
                {
                    //because of the static method that im in
                    // wil probably need to add varibale to the db
                    //atheat is for the animations which will make them server wide and everyone will see
                    //which is not a bad thing
                    // and it will also fix my problem of not being able to call the function
                    //do this when ur not drunk idiot

                    case 1: PlayerGainedHealth(2, 3, lobby); break;
                    case 2: PlayerGainedHealth(3, 3, lobby); break;
                    case 3: PlayerGainedHealth(4, 3, lobby); break;
                    case 4: PlayerGainedHealth(1, 3, lobby); break;
                    default: break;
                }

            } 
            else { }
            MinigameGeneration.GenerateNewGame(player, lobby);
            await Task.Delay(2000);
            WriteDB.ChangeInGame(lobby, player, 1);


        }
        public static async Task StartGame(int lobby)
        {
            MinigameGeneration.GenerateNewGame(1, lobby);
            MinigameGeneration.GenerateNewGame(2, lobby);
            MinigameGeneration.GenerateNewGame(3, lobby);
            MinigameGeneration.GenerateNewGame(4, lobby);
            WriteDB.GameStarted(lobby);
        }
    }
    
}
