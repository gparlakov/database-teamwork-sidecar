using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supermarket.OpenAccess.Models;
using Telerik.OpenAccess;
using Supermarket.DbContextSQL;


namespace Supermarket.TransferData
{
    public class InitialTransferer
    {
        static void Main()
        {
            using (var mySqlDbContext = new SupermarketDbMySQL())
            {
                TransferMeasures(mySqlDbContext);

                TransferVendors(mySqlDbContext);

                TransferProducts(mySqlDbContext);
            }
        }

        private static void TransferMeasures(SupermarketDbMySQL mySqlDbContext)
        {
            var measures = mySqlDbContext.Measures.ToList();

            
            using (var sqlDbContext = new SupermarketDB())
            {
                
                foreach (var measure in measures)
                {                    
                    sqlDbContext.Measures.Add(measure);//new Measure {Name = measure.MeasureName}                    
                }

                sqlDbContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT Supermarket.dbo.Measures ON");
                sqlDbContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT Supermarket.dbo.Vendors ON");
                sqlDbContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT Supermarket.dbo.Products ON");
                sqlDbContext.SaveChanges();                
            }
        }

        private static void TransferVendors(SupermarketDbMySQL mySqlDbContext)
        {
            var vendors = mySqlDbContext.Vendors.ToList();


            using (var sqlDbContext = new SupermarketDB())
            {
                foreach (var vendor in vendors)
                {
                    sqlDbContext.Vendors.Add(vendor);
                }

                sqlDbContext.SaveChanges();
            }
        }

        private static void TransferProducts(SupermarketDbMySQL mySqlDbContext)
        {
            var products = mySqlDbContext.Products.ToList();


            using (var sqlDbContext = new SupermarketDB())
            {
                foreach (var product in products)
                {
                    sqlDbContext.Products.Add(product);
                }

                sqlDbContext.SaveChanges();
            }
        }
    }
}
