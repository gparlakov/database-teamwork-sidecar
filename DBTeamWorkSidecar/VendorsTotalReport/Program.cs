using System;
using System.Collections.Generic;
using System.Data.SQLite;
using VendorsTotalReport;
using Supermarket.DbContextSQL;
using Supermarket.Models;
using Supermarket.OpenAccess;
using Telerik.OpenAccess;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Linq;

class Program
{
    const string MongoUrl = "mongodb://localhost/";
    const string MongoDatabase = "teamwork-sidecar";
    const string MongoCollection = "ReportsTest";

    static void Main()
    {
        MongoCollection reportCollection = InitializeMongoClient();

        var products = reportCollection.AsQueryable<TotalProductSales>();

        var vendorsIncomes = new Dictionary<string, double>();

        foreach (var product in products)
        {
            if (!vendorsIncomes.ContainsKey(product.VendorName))
            {
                vendorsIncomes.Add(product.VendorName, product.TotalIncomes);
            }
            else
            {
                vendorsIncomes[product.VendorName] += product.TotalIncomes;
            }
        }

        foreach (var item in vendorsIncomes)
        {
            Console.WriteLine("{0} - {1}",item.Key, item.Value);
        }

        var vendorsTaxes = new Dictionary<string, decimal>();
        var connection = new SQLiteConnection(Settings.Default.DBConnectionString);
        connection.Open();
        using (connection)
        {
            var command = new SQLiteCommand("SELECT ProductName, Tax FROM VendorTaxes", connection);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var productName = reader["ProductName"];
                var tax = reader["Tax"];


            }
        }
    }

    static void GetVendorFormSQL()
    {
        using (var db = new SupermarketDB())
        {
        }
    }

    static MongoCollection InitializeMongoClient()
    {
        MongoClient mongoClient = new MongoClient(MongoUrl);
        MongoServer mongoServer = mongoClient.GetServer();
        MongoDatabase supermarket = mongoServer.GetDatabase(MongoDatabase);

        return supermarket.GetCollection(MongoCollection);
    }
}