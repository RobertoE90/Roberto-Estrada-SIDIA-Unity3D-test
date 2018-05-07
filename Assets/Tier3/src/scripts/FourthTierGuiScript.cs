
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FourthTierGuiScript : MonoBehaviour
{
    public Text displacementLabel;
    public Slider slider;
    public Material displacementMaterial;

    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }

    //on main menu button touch
    public void GoToMainMenu() {
        GameObject.Destroy(GameObject.Find("GameGUI"));
        SceneManager.LoadScene("MainMenu");
    }

    //on slider value change
    public void ChangeDisplacement() 
    {
        int value = (int)(slider.value * 100);
        displacementLabel.text = "Displacement: " + value+"%";
        displacementMaterial.SetFloat("_Displacement", slider.value);
    }

    //on random color button click
    public void CreateRandomColor() 
    {
        Color randomColor = new Color(Random.value, Random.value, Random.value);
        displacementMaterial.SetColor("_BaseColor", randomColor);
    }
}
