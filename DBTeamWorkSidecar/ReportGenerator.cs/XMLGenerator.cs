using Supermarket.DbContextSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ReportGenerator.cs
{
    public class XMLGenerator
    {
        public void GenerateXML(SupermarketDB sqlDbContext)
        {
            Console.WriteLine("Generatin XML....");

            string fileName = "../../sales.xml";
            Encoding encoding = Encoding.GetEncoding("windows-1251");


            using (XmlTextWriter writer = new XmlTextWriter(fileName, encoding))
            {
                writer.Formatting = Formatting.Indented;
                writer.IndentChar = '\t';
                writer.Indentation = 1;

                writer.WriteStartDocument();
                writer.WriteStartElement("sales");

                var query = sqlDbContext.Products.ToList();

                foreach (var item in query)
                {
                    var data = DateTime.Now;
                    WriteSale(writer, item.Vendor.VendorName.ToString(), data, item.BasePrice);
                }

                writer.WriteEndDocument();

            }
        }

        private void WriteSale(XmlWriter writer, string vendor, DateTime date, decimal totalSum)
        {
            writer.WriteStartElement("sale");
            writer.WriteAttributeString("vendor", vendor);

            writer.WriteStartElement("summary");
            writer.WriteAttributeString("date", date.ToString());
            writer.WriteAttributeString("totalSum", totalSum.ToString());

            writer.WriteEndElement();

            writer.WriteEndElement();
        }
    }
}
