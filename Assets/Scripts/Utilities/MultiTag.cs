using System.Collections.Generic;
using UnityEngine;

public class MultiTag : MonoBehaviour
{
    [Header("Tags")]   
    public string logicTag;
    public string physicsTag;
    public List<string> extraTags;

    public bool CompareTags(string tag)
    {
        if (extraTags.Contains(tag)) return true; else return false;
    }
}
