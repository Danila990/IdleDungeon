using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class URLButton : MonoBehaviour
{
    [SerializeField] private string urlAdress; 
    public void URLOpen()
    {
        Application.OpenURL(urlAdress);
    }
}
