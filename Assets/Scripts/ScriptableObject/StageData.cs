using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "Scriptable Object/StageData")]
public class StageData : ScriptableObject
{
    public List<Character> monsters = new List<Character>();

    void OnEnable()
    {
        //3마리까지만 추가 가능
        if (monsters.Count > 3) { monsters.RemoveRange(3, monsters.Count - 3); }
    }
}
