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

    public FlexLine(Transform root, float space)
    {
        ChangeNoWrapLayout(root, space);
    }

    public float ChangeNoWrapLayout(Transform root, float space)
    {
        var tree = new FlexItemTree(root);
        var children = tree.Children;
        if(!children.Any()) { return tree.Root.GetSize(); }

        //Resize(children, space);
        Apply(children, space);

        var total = 0f;
        foreach(var child in children)
        {
            total += ChangeNoWrapLayout(child.transform, child.freeSpace);
        }
        return total;
    }

    static void SetSize(List<FlexItem> flexItems)
    {

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
            //flexItem.Resize();
            offset += flexItem.GetOffset();
        }
    }
}
