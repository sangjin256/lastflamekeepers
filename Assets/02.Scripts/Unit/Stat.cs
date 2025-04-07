using System;
using UnityEngine;

public class Stat
{
    public int BaisicValue;
    public int AddedValue;

    public int Value => BaisicValue + AddedValue;

    public Stat(int value, int addedValue = 0)
    {
        BaisicValue = value;
        AddedValue = addedValue;
    }
}
