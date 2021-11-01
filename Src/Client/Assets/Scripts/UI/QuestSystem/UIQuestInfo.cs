using Managers;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestInfo : MonoBehaviour
{
    public Text title;
    public Text[] targets;
    public Text description;
    public GameObject rewardItemPrefab;
    public Transform[] rewardSlots;

    public Dictionary<Transform, UIIconItem> rewardsItems = new Dictionary<Transform, UIIconItem>() 
    {
        
    };
    
    
    public Text rewardMoney;
    public Text rewardExp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ClearInfo()
    {
       
        for (int idx=0;idx< rewardSlots.Length; idx++)
        {
            rewardSlots[0].gameObject.SetActive(false);
        }

    }

    public void SetRewardItem(Quest quest,int rewardId,int slot)
    {
        if (rewardId != default(int))
        {
            if (!rewardsItems.ContainsKey(rewardSlots[slot]))
                rewardsItems.Add(rewardSlots[slot], null);
            if (rewardsItems[rewardSlots[slot]] == null)
            {
                rewardSlots[0].gameObject.SetActive(true);
                GameObject go = Instantiate(rewardItemPrefab, rewardSlots[slot]);
                var ui = go.GetComponent<UIIconItem>();
                rewardsItems[rewardSlots[slot]] = ui;
                var def = DataManager.Instance.Items[rewardId];
                //var def = ItemManager.Instance.Items[item.ItemId].Define;
                ui.SetMainIcon(def.Icon, quest.Define.RewardItem1Count.ToString());
            }
            else
            {
                var def = DataManager.Instance.Items[rewardId];
                rewardsItems[rewardSlots[slot]].SetMainIcon(def.Icon, quest.Define.RewardItem1Count.ToString());
            }



        }
    }
    public void SetQuestInfo(Quest quest)
    {

        //ClearInfo();
        SetRewardItem(quest, quest.Define.RewardItem1, 0);
        SetRewardItem(quest, quest.Define.RewardItem2, 1);
        SetRewardItem(quest, quest.Define.RewardItem3, 2);
        
        this.title.text = string.Format("[{0}]{1}", quest.Define.Type, quest.Define.Name);
        if(quest.Info == null)
        {
            this.description.text = quest.Define.Dialog;
        }
        else
        {
            if(quest.Info.Status == SkillBridge.Message.QuestStatus.Complated)
            {
                this.description.text = quest.Define.DialogFinish;
            }
        }

        this.rewardMoney.text = quest.Define.RewardGold.ToString();
        this.rewardExp.text = quest.Define.RewardExp.ToString();

        foreach(var fitter in this.GetComponentsInChildren<ContentSizeFitter>())
        {
            fitter.SetLayoutVertical();
        }
    }

    public void OnClickAbandon()
    {

    }
}
