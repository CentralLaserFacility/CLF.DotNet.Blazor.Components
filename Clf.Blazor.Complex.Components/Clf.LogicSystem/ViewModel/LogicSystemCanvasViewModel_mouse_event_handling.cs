//
// LogicSystemCanvasViewModel_mouse_event_handling.cs
//

using Clf.Common.ExtensionMethods;
using Clf.LogicSystem.Common.ExtensionMethods;
using FluentAssertions;
using System.Linq;

namespace Clf.LogicSystem.ViewModel
{

  public partial class LogicSystemCanvasViewModel
  {

    // private bool m_myFlag = false ;

    // private bool? m_myTriStateFlag = null ;

    public override void HandleMouseRightButtonEvent_PopulatingContextMenu (
      Clf.Common.MenuHandling.MenuDescriptor contextMenu 
    ) { 
      var logicNetworkViewModel = Parent ;

      // JUST FOR TESTING ...
      // contextMenu.AddBooleanItem(
      //   "Boolean flag",
      //   (isChecked) => {
      //     m_myFlag = isChecked ;
      //     System.Diagnostics.Debug.WriteLine(
      //       $"m_myFlag => {m_myFlag}"
      //     ) ;
      //   },
      //   m_myFlag,
      //   "This is a tooltip"
      // ) ;

#if EPAC_SIM && ENABLE_INPUT_CHANGES
    contextMenu.AddBooleanItem(
        "Enable input changes",
        (isChecked) => {
          Parent.InputChangesAreEnabled = isChecked ;
        },
        Parent.InputChangesAreEnabled,
        "Allow input values to be changed"
      ) ;
#endif

      if ( Parent.UiServices.AvailableLogicSystemsProvider != null )
      {
        var nestedMenu = contextMenu.AddNestedMenu(
          "Select Clf.Clf.LogicSystem to display"
        ) ;
        foreach ( 
          System.Type type 
          in Parent.UiServices.AvailableLogicSystemsProvider.AvailableLogicSystemTypes 
        ) {
          nestedMenu.AddActionItem(
            type.GetTypeName(showNamespace:true),
            () => { 
              Parent.ConfigureLogicSystemAndWhichNodesToDisplay(
                (LogicSystemBase) System.Activator.CreateInstance(type)!
              ) ;
            }
          ) ; 
        }
      }

      #if false
        // Commands relating to 'selection'
        // are not necessary ... the ViewModel no longer
        // defines the properties etc that we refer to here
        contextMenu.AddSeparator("Selection") ;
        contextMenu.AddActionItem(
          "Unselect all selected nodes",
          () => { 
            logicNetworkViewModel.ClearAllNodeSelections() ;
          },
          isEnabled : logicNetworkViewModel.AnyNodesSelected
        ) ;
        contextMenu.AddActionItem(
          "Highlight all selected and related nodes",
          () => { 
            logicNetworkViewModel.ApplyHighlightingToQualifyingNodes(
              new Clf.Clf.LogicSystem.LogicNodeVisibilitySpecifier.SpecifyingAllNodesInfluencedByOrContributingToMultipleAnchorNodes(
                logicNetworkViewModel.SelectedNodes.Select(
                  viewModel => viewModel.LogicNode
                )
              )
            ) ;
          },
          isEnabled : logicNetworkViewModel.AnyNodesSelected
        ) ;
        contextMenu.AddActionItem(
          "Show just the selected and related nodes",
          () => { 
            logicNetworkViewModel.ApplyHighlightingToQualifyingNodes(
              new Clf.Clf.LogicSystem.LogicNodeVisibilitySpecifier.SpecifyingAllNodesInfluencedByOrContributingToMultipleAnchorNodes(
                logicNetworkViewModel.SelectedNodes.Select(
                  viewModel => viewModel.LogicNode
                )
              )
            ) ;
            logicNetworkViewModel.RepopulateNodesAndLinksFromSameLogicSystemButWithDifferentNodesIncluded(
              logicNetworkViewModel.AllHighlightedLogicNodes
            ) ;
          },
          isEnabled : logicNetworkViewModel.AnyNodesSelected
        ) ;
      #endif

      if ( logicNetworkViewModel.InputChangesAreEnabled )
      {
        contextMenu.AddSeparator() ;
        contextMenu.AddActionItem(
          "All nullable inputs -> null",
          () => { 
            logicNetworkViewModel.LogicSystem?.SetAllNullableInputValuesNull() ;
            logicNetworkViewModel.RebuildEntireDiagram() ;
          }
        ) ;
        contextMenu.AddActionItem(
          "All boolean inputs -> true",
          () => { 
            logicNetworkViewModel.LogicSystem?.SetAllBooleanInputValues(true) ;
            logicNetworkViewModel.RebuildEntireDiagram() ;
          }
        ) ;
        contextMenu.AddActionItem(
          "All boolean inputs -> false",
          () => { 
            logicNetworkViewModel.LogicSystem?.SetAllBooleanInputValues(false) ;
            logicNetworkViewModel.RebuildEntireDiagram() ;
          }
        ) ;
      }
      contextMenu.AddSeparator() ;
      contextMenu.AddActionItem(
        "Remove highlighting",
        () => { 
          logicNetworkViewModel.RemoveAllHighlighting() ;
        },
        isEnabled : logicNetworkViewModel.AllHighlightedNodeViewModels.Any()
      ) ;
      #if DEBUG
      contextMenu.AddActionItem(
        "Rebuild diagram (DEBUG)",
        () => { 
          logicNetworkViewModel.RebuildEntireDiagram() ;
        }
      ) ;
      contextMenu.AddActionItem(
        "Confirm highlighting (DEBUG)",
        () => { 
          logicNetworkViewModel.AllHighlightedNodeViewModels.ForEachItem(
            viewModel => (
              viewModel.HighlightingChoice is Clf.Common.UI.HighlightingOption.NotHighlighted
            ).Should().BeFalse() 
          ) ;
        },
        isEnabled : logicNetworkViewModel.AllHighlightedNodeViewModels.Any()
      ) ;
      #endif
      contextMenu.AddSeparator() ;
      contextMenu.AddActionItem(
        "Show highlighted nodes only",
        () => { 
          logicNetworkViewModel.RepopulateNodesAndLinksFromSameLogicSystemButWithDifferentNodesIncluded(
            logicNetworkViewModel.AllHighlightedLogicNodes
          ) ;
        },
        isEnabled : logicNetworkViewModel.AllHighlightedNodeViewModels.Any()
      ) ;
      contextMenu.AddActionItem(
        "Show all nodes",
        () => { 
          logicNetworkViewModel.RepopulateNodesAndLinksFromSameLogicSystemButWithDifferentNodesIncluded(
            logicNetworkViewModel.LogicSystem.AllLogicNodes
          ) ;
        }
      ) ;
      contextMenu.AddSeparator() ;
      contextMenu.AddActionItem(
        "Save as scenario file ...",
        () => { 
          var pathToScenarioSourceFilesDirectory = (
            logicNetworkViewModel.PathToScenariosDirectory
          ) ;
          // If we find 3 scenarios already in the directory,
          // which would typically be named Scenario_001, 002, 003 etc,
          // we'll suggest a name of '004'.
          int nScenariosSaved = Clf.Common.Helpers.ScanFileNamesInDirectory(
            logicNetworkViewModel.PathToScenariosDirectory,
            "*.json"
          ).Count() ;
          logicNetworkViewModel.UiServices.DialogHandler.ShowNameAndDescriptionEditingPanel(
            "Scenario name",
            $"Scenario_{nScenariosSaved+1:D3}",
            "No description supplied",
            (scenarioName,description) => {
              var scenario = new Clf.LogicSystem.Scenario(
                basedOn : Clf.LogicSystem.Scenario.AllInputsSetToDefaultValues(logicNetworkViewModel.LogicSystem),
                scenarioName
              ) {
                Description = description,
                InputNodeValueSettings = logicNetworkViewModel.LogicSystem.NodesOfType<Clf.LogicSystem.LogicNodes.InputNodeBase>().Select(
                  inputNode => new Clf.LogicSystem.NodeAndValueDescriptor(inputNode,inputNode.ValueAsString)
                ).ToList(),
                ComputedNodeValuesExpected = logicNetworkViewModel.LogicSystem.OutputLogicNodes().Select(
                  outputNode => new Clf.LogicSystem.NodeAndValueDescriptor(outputNode,outputNode.ValueAsString)
                ).ToList()
                #if SUPPORTS_SHADOW_VALUES
                ,ShadowValuesThatDisagreeWithComputedNodeOutputs = logicNetworkViewModel.Clf.Clf.LogicSystem.OutputLogicNodes().Where(
                  outputNode => outputNode.ComputedValueDisagreesWithShadowValue
                ).Select(
                  outputNode => new Clf.Clf.LogicSystem.NodeAndValueDescriptor(
                    outputNode,
                    Clf.Clf.LogicSystem.LogicHelpers.GetValueAsString(outputNode.ShadowValue)
                  )
                ).ToList()
                #endif
              } ;
              var scenarioDescriptor = scenario.AsResolvedScenarioDescriptor() ;
              string json = scenarioDescriptor.AsJsonTextForPersistence() ;
              string jsonFilePath = logicNetworkViewModel.PathToScenariosDirectory + scenarioName + ".json" ;
              Clf.Common.Helpers.WriteStringToFile(
                json,
                jsonFilePath
              ) ;
              logicNetworkViewModel.MainDescriptionTextLine = (
                $"Saved as '{scenarioDescriptor.ScenarioName}' ; {scenarioDescriptor.Description}"
              ) ;
              Clf.Common.Helpers.StartWindowsNotepad(jsonFilePath) ;
              // For the time being we'll write out source code files to the JSON directory ...
              string scenarioSourceCode = scenarioDescriptor.AsSourceCode() ;
              string sourceCodeFilePath = (
                pathToScenarioSourceFilesDirectory 
              + scenarioName + ".cs" 
              ) ;
              Clf.Common.Helpers.WriteStringToFile(
                scenarioSourceCode,
                sourceCodeFilePath
              ) ;
              Clf.Common.Helpers.StartWindowsNotepad(sourceCodeFilePath) ;
            }
          ) ;
        }
      ) ;
      var parent = contextMenu.AddNestedMenu(
        "Restore scenario from JSON file"
      ) ;
      Clf.Common.Helpers.ScanFileNamesInDirectory(
        logicNetworkViewModel.PathToScenariosDirectory,
        "*.json"
      ).ForEachItem(
        fileName => parent.AddActionItem(
          fileName.Substring(
            0,
            fileName.Length - 5 // Remove the '.json' suffix
          ),
          () => { 
            logicNetworkViewModel.ApplyScenarioFromJsonFile(
              logicNetworkViewModel.PathToScenariosDirectory + fileName
            ) ;
          }
        ) 
      ) ;
      parent = contextMenu.AddNestedMenu(
        "Edit scenario JSON file"
      ) ;
      Clf.Common.Helpers.ScanFileNamesInDirectory(
        logicNetworkViewModel.PathToScenariosDirectory,
        "*.json"
      ).ForEachItem(
        fileName => parent.AddActionItem(
          fileName.Substring(
            0,
            fileName.Length - 5 // Remove the '.json' suffix
          ),
          () => {
            Clf.Common.Helpers.StartWindowsNotepad(
              logicNetworkViewModel.PathToScenariosDirectory + fileName
            ) ;
          }
        ) 
      ) ;
      contextMenu.AddActionItem(
        "Verify all JSON scenarios",
        () => {
          Clf.Common.Helpers.ScanFileNamesInDirectory(
            logicNetworkViewModel.PathToScenariosDirectory,
            "*.json"
          ).ForEachItem(
            fileName => {
              logicNetworkViewModel.ApplyScenarioFromJsonFile(
                logicNetworkViewModel.PathToScenariosDirectory + fileName
              ) ;
            }
          ) ;
        }
      ) ;
      contextMenu.AddSeparator() ;
      var scenarioMenu = contextMenu.AddNestedMenu("Restore coded scenario") ;
      logicNetworkViewModel.ScanLogicSystem_BuildingAllScenarioMenuItemsEx(
        scenarioMenu
      ) ;
    }

    public override void HandleMouseLeftButtonEvent ( )
    { }

  }

}

