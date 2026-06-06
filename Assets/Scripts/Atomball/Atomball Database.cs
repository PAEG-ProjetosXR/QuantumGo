using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AtomballDatabase", menuName = "Scriptable Objects/AtomballDatabase")]
public class AtomballDatabase : ScriptableObject
{
    public List<Atomball> atomballs = new List<Atomball>();
    [NonSerialized]
    public int selectedBallId;
}
