using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamGameV2.DatabaseConnection;

namespace TeamGameV2.PageLogic
{
    public class AnimationLogic
    {
        public bool P1Animation = false;
        public bool P2Animation = false;
        public bool P3Animation = false;
        public bool P4Animation = false;

        public async Task PlayerHealthChanged(int lobby, int player, int currenthealth, int healthchange)
        {

            WriteDB.UpdatePlayerHealth(lobby, player, currenthealth, healthchange);
            this.P1Animation = true;
            await Task.Delay(2000);
            this.P1Animation = false;

        }

    }
}
