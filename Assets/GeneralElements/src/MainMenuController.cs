using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

    //this object will pass to the other scenes
    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }

    public void loadTier(int tierNumber)
    {
        SceneManager.LoadScene("Tier"+tierNumber);
    }

    public void ExitApp() 
    {
        Application.Quit();
    }

}
