using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Character
{
    [SerializeField] Collider hitBox;
    [SerializeField] ParticleSystem trail;

    protected override void Awake()
    {
        base.Awake();
        hitBox.enabled = false;
        trail.Stop();
    }

    public void StartHitbox() { hitBox.enabled = true; }    //��Ʈ�ڽ� Ȱ��ȭ
    public void EndHitbox() { hitBox.enabled = false; }     //��Ʈ�ڽ� ��Ȱ��ȭ

    public void StartTrail() { trail.Play(); }  //Ʈ���� Ȱ��ȭ
    public void EndTrail() { trail.Stop(); }    //Ʈ���� ��Ȱ��ȭ
}