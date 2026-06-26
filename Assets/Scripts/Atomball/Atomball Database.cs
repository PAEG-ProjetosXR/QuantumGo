using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AtomballDatabase", menuName = "Scriptable Objects/AtomballDatabase")]
public class AtomballDatabase : ScriptableObject
{
    public List<Atomball> atomballs = new List<Atomball>();
    [NonSerialized]
    public int selectedBallId = 0;

    public Atomball GetChosenAtomball()
    {
        foreach (Atomball atomball in atomballs)
        {
            if (atomball.id == selectedBallId)
                return atomball;
        }

        return null;
    }

    public Atomball GetById(int id)
    {
        foreach (Atomball atomball in atomballs)
        {
            if (atomball.id == id)
                return atomball;
        }

        return null;
    }
}
