using UnityEngine;


public class Home : MonoBehaviour
{
    public GameManager gameManager;

    public void OnButton()
    {
        AudioLibrarySO.instance.PlayButtonSound();
        //if (gameLevel >= 9) gameManager.operation = SceneManager.LoadSceneAsync("Tavern");
        gameManager.operation.allowSceneActivation = true;
    }
    
}

