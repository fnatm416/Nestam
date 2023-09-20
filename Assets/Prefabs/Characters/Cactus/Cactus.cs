using UnityEngine;

public class Cactus : Character
{
    [SerializeField] Collider[] hitBoxs;
    [SerializeField] ParticleSystem[] trails;

    public void StartHitbox(int index) 
    {
        hitBoxs[index].enabled = true;
    }    
    public void EndHitbox(int index)
    {
        hitBoxs[index].enabled = false;
    }    

    public void StartTrail(int index)
    {
        trails[index].Play();
    }  
    public void EndTrail(int index)
    {
        trails[index].Stop();
    }    

    public override void Init()
    {
        for (int i = 0; i < hitBoxs.Length; i++)
        {
            hitBoxs[i].enabled = false;
        }
        for (int i = 0; i < trails.Length; i++)
        {
            trails[i].Stop();
        }
    }
}