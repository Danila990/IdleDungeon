using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ChangeHeroImage : MonoBehaviour
{
    [SerializeField] private List<Sprite> imageHeroes;
    public heroesStats heroesInfo;
    private int gameLevel;
        // Start is called before the first frame update
    void Start()
    {
        gameLevel = SaveSystem.GetGameLevel();
        gameLevel++;
        
        if (heroesInfo.dataArray.FirstOrDefault(s => s.Unlockonlvl == gameLevel) != null)
        {
            string name = heroesInfo.dataArray.FirstOrDefault(s => s.Unlockonlvl == gameLevel).Id;
            //Debug.Log("Hero="+name);
            //Debug.Log("SpriteHero="+imageHeroes.First(s => s.name == name));
            GetComponent<Image>().sprite = imageHeroes.FirstOrDefault(s => s.name == name);
            GetComponent<Image>().SetNativeSize();
        }
    }
}
