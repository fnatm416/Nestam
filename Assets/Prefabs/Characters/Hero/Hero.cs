using System.Collections.Generic;
using UnityEngine;

public class Hero : Character
{
    [SerializeField] Collider hitBox;
    [SerializeField] ParticleSystem trail;

    public void StartHitbox() { hitBox.enabled = true; }    //히트박스 활성화
    public void EndHitbox() { hitBox.enabled = false; }     //히트박스 비활성화

    public void StartTrail() { trail.Play(); }  //트레일 활성화
    public void EndTrail() { trail.Stop(); }    //트레일 비활성화

    public override void Init()
    {
        hitBox.enabled = false;
        trail.Stop();
    }
}