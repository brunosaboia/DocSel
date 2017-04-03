using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Creactive.DocSel.Attributes
{
    /// <summary>
    /// Represents a document with multiple choose values, and its options are in different fields
    /// </summary>
    public class DocumentEnumAttribute : DocumentFormBaseAttribute
    {
        /// <summary>
        /// Gets the underlying enum dictionary
        /// </summary>
        public Dictionary<string, int> EnumDictionary { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="DocumentEnumAttribute"/> with the specified value mapping.
        /// Note: If <paramref name="setValues"/> is set to True, the string which represent key and value should be constructed in the following fashion:
        /// '{Key} = {Value}' — exact regex is (\S*)\s*=\s*([0-9]+) — , where "Key" is the field name and "Value" is the underlying Enum integer value.
        /// Otherwise, values are set to 2^n, starting with 1, as follows: {1, 2, 4, 8, 16, 32, ..., 2^n}
        /// </summary>
        /// <param name="setValues">Indicates whether to use custom values to build the Enum dictionary</param>
        /// <param name="enumValues">The list of string formatted key-value pairs</param>
        public DocumentEnumAttribute(bool setValues, params string[] enumValues)
        {
            EnumDictionary = GenerateEnumDictionary(setValues, enumValues);
        }

        public DocumentEnumAttribute()
        {
            EnumDictionary = null;
        }

        private Dictionary<string, int> GenerateEnumDictionary(bool setValues, string[] enumValues)
        {
            var enumDictionary = new Dictionary<string, int>();

            if (!setValues)
            {
                int value = 1;

                foreach (var key in enumValues)
                {
                    enumDictionary.Add(key, value);
                    value = value << 1;
                }
            }
            else
            {
                foreach (var strValue in enumValues)
                {
                    var kvpPattern = @"(\S*)\s*=\s*([0-9]+)";
                    var match = Regex.Match(strValue, kvpPattern);
                    if (match.Success)
                    {
                        if (int.TryParse(match.Groups[2].Value, out int value))
                        {
                            var key = match.Groups[1].Value;
                            enumDictionary.Add(key, value);
                        }
                    }
                }
            }

            return enumDictionary;
        }
    }
}
