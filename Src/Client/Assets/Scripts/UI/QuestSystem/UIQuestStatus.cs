using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Managers;

class UIQuestStatus:MonoBehaviour
{
    public Image[] statusImage;

    private NpcQuestStatus questStatus;

    public void SetQuestStatus(NpcQuestStatus status)
    {
        this.questStatus = status;
        for(int i = 0; i < 4; i++)
        {
            if (this.statusImage[i] != null)
            {
                this.statusImage[i].gameObject.SetActive(i == (int)status);
            }
        }
    }
}
