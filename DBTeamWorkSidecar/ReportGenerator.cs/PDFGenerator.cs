using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supermarket.DbContextSQL;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;
using System.Data;

namespace ReportGenerator.cs
{
    public class PDFGenerator
    {
        public void GeneratePDF(SupermarketDB sqlDbContext)
        {
            Console.WriteLine("Generating PDF Document...");

            var query = sqlDbContext.Products.ToList();

            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/testTablePDF5.pdf";

            Document doc = new Document();
            FileStream fs = File.Create(path);
            PdfWriter.GetInstance(doc, fs);

            doc.Open();

            PdfPTable table = new PdfPTable(2);
            var cell = new PdfPCell(new Phrase("TEST Tables"));
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.Colspan = 2;
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);

            foreach (var item in query)
            {
                table.AddCell(item.ProductName);
                table.AddCell(item.Vendor.ToString());
            }

            doc.Add(table);
            doc.Close();

        }
    }
}
