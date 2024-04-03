// See https://aka.ms/new-console-template for more information
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.security;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Pkcs;
using System.Reflection.PortableExecutable;


//https://www.c-sharpcorner.com/article/adding-a-digital-signature-to-a-pdf-using-a-pfx-file-and-itextsharp-in-c-sharp/

Console.WriteLine("Certificate signing");


// Step 2: Load PDF Document
string pdfFilePath = "C:\\Users\\Admin\\Documents\\MyPDF.pdf";
PdfReader pdfReader = new PdfReader(pdfFilePath);

string digitalSignatureReason = "Digital Signature Reason";
string digitalSignatureLocation = "Digital Signature Location";

// Step 3: Load PFX Certificate
string pfxFilePath = "D:\\Uday Dodiya\\Digital_Sign\\Uday Dodiya.pfx";
string pfxPassword = "uday1234";


Pkcs12Store pfxKeyStore = new Pkcs12Store(new FileStream(pfxFilePath, FileMode.Open, FileAccess.Read), pfxPassword.ToCharArray());

// Step 4: Initialize the PDF Stamper And Creating the Signature Appearance
PdfStamper pdfStamper = PdfStamper.CreateSignature(pdfReader, new FileStream("C:\\Users\\Admin\\Documents\\MyPDF_Signed.pdf", FileMode.Create), '\0', null, true);
PdfSignatureAppearance signatureAppearance = pdfStamper.SignatureAppearance;
signatureAppearance.Reason = digitalSignatureReason;
signatureAppearance.Location = digitalSignatureLocation;

// Set the signature appearance location (in points)
float x = 360;
float y = 130;
signatureAppearance.Acro6Layers = false;
signatureAppearance.Layer4Text = PdfSignatureAppearance.questionMark;
signatureAppearance.SetVisibleSignature(new iTextSharp.text.Rectangle(x, y, x + 150, y + 50), 1, "signature");

// Step 5: Sign the Document
string alias = pfxKeyStore.Aliases.Cast<string>().FirstOrDefault(entryAlias => pfxKeyStore.IsKeyEntry(entryAlias));

if (alias != null)
{
    ICipherParameters privateKey = pfxKeyStore.GetKey(alias).Key;
    IExternalSignature pks = new PrivateKeySignature(privateKey, DigestAlgorithms.SHA256);
    MakeSignature.SignDetached(signatureAppearance, pks, new Org.BouncyCastle.X509.X509Certificate[] { pfxKeyStore.GetCertificate(alias).Certificate }, null, null, null, 0, CryptoStandard.CMS);
}
else
{
    Console.WriteLine("Private key not found in the PFX certificate.");
}

// Step 6: Save the Signed PDF
pdfStamper.Close();

Console.WriteLine("PDF signed successfully!");