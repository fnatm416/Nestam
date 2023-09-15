using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] Character character;
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
            if (other.CompareTag(attacker.targetTag))
            {
                print("Hit");
                hitter.GetDamage(character.power);
            }
        }
    }
}
