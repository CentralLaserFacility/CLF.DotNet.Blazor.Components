//
// LinkedItem_ExtensionMethods.cs
//

using Clf.Common.ExtensionMethods;
using System.Collections.Generic;

namespace Clf.LogicSystem.Common.ExtensionMethods
{

  public static partial class LinkedItem_ExtensionMethods
  {

    //
    // These helper functions assume that the 'source' item has 'links'
    // to other items, which can be obtained by calling the 'getLinkedItems'
    // function passing the 'source' in as an argument :
    //
    //  IEnumerable<TCustomAttribute> linkedItems = getLinkedItemsFunc(source) ;
    //
    // For example a source might expose a function that returns
    //   - all its 'child' items
    //   - all items that it uses for some purpose, eg to compute a Value
    //   - all items that depend on it, eg items that depend on its properties
    //
    // The 'Visit' functions just invoke an Action on each item. Typically the action
    // would add the item to a collection being built up.
    //
    // If we specify 'visitRecursively', then the visited items are themselves visited.
    // Effectively we perform a depth-first scan of an entire tree of items.
    //

    //
    // This applies a 'visit action' to all the linked items.
    //

    public static void VisitLinkedItems<TSource,TLinked> (
      this TSource                              sourceItem,
      System.Func<TSource,IEnumerable<TLinked>> getLinkedItemsFunc,
      System.Action<TLinked>                    visitAction,
      bool                                      visitRecursively
    )
    where TLinked : TSource
    {
      getLinkedItemsFunc(sourceItem).ForEachItem(
        (linkedItem) => {
          visitAction(linkedItem) ;
          if ( visitRecursively )
          {
            // Immediately after visiting an item, we visit any linked items
            // that it has. This achieves a depth-first scan of an entire tree.
            VisitLinkedItems(
              linkedItem,
              getLinkedItemsFunc,
              visitAction,
              visitRecursively
            ) ;
          }
        }
      ) ;
    }

    public static void AccumulateLinkedItems<TSource,TLinked> (
      this TSource                              sourceItem,
      System.Func<TSource,IEnumerable<TLinked>> getLinkedItemsFunc,
      HashSet<TLinked>                          linkedItemsSetBeingBuilt,
      bool                                      accumulateRecursively
    )
    where TLinked : TSource
    {
      VisitLinkedItems(
        sourceItem,
        getLinkedItemsFunc,
        node => linkedItemsSetBeingBuilt.Add(node),
        accumulateRecursively
      ) ;
    }

    public static IEnumerable<TLinked> AccumulateLinkedItems<TSource,TLinked> (
      this TSource                              sourceItem,
      System.Func<TSource,IEnumerable<TLinked>> getLinkedItemsFunc,
      bool                                      accumulateRecursively,
      System.Func<TLinked,bool>?                filter                 = null
    )
    where TLinked : TSource
    {
      var linkedItems = new HashSet<TLinked>() ;
      AccumulateLinkedItems(
        sourceItem,
        getLinkedItemsFunc,
        linkedItems,
        accumulateRecursively
      ) ;
      return linkedItems.WithOptionalFilterApplied(filter) ;
    }

  }

}
