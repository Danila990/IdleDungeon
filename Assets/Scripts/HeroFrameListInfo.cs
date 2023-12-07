using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroFrameListInfo : MonoBehaviour
{
    [Serializable]
    public struct sFrameAndButton
    {
        public GameObject Frame;
        public GameObject Button;
    }

    public List<sFrameAndButton> FrameAndButton;

    public List<GameObject> LevelUpList;

}
