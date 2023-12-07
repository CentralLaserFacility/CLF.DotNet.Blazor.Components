//
// InputNode_T.cs
//

using Clf.Common.ExtensionMethods;
using Clf.LogicSystem.ChangeDescriptors;
using Clf.LogicSystem.Common;
using Clf.LogicSystem.Common.ExtensionMethods;
using Clf.LogicSystem.Common.Utils;
using Clf.LogicSystem.Miscellaneous;
using System;
using System.Linq;

namespace Clf.LogicSystem.LogicNodes
{
    /// <summary>
    /// InputNode<T>, where it's understood that any Input node could have a null value ...
    /// </summary>
    /// <typeparam name="T">The type 'T' can be an enum or a primitive type</typeparam>
    public class InputNode<T> : InputNodeBase where T : struct
    {
        public InputNode(
          LogicSystemBase logicSystem,
          string propertyName,
          System.Action? valueChangedAction = null
        ) :
        base(
          logicSystem,
          propertyName,
          valueChangedAction
        )
        {
            SetInitialValue(null);
        }

        public static implicit operator T?( InputNode<T> node ) => node.Value;

        private T? _value;
        /// <summary>
        /// Get the input node value. 
        /// Whenever we access a value, we notify the LogicNodesManager. This lets the LogicNodesManager build the dependencies.
        /// </summary>
        public T? Value
        {
            get
            {
                LogicNodesManager.OnNodeValueBeingAccessed(this);
                return _value;
            }
        }
        
        /// <summary>
        /// Set the default value without propagating changes.
        /// </summary>
        /// <param name="value"></param>
        public void SetInitialValue(T? value)
        {
            _value = value;
        }
        
        /// <summary>
        /// Get the generic type of the input node
        /// </summary>
        public override System.Type ValueType => typeof(T?)?.GenericTypeArguments.FirstOrDefault() ?? typeof(T?);
       
        public override object? ValueAsObject => Value;

        public override string ValueAsString => LogicHelpers.GetValueAsString(Value);
        
        public override bool CanSetValue_ParsedFromString(
          string valueAsString,
          System.Action<LogicNodeChangesArisingFromInputValueChange>? changesHandler = null
        ) => valueAsString.CanParseAs<T?>(
          (parsedValue) => SetValue(
            parsedValue,
            changesHandler
          )
        );

        internal override void SetValueAsDefault_WITHOUT_PROPAGATING_CHANGES()
        {
            _value = null;
        }

        public override void SetValue(
          object? value,
          System.Action<LogicNodeChangesArisingFromInputValueChange>? changesHandler = null
        )
        {
#if SUPPORT_VALUE_VALIDITY_STATUS_CHANGE_EVENTS
        HandleValueValidityStatusChange(
          ValueIsNull,
          value 
        ) ;
#endif
            _value = value as T?;

            if (_value == null 
                && value != null)
            {
                if ($"{value}".CanParseAs(out T? parsedValue))
                {
                    _value = parsedValue;
                }
            }

            LogicNodesManager.PropagateSourceValueChange(
              this,
              changesHandler
            );
        }

        /// <summary>
        /// Set the next value to input node, it supports only boolean and enumeration type
        /// </summary>
        /// <param name="changesHandler"></param>
        public override void CycleToNextValue(
          System.Action<LogicNodeChangesArisingFromInputValueChange>? changesHandler = null
        )
        {
            if (ValueType.IsEnum)
            {
                object? valueToSet = (
                  _value.HasValue
                  ? Clf.Common.Utils.EnumHelpers.GetNextCyclicValueFromEnum(
                      _value.Value
                    )
                  : System.Enum.ToObject(
                      ValueType,
                      0
                    )
                );
                SetValue(
                  valueToSet,
                  changesHandler
                );
            }
            else if (ValueType == typeof(bool))
            {
                ToggleValue(true, changesHandler);
            }
        }

        /// <summary>
        /// Toggle input node value, it supports only applies to boolean variant.
        /// </summary>
        /// <param name="valueToSetIfNull"></param>
        /// <param name="changesHandler">Handle changes arising from input value change</param>
        public void ToggleValue(
          bool valueToSetIfNull = true,
          System.Action<LogicNodeChangesArisingFromInputValueChange>? changesHandler = null
        )
        {
            if (ValueType == typeof(bool))
            {
                bool valueToSet = (
                  _value is bool booleanValue
                  ? !booleanValue
                  : valueToSetIfNull
                );
                SetValue(
                  valueToSet,
                  changesHandler
                );
            }
        }

    }

}
