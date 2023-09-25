using UnityEngine;
using UnityEngine.SceneManagement;

namespace ui
{
#if UNITY_WEBPLAYER
        private static string _webQuitURL = "https://www.google.com";
#endif
    
    public class ButtonHandler : MonoBehaviour
    {
        public void ReloadLevel()
        {
            SceneManager.LoadScene(0);
        }

        public void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
            Application.OpenURL(_webQuitURL);
#else
            Application.Quit();
#endif
        }
    }
}