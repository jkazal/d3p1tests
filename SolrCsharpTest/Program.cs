using CommonServiceLocator;
using SolrNet;
using System;
using System.Data.SqlClient;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace SolrCsharpTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var queryWithForJson = "SELECT * FROM VSIndex";
            var conn = new SqlConnection("Server=mlljksqlserver.database.windows.net;Database=MLLJK_Victoria;Trusted_Connection=False;MultipleActiveResultSets=true;User Id=adminadmin;Password=Victoria21;Integrated Security=False;");
            var cmd = new SqlCommand(queryWithForJson, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<IndexProduct> products = new List<IndexProduct>();
            while ( reader.Read() )
            {
                IndexProduct ip = new IndexProduct();
                ip.BrandName = reader["BrandName"].ToString();
                ip.Id = (int) reader["Id"];
                ip.ProductName = reader["ProductName"].ToString();
                ip.CategoryName = reader["CategoryName"].ToString();
                ip.ColorName = reader["ColorName"].ToString();
                ip.SizeName = reader["SizeName"].ToString();
                ip.imageUrl = reader["ImageUrl"].ToString();
                products.Add(ip);
                Console.WriteLine("Add item " + ip.ProductName);
            }

            var o = 1;
            Startup.Init<IndexProduct>("http://localhost:8983/solr/azure_db_test");
            var solr = ServiceLocator.Current.GetInstance<ISolrOperations<IndexProduct>>();
            solr.AddRange(products);
            solr.Commit();
            Console.WriteLine("fin");
        }
    }
}
