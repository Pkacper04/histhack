using Histhack.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Histhack.Core
{
    public class DateController : MonoBehaviour
    {
        [SerializeField]
        private int startingYear;

        private int currentYear;

        public int CurrentYear { get => currentYear; }

        private void Awake()
        {
            currentYear = startingYear;
        }

        public void UpdateDate(int newYear)
        {
            currentYear = newYear;
            MainGameController.Instance.GameEvents.CallOnDateChanged(newYear);
        }
    }
}
