using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlexItem : MonoBehaviour {

    public float grow = 1f;
    public float shrink = 1f;
    public float basis;
    public float padding = 10f;
    public BoxSizing boxSizing;

    [HideInInspector]
    public float freeSpace;

    public enum BoxSizing
    {
        // BoxSize is basis + padding
        ContentBox,
        // BoxSize is basis
        BorderBox
    }

    public float GetSize()
    {
        switch (boxSizing)
        {
            case BoxSizing.ContentBox:
                return basis + padding * 2;
            case BoxSizing.BorderBox:
                return basis;
            default:
                var errorMessage = "undefined box sizing";
                Debug.LogError(errorMessage);
                throw new System.Exception(errorMessage);
        }
	}

    public float GetContentSize()
    {
        switch (boxSizing)
        {
            case BoxSizing.ContentBox:
                return basis;
            case BoxSizing.BorderBox:
                return basis - padding * 2;
            default:
                var errorMessage = "undefined box sizing";
                Debug.LogError(errorMessage);
                throw new System.Exception(errorMessage);
        }

    }
}