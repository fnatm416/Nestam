using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    Character character;
    IAttackable attacker;

    void Start()
    {
        Transform parent = transform.parent;
        while (!parent.GetComponent<Character>())
        {
            parent = parent.parent;
        }
        character = parent.GetComponent<Character>();

        attacker = character.transform.parent.GetComponent<IAttackable>();
    }

    void OnTriggerEnter(Collider other)
    {
        IHittable hitter = other.GetComponent<IHittable>();

        if (hitter != null)
        {
            if (other.CompareTag(attacker.TargetTag))
            {
                string effectName = "";
                if (other.CompareTag("Player")) { effectName = "SwordImpactRed"; }
                else if (other.CompareTag("Monster")) { effectName = "SwordImpactBlue"; }

                GameObject effect = PoolManager.Instance.Get(effectName);
                hitter.GetDamage(character.Power);
                effect.transform.position = other.ClosestPointOnBounds(transform.position);
                StartCoroutine(ParticleEffect(effect.GetComponent<ParticleSystem>()));
            }
        }
    }

    IEnumerator ParticleEffect(ParticleSystem effect)
    {
        while (effect.GetComponent<ParticleSystem>().isPlaying)
        {
            yield return null;
        }

        PoolManager.Instance.Return(effect.gameObject);
    }
}