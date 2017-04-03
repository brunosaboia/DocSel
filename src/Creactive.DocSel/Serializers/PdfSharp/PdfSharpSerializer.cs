using Creactive.DocSel.Serializers;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace Creactive.DocSel.PdfSharp
{
    /// <summary>
    /// Represents a class that helps serializing a Pdf document using PdfSharp
    /// </summary>
    public static class PdfSharpSerializer
    {
        /// <summary>
        /// Writes a PDF document from an object
        /// </summary>
        /// <typeparamref name="T"/>
        /// <param name="obj">The object to construct the PDF from</param>
        /// <param name="sourcePath">The path of the source (template) PDF</param>
        /// <param name="destinationPath">The path to write the document to</param>
        /// <param name="flatten">Determines whether to flatten (i.e. disallow further changes) to a document</param>
        public static void WriteDocumentFromClass<T>(object obj, string sourcePath, string destinationPath, bool flatten = true) where T : class
        {
            var valuesDict = DocumentDictionaryGenerator.GenerateValuesDictionary<T>(obj);

            PdfSharpHelper.WritePdfValues(sourcePath, destinationPath, valuesDict, flatten);
        }

        /// <summary>
        /// Creates a serialized <see cref="PdfDocument"/> from an object and a given PDF boilerplate file
        /// </summary>
        /// <typeparamref name="T"/>
        /// <param name="obj">The object to create the PDF document from</param>
        /// <param name="sourcePath">The path of the source (template) PDF document</param>
        /// <param name="flatten">Determines whether to flatten (i.e. disallow further changes) to a document</param>
        /// <returns>A serialized <see cref="PdfDocument"/> from the object</returns>
        public static PdfDocument DocumentFromClass<T>(object obj, string sourcePath, bool flatten = true) where T : class
        {
            var doc = PdfReader.Open(sourcePath, PdfDocumentOpenMode.Modify);

            return DocumentFromClass<T>(obj, doc, flatten);
        }

        /// <summary>
        /// Serializes an object into a <see cref="PdfDocument"/>
        /// </summary>
        /// <typeparamref name="T"/>
        /// <param name="obj">The object to serializes</param>
        /// <param name="source">The source (template) <see cref="PdfDocument"/></param>
        /// <param name="flatten">Determines whether to flatten (i.e. disallow further changes) to a document</param>
        /// <returns></returns>
        public static PdfDocument DocumentFromClass<T>(object obj, PdfDocument source, bool flatten = false) where T : class
        {
            var valuesDict = DocumentDictionaryGenerator.GenerateValuesDictionary<T>(obj);

            PdfSharpHelper.WritePdfValues(source, valuesDict, flatten);

            return source;
        }
    }
}
