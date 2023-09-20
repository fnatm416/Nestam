using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyes : Character
{
    [SerializeField] Collider hitBox;
    [SerializeField] ParticleSystem[] trails;

    public void StartHitbox() { hitBox.enabled = true; }    //히트박스 활성화
    public void EndHitbox() { hitBox.enabled = false; }     //히트박스 비활성화

    public void StartTrail() 
    {
        //트레일 활성화
        for (int i=0; i<trails.Length; i++)
        {
            trails[i].Play();
        }
    }  
    public void EndTrail() 
    {
        //트레일 비활성화
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