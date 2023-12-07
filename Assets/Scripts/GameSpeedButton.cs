using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSpeedButton : MonoBehaviour
{

    public AutoSkill autoSkill;
    private float fixedDeltaTime;

    private Image switchImage;
    [SerializeField] private Sprite[] switchSprites;
    private int buttonState;

    
    // Start is called before the first frame update
    void Awake()
	{
        buttonState = SaveSystem.GetAutoPlayActive() ? 1 : 0;

		switchImage = GetComponent<Button>().image;
        switchImage.sprite = switchSprites[buttonState];
        
        if (buttonState == 1)
        {
            Time.timeScale = 2.0f;
            
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    private void Update()
    {
        
    }
    
    
    public void OnButtonPressed()
	{
		AudioLibrarySO.instance.PlayButtonSound();
		buttonState = 1 - buttonState;
        SaveSystem.SetAutoPlayActive(buttonState == 1, false);
        switchImage.sprite = switchSprites[buttonState];
        if (Time.timeScale == 1.0f)
        {
            Time.timeScale = 2.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }
    
}
