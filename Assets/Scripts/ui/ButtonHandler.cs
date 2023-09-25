using UnityEngine;
using UnityEngine.SceneManagement;

namespace ui
{
    public class ButtonHandler : MonoBehaviour
    {
        
#if UNITY_WEBPLAYER
        private static string _webQuitURL = "https://www.google.com";
#endif

        [SerializeField] private GameObject _consoleWindow;
        
        public void ReloadLevel()
        {
            SceneManager.LoadScene(0);
        }

        public void ToggleConsole()
        {
            _consoleWindow.SetActive(!_consoleWindow.activeSelf);
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