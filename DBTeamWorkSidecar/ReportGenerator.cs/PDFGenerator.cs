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
using Supermarket.Models;

namespace ReportGenerator.cs
{
    public class PDFGenerator
    {
        private const string GeneratingMsg = "Generating PDF Document...";
        private const string PdfPath = "../../testTablePDF5.pdf";
        private const string TableHeader = "Aggregated Sales Report";

        public void GeneratePDF(SupermarketDB sqlDbContext)
        {
            Console.WriteLine(GeneratingMsg);

            string path = PdfPath;

            Document doc = new Document();
            FileStream fs = File.Create(path);
            PdfWriter.GetInstance(doc, fs);

            doc.Open();

            PdfPTable table = new PdfPTable(5);

            var cell = new PdfPCell(new Phrase(TableHeader));

            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.Colspan = 5;
            cell.HorizontalAlignment = 1;

            table.AddCell(cell);

            DateTime currentDate = DateTime.Now;
            decimal sum = -1;
            decimal grandTotal = 0;

            var query = sqlDbContext.Database
                .SqlQuery<AggregatedReportModel>(@"select p.ProductName as Product,
                                                          sd.Quantity as Quantity,
                                                          sd.UnitPrice as UnitPrice,
                                                          s.StoreName as Location,
                                                          sd.Sum as Sum, 
                                                          sd.Date as Date
                                                from SaleByDates sd
                                                join Products p on sd.ProductId = p.ID
                                                join Measures m on m.ID = p.Measure_ID
                                                join Supermarkets s on s.Id = sd.SupermarketId
                                                group by sd.Date, p.ProductName, sd.Quantity, 
                                                         sd.UnitPrice, s.StoreName, sd.Sum").ToList();
                
                //(from sa in sqlDbContext.SalesByDate
                //        join sm in sqlDbContext.Supermarkets on sa.SupermarketId equals sm.Id //into smsa
                //        join p in sqlDbContext.Products on sa.ProductId equals p.ID into sap
                //        from s in sap
                //        select new //AggregatedReportModel
                //        {
                //            Date = sa.Date,
                //            Product = s.ProductName,// p.ProductName,
                //            Quantity = sa.Quantity + " " + s.Measure,//p.Measure,
                //            UnitPrice = sa.UnitPrice,
                //            Location = sm.StoreName,
                //            Sum = sa.Sum
                //        }).ToList();

            

                //sqlDbContext.SalesByDate.OrderBy(x => x.Date).ToList();
            foreach (var item in query)
            {
                if (item.Date != currentDate)
                {
                    currentDate = item.Date;
                    if (sum != -1)
                    {
                        var totalSumTextCell = new PdfPCell(new Phrase("Total sum for " + item.Date + ":"));
                        totalSumTextCell.Colspan = 4;
                        table.AddCell(totalSumTextCell);

                        var totalSumCell = new PdfPCell(new Phrase(item.Sum.ToString()));
                        table.AddCell(totalSumCell);

                        sum = -1;
                    }

                    var newDateCell = new PdfPCell(new Phrase("Date: " + item.Date));
                    newDateCell.Colspan = 5;
                    table.AddCell(newDateCell);

                    var product = new PdfPCell(new Phrase("Product"));
                    table.AddCell(product);

                    var quantity = new PdfPCell(new Phrase("Quantity"));
                    table.AddCell(quantity);

                    var unitPrice = new PdfPCell(new Phrase("Unit Price"));
                    table.AddCell(unitPrice);

                    var location = new PdfPCell(new Phrase("Location" ));
                    table.AddCell(location);

                    var sumHeaderCell = new PdfPCell(new Phrase("Sum"));
                    table.AddCell(sumHeaderCell);
                    
                }

                //table.AddCell(item.Product);
                //table.AddCell(item.Quantity);
                //table.AddCell(item.UnitPrice.ToString());
                //table.AddCell(item.Location);
                //table.AddCell(item.Sum.ToString());

                sum += item.Sum;
                grandTotal += item.Sum;
            }

            var grandTotalCell = new PdfPCell(new Phrase("Grand Total: " + grandTotal));
            //add grand total colspan cell 5

            doc.Add(table);
            doc.Close();

        }
    }
}
