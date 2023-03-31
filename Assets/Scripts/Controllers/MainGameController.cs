using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Histhack.Core
{
    public class MainGameController : Singleton<MainGameController>
    {

        #region SerializedProperties

        [SerializeField]
        private AddictionalMethods addictionalMethods;

        #endregion SerializedProperties


        #region PublicProperties

        public AddictionalMethods AddictionalMethods { get => addictionalMethods; }

        #endregion PublicProperties



        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                DontDestroyOnLoad(this);
            }
        }
    }
}
