using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FlexItemTree
{
    private List<FlexItem> children;
    public List<FlexItem> GetChildren()
    {
        return children;
    }

    public FlexItemTree(Transform transform)
    {
        children = GetTopLevelChildren(transform);
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
