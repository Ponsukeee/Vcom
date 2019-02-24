using System.Collections.Generic;

namespace VRUtils.Components
{
public static class ListExtension
{
    /// <summary>
    /// 指定した要素の次の要素を返す。指定した要素がリストの最後の場合にはインデックスが0の要素を返す。
    /// </summary>
    /// <param name="list"></param>
    /// <param name="item"></param>
    /// <param name="canLoop"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T NextItem<T>(this List<T> list, T item, bool canLoop = false)
    {
        T returnValue = item;
        var indexOfCurrentItem = list.IndexOf(item);
        if (list.Count - 1 == indexOfCurrentItem)
        {
            if(canLoop)
            {
                returnValue = list[0];
            }
        }
        else
        {
            returnValue = list[indexOfCurrentItem + 1];
        }

        return returnValue;
    }

    /// <summary>
    /// 指定した要素の一つ前の要素を返す。指定した要素のインデックスが0の場合にはリストの最後の要素を返す。
    /// </summary>
    /// <param name="list"></param>
    /// <param name="item"></param>
    /// <param name="canLoop"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T PreviousItem<T>(this List<T> list, T item, bool canLoop = false)
    {
        T returnValue = item;
        var indexOfCurrentGroup = list.IndexOf(item);
        if (indexOfCurrentGroup == 0)
        {
            if (canLoop)
            {
                returnValue = list[list.Count - 1];
            }
        }
        else
        {
            returnValue = list[indexOfCurrentGroup - 1];
        }

        return returnValue;
    }
}
}