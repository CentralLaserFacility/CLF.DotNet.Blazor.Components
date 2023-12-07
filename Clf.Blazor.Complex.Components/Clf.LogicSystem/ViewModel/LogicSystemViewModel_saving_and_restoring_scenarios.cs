//
// LogicSystemViewModel_saving_and_restoring_scenarios.cs
//

using Clf.LogicSystem.Common.ExtensionMethods;
using Clf.Common.ExtensionMethods;
using Clf.Common.MenuHandling;

namespace Clf.LogicSystem.ViewModel
{

  public partial class LogicSystemViewModel
  {

    internal string PathToScenariosDirectory => $"C:\\temp\\MachineSafety\\{LogicSystem.LogicSystemClassName}\\Scenarios\\" ;

    // FIX_THIS : do it via a context menu activated by a right-click on the background ...
    // COMPUTE ONCE, THEN ADD TO CONTEXT MENU - LAZILY EVALUATED ...

    public void ScanLogicSystem_BuildingAllScenarioMenuItems ( out MenuDescriptor? scenariosMenuToHoldAdditionalItems )
    {
      // FIX_THIS : have the client check for the existence of the Menu !!!
      var menuCreated = UiServices.MenuDescriptorsProvider?.GetMenuDescriptorIfAvailable("Scenarios") ;
      if ( menuCreated != null )
      {
        LogicSystem?.ScenarioCollections.ForEachItem(
          scenariosCollection => {
            // FIX_THIS ...
            var menuItemForThisScenariosCollection = menuCreated.AddNestedMenu(
              scenariosCollection.Name
            ) ;
            scenariosCollection.AvailableScenarios.ForEachItem(
              scenario => {
                menuItemForThisScenariosCollection.AddActionItem(
                  scenario.Name,
                  () => InvokeScenario_LoadingResultAsCurrentLogicSystem(
                    scenario
                  )
                ) ;
              }
            ) ;
          }
        ) ;
      }
      scenariosMenuToHoldAdditionalItems = menuCreated ;
    }

    public void ScanLogicSystem_BuildingAllScenarioMenuItemsEx ( MenuDescriptor scenariosMenu )
    {
      LogicSystem?.ScenarioCollections.ForEachItem(
        scenariosCollection => {
          var menuItemForThisScenariosCollection = scenariosMenu.AddNestedMenu(
            scenariosCollection.Name
          ) ;
          scenariosCollection.AvailableScenarios.ForEachItem(
            scenario => {
              menuItemForThisScenariosCollection.AddActionItem(
                scenario.Name,
                () => InvokeScenario_LoadingResultAsCurrentLogicSystem(
                  scenario
                )
              ) ;
            }
          ) ;
        }
      ) ;
    }

    internal void ApplyScenarioFromJsonFile ( 
      string                         fullPathToJsonFile, 
      System.Action<string,string?>? scenarioResultAction = null 
    ) {
      tryAgain:
      try
      {
        string? discrepancyTextLines = null ;
        Clf.LogicSystem.ComputedNodeValueDiscrepancy? discrepancy = null ;
        var scenarioDescriptor = Clf.Common.Helpers.ReadAllTextFromFile(
          fullPathToJsonFile
        ).ParsedAsJsonToCreateInstanceOf<Clf.LogicSystem.ScenarioDescriptor>() ;
        LogicSystem.ApplyScenario(
          Clf.LogicSystem.Scenario.FromScenarioDescriptor(
            scenarioDescriptor,
            LogicSystem
          ),
          scenarioOutcomeNotAsExpected : (scenario,computedNode,expectedValue) => {
            discrepancy = new Clf.LogicSystem.ComputedNodeValueDiscrepancy(
              scenario,
              computedNode.PropertyName,
              expectedValue, // arsedAs<bool?>(),
              computedNode.ValueAsString
            ) ;
            // TODO **************************
            discrepancyTextLines = discrepancy.ToString_ForMessageBoxBodyText() ;
            // discrepancyTextLines = new[]{
            //   $"Value of {computedNode.PropertyName} :",
            //   $"Scenario tells us to expect {MachineSafety.LogicHelpers.GetValueAsStringXX(expectedValue)}", 
            //   $"Actual Clf.Clf.LogicSystem value is {computedNode.ValueAsString}"
            // }.AsSingleLine("\r\n") ;
          }
        ) ;
        RebuildEntireDiagram() ;
        scenarioResultAction?.Invoke(scenarioDescriptor.ScenarioName,discrepancyTextLines) ;
        MainDescriptionTextLine = (
          $"Restored from '{scenarioDescriptor.ScenarioName}' ; {scenarioDescriptor.Description}" 
        ) ;
        if ( 
           discrepancyTextLines is not null
        && scenarioResultAction is null 
        ) {
          var result = UiServices.DialogHandler.ShowMessageBox_AskingYesOrNo(
            $"Discrepancy in scenario '{scenarioDescriptor.ScenarioName}' - fix it now ??",
            discrepancyTextLines
          ) ;
          if ( result is true )
          {
            Clf.Common.Helpers.StartNotepadPlusPlus_AtFirstOccurrenceOfText(
              fullPathToJsonFile,
              discrepancy!.ComputedLogicNodePropertyName
            ).WaitForExit() ;
            goto tryAgain ; 
          }
        }
      }
      catch ( System.Exception x )
      {
        UiServices.DialogHandler.ShowMessageBox(
          $"Syntax error in scenario file : '{fullPathToJsonFile.Substring(PathToScenariosDirectory.Length)}'",
          x.Message
        ) ;
        Clf.Common.Helpers.StartWindowsNotepad(
          fullPathToJsonFile
        ) ;
      }
    }

    private void InvokeScenario_LoadingResultAsCurrentLogicSystem ( Clf.LogicSystem.Scenario scenario ) 
    {
      LogicSystem.ApplyScenario(scenario) ;
      RebuildEntireDiagram() ;
      string descriptionIfAvailable = (
        scenario.Description is null 
        ? ""
        : $"( {scenario.Description} )"
      ) ;
      MainDescriptionTextLine = (
        $"Scenario : {scenario.Name} {descriptionIfAvailable}"
      ) ;
    }

  }

}
