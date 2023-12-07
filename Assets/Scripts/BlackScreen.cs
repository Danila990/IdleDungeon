using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreen : MonoBehaviour
{
    public float timer=0;
    private Image _image;
    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0f)
        {
            timer = 0f;
            _image.enabled = false;
        }
        else
        {
            _image.enabled = true;
            timer -= Time.deltaTime;
        }
    }
}
