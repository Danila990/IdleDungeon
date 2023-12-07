using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorSpawner : MonoBehaviour
{
    public enum SpawnType
    {
        OneForAll,
        Different,
        NoRepeat
    }
    public SpawnType spawnType;
    public Transform[] spawnPoints;
    public Sprite[] sprites;

    private void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        switch (spawnType)
        {
            case SpawnType.OneForAll:
                SpawnSame(spawnPoints, sprites);
                break;
            case SpawnType.Different:
                SpawnDiff(spawnPoints, sprites);
                break;
            case SpawnType.NoRepeat:
                SpawnNoRepeat(spawnPoints, sprites);
                break;
        }
    }


    private void SpawnDecor(Transform point, Sprite decor)
    {
        point.GetComponent<SpriteRenderer>().sprite = decor;
    }

    private void SpawnDiff(Transform[] points, Sprite[] sprites)
    {
        foreach (Transform point in points)
        {
            Sprite decor = sprites[Random.Range(0, sprites.Length)];
            SpawnDecor(point, decor);
        }
    }

    private void SpawnSame(Transform[] points, Sprite[] sprites)
    {
        Sprite decor = sprites[Random.Range(0, sprites.Length)];
        foreach (Transform point in points)
        {
            SpawnDecor(point, decor);
        }
    }

    private void SpawnNoRepeat(Transform[] points, Sprite[] sprites)
    {
        List<Sprite> usedAlready = new List<Sprite>();
        foreach (Transform point in points)
        {
            Sprite decor = sprites[Random.Range(0, sprites.Length)];
            while (usedAlready.Contains(decor))
            {
                decor = sprites[Random.Range(0, sprites.Length)];
            }
            usedAlready.Add(decor);
            SpawnDecor(point, decor);
        }

    }
}
