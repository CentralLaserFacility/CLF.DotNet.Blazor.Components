//
// VisualisationOptions.cs
//

namespace Clf.LogicSystem.ViewModel
{

  // RENAME => InteractionOptions ??

  // NEED TO REWORK THIS TO REFLECT THE OPTIONS WE ACTUALLY NEED !!!

  // FIX_THIS : ADD DEBUG VISUALISATION FLAGS (WAY-POINTS ON LINES ETC)

  public record VisualisationOptions (
    bool SupportMouseButtonInteraction,
    bool SupportReplacingCurrentNodes,
    bool SupportChangingInputValues,
    bool SupportChangingShadowValues,
    bool ShowDisabledItemsAsGreyedOut, // Instead of not showing at all ...
    bool SupportChangingVisualisationOptions
  ) {

    public static readonly VisualisationOptions Default = new(
      SupportMouseButtonInteraction       : true,
      SupportReplacingCurrentNodes        : false,
#if EPAC_SIM && ENABLE_INPUT_CHANGES
      SupportChangingInputValues: true,
#else
     SupportChangingInputValues: false,
#endif
      SupportChangingShadowValues         : false,
      ShowDisabledItemsAsGreyedOut        : true,
      SupportChangingVisualisationOptions : true
    ) ;

    public static readonly VisualisationOptions EnableAll = new(
      SupportMouseButtonInteraction       : true,
      SupportReplacingCurrentNodes        : true,
      SupportChangingInputValues          : true,
      SupportChangingShadowValues         : true,
      ShowDisabledItemsAsGreyedOut        : true,
      SupportChangingVisualisationOptions : true
    ) ;

  }

}

