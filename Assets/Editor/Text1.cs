using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Text1
{
    [MenuItem("GameObject/MyText/Text1",true,1)]
    static bool ShowObjectNameValue()
    {
        if(Selection.objects.Length > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    [MenuItem("GameObject/MyText/Text1",false,1)]
    static  void ShowObjectName()
    {
        Debug.Log(Selection.activeObject.name);
    }
}
