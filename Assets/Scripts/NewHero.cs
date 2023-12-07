using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class NewHero : MonoBehaviour
{
	// Start is called before the first frame update
	[SerializeField] private int id;

	[SerializeField] private GameObject adGO;
    [SerializeField] private TextMeshProUGUI _text;

    private void OnEnable()
    {
		if (LocalizationSettings.SelectedLocale.ToString() == "English")
		{
            _text.fontSize = 15;
            _text.text = "New";
		}
		else 
		{
            _text.fontSize = 11;
            _text.text = "Новый";
        } 
    }

    void Update()
	{
		var hero = SaveSystem.GetHero(id);
		if (hero.bUnlocked > HeroUnlockState.Locked && hero.place == -1 && hero.level == 1)
		{
			GetComponent<Image>().enabled = true;
            _text.gameObject.SetActive(true);

        }
		else
		{
			GetComponent<Image>().enabled = false;
            _text.gameObject.SetActive(false);
        }

		adGO.SetActive(hero.bUnlocked == HeroUnlockState.InLibrary);
	}
}
