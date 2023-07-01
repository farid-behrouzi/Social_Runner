using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Light
{
    public int id;
    public bool active;
    public Color color;
    public Sprite sprite;

    public Light()
    {
        id = 0;
        active = false;
        color = Color.white;
        sprite = null;
    }
    
}
