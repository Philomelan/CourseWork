using UnityEngine;
using System;

[Serializable]
public class Enemy
{
    public string name;
    public GameObject prefab;

    public Enemy(string name, GameObject prefab) {
        this.name = name;
        this.prefab = prefab;
    }
}