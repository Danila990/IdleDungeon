using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FingerManager : MonoBehaviour
{
    [SerializeField] private GameObject[] finger;
    [SerializeField] private GameObject[] button;
    [SerializeField] private Text money;
    [SerializeField] private GameObject[] lastTutorial;

    private bool key = false;
    private bool tut_num;
    private bool tut_num1=false;
    private bool tut_num2=false;
    
    private bool Tutorial1=true;
    // Start is called before the first frame update
    void Start()
    {
        tut_num = SaveSystem.GetTutorial() == 1;
        FingerOff();
        ButtonOff();
        if (tut_num)
        {
            finger[0].SetActive(true);
            button[0].GetComponent<Button>().enabled = true;
        }
        else
		{
			if (SaveSystem.GetHero(4).place == 4)
			{
				button[1].GetComponent<Button>().enabled = true;
				key = true;
			}
			else
			{
				//Первое попадание в таверну
				Tutorial1 = false;

				//Указатель и кнопка
				finger[0].SetActive(true);
				button[0].GetComponent<Button>().enabled = true;
			}
		}
    }

    public void step1()
    {
        if (tut_num)
        {
            button[5].GetComponent<Button>().enabled = true;
            button[4].GetComponent<Button>().enabled = false;
            finger[0].SetActive(false);
            button[0].GetComponent<Button>().enabled = false;
            finger[6].SetActive(true);
        }
        else
        {
            finger[0].SetActive(false);
            button[0].GetComponent<Button>().enabled = false;
            finger[1].SetActive(true);    
        }
       
    }
    
    public void step2()
    {
        finger[1].SetActive(false);
        finger[2].SetActive(true);
    }
    //Выбрали героя
    public void step3()
    {
        finger[2].SetActive(false);
        finger[3].SetActive(true);
        button[4].GetComponent<Button>().enabled = false;
    }
    
    
    
    public void step4()
    {
        button[2].GetComponent<Button>().enabled = true;
        button[3].GetComponent<Button>().enabled = true;
    }
    //Кнопка в бой
    public void step5()
    {
        finger[4].SetActive(false);
        finger[5].SetActive(true);
        button[1].GetComponent<Button>().enabled = true;
    }
    
    //увеличить уровень
    public void step6()
    {
        button[5].GetComponent<Button>().enabled = false;
        button[6].GetComponent<Button>().enabled = true;
        finger[6].SetActive(false);
        finger[7].SetActive(true);
    }
    
    public void step7()
    {
        button[2].GetComponent<Button>().enabled = true;
        button[3].GetComponent<Button>().enabled = true;
        finger[7].SetActive(false);
        finger[4].SetActive(true);
        button[1].GetComponent<BattleButton>().nameLevel = "Battle 1";
    }
    
    void FingerOff()
    {
       /* foreach (var f in finger)
        {
            f.SetActive(false);
        }*/
    }

    void ButtonOff()
    {
        /*foreach (var f in button)
        {
            f.GetComponent<Button>().enabled=false;
        }*/
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!tut_num)
        {
            // Активирем коллайдер 4 рамки на полу
            if (lastTutorial[1].GetComponent<OnFrame>().RotateFrame&&!tut_num1)
            {
                tut_num1 = true;
                //FingerOff();
                //ButtonOff();
                lastTutorial[1].GetComponent<BoxCollider2D>().enabled = true;
                //finger[8].SetActive(true);
            }

            if (lastTutorial[1].GetComponent<SpriteRenderer>().sprite.name != "frame_plus"&&!tut_num2 && !key)
            {
                //Debug.Log("sdfdsf");
                tut_num2 = true;
                lastTutorial[1].GetComponent<BoxCollider2D>().enabled = false;
                finger[3].SetActive(false);
                finger[4].SetActive(true);
                step4();
                //StartCoroutine(Game());
            }
        }
        /*if (money.text == "0" && Tutorial1)
        {
            Tutorial1 = false;
            FingerOff();
            ButtonOff();
            button[2].GetComponent<Button>().enabled = true;
            button[3].GetComponent<Button>().enabled = true;
            finger[1].SetActive(true);
            finger[3].SetActive(true);
            
            button[1].GetComponent<Button>().enabled = true;
            finger[5].SetActive(true);
        }*/
        
    }

    public void ButtonTutorial1()
    {
        GameObject.Find("Game").GetComponent<TavernManager>().HeroLevelUp();
        Tutorial1 = false;
        ButtonTutBack();
    }

    private void ButtonTutBack()
    {
        //FingerOff();
        //ButtonOff();

        //finger[1].SetActive(true);
        //finger[3].SetActive(true);

        //
        //button[1].GetComponent<Button>().enabled = true;
        //finger[5].SetActive(true);
    }
    
    private IEnumerator Game()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadSceneAsync("LoadingTavern");
    }
}
