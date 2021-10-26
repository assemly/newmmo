using Common.Data;
using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterObjecet : MonoBehaviour
{
    public int ID;
    Mesh mesh = null;

    void Start()
    {
        this.mesh = this.GetComponent<MeshFilter>().sharedMesh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (this.mesh != null)
        {
            Gizmos.DrawWireMesh(this.mesh, this.transform.position + Vector3.up * this.transform.localScale.y * .5f, this.transform.rotation);
        }
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.ArrowHandleCap(0, this.transform.position, this.transform.rotation, 1f, EventType.Repaint);
    }
#endif

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        PlayerInputController playerController = other.GetComponent<PlayerInputController>();
        if(playerController!=null && playerController.isActiveAndEnabled)
        {
            TeleporterDefine map = DataManager.Instance.Teleporters[this.ID];
            if(map == null)
            {
                Debug.LogErrorFormat("TeleporterObject: Character [{0}] Enter Teleporter [{1}],But TeleporterDefine not existed", playerController.character.Info.Name, this.ID);
                return;
            }
            if(map.LinkTo >0)
            {
                if (DataManager.Instance.Teleporters.ContainsKey(map.LinkTo))
                {
                    MapService.Instance.SendMapTeleport(this.ID);
                }
                else
                {
                    Debug.LogErrorFormat("Teleporter ID:{0} LinkID {1} error!", map.ID, map.LinkTo);
                }
            }
        }
    }
}
