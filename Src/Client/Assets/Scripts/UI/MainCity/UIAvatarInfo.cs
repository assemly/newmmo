using Models;
using UnityEngine;
using UnityEngine.UI;

public class UIAvatarInfo : MonoBehaviour
{
    public Text characterName;
    public Text characterLv;
    // Start is called before the first frame update
    void Start()
    {
        characterName.text = User.Instance.CurrentCharacterInfo.Name;
        characterLv.text = User.Instance.CurrentCharacterInfo.Level.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        UpdataInfo();
    }

    void UpdataInfo()
    {
        if (characterLv == null) return;
        //if(characterLv.text != User.Instance.CurrentCharacter.Level.ToString())
        //{
        //    characterLv.text = User.Instance.CurrentCharacter.Level.ToString();
        //}
    }
}
