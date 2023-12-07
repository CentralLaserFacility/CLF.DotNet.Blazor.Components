//
// NodeAttributeNames.cs
//

using Clf.LogicSystem.Common.ExtensionMethods;

namespace Clf.LogicSystem.Attributes
{

  public static class NodeAttributeNames
  {

    // These attributes are assigned from a computation of the size required
    // to accommodate whatever text we're going to show as the box's Label.

    public static readonly string WidthX          = "WidthX" ;
    
    public static readonly string HeightY         = "HeightY" ;

    // These attributes are applied from the [Attribute] applied to the Property

    public static readonly string DisplayName     = typeof(DisplayNameAttribute)     .AttributeName() ;

    public static readonly string Description     = typeof(DescriptionAttribute)     .AttributeName() ;

    public static readonly string Caption         = typeof(CaptionAttribute)         .AttributeName() ;

    public static readonly string GroupName       = typeof(GroupNameAttribute)       .AttributeName() ;

    public static readonly string Formula         = typeof(FormulaAttribute)         .AttributeName() ;

    public static readonly string PositionedBelow = typeof(PositionedBelowAttribute) .AttributeName() ;

    public static readonly string ChannelName     = typeof(ChannelNameAttribute)     .AttributeName() ;

    public static readonly string ChannelType     = typeof(ChannelTypeAttribute)     .AttributeName() ;

    public static readonly string Rank            = typeof(RankAttribute)            .AttributeName() ;

    // Experimental !!!

    public static readonly string ShowAsCollapsed = typeof(ShowAsCollapsedAttribute) .AttributeName() ;

  }

}
