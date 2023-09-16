using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] Character character;
    [SerializeField] ParticleSystem effect;

    IAttackable attacker;

    void Start()
    {
        attacker = character.transform.parent.GetComponent<IAttackable>();
    }

    void OnTriggerEnter(Collider other)
    {
        IHittable hitter = other.GetComponent<IHittable>();

        if (hitter != null)
        {
            if (other.CompareTag(attacker.TargetTag))
            {
                hitter.GetDamage(character.power);
                GameObject effect = PoolManager.Instance.Get(this.effect.name);

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
