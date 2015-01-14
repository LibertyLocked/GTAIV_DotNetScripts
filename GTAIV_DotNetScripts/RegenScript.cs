using System;
using GTA;

namespace GTAIV_DotNetScripts
{
    public class RegenScript : Script
    {
        const int MAX_HEALTH = 100;

        const int MAX_IDLE = 40;
        const int MAX_RECOVER = 4;
        const int HEALTH_PER_REGEN = 1;

        int lastHealth = 0;
        int idleTimer = 0;
        int recoverTimer = 0;

        public RegenScript()
        {
            Interval = 250; // update interval
            this.Tick += new EventHandler(this.Update);
        }

        private void Update(object sender, EventArgs e)
        {
            if ((Player.Character.Health < MAX_HEALTH / 4) && Player.isActive)
            {
                int currentHealth = Player.Character.Health;

                if (currentHealth >= lastHealth)
                {
                    idleTimer++;
                    // Make sure idle timer doesn't go too large
                    idleTimer = Math.Min(idleTimer, MAX_IDLE);
                }
                else
                {
                    // Player lost health, reset idle and recover timers.
                    idleTimer = 0;
                    recoverTimer = 0;
                }

                if (idleTimer >= MAX_IDLE)
                {
                    recoverTimer++;
                    if (recoverTimer >= MAX_RECOVER)
                    {
                        Player.Character.Health += HEALTH_PER_REGEN;
                        recoverTimer = 0;
                    }
                }

                lastHealth = currentHealth;
            }
        }
    }
}
