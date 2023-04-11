using System;
using UnityEngine;

namespace Histhack.Core.Events
{
    public class GameEvents
    {

        public event Action OnSaveGame;
        public void CallOnSaveGame()
        {
            if (OnSaveGame != null)
                OnSaveGame();
        }

        public event Action OnPreLoad;
        public void CallOnPreLoad()
        {
            if (OnPreLoad != null)
                OnPreLoad();
        }

        public event Action OnLoadGame;
        public void CallOnLoadGame()
        {
            if (OnLoadGame != null)
                OnLoadGame();
        }

    }
}