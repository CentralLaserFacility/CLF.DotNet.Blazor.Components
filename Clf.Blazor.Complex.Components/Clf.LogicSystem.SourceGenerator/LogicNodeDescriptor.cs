//
// LogicNodeDescriptor.cs
//

using System.Collections.Generic;

namespace Clf.LogicSystem.SourceGenerator
{
    /// <summary>
    /// This describes a logic node property declared in a LogicSystem.
    /// </summary>
    public abstract class LogicNodeDescriptor
    {

        public string? PropertyName { get; set; } = null;

        public string? FullyQualifiedPropertyTypeName { get; set; } = null;

        // Attributes
        private Dictionary<string, string> m_attributesDictionary = new();

        public IReadOnlyDictionary< string, string> /*Name of the attribute, and Value as a string */
          AttributesDictionary => m_attributesDictionary;

        public void AddAttribute(string name, string value)
        {
            m_attributesDictionary.Add(name, value);
        }
    
        public void OverwriteAttributes(Dictionary<string, string>  keyValuePairs)
        {
            m_attributesDictionary = keyValuePairs;
        }
    }

}