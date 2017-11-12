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

        var offset = flexItems.First().GetHead(space);

        foreach (var flexItem in flexItems)
        {
            flexItem.SetPosition(offset, 0);
            flexItem.Resize();
            offset += flexItem.GetOffset();
        }
    }
}
