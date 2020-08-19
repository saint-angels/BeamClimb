using System.Collections;
using System.Collections.Generic;
using Helpers;
using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] LocalCacheManager localCacheManager = null;
    [SerializeField] UiManager uiManager = null;


    public LocalCacheManager LocalCacheManager => localCacheManager;
    public UiManager UiManager => uiManager;
    
    
    
    void Start()
    {
        //Init game modules here
        
        uiManager.Init();
    }
}
