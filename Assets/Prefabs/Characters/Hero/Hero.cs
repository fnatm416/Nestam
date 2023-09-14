using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Character
{
    [SerializeField] BoxCollider sword;

    public void Hit()
    {
        Vector3 position = sword.transform.position + sword.transform.rotation * sword.center;
        Quaternion rotation = sword.transform.rotation;
        Vector3 size = sword.size * 0.5f;

        Collider[] hitColliders = Physics.OverlapBox(position, size, rotation);

        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag(attackable.targetTag))
                collider.GetComponent<IHittable>().GetDamage(power);
        }
    }

    //void OnDrawGizmos()
    //{
    //    Vector3 position = sword.transform.position + sword.transform.rotation * sword.center;
    //    Quaternion rotation = sword.transform.rotation;
    //    Vector3 size = sword.size;

    //    Gizmos.color = Color.green;
    //    Gizmos.matrix = Matrix4x4.TRS(position, rotation, size);
    //    Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    //}
}
