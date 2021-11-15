using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UINameBar : MonoBehaviour
{
    public Image avatar;
    public Text CharacterName;
    public Entities.Creature character;
    // Start is called before the first frame update
    void Start()
    {
        if (this.character != null)
        {
            if (character.Info.Type == SkillBridge.Message.CharacterType.Monster)
                this.avatar.gameObject.SetActive(false);
            else
                this.avatar.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.UpdateInfo();
    }

    void UpdateInfo()
    {
        if (this.character != null)
        {
            string name = this.character.Name + " Lv." + this.character.Info.Level;
            if (name != this.CharacterName.text)
            {
                this.CharacterName.text = name;
            }
        }
    }
}
