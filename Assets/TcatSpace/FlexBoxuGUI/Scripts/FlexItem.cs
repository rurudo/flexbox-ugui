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

    [HideInInspector]
    public RectTransform rectTransform;

    [HideInInspector]
    public FlexLine.Orientation orientation; 

    public enum BoxSizing
    {
        // BoxSize is basis + padding
        ContentBox,
        // BoxSize is basis
        BorderBox
    }

    public void SetSize(float size)
    {
        basis = size - padding * 2;
        freeSpace = basis;
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

    public void SetPosition(float x, float y)
    {
        switch (orientation)
        {
            case FlexLine.Orientation.Horizontal:
                rectTransform.localPosition = new Vector3(x, y, 0);
                break;
            case FlexLine.Orientation.Vertical:
                rectTransform.localPosition = new Vector3(y, x, 0);
                break;
        }
    }

    public void Resize()
    {
        switch (orientation)
        {
            case FlexLine.Orientation.Horizontal:
                rectTransform.sizeDelta = new Vector2(
                    GetContentSize(),
                    rectTransform.sizeDelta.y);
                break;
            case FlexLine.Orientation.Vertical:
                rectTransform.sizeDelta = new Vector2(
                    rectTransform.sizeDelta.x,
                    GetContentSize());
                break;
        }
    }
}