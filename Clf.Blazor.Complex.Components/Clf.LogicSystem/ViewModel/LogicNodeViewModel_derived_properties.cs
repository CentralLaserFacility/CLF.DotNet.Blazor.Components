//
// LogicNodeViewModel_derived_properties.cs
//

using System.Collections.Generic;
using Clf.Common.ExtensionMethods;
using System.Linq;
using Clf.LogicSystem.Attributes;
using Clf.LogicSystem.Common.Attributes;
using Clf.LogicSystem.Miscellaneous;
using Clf.LogicSystem.LogicNodes;
using Clf.Common.Attributes;

namespace Clf.LogicSystem.ViewModel
{

  //
  // These are properties computed from the 'primary' properties
  // that shadow those in the Logic System.
  //
  // For example LabelTextLines, ToolTipTextLines, FillColour.
  //

  public partial class LogicNodeViewModel 
  { 

    //
    // Aha ! Pattern matching gives us a very slick way of mapping
    // the Value of a LogicNode to an outcome !!! This handles values
    // of type T or Nullable<T> transparently ... so there's no need
    // for shenanegans with testing for Nullable<T> and drilling into
    // the non-null Value, which is really hard anyway !!!
    //

    [ReliesOnObservableProperty(nameof(ValueAsString))]
    public Clf.LogicSystem.Common.GeomertyPrimitives.Colour NodeFillColour => LogicNode.ValueAsObject switch {
      null                  => Clf.LogicSystem.Miscellaneous.LogicSystemVisualisationSettings.Instance.ColourForNodesOfValueNull,
      true                  => Clf.LogicSystem.Miscellaneous.LogicSystemVisualisationSettings.Instance.ColourForNodesOfValueTrue,
      false                 => Clf.LogicSystem.Miscellaneous.LogicSystemVisualisationSettings.Instance.ColourForNodesOfValueFalse,
      // 2                     => Colour.Green, // Just to test whether this works for integers ... yes it does :)
      // 3                     => Colour.Red,
      System.Enum enumValue => ChooseColourForEnumNodeValue(enumValue),
      _  => Clf.LogicSystem.Miscellaneous.LogicSystemVisualisationSettings.Instance.ColourForNodesOfValueOther
    } ;

    private Clf.LogicSystem.Common.GeomertyPrimitives.Colour ChooseColourForEnumNodeValue ( System.Enum? enumValue )
    {
      string? displayColourName = enumValue?.GetCustomAttributeOrNull<DisplayColourAttribute>()?.DisplayColour ;
      return (
        displayColourName is null
        ? Clf.LogicSystem.Miscellaneous.LogicSystemVisualisationSettings.Instance.ColourForNodesOfValueEnum 
        : Clf.LogicSystem.Common.GeomertyPrimitives.Colour.FromNameOrHexEncodedString(displayColourName)
      ) ;
    }
                                     
    [ReliesOnObservableProperty(nameof(ValueAsString))]
    public IEnumerable<string> LabelTextLines => LabelTextLinesTemplate.Select(
      line => line.Replace(
        MagicStrings.VAL_PLACEHOLDER, 
        ValueAsString
      )
    ) ;
                                
    [ReliesOnObservableProperty(nameof(ValueAsString))]
    public override IEnumerable<string> ToolTipTextLines => (
      new string?[]{
        $"{LogicNode.GetType().GetTypeName()}", // #{LogicNode.UniqueIntegerIdentifierAsString}",
        LogicNode.PropertyName.PrefixedBy("PropertyName = "),
        LogicNode.OptionalAttributeValueOrNull<Clf.LogicSystem.Attributes.PositionedBelowAttribute>().PrefixedBy("PositionedBelow = "),
        LogicNode.OptionalAttributeValueOrNull<Clf.LogicSystem.Attributes.DescriptionAttribute>(),
        (
          LogicNode.OptionalAttributeValueOrNull<Clf.LogicSystem.Attributes.FormulaAttribute>()
          ?? (
            LogicNode is ComputedNodeBase computedNode
            ? computedNode.FormulaTextLine_DerivedFromLambdaFunctionBody
            : null
          )
        )
        #if SUPPORTS_SHADOW_VALUES
        ,$"Value : {LogicNode.ValueAsString}",
        (
           LogicNode is Clf.Clf.LogicSystem.ComputedNodeBase computedNode2 
        && computedNode2.ComputedValueDisagreesWithShadowValue
        )
        ? $"Computed Value '{computedNode2.ValueAsString}' disagrees with monitored PV value : '{LogicHelpers.GetValueAsString(computedNode2.ShadowValue)}'"
        : null
        #endif
      }.WhereNotNullOrEmpty()
    ) ;

    #if SUPPORTS_SHADOW_VALUES
    [ReliesOnObservableProperty(nameof(IsComputedOutputNode_WhoseValueDisagreesWithShadowValue))]
    #endif
    public Clf.LogicSystem.Common.GeomertyPrimitives.Colour? ExternalBorderColour => (
      #if SUPPORTS_SHADOW_VALUES
      IsComputedOutputNode_WhoseValueDisagreesWithShadowValue
      ? Clf.Clf.LogicSystem.LogicSystemVisualisationSettings.Instance.ColourToIndicateShadowValueDiscrepancy
      : 
      #endif
      null
    ) ;
    
  }

}

