using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstTierGuiScript : MonoBehaviour
{
    public GameController gameController;

    //on play button touch
    public void PlayGame() {
        gameController.StartRunning();
        GetComponent<Animator>().SetInteger("MenuChangeParameter", 1);

    }

    //on main menu button touch
    public void GoToMainMenu() {
        GameObject.Destroy(GameObject.Find("GameGUI"));
        SceneManager.LoadScene("MainMenu");
    }

    //on restart tier button touch
    public void RestartTier()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Tier0");
    }

    //on pause button touch
    public void Pause()
    {
        Time.timeScale = 0.0f;
        for (int chidlIndex = 0; chidlIndex < transform.childCount; chidlIndex++) 
        {
            transform.GetChild(chidlIndex).gameObject.SetActive(false);
        }
        transform.GetChild(2).gameObject.SetActive(true);

    }

    //on contunue button touch
    public void Unpause()
    {
        Time.timeScale = 1.0f;
        for (int chidlIndex = 0; chidlIndex < transform.childCount; chidlIndex++)
        {
            transform.GetChild(chidlIndex).gameObject.SetActive(true);
        }
        transform.GetChild(2).gameObject.SetActive(false);

    }


}
