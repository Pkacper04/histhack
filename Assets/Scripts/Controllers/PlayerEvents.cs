using System;


namespace Histhack.Core.Events
{
    public class PlayerEvents
    {
        public event Action<string> OnPlayerDeath;

        public void CallOnPlayerDeath(string deathType)
        {
            if (OnPlayerDeath != null)
                OnPlayerDeath(deathType);
        }

    }
}
