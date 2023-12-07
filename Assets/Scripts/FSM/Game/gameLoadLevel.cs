using UnityEngine;

public class gameLoadLevel : State
{
    #region Приватные поля

    private GameManager gameManager;

    #endregion

    #region Публичные Методы

    public gameLoadLevel(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public override void Enter()
    {
            var transform = gameManager.transform;
            gameManager.CyrLevel = Object.Instantiate(gameManager.Level, transform.position, Quaternion.identity, transform);
            gameManager.LevelDB = gameManager.CyrLevel.GetComponent<LevelData>();
            gameManager.ChangeState(GameManager.State.HeroesSet);
    }
    
    #endregion
}
