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

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            DM = ReadDB.GetVars(lobbynumber);
            //js the three below
            var dimension = await Service.GetDimensions();
            MV.Height = dimension.Height;
            MV.Width = dimension.Width;
            MV.PlayerIAm = playernumber;
            MV.MyLobby = lobbynumber;
            WriteDB.UpdateMyMouseCoords(MV);
            StateHasChanged();

        }
        //todo fix doubling player number when double joining on loading
        //oninitizlized async is called twice with server and component render
        //onafter is only called once afterwards so i used it to update the db and not get doubles
        protected override void OnInitialized()
        {
            

        }
       

        
        
    }
    
}
