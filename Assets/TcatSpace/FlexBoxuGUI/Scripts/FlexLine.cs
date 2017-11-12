using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FlexLine {

    Orientation orientation;
    public enum Orientation
    {
        Horizontal,
        Vertical,
    }

    class FlexBoxElement
    {
        public FlexBoxElement(Orientation orientation, FlexItem flexItem, RectTransform rectTransform)
        {
            this.orientation = orientation;
            this.flexItem = flexItem;
            this.rectTransform = rectTransform;
        }
        public readonly Orientation orientation;
        public readonly FlexItem flexItem;
        public RectTransform rectTransform { get; set; }
    }

    static void Apply(FlexItem flexItem, Orientation orientation)
    {
        var children = flexItem.transform.Cast<RectTransform>()
            .Select(rectTransform => new FlexBoxElement(
                orientation,
                rectTransform.GetComponent<FlexItem>(),
                rectTransform))
            .Where(element => element.flexItem != null);

        //var flexLine = new FlexLine(orientation, children);
    }

    static void Flexboxfy(Transform root, Orientation orientation)
    {
        foreach(Transform transform in root)
        {
            var flexItem = transform.GetComponent<FlexItem>();
            if(flexItem != null)
            {

            }
        }
    }

    static IEnumerable<FlexBoxElement> ToElements(Transform transform, Orientation orientation)
    {
        return transform.Cast<RectTransform>()
            .Select(rectTransform => new FlexBoxElement(
                orientation,
                rectTransform.GetComponent<FlexItem>(),
                rectTransform))
            .Where(element => element.flexItem != null);

    }

    static bool HasChild(Transform transform)
    {
        return transform.GetComponentInChildren<FlexItem>() != null;
    }

    public static List<FlexLine> Split(Transform root, Orientation orientation)
    {
        var lines = new List<FlexLine>();

        var elements = ToElements(root, orientation);

        var space = root.GetComponent<RectTransform>().sizeDelta.x;
        var flexLine = new FlexLine(orientation, elements, space);
        foreach(var element in elements)
        {
            var elms = ToElements(element.rectTransform, orientation);
            foreach(var elm in elms)
            {
                new FlexLine(orientation, elms, element.flexItem.freeSpace);
                break;
            }
        }
        return lines;
    }

    static IEnumerable<T> GetComponentInChildrenOnly<T>(RectTransform root)
    {
        return
            from children in root.Cast<RectTransform>()
            let component = children.GetComponentInChildren<T>()
            where component != null
            select component;
    }

    static void ToFlexLine(IEnumerable<RectTransform> items)
    {
        var flexItemTransforms = items
            .Select(transform => new
            {
                transform,
                flexItem = transform.GetComponent<FlexItem>()
            })
            .Where(layout => layout.flexItem != null);
        foreach(var layout in flexItemTransforms)
        {
        }
    }

    public static void Execute(Transform root, float space)
    {
        var tree = new FlexItemTree(root);
        var children = tree.GetChildren();
        if(!children.Any()) { return; }

        Resize(children, space);
        Apply(children, space);

        foreach(var child in children)
        {
            Execute(child.transform, child.freeSpace);
        }
    }

    static void Resize(List<FlexItem> flexItems, float space)
    {
        var unitSize = space / flexItems.Count;
        foreach(var flexItem in flexItems)
        {
            flexItem.SetSize(unitSize);
        }
    }

    static void Apply(List<FlexItem> flexItems, float space)
    {
        if(!flexItems.Any()) { return; }

        var firstItemOffset = flexItems.First().GetSize() / 2f;
        var offset = -space / 2f + firstItemOffset;
        foreach(var flexItem in flexItems)
        {
            flexItem.SetPosition(offset, 0);
            flexItem.Resize();
            offset += flexItem.GetSize();
        }
    }

    FlexLine(Orientation orientation, IEnumerable<FlexBoxElement> elements, float space)
    {
        this.orientation = orientation;
        var list = elements.ToList();
        var unitSize = space / list.Count;
        foreach(var element in list)
        {
            element.flexItem.SetSize(unitSize);
            if(element.orientation == Orientation.Horizontal)
            {
                //element.flexItem.basis = element.rectTransform.sizeDelta.x;
            }
            else
            {
                //element.flexItem.basis = element.rectTransform.sizeDelta.y;
            }
        }

        var offset = -space / 2f + list[0].flexItem.GetSize() / 2f;
        foreach(var element in list)
        {
            var size = element.flexItem.GetSize();
            element.rectTransform.localPosition = new Vector3(offset, 0, 0);
            element.rectTransform.sizeDelta = new Vector2(element.flexItem.GetContentSize(), element.rectTransform.sizeDelta.y);
            offset += element.flexItem.GetSize();
        }
    }

}
