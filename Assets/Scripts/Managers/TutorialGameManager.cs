using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGameManager : MonoBehaviour
{
    [SerializeField] private UnitControllerTutorial enemy;
    [SerializeField] private UnitControllerTutorial hero;
    [SerializeField] private GameObject text1;
    [SerializeField] private GameObject text2;
    [SerializeField] private GameObject bFight;
    [SerializeField] private GameObject bSkill;
    [SerializeField] private GameObject cyrLevel;
    [SerializeField] private GameObject[] Level;
    private GameObject nextLevel;
    
    private int cyrIndexEnemy=0;
    
    public delegate void Command(int command);
    public static event Command UnitCommand;
    
    private bool moveLevel = false;
    private bool moveWait = true;

    public int indexTut = 0;
    private void Start()
    {
        AudioLibrarySO.instance.PlayGameMusic();
        if(indexTut==0)StartCoroutine(StartTutorial_1());
        else StartCoroutine(StartTutorial_3());
    }
    
    // Обучалка, выводим текст и кнопку бой
    private IEnumerator StartTutorial_1()
    {
        SeachEnemy();
        yield return new WaitForSeconds(1f);
        UnitCommand?.Invoke(0);
        yield return new WaitForSeconds(2.5f);
        text1.SetActive(true);
        yield return new WaitForSeconds(1f);
        text2.SetActive(true);
        yield return new WaitForSeconds(1f);
        bFight.SetActive(true);
    }

    public void OnButtonFight()
	{
		AudioLibrarySO.instance.PlayButtonSound();
		bSkill.SetActive(true);
        
        // Debug.Log("Start");
        hero.GetComponent<UnitControllerTutorial>().startBattle = true;
        enemy.GetComponent<UnitControllerTutorial>().startBattle = true;
        
        hero.GetComponent<AnimationController>().SetMove();
        enemy.GetComponent<AnimationController>().SetMove();
        
        text1.SetActive(false);
        text2.SetActive(false);
        
        bFight.SetActive(false);    
    }
    
    private IEnumerator StartTutorial_2()
    {
        SeachEnemy();
        yield return new WaitForSeconds(1f);
        UnitCommand?.Invoke(1);
        yield return new WaitForSeconds(.7f);
        bSkill.SetActive(true);
        //yield return new WaitForSeconds(1f);
        //text2.SetActive(true);
        //yield return new WaitForSeconds(1f);
        //bFight.SetActive(true);
    }
    
    private IEnumerator StartTutorial_3()
    {
        SeachEnemy();
        yield return new WaitForSeconds(1f);
        UnitCommand?.Invoke(2);
        yield return new WaitForSeconds(.7f);
        bSkill.SetActive(true);
        //yield return new WaitForSeconds(1f);
        //text2.SetActive(true);
        //yield return new WaitForSeconds(1f);
        //bFight.SetActive(true);
    }
    private bool SeachEnemy()
    {
        
        //var rez = CompareTag("Hero")?GameObject.FindWithTag("Enemy"):GameObject.FindWithTag("Hero");
        var rez = GameObject.FindWithTag("Enemy");
        if (rez == null) return false;
        else
        {
            //Debug.Log("Поиск");
            enemy=rez.GetComponent<UnitControllerTutorial>();
            moveWait = true;
        }

        return true;
    }
    
    void Update()
    {
        // Если HP врага <0 задержка перед следующим уровнем
        if (enemy.UnitCyrHP <= 0f && moveWait )
        {
            if (!SeachEnemy())
            {
                //Debug.Log("End");
                moveWait = false;
                StartCoroutine(WaitNextLevel());
            }
        }

        if (moveLevel)
        {
            var camWidth = Camera.main.pixelWidth;
            var camHeight = Camera.main.pixelHeight;

            var cyrLevelGO = cyrLevel.GetComponent<LevelData>().Room;
            var cyrLevelCanvas = cyrLevel.GetComponent<LevelData>().RoomCanvas;


            cyrLevelGO.transform.position = Vector2.MoveTowards(cyrLevelGO.transform.position,
                new Vector2(-cyrLevel.GetComponent<LevelData>().RoomCanvas.GetComponent<RectTransform>().sizeDelta.x,
                    0f),
                20f * Time.deltaTime);
            nextLevel.GetComponent<LevelData>().Room.transform.position = Vector2.MoveTowards(
                nextLevel.GetComponent<LevelData>().Room.transform.position, new Vector2(0f, 0f), 20f * Time.deltaTime);
            if (Vector2.Distance(nextLevel.GetComponent<LevelData>().Room.GetComponent<RectTransform>().offsetMin,
                    new Vector2(0f, 0f)) <= 0f)
            {
                moveLevel = false;
                nextLevel.GetComponent<LevelData>().RoomCanvas.transform.GetComponent<Canvas>().sortingOrder = 1;

                Destroy(cyrLevel);
                Destroy(enemy.gameObject);
                //ClearDie();
                cyrLevel = nextLevel;

                int number = 0;
                //Debug.Log("Событие");
                
                if (cyrLevelIndex < Level.Length)StartCoroutine(StartTutorial_3());
                else StartCoroutine(StartTutorial_2());
            }
        }


    }

    private IEnumerator WaitNextLevel()
    {
        yield return new WaitForSeconds(2f);
        NextLevel();
    }

    private int cyrLevelIndex = 0;

    /// <summary>
    ///   <para>Какая то фигня.</para>
    /// <para>Какая то фигня.2</para>
    /// 
    /// </summary>
    void NextLevel()
    {

        Transform go;
        var camWidth = Camera.main.pixelWidth;
        var camHeight = Camera.main.pixelHeight;
        //camWidth = 2170;
        //camHeight = 1080;
        //levelNapr = 0;
        if (cyrLevelIndex < Level.Length)
        {
            nextLevel = Instantiate(Level[cyrLevelIndex], transform.position + new Vector3(0f, .0f, 0f), Quaternion.identity,
                transform);


            nextLevel.GetComponent<LevelData>().Room.GetComponent<RectTransform>().offsetMin =
                new Vector2(nextLevel.GetComponent<LevelData>().RoomCanvas.GetComponent<RectTransform>().sizeDelta.x,
                    0f);
            nextLevel.GetComponent<LevelData>().Room.GetComponent<RectTransform>().offsetMax =
                new Vector2(nextLevel.GetComponent<LevelData>().RoomCanvas.GetComponent<RectTransform>().sizeDelta.x,
                    0f);
            nextLevel.GetComponent<LevelData>().RoomCanvas.transform.GetComponent<Canvas>().sortingOrder = 2;

            cyrLevelIndex++;
            moveLevel = true;
            //Debug.Log("NextLevel");
        }
    }
}
