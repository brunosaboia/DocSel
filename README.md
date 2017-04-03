# DocSel #
DocSel is a library which aims to make it easy to create document files (PDF, DOCX) from an object.

## Details ##
Chances are that a developer will have to handle with document filling during his coding adventures. This library helps this task, providing an easy way to create document files from a class.

Currently, for PDF, the [PDFsharp](https://github.com/empira/PDFsharp "PDFSharp") library is the only one supported, but we plan to add [iTextSharp](https://github.com/itext/itextsharp "iTextSharp") in a near future.

There is no support for DOCX, but we will add it soon.


## Features ##
Currently, DocSerializer can serialize the following object types:

- String (text fields);
- Enum (multiple choice);
- Bool (yes/no or similar);
- Byte[] (for images) (**experimental**);
- KeyValuePair (for when an answer can go in different fields);
- IList<T> (for a list with sequential integer values for field names);


 

 