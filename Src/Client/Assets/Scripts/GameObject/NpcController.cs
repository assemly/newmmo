﻿using Common.Data;
using Managers;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    public int npcID;
    public new SkinnedMeshRenderer renderer;
    Animator anim;
    Color orignColor;
    private bool inInteractive = false;

    NpcDefine npc;
    NpcQuestStatus questStatus;



    void Start()
    {
        renderer = this.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        anim = this.gameObject.GetComponent<Animator>();
        orignColor = renderer.sharedMaterial.color;
        npc = NpcManager.Instance.GetNpcDefine(npcID);
      
        this.StartCoroutine(Acitons());
        RefreshNpcStatus();
        QuestManager.Instance.OnQuestStatusChanged += OnQuestStatusChanged;
    }

    void OnQuestStatusChanged(Quest quest)
    {
        this.RefreshNpcStatus();
    }

    private void RefreshNpcStatus()
    {
        questStatus = QuestManager.Instance.GetQuestStatusByNpc(this.npcID);
        UIWorldElementManager.Instance.AddNpcQuestStatus(this.transform, questStatus);
    }

    private void OnDestroy()
    {
        QuestManager.Instance.OnQuestStatusChanged -= OnQuestStatusChanged;
        if (UIWorldElementManager.Instance != null)
            UIWorldElementManager.Instance.RemoveNpcQuestStatus(this.transform);
    }

    void Update()
    {

    }

    IEnumerator Acitons()
    {
        while (true)
        {
            if (inInteractive)
                yield return new WaitForSeconds(2f);
            else
                yield return new WaitForSeconds(UnityEngine.Random.Range(5f, 10f));
            this.Relex();
        }
    }

    private void Relex()
    {
        anim.SetTrigger("Relax");
    }

    void Interactive()
    {
        if (!inInteractive)
        {
            inInteractive = true;
            StartCoroutine(DoInteractive());
        }
    }
    
    IEnumerator DoInteractive()
    {
        yield return FaceToPlayer();
        if (NpcManager.Instance.Interactive(npc))
        {
            anim.SetTrigger("Talk");
        }
        yield return new WaitForSeconds(3f);
        inInteractive = false;
    }

    IEnumerator FaceToPlayer()
    {
        Vector3 faceTo = (User.Instance.CurrentCharacterObject.transform.position - this.transform.position).normalized;
        while(Mathf.Abs(Vector3.Angle(this.gameObject.transform.forward,faceTo)) > 5)
        {
            this.gameObject.transform.forward = Vector3.Lerp(this.gameObject.transform.forward, faceTo, Time.deltaTime * 5f);
            yield return null;
        }
    }
    private void OnMouseDown()
    {
        Interactive();
    }

    private void OnMouseOver()
    {
        Highlight(true);
    }

    private void OnMouseEnter()
    {
        Highlight(true);
    }

    private void OnMouseExit()
    {
        Highlight(false);
    }
    private void Highlight(bool highlight)
    {
        if (highlight)
        {
            if (renderer.sharedMaterial.color != Color.yellow)
                renderer.sharedMaterial.color = Color.yellow;
        }
        else
        {
            if (renderer.sharedMaterial.color != orignColor)
                renderer.sharedMaterial.color = orignColor;
        }
    }
}
