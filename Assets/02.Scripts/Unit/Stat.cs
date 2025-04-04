using System;
using UnityEngine;

public struct Stat
{
    public int Value;
    public int AddedValue;


    public Stat(int value, int addedValue = 0)
    {
        Value = value;
        AddedValue = addedValue;
    }
}
