using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleButton : MonoBehaviour
{
    public string nameLevel;
    [SerializeField] private HeroList heroBaseList;
    [SerializeField] private HeroBase heroBase;
    
    public void OnButton()
	{
		AudioLibrarySO.instance.PlayButtonSound();
		GetComponent<Button>().interactable = false;
        heroBase.HeroList.Clear();
        
        for(int index=0;index<6;index++)
            foreach (var cyr in heroBaseList.Place)
            {
                if (cyr == index)
                {
                    heroBase.HeroList.Add(heroBaseList.UnitPrefab[heroBaseList.Place.IndexOf(cyr)]);
                } 
            }

        SceneManager.LoadSceneAsync(nameLevel);
    }

}
