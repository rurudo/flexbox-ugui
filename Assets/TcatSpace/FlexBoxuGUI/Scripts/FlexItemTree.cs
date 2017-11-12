using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FlexItemTree
{
    private FlexItem root;
    private List<FlexItem> children;

    public FlexItem Root
    {
        get
        {
            return root;
        }
    }

    public List<FlexItem> Children
    {
        get
        {
            return children;
        }
    }

    public FlexItemTree(Transform transform)
    {
        root = transform.GetComponent<FlexItem>();
        children = GetTopLevelChildren(transform);
    }

    public FlexItem GetRoot()
    {
        return Root;
    }

    void SearchTopLevelChildren(List<FlexItem> result, Transform transform)
    {
        foreach(var childrenTransform in transform.Cast<Transform>())
        {
            var hasFlexItem = childrenTransform.GetComponentInChildren<FlexItem>() != null;
            if (!hasFlexItem)
            {
                continue;
            }

            var flexItem = childrenTransform.GetComponent<FlexItem>();
            if(flexItem == null)
            {
                SearchTopLevelChildren(result, childrenTransform);
            }
            else
            {
                result.Add(flexItem);
            }
        }
    }

    List<FlexItem> GetTopLevelChildren(Transform transform)
    {
        var result = new List<FlexItem>();
        SearchTopLevelChildren(result, transform);
        return result;
    }
}
