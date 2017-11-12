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
}
