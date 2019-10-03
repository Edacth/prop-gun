using UnityEngine;
using System;

[Serializable]
public class MaterialUI
{
    public string name;
    public PhysicMaterial material;
    public Sprite image;
}

[Serializable]
public class LayerUI
{
    public string name;
    public int layer;
    public Sprite image;
}
