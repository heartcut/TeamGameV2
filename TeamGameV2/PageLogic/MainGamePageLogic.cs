using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamGameV2.DatabaseConnection;
using TeamGameV2.Pages;

//to link to component

namespace TeamGameV2.PageLogic
{
    public partial class MainGamePage : ComponentBase
    {

        //all of these are instead of doing the at inject
        [Inject]
        protected NavigationManager navManager { get; set; }

        [Parameter]
        public int lobbynumber { get; set; }

        protected int lobby;
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //add 1 to players in lobby
            //get player number here\
            DatabaseModel DBVars = ReadDB.GetVars(lobbynumber);
            if (DBVars.PlayersInLobby > 3) //lobby full
            {
                navManager.NavigateTo("/lobbyfull");
            }
            else
            {
                //being game stuff
                //update db with new playters and own player number
            }

        }
        //todo fix doubling player number when double joining on loading
        //oninitizlized async is called twice with server and component render
        //onafter is only called once afterwards so i used it to update the db and not get doubles
        protected override async Task OnInitializedAsync()
        {
            //will have to do some things here because it runs before its rendered
            lobby = lobbynumber;
        }

    }
}
