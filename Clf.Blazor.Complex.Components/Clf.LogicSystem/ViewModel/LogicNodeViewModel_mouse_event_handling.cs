//
// LogicNodeViewModel_mouse_event_handling.cs
//

using Clf.LogicSystem.ChangeDescriptors;
using Clf.LogicSystem.Common;
using Clf.Common.MenuHandling;
using Clf.LogicSystem.Common.Utils;
using System.Linq;
using Clf.Common.Utils;

namespace Clf.LogicSystem.ViewModel
{

  public partial class LogicNodeViewModel
  { 

    private void ShowChangeInfo ( LogicNodeChangesArisingFromInputValueChange changes )
    {
      Parent.MainDescriptionTextLine = changes.AsSingleLineSummary ;
    }

    // ?? We could provide a configuration based choice
    // for how to handle 'InputChangesAreEnabled' being false ...
    // Either (A) don't show those items at all (as we do here)
    // or (B) show them as disabled ?

    public override void HandleMouseRightButtonEvent_PopulatingContextMenu ( MenuDescriptor contextMenu ) 
    {
      WriteLogMessage(
        $"Right click on node '{PropertyName}'"
      ) ;

      contextMenu.AddSeparator() ;
      if ( 
         LogicNode is Clf.LogicSystem.LogicNodes.InputNodeBase inputNode 
      && Parent.VisualisationOptions.SupportChangingInputValues
      ) {
        // Right clicking on an 'Input' node lets you submit a different value
        if ( inputNode is Clf.LogicSystem.LogicNodes.InputNode<bool> booleanOrNullValuedInputNode )
        {
            contextMenu.AddActionItem(
                "=> true",
                () => booleanOrNullValuedInputNode.SetValue(true, ShowChangeInfo)
            );
            contextMenu.AddActionItem(
                "=> false",
                () => booleanOrNullValuedInputNode.SetValue(false, ShowChangeInfo)
            );
            contextMenu.AddActionItem(
                "=> null",
                () => booleanOrNullValuedInputNode.SetValue(null, ShowChangeInfo)
            );
        }
        else if (inputNode.ValueType is System.Type enumOrNullValuedInputNode && enumOrNullValuedInputNode.IsEnum )
        {
          foreach ( var enumNameAndValue in EnumHelpers.GetNamesAndIntegerValuesForEnumType(enumOrNullValuedInputNode) )
          {
            contextMenu.AddActionItem(
              $"=> {enumNameAndValue.Item1} ({enumNameAndValue.Item2})",
              () => inputNode.SetValue(enumNameAndValue.Item2, ShowChangeInfo)
            ) ;
          }
          contextMenu.AddActionItem(
            "=> null",
            () => inputNode.SetValue("null",ShowChangeInfo)
          ) ;
        }
        else if ( 
           inputNode is Clf.LogicSystem.LogicNodes.InputNode<double> doubleValuedInputNode 
        && Parent.VisualisationOptions.SupportChangingInputValues 
        ) {
          contextMenu.AddActionItem(
            "=> null",
            () => doubleValuedInputNode.SetValue((double?)null,ShowChangeInfo)
          ) ;
          contextMenu.AddActionItem(
            "Edit value ...",
            () => {
              // We should include some validation here !!
              var currentValue = doubleValuedInputNode.Value ;
              Parent.UiServices.DialogHandler.ShowTextEntryPanel(
                "Enter a 'double' value or 'null' :",
                doubleValuedInputNode.ValueAsString,
                okAction : (valueEntered) => {
                  doubleValuedInputNode.CanSetValue_ParsedFromString(
                    valueEntered,
                    ShowChangeInfo
                  ) ;
                }
              ) ;
            }
          ) ;
        }
        contextMenu.AddSeparator() ;
        contextMenu.AddActionItem(
          "Highlight influenced nodes",
          () => { 
            Parent.ApplyHighlightingToQualifyingNodes(
              new Clf.LogicSystem.LogicNodeVisibilitySpecifier.SpecifyingAllNodesInfluencedByAnchorNode(
                inputNode
              )
            ) ;
          } 
        ) ;
        contextMenu.AddActionItem(
          "Show influenced nodes only",
          () => { 
            Parent.RepopulateNodesAndLinksFromSameLogicSystemButWithDifferentNodesIncluded(
              new Clf.LogicSystem.LogicNodeVisibilitySpecifier.SpecifyingAllNodesInfluencedByAnchorNode(
                inputNode
              )
            ) ;
          } 
        ) ;
      }
      else if ( LogicNode is Clf.LogicSystem.LogicNodes.ComputedNodeBase computedLogicNode )
      {
        contextMenu.AddActionItem(
          "Highlight contributing source nodes",
          () => { 
            Parent.ApplyHighlightingToQualifyingNodes(
              new Clf.LogicSystem.LogicNodeVisibilitySpecifier.SpecifyingAllNodesContributingToAnchorNode(
                computedLogicNode
              )
            ) ;
          } 
        ) ;
        contextMenu.AddActionItem(
          "Highlight influenced nodes",
          () => { 
            Parent.ApplyHighlightingToQualifyingNodes(
              new Clf.LogicSystem.LogicNodeVisibilitySpecifier.SpecifyingAllNodesInfluencedByAnchorNode(
                computedLogicNode
              )
            ) ;
          } 
        ) ;
        contextMenu.AddActionItem(
          "Highlight all related nodes",
          () => { 
            Parent.ApplyHighlightingToQualifyingNodes(
              new Clf.LogicSystem.LogicNodeVisibilitySpecifier.SpecifyingAllNodesInfluencedByOrContributingToAnchorNode(
                computedLogicNode
              )
            ) ;
          } 
        ) ;
        contextMenu.AddSeparator() ;
        contextMenu.AddActionItem(
          "Open new window showing highlighted nodes only",
          () => { 
            Parent.UiServices.WindowOpeningService!.OpenNewWindow(
              Parent.LogicSystem,
              new Clf.LogicSystem.LogicNodeVisibilitySpecifier.SpecifyingNodeInstances(
                Parent.AllHighlightedLogicNodes
              ) 
            ) ;
          },
          isEnabled : (
             Parent.AllHighlightedLogicNodes.Any()
          && Parent.UiServices.WindowOpeningService != null
          )
        ) ;
        if ( Parent.VisualisationOptions.SupportReplacingCurrentNodes ) 
        {
          contextMenu.AddSeparator() ;
          contextMenu.AddActionItem(
            "Show contributing source nodes only",
            () => { 
              Parent.RepopulateNodesAndLinksFromSameLogicSystemButWithDifferentNodesIncluded(
                new Clf.LogicSystem.LogicNodeVisibilitySpecifier.SpecifyingAllNodesContributingToAnchorNode(
                  computedLogicNode
                )
              ) ;
            } 
          ) ;
          contextMenu.AddActionItem(
            "Show influenced nodes only",
            () => { 
              Parent.RepopulateNodesAndLinksFromSameLogicSystemButWithDifferentNodesIncluded(
                new Clf.LogicSystem.LogicNodeVisibilitySpecifier.SpecifyingAllNodesInfluencedByAnchorNode(
                  computedLogicNode
                )
              ) ;
            } 
          ) ;
          contextMenu.AddActionItem(
            "Show related nodes only",
            () => { 
              Parent.RepopulateNodesAndLinksFromSameLogicSystemButWithDifferentNodesIncluded(
                new Clf.LogicSystem.LogicNodeVisibilitySpecifier.SpecifyingAllNodesInfluencedByOrContributingToAnchorNode(
                  computedLogicNode
                )
              ) ;
            } 
          ) ;
        }
        #if SUPPORTS_SHADOW_VALUES
        if ( 
           Parent.VisualisationOptions.SupportChangingShadowValues
        && computedLogicNode.IsOutputNodeWithNoDirectDependents()
        ) {
          contextMenu.AddSeparator() ;
          contextMenu.AddActionItem(
            "Shadow value => true",
            () => computedLogicNode.ShadowValue = true 
          ) ;
          contextMenu.AddActionItem(
            "Shadow value => false",
            () => computedLogicNode.ShadowValue = false
          ) ;
          contextMenu.AddActionItem(
            "Shadow value => null",
            () => computedLogicNode.ShadowValue = null
          ) ;
        }
        #endif
      }
    }

    public override void HandleMouseLeftButtonEvent ( ) 
    { 
      WriteLogMessage(
        $"Left click on node '{PropertyName}'"
      ) ;
      if ( Parent.InputChangesAreEnabled )
      {
        if (LogicNode is Clf.LogicSystem.LogicNodes.InputNodeBase inputNode)
        {
           inputNode.CycleToNextValue(ShowChangeInfo);
        }
      }
    }

  }

}

