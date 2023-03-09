using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions 
{


    public static Color ToColor(this string str)
    {

        Color newCol;

        if (!ColorUtility.TryParseHtmlString(str, out newCol))
        {
            ColorUtility.TryParseHtmlString("FFFFFF", out newCol);
        }
        return newCol;
    }
}
