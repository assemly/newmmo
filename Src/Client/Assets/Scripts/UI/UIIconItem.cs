using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIIconItem : MonoBehaviour
{
    public Image mainImage;
    public Image sceondImage;
    public Text mainText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMainIcon(string iconName,string text)
    {
        this.mainImage.overrideSprite = Resloader.Load<Sprite>(iconName);
        this.mainText.text = text;
    }
}
