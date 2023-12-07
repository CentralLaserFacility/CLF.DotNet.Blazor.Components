//
// ComputedNode_T.cs
//

using System;
using System.Runtime.CompilerServices;
using Clf.LogicSystem.Miscellaneous;
using FluentAssertions;

namespace Clf.LogicSystem.LogicNodes
{

  // Here 'T' can be any ValueType, eg int/double/bool.
  // It can even be an 'enum' type. However just like 'InputNode_MaybeNull',
  // we aren't able to provide properties that apply only for 'enum' types.
  // Maybe we should consider creating a 'ComputedNode_Enum<TEnum>' type
  // following the pattern we've used for 'InputNode_EnumOrNull'.

  public class ComputedNode<TValue> : ComputedNodeBase where TValue : struct
  {

    public ComputedNode ( 
      LogicSystemBase     logicSystem,
      string              propertyName,
      System.Func<TValue> computeValueFunc,
      string              formulaTextLines,
      System.Action?      valueChangedAction = null
    ) :
    base(
      logicSystem,
      propertyName,
      formulaTextLines,
      valueChangedAction = null
    ) {
      ComputeValueFunc = computeValueFunc ;
      // Just a paranoid check !!
      TValue vA = default(TValue) ;
      TValue vB = default(TValue) ;
      bool defaultInstancesCompareAsEqual = vA.Equals(vB) ;
      defaultInstancesCompareAsEqual.Should().BeTrue() ;
    }

    public override void Initialise ( )
    {
      m_computedValue_cached           = default ;
      m_computedValue_previous         = default ;
      m_cachedValueIsBelievedToBeValid = false ;
    }

    public System.Func<TValue> ComputeValueFunc { get ; }

    private TValue m_computedValue_cached = default ; 

    private bool m_cachedValueIsBelievedToBeValid = false ;

    public TValue Value
    {
      get
      {
        if ( LogicNodesManager.IsBuildingDependencies )
        {
          // We have already called SetCacheAndBuildDependencyLinks() method explicitly.
        }
        else 
        {
          //
          // This is a normal runtime access of the computed value.
          //
          // When we determined that the computed value might be out of date,
          // because one of the contributing inputs suffered a change, all we did 
          // was set the 'm_cachedValueIsBelievedToBeValid' flag to false, 
          // rather than recompute the value. So now that someone is wanting
          // to access the Value, we need to ensure that it's valid.
          //
          if ( m_cachedValueIsBelievedToBeValid )
          {
            if ( LogicSystemConfigurationSettings.Instance.DoParanoidCheckWhenReturningCachedComputedValue )
            {
              // If we're paranoid we check at this point that the cached value
              // really *is* the same as the one we get by running 'ComputeValueFunc()' again ...
              var actualCurrentValue = ComputeValueFunc() ;
              m_computedValue_cached.Should().Be(actualCurrentValue) ;
            }
          }
          else
          {
            m_computedValue_cached = ComputeValueFunc() ;
            m_cachedValueIsBelievedToBeValid = true ;
          }
        }
        return m_computedValue_cached ;
      }
    }

    public override System.Type ValueType => typeof(TValue) ;

    public override object? ValueAsObject => Value ;

    public override string ValueAsString => LogicHelpers.GetValueAsString(Value) ;

    public override object? DefaultValueAsObject => default(TValue) ;
    
    public static implicit operator TValue ( ComputedNode<TValue> computedNode ) => computedNode.Value ;

    private TValue m_computedValue_previous = default ; 

    public TValue Value_Previous => m_computedValue_previous ;

    public override void SetPreviousValueFromCurrentValue ( ) 
    => m_computedValue_previous = m_computedValue_cached ; 

    //
    // THIS COMPARISON IS CRUCIAL !!
    //
    // We're relying on the fact that TValue is constrained to be a struct,
    // ie the values we're comparing are all 'System.ValueType'. 
    // The 'Equals' method invoked here has been overridden by
    // System.ValueType, and returns true if the two objects are equal,
    // for *any* 'struct' type eg int, double, enum.
    // https://learn.microsoft.com/en-us/dotnet/api/system.valuetype.equals?view=net-7.0
    //

    public override bool CurrentValueIsDifferentFromPrevious 
    => (
      ! m_computedValue_cached.Equals(m_computedValue_previous)        
    ) ;

    public override bool CachedComputedValueIsDefinitelyCorrect => (
      m_computedValue_cached.Equals(
        ComputeValueFunc()
      )
    ) ;

    public override bool CachedComputedValueIsBelievedToBeCorrect => m_cachedValueIsBelievedToBeValid is true ;

    public override bool CachedComputedValueMightBeOutOfDate => m_cachedValueIsBelievedToBeValid is false ;

    public override void DeclareCachedComputedValueMightBeOutOfDate ( )
    {
      m_cachedValueIsBelievedToBeValid = false ;
    }

    public static void Test ( )
    {
      var node = new ComputedNode<int>(null!,"xx",()=>123,"") ;
    }

    /// <summary>
    /// Build all dependency node links for the computed node
    /// </summary>
    public void SetCacheAndBuildDependencyLinks()
    {
        if (LogicNodesManager.IsBuildingDependencies)
        {
            LogicNodesManager.OnNodeValueBeingAccessed(this);
            LogicNodesManager.OnNodeValueBeingComputed_Begin(this);

            if (NodesAccessedInComputedValueFuncXX != null)
                foreach (var inputNode in NodesAccessedInComputedValueFuncXX)
                {
                    LogicNodesManager.OnNodeValueBeingAccessed(inputNode);
                }

            m_computedValue_cached = ComputeValueFunc();

            LogicNodesManager.OnNodeValueBeingComputed_End(this);
            m_computedValue_previous = m_computedValue_cached;
            m_cachedValueIsBelievedToBeValid = true;
        }
    }
  }

}
