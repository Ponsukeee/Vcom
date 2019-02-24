using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace VRUtils.Components
{
public class GroupList<TRoot, TItem>
{
    private readonly Dictionary<TRoot, List<TItem>> groupDictionary = new Dictionary<TRoot, List<TItem>>();
    private readonly List<TRoot> _rootList = new List<TRoot>();
    public TRoot CurrentRoot { get; private set; }
    public TItem CurrentItem { get; private set; }

#region Utility

    public int IndexOf(TItem item)
    {
        return groupDictionary[CurrentRoot].IndexOf(item);
    }

    public int IndexOfCurrentItem()
    {
        return groupDictionary[CurrentRoot].IndexOf(CurrentItem);
    }

    public int IndexOfCurrentGroup()
    {
        return _rootList.IndexOf(CurrentRoot);
    }

    public int IndexOfInGroup(TRoot root)
    {
        return _rootList.IndexOf(root);
    }

    public int ListCount(TRoot root)
    {
        if (CurrentItem == null) return 0;
        return groupDictionary[root].Count;
    }

    public int CurrentListCount()
    {
        if (CurrentItem == null) return 0;
        return groupDictionary[CurrentRoot].Count;
    }

    public int GroupCount()
    {
        if (CurrentRoot == null) return 0;
        return groupDictionary.Count;
    }

    public TRoot GetRoot(TItem target)
    {
        foreach (var pair in groupDictionary)
        {
            foreach (var item in pair.Value)
            {
                if (ReferenceEquals(target, item))
                    return pair.Key;
            }
        }

        return default(TRoot);
    }

    public List<TRoot> GetRootList()
    {
        return _rootList.ToList();
    }

    public TRoot GetNextGroup()
    {
//        var rootList = groupDictionary.Keys.ToList();
        if (_rootList.IndexOf(CurrentRoot) == _rootList.Count - 1)
        {
            Debug.Assert(_rootList.IndexOf(CurrentRoot) < _rootList.Count - 1);
            return CurrentRoot;
        }
        
        return _rootList[_rootList.IndexOf(CurrentRoot) + 1];
    }
    
    public TRoot GetPreviousGroup()
    {
//        var _rootList = groupDictionary.Keys.ToList();
        if (_rootList.IndexOf(CurrentRoot) == 0)
        {
            Debug.Assert(_rootList.IndexOf(CurrentRoot) > 0);
            return CurrentRoot;
        }
        
        return _rootList[_rootList.IndexOf(CurrentRoot) - 1];
    }

    public TItem GetItem(int index)
    {
        return groupDictionary[CurrentRoot][index];
    }

    public TItem GetItem(int index, TRoot root)
    {
        return groupDictionary[root][index];
    }

#endregion

#region ChangeCurrent

    public void ChangeToNextItem(bool canLoop = false)
    {
        if (ListCount(CurrentRoot) < 2 && CurrentItem == null) return;

        CurrentItem = groupDictionary[CurrentRoot].NextItem(CurrentItem, canLoop);
    }

    public void ChangeToPreviousItem(bool canLoop = false)
    {
        if (ListCount(CurrentRoot) < 2 && CurrentItem == null) return;

        CurrentItem = groupDictionary[CurrentRoot].PreviousItem(CurrentItem, canLoop);
    }

    public void ChangeToNextGroup(int indexOfItem = 0, bool canLoop = false)
    {
        if (groupDictionary.Count < 2 && CurrentRoot == null) return;

//        var keys = groupDictionary.Keys.ToList();
        CurrentRoot = _rootList.NextItem(CurrentRoot, canLoop);
        CurrentItem = groupDictionary[CurrentRoot][indexOfItem];
    }

    public void ChangeToPreviousGroup(int indexOfItem = 0, bool canLoop = false)
    {
        if (groupDictionary.Count < 2 && CurrentRoot == null) return;

//        var keys = groupDictionary.Keys.ToList();
        CurrentRoot = _rootList.PreviousItem(CurrentRoot, canLoop);
        CurrentItem = groupDictionary[CurrentRoot][indexOfItem];
    }

#endregion

#region Add

    public void AddItemToNewGroup(TRoot root, TItem item, int indexOfGroup, bool isSwapping = false)
    {
        groupDictionary.Add(root, new List<TItem>());
        groupDictionary[root].Add(item);
        _rootList.Insert(indexOfGroup, root);
        if (isSwapping)
        {
            CurrentRoot = root;
            CurrentItem = item;
        }
    }

    public void AddItemToCurrentGroup(TItem item, int index = 0, bool isSwapping = false)
    {
        groupDictionary[CurrentRoot].Insert(index, item);
        if (isSwapping)
        {
            CurrentItem = item;
        }
    }

#endregion

#region Remove

    public void RemoveAllGroup()
    {
        if (groupDictionary.Count == 0) return;

        CurrentRoot = default(TRoot);
        CurrentItem = default(TItem);
        groupDictionary.Clear();
        _rootList.Clear();
    }

    public void RemoveCurrentItem(bool isNext = false)
    {
        if (CurrentItem == null) return;

        var list = groupDictionary[CurrentRoot];
        if (list.Count == 1)
        {
            RemoveCurrentGroup();
            return;
        }

        var removingItem = CurrentItem;
        CurrentItem = GetInsted(removingItem, list, isNext);
        list.Remove(removingItem);
    }

    public void RemoveCurrentItem(TItem item)
    {
        groupDictionary[CurrentRoot].Remove(item);
    }

    public void RemoveCurrentGroup(int nextIndex = 0, bool isNext = false)
    {
        if (groupDictionary.Count < 2)
        {
            RemoveAllGroup();
            return;
        }

        var removingRoot = CurrentRoot;
        CurrentRoot = GetInsted(removingRoot, _rootList, isNext);
        CurrentItem = groupDictionary[CurrentRoot][nextIndex];
        groupDictionary.Remove(removingRoot);
        _rootList.Remove(removingRoot);
    }

    private T GetInsted<T>(T removing, List<T> list, bool isNext = false)
    {
        if (isNext)
        {
            if (list.IndexOf(removing) == list.Count - 1)
                return list.PreviousItem(removing);
            
            return list.NextItem(removing);
        }

        if (list.IndexOf(removing) == 0)
            return list.NextItem(removing);
            
        return list.PreviousItem(removing);
    }

#endregion
}
}