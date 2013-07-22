using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Linq;
using Supermarket.Models;
using Supermarket.DbContextSQL;
using Supermarket.OpenAccess;

namespace Supermarket.MongoDBTest
{
    //  mongodb://<dbuser>:<dbpassword>@ds037358.mongolab.com:37358/teamwork-sidecar
    
    class Program
    {
        const string CollectionReportsTest = "ReportsTest";
        const string Local = "mongodb://localhost";
        const string ConnectionString =@"mongodb://parlakov:parlakov@ds037358.mongolab.com:37358";
        const string Database = "teamwork-sidecar";

        static void Main(string[] args)
        {
            using (var supermarketDB = new SupermarketDB())
            {
                //AllSalesReportsSqlToMongo(supermarketDB);

                var server = MongoServer.Create(Local);
                var db = server.GetDatabase(Database);
                var collection = db.GetCollection<TotalProductSales>(CollectionReportsTest).FindAll();
                
                foreach (var item in collection)
                {
                    Console.WriteLine("{0} -> {1}",item.ProductName,item.QuantitySold);
                }
            }
        }

        private static void AllSalesReportsSqlToMongo(SupermarketDB supermarketDB)
        {
            var productReports = supermarketDB.Database.SqlQuery<TotalProductSales>(@"
                    SELECT  SUM(s.UnitPrice * s.Quantity) as TotalIncomes,
		                    SUM(s.Quantity) as [QuantitySold],		
		                    s.ProductId ,
		                    MAX(p.ProductName) as ProductName,
		                    MAX(v.VendorName) as VendorName
                    FROM [Supermarket].[dbo].[SaleByDates] s
	                    JOIN Products p ON s.ProductId = p.ID
	                    JOIN Vendors v ON p.Vendor_ID = v.ID
                    GROUP BY s.ProductId
                    ORDER BY s.ProductId").ToList();


            var server = MongoServer.Create(Local);

            var db = server.GetDatabase(Database);
            if (!db.CollectionExists(CollectionReportsTest))
            {
                db.CreateCollection(CollectionReportsTest);
            }

            var collection = db.GetCollection<TotalProductSales>(CollectionReportsTest);
            collection.InsertBatch(productReports);
        }
    }
}
