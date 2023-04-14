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



        public event Action<SlotPuzzlePiece> OnSlotWrongSelect;

        public void CallOnSlotWrongSelect(SlotPuzzlePiece wrongSlot)
        {
            if (OnSlotWrongSelect != null)
            {
                OnSlotWrongSelect(wrongSlot);
            }
        }

        public event Action<int> OnMinigameFinished;
        public void CallOnMinigameFinished(int minigameId)
        {
            if (OnMinigameFinished != null)
            {
                OnMinigameFinished(minigameId);
            }
        }

        public event Action OnGameDailogueStart;
        public void CallOnGameDailogueStart()
        {
            if (OnGameDailogueStart != null)
            {
                OnGameDailogueStart();
            }
        }

        public event Action OnGameFinish;

        public void CallOnGameFinish()
        {
            if (OnGameFinish != null)
            {
                OnGameFinish();
            }
        }

    }
}
