using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearnBattle : MonoBehaviour
{
    [SerializeField] private UnitControllerTutorial enemy;
    [SerializeField] private GameObject fightB;
    [SerializeField] private GameObject Level;
    [SerializeField] private GameObject nextLevel;
    [SerializeField] private GameObject cyrLevel;
    
    public delegate void Command(int command);
    public static event Command UnitCommand;
    
    private bool moveLevel = false;

    private bool moveWait = true;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(StartBattle());
    }

    //private IEnumerator StartBattle()
    //{

    /* var nearestTarget = GameObject.FindGameObjectsWithTag("Enemy");
         
     foreach (var n in nearestTarget)
     {
         n.GetComponent<UnitController>().SetTeleport();
     }
         
     nearestTarget = GameObject.FindGameObjectsWithTag("Hero");
     
     foreach (var n in nearestTarget)
     {
         n.GetComponent<UnitController>().SetTeleport();
     }

     yield return 0;*/
    //}
    // Update is called once per frame
    void Update()
    {
        // Если HP врага <0
        if (enemy.UnitCyrHP <= 0f && moveWait)
        {
            moveWait = false;
            StartCoroutine(WaitNextLevel());
        }

        if (moveLevel)
        {
            var camWidth = Camera.main.pixelWidth;
            var camHeight = Camera.main.pixelHeight;
            //camWidth = 2170;
            //camHeight = 1080;

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
                //ClearDie();
                cyrLevel = nextLevel;

                int number = 0;
                //Debug.Log("Событие");
                UnitCommand?.Invoke(1);

            }
        }


    }

    private IEnumerator WaitNextLevel()
    {
        yield return new WaitForSeconds(2f);
        NextLevel();
    }

    void NextLevel()
    {

        Transform go;
        var camWidth = Camera.main.pixelWidth;
        var camHeight = Camera.main.pixelHeight;
        //camWidth = 2170;
        //camHeight = 1080;
        //levelNapr = 0;

        nextLevel = Instantiate(Level, transform.position + new Vector3(0f, .0f, 0f), Quaternion.identity, transform);
        nextLevel.GetComponent<LevelData>().Room.GetComponent<RectTransform>().offsetMin =
            new Vector2(nextLevel.GetComponent<LevelData>().RoomCanvas.GetComponent<RectTransform>().sizeDelta.x, 0f);
        nextLevel.GetComponent<LevelData>().Room.GetComponent<RectTransform>().offsetMax =
            new Vector2(nextLevel.GetComponent<LevelData>().RoomCanvas.GetComponent<RectTransform>().sizeDelta.x, 0f);
        nextLevel.GetComponent<LevelData>().RoomCanvas.transform.GetComponent<Canvas>().sortingOrder = 2;

        moveLevel = true;

    }
}
    
    

