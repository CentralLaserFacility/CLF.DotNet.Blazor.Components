//
// ILogicNodeViewModel.cs
//

using Clf.ChannelAccess;
using System.Collections.Generic;

namespace Clf.LogicSystem.ViewModel
{

  // ILogicSystemViewModelElement ???

  //
  // Most of these properties come from the LogicNode that we're observing,
  // and are not changed via UI interactions, at least not directly.
  // So they only have 'getters', and those getters retrive values
  // by querying the LogicNode properties.
  //
  // The infrastructure arranges to invoke 'PropertyChanged' whenever
  // it's necessary to obtain updated values.
  //
  // There are complex interrelationships between these properties,
  // for example between the 'primary' properties and the 'derived' properties.
  // However in practice, when we raise a PropertyChanged event we specify
  // a null string for the PropertyName, and that is taken to mean that
  // all the properties have changed. Because of that, we don't need to 
  // be explicit about the 'derived' changes that flow from a 'primary' change.
  //

  public interface ILogicNodeViewModel : System.ComponentModel.INotifyPropertyChanged
  {

    //
    // These properties come from the LogicNode but don't change.
    //
    // Individually they are not necessarily of direct interest
    // to a visualiser, but their values contribute to information
    // displayed as the textual content of a Node, and to the tooltip.
    //

    int UniqueIntegerIdentifier { get ; }

    string PropertyName { get ; }

    string ChannelName { get ; }

    ChannelType ChannelType { get ; }

    // LogicNodeCategory LogicNodeCategory { get ; }

    Clf.LogicSystem.Common.GeomertyPrimitives.CentredRectangleF NodeOutlineRectangle { get ; }

    bool ShowAsCollapsed { get ; }

    int StackingOrderZ { get ; }

    //
    // These are the 'primary' properties that come from the LogicNode
    // and which change over time.
    //
    // Various other properties are 'derived' from these values.
    //

    string ValueAsString { get ; }

    #if SUPPORTS_SHADOW_VALUES
    bool? ShadowValue { get ; }
    #endif

    System.Type ValueType { get ; }

    object? ValueAsObject { get ; }

    IEnumerable<string> LabelTextLines { get ; }

    IEnumerable<string> ToolTipTextLines { get ; }

    #if SUPPORTS_SHADOW_VALUES
    bool IsComputedOutputNode_WhoseValueDisagreesWithShadowValue { get ; }
    #endif

    bool IsComputedOutputNode_WhoseValueIsUnexpectedlyNull { get ; }

    //
    // Derived properties - these change, depending on the values of the
    // 'primary properties' that come from the LogicNode. In principle
    // their values can be obtained from the primary properties.
    //

    Clf.LogicSystem.Common.GeomertyPrimitives.Colour NodeOutlineColour { get ; }

    Clf.LogicSystem.Common.GeomertyPrimitives.LineThickness NodeOutlineThickness { get ; }

    Clf.LogicSystem.Common.GeomertyPrimitives.Colour NodeFillColour { get ; }

    Clf.LogicSystem.Common.GeomertyPrimitives.Colour? ExternalBorderColour { get ; }

    Clf.LogicSystem.Common.GeomertyPrimitives.LineThickness ExternalBorderThickness { get ; }

    // These are 'local' properties that don't depend on the LogicNode's state
    // and are purely to do with the visual presentation.

    Clf.Common.UI.HighlightingOption HighlightingChoice { get ; set ; } 
   
  }

}

