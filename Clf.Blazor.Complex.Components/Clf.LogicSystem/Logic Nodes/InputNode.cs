//
// InputNode.cs
//

using Clf.LogicSystem.ChangeDescriptors;
using Clf.LogicSystem.Miscellaneous;

namespace Clf.LogicSystem.LogicNodes
{

    /// <summary>
    /// This provides a Value of type 'object?', which is what we receive
    /// when an input is coming from a Channel. A Computed node would connect
    /// to this and perform the conversion to the type that we hope it is.
    /// </summary>
    public class InputNode : InputNodeBase
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

        private object? _value;
        /// <summary>
        /// Get the input node value. 
        /// Whenever we access a value, we notify the LogicNodesManager. This lets the LogicNodesManager build the dependencies.
        /// </summary>
        public object? Value
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
        public void SetInitialValue(object? value)
        {
            _value = value;
        }

        public override System.Type ValueType => typeof(object);

        public override object? ValueAsObject => Value;

        public override string ValueAsString => LogicHelpers.GetValueAsString(Value);

        public override bool CanSetValue_ParsedFromString(
          string valueAsString,
          System.Action<LogicNodeChangesArisingFromInputValueChange>? changesHandler = null
        )
        { 
            SetValue(
                valueAsString,
                changesHandler
              );
            return true;
        }

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
            _value = value;
            LogicNodesManager.PropagateSourceValueChange(
              this,
              changesHandler
            );
        }

    }

}
