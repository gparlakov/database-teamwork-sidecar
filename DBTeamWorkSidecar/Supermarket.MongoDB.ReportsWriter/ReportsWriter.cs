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
                var productReports = supermarketDB.Products.Join(
            }
            
            
            
            
            
            
            
            
            var server = MongoServer.Create(Local);

            var db = server[Database];
            if (!db.CollectionExists(CollectionReportsTest))
            {
                db.CreateCollection(CollectionReportsTest);
            }

            var collection = db.GetCollection<TotalProductSales>(CollectionReportsTest);
                        

            //collection.InsertBatch();
            
            //var query = Query<TotalProductSales>.EQ(t => t.ProductName, "Tea");
            //var inserted = collection.Find(query);
            //foreach (var item in inserted)
            //{
            //    Console.WriteLine(item.ProductId); 
            //}
        }
    }
}
