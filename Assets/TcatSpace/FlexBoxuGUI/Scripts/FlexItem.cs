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

    [HideInInspector]
    public FlexContainer.FlexDirection direction;

    public enum BoxSizing
    {
        // BoxSize is basis + padding
        ContentBox,
        // BoxSize is basis
        BorderBox
    }

    public void LoadRectTransform()
    {
        switch (orientation)
        {
            case FlexLine.Orientation.Horizontal:
                SetContentSize(rectTransform.sizeDelta.x);
                break;
            case FlexLine.Orientation.Vertical:
                SetContentSize(rectTransform.sizeDelta.y);
                break;
        }
    }

    public void SetSize(float size)
    {
        switch (boxSizing)
        {
            case BoxSizing.ContentBox:
                basis = size - padding * 2;
                freeSpace = basis;
                break;
            case BoxSizing.BorderBox:
                basis = size;
                freeSpace = basis - padding * 2;
                break;
        }
    }

    public void SetContentSize(float size)
    {
        Debug.Log(size);
        switch (boxSizing)
        {
            case BoxSizing.ContentBox:
                basis = size;
                freeSpace = basis;
                break;
            case BoxSizing.BorderBox:
                basis = size;
                freeSpace = basis - padding * 2;
                break;
        }
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

    public float GetHead(float space)
    {
        var half = GetSize() / 2f;
        switch (direction)
        {
            case FlexContainer.FlexDirection.Row:
                return -space / 2f + half;
            case FlexContainer.FlexDirection.RowReverse:
                return space / 2f - half;
            case FlexContainer.FlexDirection.Column:
                return space / 2f - half;
            case FlexContainer.FlexDirection.ColumnReverse:
                return -space / 2f + half;
            default:
                var errorMessage = "undefined flex direction";
                Debug.LogError(errorMessage);
                throw new System.Exception(errorMessage);
        }
    }

    public float GetOffset()
    {
        switch (direction)
        {
            case FlexContainer.FlexDirection.Row:
                return GetSize();
            case FlexContainer.FlexDirection.RowReverse:
                return -GetSize();
            case FlexContainer.FlexDirection.Column:
                return -GetSize();
            case FlexContainer.FlexDirection.ColumnReverse:
                return GetSize();
            default:
                var errorMessage = "undefined flex direction";
                Debug.LogError(errorMessage);
                throw new System.Exception(errorMessage);
        }

    }
}