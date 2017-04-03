namespace Creactive.DocSel.Attributes
{
    /// <summary>
    /// Represents a field which value is the concatenation of two different properties
    /// </summary>
    public class DocumentJoinValueAttribute : DocumentValueAttribute
    {
        private const string DEFAULT_JOINER = " & ";

        /// <summary>
        /// Gets or sets the joiner which will be used to concatenate values
        /// </summary>
        public string Joiner { get; private set; }
        /// <summary>
        /// Creates a new instance of <see cref="DocumentJoinValueAttribute"/> with the joiner and the document field name
        /// </summary>
        /// <param name="fieldName">The document field name</param>
        /// <param name="joiner">The joiner which will be used to concatenate values</param>
        public DocumentJoinValueAttribute(string fieldName, string joiner) : base(fieldName)
        {
            Joiner = joiner;
        }

        /// <summary>
        /// Creates a new instance of <see cref="DocumentJoinValueAttribute"/> with the document field name
        /// </summary>
        /// <param name="fieldName">The document field name</param>
        public DocumentJoinValueAttribute(string fieldName) : base(fieldName)
        {
            Joiner = DEFAULT_JOINER;
        }
    }
}
