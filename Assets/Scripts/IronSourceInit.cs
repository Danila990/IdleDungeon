using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronSourceInit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       StartCoroutine(IronSourceInitialization());
    }
    
    
    IEnumerator IronSourceInitialization()
    {
     
        // IronSource
#if UNITY_ANDROID
        string appKey = "85460dcd";
#elif UNITY_IPHONE
        string appKey = "8545d445";
#else
        string appKey = "unexpected_platform";
#endif
        //#IronSource.Agent.validateIntegration();
        // SDK init
        //#IronSource.Agent.init(appKey);
        yield return null;
    }
}
