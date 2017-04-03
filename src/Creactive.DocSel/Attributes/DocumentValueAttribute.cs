using System;

namespace Creactive.DocSel.Attributes
{
    /// <summary>
    /// Represents a document form attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class DocumentValueAttribute : DocumentFormBaseAttribute
    {
        /// <summary>
        /// Gets the associated document field name
        /// </summary>
        public string FieldName { get; private set; }

        /// <summary>
        /// Creates a new instance of DocumentValueAttribute with the given document field name
        /// </summary>
        /// <param name="fieldName">The associated document field name</param>
        public DocumentValueAttribute(string fieldName)
        {
            FieldName = fieldName;
        }

        /// <summary>
        /// Gets a unique identifier for this attribute.
        /// </summary>
        public override object TypeId
        {
            get
            {
                return this;
            }
        }
    }
}
