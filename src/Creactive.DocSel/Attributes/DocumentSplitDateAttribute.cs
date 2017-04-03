namespace Creactive.DocSel.Attributes
{
    /// <summary>
    /// Represents a date in a document where year, month and day are in different fields.
    /// </summary>
    public class DocumentSplitDateAttribute : DocumentFormBaseAttribute
    {
        /// <summary>
        /// Gets the year field name
        /// </summary>
        public string YearField { get; private set; }
        /// <summary>
        /// Gets the month field name
        /// </summary>
        public string MonthField { get; private set; }
        /// <summary>
        /// Gets the day field name
        /// </summary>
        public string DayField { get; private set; }

        /// <summary>
        /// Creates a new instance of DocumentSplitDateAttribute with the specified year, day and month fields
        /// </summary>
        /// <param name="yearField">The year field name</param>
        /// <param name="monthField">The month field name</param>
        /// <param name="dayField">The day field name</param>
        public DocumentSplitDateAttribute(string yearField, string monthField, string dayField)
        {
            YearField = yearField;
            MonthField = monthField;
            DayField = dayField;
        }
    }
}
