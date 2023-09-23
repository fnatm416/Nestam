using System.Collections;
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

                GameObject effect = ObjectPool.Instance.Get(effectName);
                hitter.GetDamage(character.Power);
                effect.transform.position = other.ClosestPointOnBounds(transform.position);
                StartCoroutine(ParticleEffect(effect.GetComponent<ParticleSystem>()));
                SoundManager.Instance.PlaySfx(SoundManager.Sfx.Hit);
                PlayDirector.Instance.CameraShake();
            }
        }
    }

    IEnumerator ParticleEffect(ParticleSystem effect)
    {
        while (effect.GetComponent<ParticleSystem>().isPlaying)
        {
            yield return null;
        }

        ObjectPool.Instance.Return(effect.gameObject);
    }
}
