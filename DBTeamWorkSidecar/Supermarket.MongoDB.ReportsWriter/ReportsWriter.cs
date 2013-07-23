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
using Newtonsoft.Json;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Options;

namespace Supermarket.MongoDBTest
{
    //  mongodb://<dbuser>:<dbpassword>@ds037358.mongolab.com:37358/teamwork-sidecar
    
    class Program
    {
        const string CollectionReportsTest = "ReportsTest";
        const string Local = "mongodb://localhost";
        const string ConnectionString =@"mongodb://parlakov:parlakov@ds037358.mongolab.com:37358";
        const string Database = "teamwork-sidecar";

        const string ProductId = "product-id";
        const string ProductName = "product-name";
        const string VendorName = "vendor-name";
        const string TotalQuantity = "total-quantity-sold";
        const string TotalIncome = "total-incomes";

        static void Main(string[] args)
        {
            using (var supermarketDB = new SupermarketDB())
            {
                var productReports = GetTotalProductSales(supermarketDB);

                var client = new MongoClient(Local);
                //AllSalesReportsSqlToMongo(supermarketDB, productReports, client);

                TestSaved(client);                             
                
                var json = JsonConvert.SerializeObject(productReports, Formatting.Indented);
                Console.WriteLine(json);
                
            }
        }

        private static void TestSaved(MongoClient client)
        {
            var db = client.GetServer().GetDatabase(Database);
            var collection = db.GetCollection(CollectionReportsTest).FindAll();     

            foreach (var item in collection)
            {
                Console.WriteLine("{0} -> {1}", item[ProductName], item[TotalQuantity]);
            }
        }

        private static void AllSalesReportsSqlToMongo(
            SupermarketDB supermarketDB, List<TotalProductSales> productReports, MongoClient client)
        {
            var db = client.GetServer().GetDatabase(Database);
            if (!db.CollectionExists(CollectionReportsTest))
            {
                db.CreateCollection(CollectionReportsTest);
            }
            db.DropCollection(CollectionReportsTest);
            db.CreateCollection(CollectionReportsTest);

            RegisterCustomSerializer();

            var collection = db.GetCollection<TotalProductSales>(CollectionReportsTest);
            collection.InsertBatch(productReports);
        }

        private static void RegisterCustomSerializer()
        {
            var map = BsonClassMap.RegisterClassMap<TotalProductSales>(cm =>
            {
                cm.AutoMap();
                cm.GetMemberMap(t => t.ProductId).SetElementName(ProductId).SetOrder(1);
                cm.GetMemberMap(t => t.ProductName).SetElementName(ProductName).SetOrder(2);
                cm.GetMemberMap(t => t.VendorName).SetElementName(VendorName).SetOrder(3);
                cm.GetMemberMap(t => t.QuantitySold).SetElementName(TotalQuantity).SetOrder(4);
                cm.GetMemberMap(t => t.TotalIncomes).SetElementName(TotalIncome).SetOrder(5);
            });                        
        }

        private static List<TotalProductSales> GetTotalProductSales(SupermarketDB supermarketDB)
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
            return productReports;
        }
    }
}
