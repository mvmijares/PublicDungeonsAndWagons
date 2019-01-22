using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that handles loading screen user interfaces
/// </summary>
namespace Overworld.UserInterface {
    public class LoadingScreen : MonoBehaviour {
        #region Data
        public Slider loadingBar;
        public Text progressText;

        #endregion
        
        public void UpdateProgressUI(float progress) {
            loadingBar.value = progress;
            progressText.text = (int)(progress * 100f) + "%";
        }
    }
}
