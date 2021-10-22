using Managers;
using Models;
using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainCity : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnCharacterLeaveMap()
    {
       
        //UIWorldElementManager.Instance.Clear();
        //CharacterManager.Instance.Clear();

        
        SceneManager.Instance.LoadScene("CharSelect");
        UserService.Instance.SendGameLeave();

    }
}
