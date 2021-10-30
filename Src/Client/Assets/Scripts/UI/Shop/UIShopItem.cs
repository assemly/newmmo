using Common.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIShopItem : MonoBehaviour,ISelectHandler
{
    private UIShop shop;

    public int ShopItemId { get; set; }
    private ShopItemDefine ShopItem { get;  set; }
    private ItemDefine item;
    
    public Text title;
    public Text count;
    public Text price;
    public Image icon;
    public Text limitClass;

    public Image background;
    public Sprite normalBg;
    public Sprite selectedBg;

    private bool selected;
    public bool Selected
    {
        get { return selected; }
        set
        {
            selected = value;
            this.background.overrideSprite = selected ? selectedBg : normalBg;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetShopItem(int id, ShopItemDefine shopItem, UIShop owner)
    {
        this.shop = owner;
        this.ShopItemId = id;
        this.ShopItem = shopItem;
        this.item = DataManager.Instance.Items[this.ShopItem.ItemID];

        this.title.text = this.item.Name;
        this.count.text = "x"+ShopItem.Count.ToString();
        this.price.text = ShopItem.Price.ToString();
        if(this.item.LimitClass != 0)
        {
            this.limitClass.text = this.item.LimitClass.ToString();
        }
        else
        {
            this.limitClass.text = "";
        }
        
        this.icon.overrideSprite = Resloader.Load<Sprite>(item.Icon);
    }

    public void OnSelect(BaseEventData eventData)
    {
        this.Selected = true;
        this.shop.SelectShopItem(this);
    }
}
