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

        public event Action<int> OnDateChanged;

        public void CallOnDateChanged(int newDate)
        {
            if(OnDateChanged != null)
            {
                OnDateChanged(newDate);
            }
        }

        public event Action<SlotPuzzlePiece> OnSlotFinished;

        public void CallOnSlotFinished(SlotPuzzlePiece finishedSlot)
        {
            if (OnSlotFinished != null)
            {
                OnSlotFinished(finishedSlot);
            }
        }

    }
}
