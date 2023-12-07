using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSpeedButtonSlow : MonoBehaviour
{
 private float fixedDeltaTime;

    private Image switchImage;
    [SerializeField] private Sprite[] switchSprites;
    private int buttonState;
    
    // Start is called before the first frame update
    void Awake()
    {
        // Make a copy of the fixedDeltaTime, it defaults to 0.02f, but it can be changed in the editor
        buttonState = 0;
        switchImage = GetComponent<Button>().image;
        //switchImage.sprite = switchSprites[buttonState];
        fixedDeltaTime = Time.fixedDeltaTime;
        
        gameObject.GetComponent<Button>().onClick.AddListener(OnButtonPressed);
    }

    bool pause;

    public void OnButtonPause()
    {
        if (!pause)
        {
            Time.timeScale = 0f;
            pause = true;
        }
        else
        {
            OnButtonX1();
        }
    }
    
    public void OnButtonX2()
    {
        Time.timeScale = 15f;
    }
    
    public void OnButtonX1()
    {
        Time.timeScale = 1f;
        pause = false;
    }
    
    public void OnButtonX25()
    {
        Time.timeScale = 0.25f;
    }
    
    
    public void OnButtonPressed()
    {
        //buttonState = 1 - buttonState;
        //switchImage.sprite = switchSprites[buttonState];

        Time.timeScale -= 0.1f;
        
        if (Time.timeScale == 1.0f)
            Time.timeScale = 2.0f;
        else
            Time.timeScale = 1.0f;
    }
}
