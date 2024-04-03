


//https://www.c-sharpcorner.com/article/adding-a-digital-signature-to-a-pdf-using-a-pfx-file-and-itextsharp-in-c-sharp/

using iTextSharp.text.pdf;
using iTextSharp.text.pdf.security;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Pkcs;

Console.WriteLine("Certificate signing");


// Step 2: Load PDF Document
string pdfFileDirectory = $"C:\\Temp\\";
string documentName = "testsign";
string pdfFilePath = $"{pdfFileDirectory}{documentName}.pdf";
string destinationPath = $"{pdfFileDirectory}{documentName}_signed.pdf";
//PdfReader pdfReader = new PdfReader(pdfFilePath);

string digitalSignatureReason = "Pretty Darn Fast Signature";
string digitalSignatureLocation = "Portsmouth, UK";

// Step 3: Load PFX Certificate
string pfxFilePath = "C:\\Temp\\prettydarnfastcertificate256.pfx";
string pfxPassword = "";



Pkcs12Store pfxKeyStore = new Pkcs12Store(new FileStream(pfxFilePath, FileMode.Open, FileAccess.Read), pfxPassword.ToCharArray());

PdfReader pdfReader = new PdfReader(pdfFilePath);
// Step 4: Count the pages and add signature in each page
int page = pdfReader.NumberOfPages;
for (int i = 1; i <= page; i++)
{
    if (i > 1)
    {
        FileStream stremfile = new FileStream(destinationPath, FileMode.Open, FileAccess.Read);
        pdfReader = new PdfReader(stremfile);
        File.Delete(destinationPath);
    }

    string alias = pfxKeyStore.Aliases.Cast<string>().FirstOrDefault(entryAlias => pfxKeyStore.IsKeyEntry(entryAlias));
    ICipherParameters privateKey = pfxKeyStore.GetKey(alias).Key;


    // Step 4.1: Initialize the PDF Stamper And Creating the Signature Appearance
    FileStream signedPdf = new FileStream(destinationPath, FileMode.Create, FileAccess.ReadWrite);
    PdfStamper pdfStamper = PdfStamper.CreateSignature(pdfReader, signedPdf, '\0', null, true);
    PdfSignatureAppearance signatureAppearance = pdfStamper.SignatureAppearance;

    //signatureAppearance.Reason = "Digital Signature Reason";
    //signatureAppearance.Location = "Digital Signature Location";
    signatureAppearance.Acro6Layers = false;

    // Set the signature appearance location (in points)
    float x = 0;
    float y = 0;
    signatureAppearance.Acro6Layers = false;
    signatureAppearance.CertificationLevel = 1;
    signatureAppearance.SignDate = DateTime.Now;
    signatureAppearance.Certificate = pfxKeyStore.GetCertificate(alias).Certificate;
    signatureAppearance.Layer4Text = "";
    //signatureAppearance.SetVisibleSignature(new iTextSharp.text.Rectangle(x, y, x + 150, y + 50), i, null);


    // Step 4.2: Sign the Document
    
    IExternalSignature pks = new PrivateKeySignature(privateKey, DigestAlgorithms.SHA256);

    MakeSignature.SignDetached(signatureAppearance, pks, new Org.BouncyCastle.X509.X509Certificate[] { pfxKeyStore.GetCertificate(alias).Certificate }, null, null, null, 0, CryptoStandard.CMS);

    // Step 4.3: Save the Signed PDF
    pdfReader.Close();
    pdfStamper.Close();
}
Console.WriteLine("PDF signed successfully!");




Console.WriteLine("PDF signed successfully!");