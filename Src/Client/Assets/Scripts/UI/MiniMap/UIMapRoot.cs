using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMapRoot : MonoBehaviour
{
    // Start is called before the first frame update
    public Collider minimapBoundingBox;
    void Start()
    {
        if (minimapBoundingBox != null)
        {
            MiniMapManager.Instance.UpdateMinimap(minimapBoundingBox);
            Debug.Log("minimapBoundingBox UIMapRoot");
        }
           
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
