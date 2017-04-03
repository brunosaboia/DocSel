namespace Creactive.DocSel.Attributes
{
    /// <summary>
    /// Represents a boolean value in a document, where the mutual exclusive options are in different fields
    /// </summary>
    public class DocumentSplitBoolAttribute : DocumentFormBaseAttribute
    {
        /// <summary>
        /// Gets the name of the field which represents "true" in boolean value
        /// </summary>
        public string TrueField { get; private set; }
        /// <summary>
        /// Gets the name of the field which represents "false" in boolean value
        /// </summary>
        public string FalseField { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="DocumentSplitBoolAttribute"/> with the appropriate values for "true" and "false" fields
        /// </summary>
        /// <param name="trueField">The name of the field which represents "true"</param>
        /// <param name="falseField">The name of the field which represents "false"</param>
        public DocumentSplitBoolAttribute(string trueField, string falseField)
        {
            TrueField = trueField;
            FalseField = falseField;
        }
    }
}
