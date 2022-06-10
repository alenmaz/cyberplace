using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : ScriptableObject
{
    public string Name = "New ability";
    public string Description = "No description";
    public Sprite Icon;

    public virtual void Activate(GameObject parent) { }
}
