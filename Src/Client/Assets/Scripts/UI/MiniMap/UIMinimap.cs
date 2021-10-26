using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Managers;
using Models;

public class UIMinimap : MonoBehaviour
{
    public Collider minimapBoundingBox;
    public Image minimap;
    public Image NavArrow;
    public Text mapName;

    private Transform playerTransform;
    void Start()
    {
        Debug.LogWarning("UIMinimap Start " + this.GetInstanceID());
        MiniMapManager.Instance.Minimap = this;
        
        UpdateMap();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform == null)
            playerTransform = MiniMapManager.Instance.PlayerTransform;
        //if (this.minimapBoundingBox == null)
        //{
        //    UpdateMap();
        //    //return;
        //}
        if(playerTransform == null || this.minimapBoundingBox == null)
        {
            return;
        }
        float realWidth = minimapBoundingBox.bounds.size.x;
        float realHeight = minimapBoundingBox.bounds.size.z;

        float realX = playerTransform.position.x - minimapBoundingBox.bounds.min.x;
        float realY = playerTransform.position.z - minimapBoundingBox.bounds.min.z;

        float pivotX = realX / realWidth;
        float pivotY = realY / realHeight;

        this.minimap.rectTransform.pivot = new Vector2(pivotX, pivotY);
        this.minimap.rectTransform.localPosition = Vector2.zero;
        this.NavArrow.transform.eulerAngles = new Vector3(0, 0, -playerTransform.eulerAngles.y);
        MiniMapManager.Instance.UpdateMinimap(minimapBoundingBox);
    }

    public void UpdateMap()
    {
        this.mapName.text = User.Instance.CurrentMapData.Name;
        this.minimap.overrideSprite = MiniMapManager.Instance.LoadCurrentMinimap();
        this.minimap.SetNativeSize();
        this.minimap.transform.localPosition = Vector3.zero;
        this.minimapBoundingBox = MiniMapManager.Instance.MinimapBoundingBox;
        this.playerTransform = null;
    }
}
