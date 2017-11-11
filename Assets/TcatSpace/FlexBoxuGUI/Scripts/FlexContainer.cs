using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class FlexContainer : MonoBehaviour {

    public enum FlexDirection
    {
        Row,
        RowReverse,
        Column,
        ColumnReverse
    }

    public enum FlexWrap
    {
        NoWrap,
        Wrap
    }

    public enum JustifyContent
    {
        Start,
        End,
        Center,
        SpaceBetween,
        SpaceAround
    }

    public enum AlignItems
    {
        Stretch,
        Start,
        End,
        Center,
        BaseLine
    }

    public enum AlignContent
    {
        Stretch,
        Start,
        End,
        Center,
        SpaceBetween,
        SpaceAround
    }

    public FlexDirection flexDirection;
    public FlexWrap flexWrap;
    public JustifyContent justifyContent;
    public AlignItems alignItems;
    public AlignContent alignContent;

    class FlexLineOld
    {
        public float freeSpace;
        public float maxSize;
        public List<FlexItem> items;
    }

    class DirectionSize
    {
        public float main;
        public float sub;
    }

	// Use this for initialization
	void Start () {
        Flexboxfy<Image>();
        FlexLine.Split(transform, FlexLine.Orientation.Horizontal);
        /*
        switch(flexWrap)
        {
            case FlexWrap.NoWrap:
                ChangeNoWrapLayout();
                break;
            case FlexWrap.Wrap:
                ChangeWrapLayout();
                break;
            default:
                Debug.LogError("Undefined flexWrap");
                break;
        }
        */
	}

	// Update is called once per frame
	void Update () {
	}

    void Flexboxfy<T>() where T : Component
    {
        foreach(var component in GetComponentsInChildren<T>())
        {
            var flexItem = component.gameObject.AddComponent<FlexItem>();
        }
    }

    bool IsRowDirection()
    {
        return flexDirection == FlexDirection.Row || flexDirection == FlexDirection.RowReverse;
    }

    int GetDirectionLength()
    {
        return IsRowDirection() ? Screen.width : Screen.height;
    }

    List<FlexLineOld> GetFlexLinesOld()
    {
        var result = new List<FlexLineOld>();
        var flexItems = GetComponentsInChildren<FlexItem>();
        var list = new List<FlexItem>();
        var areaSize = GetDirectionLength();

        var current = 0f;
        var maxSize = 0f;
        foreach(var flexItem in flexItems)
        {
            var sizeDelta = flexItem.GetComponent<RectTransform>().sizeDelta;
            var directionSize = GetCurrentDirectionSize(sizeDelta);

            if(maxSize < directionSize.sub)
            {
                maxSize = directionSize.sub;
            }

            if(current + directionSize.main > areaSize)
            {
                var flexLine = new FlexLineOld()
                {
                    freeSpace = areaSize - current,
                    items = list,
                    maxSize = maxSize
                };
                result.Add(flexLine);

                list = new List<FlexItem>();
                current = 0f;
                maxSize = 0f;
            }
            list.Add(flexItem);
            current += directionSize.main;
        }

        var flexLine2 = new FlexLineOld()
        {
            freeSpace = areaSize - current,
            items = list,
            maxSize = maxSize
        };
        result.Add(flexLine2);
        return result;
    }

    void ChangeDefaultLayout()
    {
        var height = GetComponent<RectTransform>().sizeDelta.y;

        var components = GetComponentsInChildren<FlexItem>();
        var currentPosition = 0f;
        foreach(var component in components)
        {
            var rectTransform = component.gameObject.GetComponent<RectTransform>();
            ChangeLeftUpLayout(rectTransform);
            rectTransform.position = new Vector3(currentPosition, height, 0);
            currentPosition += rectTransform.sizeDelta.x;
        }
    }

    void ChangeNoWrapLayout()
    {
        var height = GetComponent<RectTransform>().sizeDelta.y;
        var components = GetComponentsInChildren<FlexItem>();
        var width = Screen.width / components.Length; 
        var currentPosition = 0f;
        for(int i = 0; i < components.Length; i++)
        {
            var rectTransform = components[i].gameObject.GetComponent<RectTransform>();
            ChangeLeftUpLayout(rectTransform);
            rectTransform.position = new Vector3(currentPosition, height, 0);
            rectTransform.sizeDelta = new Vector2(width, rectTransform.sizeDelta.y);
            currentPosition += width;
        }
    }

    DirectionSize GetCurrentDirectionSize(Vector2 sizeDelta)
    {
        if(flexDirection == FlexDirection.Row || flexDirection == FlexDirection.RowReverse)
        {
            return new DirectionSize() { main = sizeDelta.x, sub = sizeDelta.y };
        }
        else
        {
            return new DirectionSize() { main = sizeDelta.y, sub = sizeDelta.x };
        }
    }

    Vector3 ToPosition(DirectionSize directionSize)
    {
        var height = GetComponent<RectTransform>().sizeDelta.y;
        if(flexDirection == FlexDirection.Row || flexDirection == FlexDirection.RowReverse)
        {
            return new Vector3(directionSize.main, height - directionSize.sub, 0);
        }
        else
        {
            return new Vector3(directionSize.sub, height - directionSize.main, 0);
        }
    }

    class Padding
    {
        public float start;
        public float itemToItem;
    }

    Padding GetPadding(float freeSpace, int itemCount)
    {
        switch (justifyContent)
        {
            case JustifyContent.Start:
                return new Padding() { start = 0, itemToItem = 0 };
            case JustifyContent.End:
                return new Padding() { start = freeSpace, itemToItem = 0 };
            case JustifyContent.Center:
                return new Padding() { start = freeSpace / 2, itemToItem = 0 };
            case JustifyContent.SpaceBetween:
                if(itemCount <= 1)
                {
                    return new Padding() { start = 0, itemToItem = 0 };
                }
                return new Padding() { start = 0, itemToItem = freeSpace / (itemCount - 1) };
            case JustifyContent.SpaceAround:
                var unit = freeSpace / (itemCount * 2);
                return new Padding() { start = unit, itemToItem = unit * 2 };
            default:
                Debug.LogError("undefined justify content");
                throw new System.Exception("undefined justify content");
        }
    }

    Padding GetAlignPadding(float freeSpace, int itemCount)
    {
        switch (alignContent)
        {
            case AlignContent.Stretch:
                return new Padding() { start = 0, itemToItem = 0 };
            case AlignContent.Start:
                return new Padding() { start = 0, itemToItem = 0 };
            case AlignContent.End:
                return new Padding() { start = freeSpace, itemToItem = 0 };
            case AlignContent.Center:
                return new Padding() { start = freeSpace / 2, itemToItem = 0 };
            case AlignContent.SpaceBetween:
                if(itemCount <= 1)
                {
                    return new Padding() { start = 0, itemToItem = 0 };
                }
                return new Padding() { start = 0, itemToItem = freeSpace / (itemCount - 1) };
            case AlignContent.SpaceAround:
                var unit = freeSpace / (itemCount * 2);
                return new Padding() { start = unit, itemToItem = unit * 2 };
            default:
                Debug.LogError("undefined align content");
                throw new System.Exception("undefined align content");
        }
    }

    float GetFlexLineOldPadding(float freeSpace)
    {
        switch (alignItems)
        {
            case AlignItems.Stretch:
                return 0;
            case AlignItems.Start:
                return 0;
            case AlignItems.End:
                return freeSpace;
            case AlignItems.Center:
                return freeSpace / 2;
            case AlignItems.BaseLine:
                // 未実装
                return 0;
            default:
                Debug.LogError("undefined align items");
                throw new System.Exception("undefined align items");
        }

    }

    void ChangeWrapLayout()
    {
        var flexLines = GetFlexLinesOld();
        var subPosition = 0f;
        foreach(var flexLine in flexLines)
        {
            var padding = GetPadding(flexLine.freeSpace, flexLine.items.Count);
            var mainPosition = padding.start;
            foreach(var flexItem in flexLine.items)
            {
                //alignContentを考慮したレイアウトにする
                var rectTransform = flexItem.GetComponent<RectTransform>();
                ChangeLeftUpLayout(rectTransform);
                var directionSize = GetCurrentDirectionSize(rectTransform.sizeDelta);
                var flexLinePadding = GetFlexLineOldPadding(flexLine.maxSize - directionSize.sub);
                rectTransform.position = ToPosition(new DirectionSize
                {
                    main = mainPosition,
                    sub = subPosition + flexLinePadding
                });

                if(alignItems == AlignItems.Stretch)
                {
                    if(IsRowDirection())
                    {
                        rectTransform.sizeDelta = new Vector2(directionSize.main, flexLine.maxSize); 
                    }
                    else
                    {
                        rectTransform.sizeDelta = new Vector3(flexLine.maxSize, directionSize.main);
                    }
                }

                mainPosition += directionSize.main + padding.itemToItem;
            }
            subPosition += flexLine.maxSize;
        }
    }

    void ChangeLeftUpLayout(RectTransform rectTransform)
    {
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.pivot = new Vector2(0, 1);
    }
}
