using Creactive.DocSel.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Creactive.DocSel.Serializers
{
    /// <summary>
    /// A static class with methods to help creating the dictionaries necessary to serialize documents (library-agnostic)
    /// </summary>
    public static class DocumentDictionaryGenerator
    {
        /// <summary>
        /// Get the values from properties in an object
        /// </summary>
        /// <typeparamref name="T"/>
        /// <param name="obj">The object to get the values from</param>
        /// <returns>A dictionary with the field name and the value to be serialized</returns>
        public static Dictionary<string, object> GenerateValuesDictionary<T>(object obj) where T : class
        {
            return GenerateValuesDictionary<T>(obj, -1);
        }

        /// <summary>
        /// Gets the value from properties in an object which represents an element in a list
        /// </summary>
        ///  <typeparamref name="T"/>
        /// <param name="obj">The object to get the values from</param>
        /// <param name="index">The index of the object in the list</param>
        /// <returns>A dictionary with the field name and the value to be serialized</returns>
        public static Dictionary<string, object> GenerateValuesDictionary<T>(object obj, int index) where T : class
        {
            return GenerateValuesDictionary(typeof(T), obj, index);
        }

        /// <summary>
        /// Gets the value from properties in an object which represents an element in a list. If index is negative, assumes that the object is not a list element
        /// </summary>
        /// <param name="type">The type of the object to read the properties from</param>
        /// <param name="obj">The object to get the values from</param>
        /// <param name="index">The index of the object in the list</param>
        /// <returns></returns>
        public static Dictionary<string, object> GenerateValuesDictionary(Type type, object obj, int index)
        {
            var valuesDict = new Dictionary<string, object>();
            if (obj == null) return valuesDict;

            foreach (var prop in type.GetProperties())
            {
                var DocumentKeyAttributes = Attribute
                    .GetCustomAttributes(prop, typeof(DocumentFormBaseAttribute))
                    .Cast<DocumentFormBaseAttribute>();

                foreach (var DocumentKeyAttribute in DocumentKeyAttributes)
                {
                    if (DocumentKeyAttribute != null)
                    {
                        var value = prop.GetValue(obj);
                        object defaultValue;

                        if (prop.PropertyType.IsValueType)
                        {
                            defaultValue = Activator.CreateInstance(prop.PropertyType);
                        }
                        else
                        {
                            defaultValue = null;
                        }
                        if (value == null || value == defaultValue)
                        {
                            continue;
                        }

                        if (DocumentKeyAttribute is DocumentJoinValueAttribute)
                        {
                            var fieldName = (DocumentKeyAttribute as DocumentJoinValueAttribute).FieldName;
                            var joiner = (DocumentKeyAttribute as DocumentJoinValueAttribute).Joiner;

                            if (valuesDict.Keys.Contains(fieldName))
                            {
                                valuesDict[fieldName] += $"{joiner}{value}";
                            }
                            else
                            {
                                valuesDict[fieldName] = value;
                            }

                            continue;
                        }
                        else if (DocumentKeyAttribute is DocumentValueAttribute)
                        {
                            var fieldName = (DocumentKeyAttribute as DocumentValueAttribute).FieldName;
                            if (index >= 0)
                            {
                                fieldName = fieldName.Replace("{}", index.ToString());
                            }
                            valuesDict.Add(fieldName, prop.GetValue(obj));
                        }
                        else if (DocumentKeyAttribute is DocumentSplitDateAttribute)
                        {
                            if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                            {
                                var pdfDateValues = DocumentKeyAttribute as DocumentSplitDateAttribute;
                                var dateObj = (DateTime)prop.GetValue(obj);

                                if (dateObj != default(DateTime))
                                {
                                    valuesDict.Add(pdfDateValues.YearField, dateObj.Year.ToString());
                                    valuesDict.Add(pdfDateValues.MonthField, dateObj.Month.ToString());
                                    valuesDict.Add(pdfDateValues.DayField, dateObj.Day.ToString());
                                }
                            }
                        }
                        else if (DocumentKeyAttribute is DocumentSplitBoolAttribute)
                        {
                            var boolVal = (bool)value;

                            valuesDict.Add((DocumentKeyAttribute as DocumentSplitBoolAttribute).TrueField, boolVal);
                            valuesDict.Add((DocumentKeyAttribute as DocumentSplitBoolAttribute).FalseField, !boolVal);
                        }
                        else if (DocumentKeyAttribute is DocumentListAttribute)
                        {
                            var elements = value as IList;
                            int i = (DocumentKeyAttribute as DocumentListAttribute).InitialIndex;

                            foreach (var element in elements)
                            {
                                if (element == null) continue;
                                GenerateValuesDictionary(element.GetType(), element, i)
                                    .ToList()
                                    .ForEach(kvp => valuesDict[kvp.Key] = kvp.Value);

                                i++;
                            }
                        }
                        else if (DocumentKeyAttribute is DocumentEnumAttribute)
                        {
                            var enumValues = ((DocumentEnumAttribute)DocumentKeyAttribute).EnumDictionary;

                            // If we are not using custom mapping, only a single value is allowed.
                            // This is due to the way enums work in .NET. If not set explicitly, their
                            // value starts in zero, and increase by one at each new enum (such as {0,1,2,...})
                            // Because of that, we cannot perform bitwise operations. We need it in order to have
                            // multiple values. Refer to https://msdn.microsoft.com/en-us/library/sbbt4032.aspx
                            // for more information.

                            if(enumValues != null)
                            {
                                foreach (var kvp in enumValues)
                                {
                                    if ((kvp.Value & (int)value) != 0)
                                    {
                                        valuesDict.Add(kvp.Key, true);
                                    }
                                }
                            }
                            else
                            {
                                foreach (var enumValue in Enum.GetValues(value.GetType()))
                                {
                                    if(((int)enumValue & (int)value) != 0)
                                    {
                                        valuesDict.Add(Enum.GetName(value.GetType(), enumValue), true);
                                    }
                                }
                            }
                            
                        }
                        else if (DocumentKeyAttribute is DocumentKeyValueAttribute)
                        {
                            var kvp = value as KeyValuePair<string, string>?;

                            if (kvp != null)
                            {
                                if (kvp.HasValue)
                                {
                                    valuesDict.Add(kvp.Value.Key, kvp.Value.Value);
                                }
                            }
                        }
                    }
                }
            }

            return valuesDict;
        }
    }
}
