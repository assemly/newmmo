using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWorldElement : MonoBehaviour
{
    public Camera lookAt;
    public Transform owner;
    public float height = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (owner != null)
        {
            this.transform.position = owner.position + Vector3.up * height;
            //Debug.Log("UIWorldElement.owner:", this.owner);
        }
        if (lookAt != null)
        {
            this.transform.forward = lookAt.transform.forward;
            //Vector3 targetPoint = lookAt.transform.position;
            //targetPoint.y = this.transform.position.y;
            ////this.transform.rotation = Camera.main.transform.rotation;
            //this.transform.LookAt(targetPoint,Vector3.up);
           // Debug.Log("UIWorldElement.transform:", this.transform);
        }
            
    }
}
