using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bin {

	public List<float> items { get; private set; }
    public float capacity { get; private set; }

    public Bin()
    {
        items = new List<float>();
        capacity = 1f;
    }

    public bool CanFit(float item)
    {
        return item <= capacity;
    }

    // return true for success, false for failure
    public bool Add(float item)
    {
        if (item <= capacity)
        {
            items.Add(item);
            capacity -= item;
            return true;
        }
        else
        {
            return false;
        }
    }
}
