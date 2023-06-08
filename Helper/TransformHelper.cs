using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformHelper 
{
    public static Transform DeepFind(this Transform parant,string targetName)
    {
        Transform temp;
        foreach (Transform child in parant)
        {
            if (child.name==targetName)
            {
                return child;
            }
            else
            {
                temp = DeepFind(child,targetName);
                if (temp!=null)
                {
                    return temp;
                }
            }
        }
        return null;
    }
}
