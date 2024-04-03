// See https://aka.ms/netestdoc.pdfw-console-template for more information

using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Extgstate;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

Console.WriteLine("Start watermarking using iText7 (v8.0.0)");

try
{
    string location = @"C:\Users\Justi\OneDrive\Documents\Pretty Darn Fast\Book With Chapters\book pdf\";
    string fileName = "20240114_v2_Pretty Darn Fast_Entire_Book_Spread_KEEP.pdf";
    // Ideal size lndscape="DRAFT v1-Justin Jones PO7 8RT to Jane Finch 30 June 2023"
    string watermarkText = "Draft 20240114v2- amjandjjj@gmail.com to kerry@windsorcme.com";
    bool isDocumentInPortrait = false;


    string[] fileNameSplit = fileName.Split('.');
    string fullPathSrc = location + fileName;
    string fullPathDest = location + fileNameSplit[0] + $"_{DateTime.Now.Ticks}.pdf";

    Console.WriteLine($"Converting {fullPathSrc}. Output is {fullPathDest}");
    Console.WriteLine(isDocumentInPortrait ? "Portrait" : "Landscape");

    PdfDocument pdfDoc = new PdfDocument(new PdfReader(fullPathSrc), new PdfWriter(fullPathDest));
    Document document = new Document(pdfDoc);
    Rectangle pageSize;
    PdfCanvas canvas;
    int n = pdfDoc.GetNumberOfPages();
    for (int i = 1; i <= n; i++)
    {
        PdfPage page = pdfDoc.GetPage(i);
        pageSize = page.GetPageSize();
        canvas = new PdfCanvas(page);

        if (isDocumentInPortrait)
        {
            Paragraph p = new Paragraph(watermarkText).SetFontSize(32);
            canvas.SaveState();
            PdfExtGState gs1 = new PdfExtGState().SetFillOpacity(0.1f);
            canvas.SetExtGState(gs1);
            document.ShowTextAligned(p
                , pageSize.GetWidth() / 2
                , pageSize.GetHeight() / 2
                , pdfDoc.GetPageNumber(page)
                , TextAlignment.CENTER
                , VerticalAlignment.MIDDLE
                , 45);  /* angle should be in radians */
            canvas.RestoreState();
        }
        else
        {
            // Landscape
            Paragraph p = new Paragraph(watermarkText).SetFontSize(32);
            canvas.SaveState();
            PdfExtGState gs1 = new PdfExtGState().SetFillOpacity(0.05f);
            canvas.SetExtGState(gs1);
            document.ShowTextAligned(p
                , pageSize.GetWidth() / 2
                , pageSize.GetHeight() / 2
                , pdfDoc.GetPageNumber(page)
                , TextAlignment.CENTER
                , VerticalAlignment.MIDDLE
                , (float)0.610865); /* angle is in radians */
            canvas.RestoreState();
        }

        


    }
    document.Close();
    pdfDoc.Close();
    
  
    Console.WriteLine("Done");
    Console.WriteLine($"See {fullPathDest}");


}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}

