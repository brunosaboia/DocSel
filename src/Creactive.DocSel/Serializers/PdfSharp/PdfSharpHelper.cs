using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.AcroForms;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Creactive.DocSel.PdfSharp
{
    /// <summary>
    /// A static class with helper methods to handle Pdfs, using PdfSharp
    /// </summary>
    public static class PdfSharpHelper
    {
        /// <summary>
        /// Flattens a document (make its fields non-editable)
        /// </summary>
        /// <param name="document">The document to flatten</param>
        public static void FlattenDocument(PdfDocument document)
        {
            document.AcroForm.Fields.Names.ToList().ForEach(n =>
            {
                document.AcroForm.Fields[n].ReadOnly = true;
            });
        }

        /// <summary>
        /// Inserts a image in a given page and position
        /// </summary>
        /// <param name="document">The <see cref="PdfDocument"/> to insert the image to</param>
        /// <param name="image">The <see cref="Bitmap"/> that will be inserted</param>
        /// <param name="page">The page number</param>
        /// <param name="x">Image's X position on page</param>
        /// <param name="y">Image's Y position on page</param>
        public static void InsertImage(PdfDocument document, Bitmap image, int page, double x, double y)
        {
            using (var memStream = new MemoryStream())
            {
                image.Save(memStream, System.Drawing.Imaging.ImageFormat.Png);
                InsertImage(document, memStream, page, x, y);
            }
        }

        /// <summary>
        /// Inserts a image in a given page and position
        /// </summary>
        /// <param name="document">The <see cref="PdfDocument"/> to insert the image to</param>
        /// <param name="imageBytes">A byte array representing the image to be inserted</param>
        /// <param name="page">The page number</param>
        /// <param name="x">Image's X position on page</param>
        /// <param name="y">Image's Y position on page</param>
        public static void InsertImage(PdfDocument document, byte[] imageBytes, int page, double x, double y)
        {
            if (imageBytes != null)
            {
                using (var gfx = XGraphics.FromPdfPage(document.Pages[page]))
                using (var memStream = new MemoryStream(imageBytes))
                {
                    gfx.DrawImage(XImage.FromStream(memStream), x, y);
                }
            }
        }

        /// <summary>
        /// Inserts a image in a given page and position
        /// </summary>
        /// <param name="document">The <see cref="PdfDocument"/> to insert the image to</param>
        /// <param name="memoryStream">A <see cref="MemoryStream"/> contanining the image to be inserted</param>
        /// <param name="page">The page number</param>
        /// <param name="x">Image's X position on page</param>
        /// <param name="y">Image's Y position on page</param>
        public static void InsertImage(PdfDocument document, MemoryStream memoryStream, int page, double x, double y)
        {
            if (memoryStream != null)
            {
                using (var gfx = XGraphics.FromPdfPage(document.Pages[page]))
                {
                    gfx.DrawImage(XImage.FromStream(memoryStream), x, y);
                }
            }
        }


        /// <summary>
        /// Inserts a image in a given page and position, with a certain width and height
        /// </summary>
        /// <param name="document">The <see cref="PdfDocument"/> to insert the image to</param>
        /// <param name="imageBytes">A byte array representing the image to be inserted</param>
        /// <param name="page">The page number</param>
        /// <param name="x">Image's X position on page</param>
        /// <param name="y">Image's Y position on page</param>
        /// <param name="w">Image's width</param>
        /// <param name="h">Image's length</param>
        public static void InsertImage(PdfDocument document, byte[] imageBytes, int page, double x, double y, double w, double h)
        {
            InsertImage(document, imageBytes, page, new XRect(x, y, w, h));
        }

        public static void InsertImage(PdfDocument document, byte[] imageBytes, int page, XRect rect)
        {
            if (imageBytes != null)
            {
                using (var gfx = XGraphics.FromPdfPage(document.Pages[page]))
                using (var memStream = new MemoryStream(imageBytes))
                {
                    gfx.DrawImage(XImage.FromStream(memStream), rect);
                }
            }
        }

        /// <summary>
        /// Inserts a image in a given page and position, with a certain width and height
        /// </summary>
        /// <param name="document">The <see cref="PdfDocument"/> to insert the image to</param>
        /// <param name="memoryStream">A <see cref="MemoryStream"/> contanining the image to be inserted</param>
        /// <param name="page">The page number</param>
        /// <param name="x">Image's X position on page</param>
        /// <param name="y">Image's Y position on page</param>
        /// <param name="w">Image's width</param>
        /// <param name="h">Image's length</param>
        public static void InsertImage(PdfDocument document, MemoryStream memoryStream, int page, double x, double y, double w, double h)
        {
            if (memoryStream != null)
            {
                using (var gfx = XGraphics.FromPdfPage(document.Pages[page]))
                {
                    gfx.DrawImage(XImage.FromStream(memoryStream), x, y, w, h);
                }
            }
        }

        /// <summary>
        /// Writes a filled PDF from a boilerplate and a class
        /// </summary>
        /// <param name="sourcePath">The boilerplate PDF path</param>
        /// <param name="destinationPath">The filled PDF path</param>
        /// <param name="dict">A <see cref="Dictionary{string, object}"/> containing the fields and values to fill the PDF with</param>
        /// <param name="flatten">Indicates whether to flatten the document, that is prevet further changes after saving</param>
        public static void WritePdfValues(string sourcePath, string destinationPath, Dictionary<string, object> dict, bool flatten = true)
        {
            using (var pdfDocument = PdfReader.Open(sourcePath, PdfDocumentOpenMode.Modify))
            {
                WritePdfValues(pdfDocument, dict, flatten);
                pdfDocument.Save(destinationPath);
                pdfDocument.Close();
            }
        }

        /// <summary>
        /// Fills a <see cref="PdfDocument"/> AcroFields with the given values
        /// </summary>
        /// <param name="document">The <see cref="PdfDocument"/> to be filled</param>
        /// <param name="dict">A <see cref="Dictionary{string, object}"/> containing the fields and values to fill the PDF with</param>
        /// <param name="flatten">Indicates whether to flatten the document, that is prevet further changes after saving</param>
        public static void WritePdfValues(PdfDocument document, Dictionary<string, object> dict, bool flatten = true)
        {
            var form = document.AcroForm;
            if (form.Elements.ContainsKey("/NeedAppearances"))
            {
                form.Elements["/NeedAppearances"] = new PdfBoolean(true);
            }
            else
            {
                form.Elements.Add("/NeedAppearances", new PdfBoolean(true));
            }

            var fields = document.AcroForm.Fields;

            foreach (var kvp in dict)
            {
                if (!fields.DescendantNames.Contains(kvp.Key))
                    continue;

                fields[kvp.Key].ReadOnly = false;

                if (kvp.Value is bool)
                {
                    var chkbox = fields[kvp.Key] as PdfCheckBoxField;
                    if (chkbox != null)
                    {
                        chkbox.Checked = (bool)kvp.Value;
                    }
                }
                else if (kvp.Value is byte[])
                {
                    var field = fields[kvp.Key];

                    var pageReference = field.Elements["/P"];

                    var page = document.Pages.Cast<PdfPage>()
                        .Select((p, i) => new { Page = p, Index = i })
                        .FirstOrDefault(p => p.Page.Reference == pageReference);

                    if (page != null)
                    {
                        var rect = field.Elements["/Rect"] as PdfArray;
                        var pageHeight = page.Page.Height.Point;

                        if (rect != null)
                        {
                            // X and Y are the coordinates, H is the height of the document field
                            var x = Convert.ToDouble(rect.Elements[0].ToString());
                            var y = Convert.ToDouble(rect.Elements[1].ToString());
                            var h = Convert.ToDouble(rect.Elements[3].ToString());

                            var byteArray = kvp.Value as byte[];

                            // Since PDFSharp uses inverted origin (top-left instead of default bottom-left),
                            // the PDF rectangle structure returns a different value than PDF sharp needs in order
                            // to position the image correctly. We need then to have the page height, and offset
                            // the box size (because PDF sharp writes Y to the top-left coordinate of the image)
                            InsertImage(document, byteArray, page.Index, x, (pageHeight - (y + (h - y))));
                        }
                    }
                }
                else
                {
                    fields[kvp.Key].Value = new PdfString(kvp.Value.ToString());
                }
            }
            if (flatten)
                FlattenDocument(document);
        }
    }
}