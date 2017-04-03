namespace Creactive.DocSel.Attributes
{
    /// <summary>
    /// Represents a list in a document
    /// </summary>
    public class DocumentListAttribute : DocumentFormBaseAttribute
    {
        /// <summary>
        /// Gets the initial index for the list
        /// </summary>
        public int InitialIndex { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="DocumentListAttribute"/> with zero-based index
        /// </summary>
        public DocumentListAttribute()
        {
            InitialIndex = 0;
        }

        /// <summary>
        /// Creates a new instance of <see cref="DocumentListAttribute"/> with the specified first index
        /// </summary>
        /// <param name="initialIndex">The initial index to use (usually 0 or 1)</param>
        public DocumentListAttribute(int initialIndex)
        {
            InitialIndex = initialIndex;
        }
    }
}
