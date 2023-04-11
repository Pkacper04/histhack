using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Histhack.Core.Effects
{
    public class PostprocessManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject depthOfField;

        [SerializeField]
        private GameObject vignette;

        [SerializeField]
        private GameObject filmGrain;

        public void ChangePostProcess(PostProcessesToChange postProcessToChange, bool value)
        {
            switch (postProcessToChange)
            {
                case PostProcessesToChange.DepthOfField:
                    depthOfField.SetActive(value);
                    break;
                case PostProcessesToChange.Vignette:
                    vignette.SetActive(value);
                    break;
                case PostProcessesToChange.FilmGrain:
                    filmGrain.SetActive(value);
                    break;
            }
        }

    }

    public enum PostProcessesToChange
    {
        DepthOfField,
        Vignette,
        FilmGrain
    }
}
