using UnityEngine.SceneManagement;
using UnityEngine;

public class ThirdTierGuiController : MonoBehaviour {

    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }

    //on main menu button touch
    public void GoToMainMenu()
    {
        GameObject.Destroy(GameObject.Find("GameGUI"));
        SceneManager.LoadScene("MainMenu");
    }
}
