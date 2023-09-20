using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyes : Character
{
    [SerializeField] Collider hitBox;
    [SerializeField] ParticleSystem[] trails;

    public void StartHitbox() { hitBox.enabled = true; }    //��Ʈ�ڽ� Ȱ��ȭ
    public void EndHitbox() { hitBox.enabled = false; }     //��Ʈ�ڽ� ��Ȱ��ȭ

    public void StartTrail() 
    {
        //Ʈ���� Ȱ��ȭ
        for (int i=0; i<trails.Length; i++)
        {
            trails[i].Play();
        }
    }  
    public void EndTrail() 
    {
        //Ʈ���� ��Ȱ��ȭ
        for (int i = 0; i < trails.Length; i++)
        {
            trails[i].Stop();
        }
    }    

    public override void Init()
    {
        hitBox.enabled = false;
        for (int i = 0; i < trails.Length; i++)
        {
            trails[i].Stop();
        }
    }
}