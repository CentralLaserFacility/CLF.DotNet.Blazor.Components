//
// IAcceptsNamedInputNodeChangeSubmissions.cs
//

using System.Collections.Generic;

namespace Clf.LogicSystem.Miscellaneous
{

  public interface IAcceptsNamedInputNodeChangeSubmissions
  {

    public void SubmitInputChange (
      string                                       inputNodePropertyNameOrChannelName,
      bool?                                        changedValue,
      System.Action<IEnumerable<(string,string)>>? changesHandler = null
    ) {
      changesHandler?.Invoke(
        new[]{
          (
            inputNodePropertyNameOrChannelName,
            LogicHelpers.GetValueAsString(changedValue)
          )
        }
      ) ;
    }

  }

}
