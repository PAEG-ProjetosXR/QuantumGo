using System;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Atomball", menuName = "Scriptable Objects/Atomball")]
public class Atomball : ScriptableObject
{
    public int id;
    public GameObject atomballPrefab;
    public Sprite menuIcon;
    public int damageToHealth;
    public int captureTimes;
}
