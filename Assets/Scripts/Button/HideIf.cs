using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideIf : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject hideObject0;
    [SerializeField] private GameObject hideObject1;
    void Update()
    {
        hideObject0.SetActive(target.activeSelf);
        hideObject1.SetActive(target.activeSelf);
    }
}
