using CommonServiceLocator;
using SolrNet;
using System;
using System.Data.SqlClient;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SolrCsharpTest
{
    class Program
    {
        static void reindexSolr(ISolrOperations<IndexProduct> solr)
        {
            var queryWithForJson = "SELECT * FROM VSIndex";
            var conn = new SqlConnection("Server=mlljksqlserver.database.windows.net;Database=MLLJK_Victoria;Trusted_Connection=False;MultipleActiveResultSets=true;User Id=adminadmin;Password=Victoria21;Integrated Security=False;");
            var cmd = new SqlCommand(queryWithForJson, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<IndexProduct> products = new List<IndexProduct>();
            while (reader.Read())
            {
                IndexProduct ip = new IndexProduct();
                ip.BrandName = reader["BrandName"].ToString();
                ip.Id = (int)reader["Id"];
                ip.ProductName = reader["ProductName"].ToString();
                ip.CategoryName = reader["CategoryName"].ToString();
                ip.ColorName = reader["ColorName"].ToString();
                ip.SizeName = reader["SizeName"].ToString();
                ip.imageUrl = reader["ImageUrl"].ToString();
                products.Add(ip);
                Console.WriteLine("Add item " + ip.ProductName);
            }

            var o = 1;
            solr.AddRange(products);
            solr.Commit();
            Console.WriteLine("fin");
        }

        static async System.Threading.Tasks.Task Main(string[] args)
        {
            Console.WriteLine("1. Reindex all");
            Console.WriteLine("2. Delete id");
            Console.WriteLine("3. Clear All");

            int x = Int32.Parse(Console.ReadLine());
            Startup.Init<IndexProduct>("http://localhost:8983/solr/azure_db_test");
            var solr = ServiceLocator.Current.GetInstance<ISolrOperations<IndexProduct>>();

            switch (x)
            {
                case 3:
                    using (var httpClient = new HttpClient())
                    {
                        using (var request = new HttpRequestMessage(new HttpMethod("POST"), "http://localhost:8983/solr/azure_db_test/update?commit=true"))
                        {
                            request.Content = new StringContent("<delete><query>*:*</query></delete>");
                            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("text/xml; charset=utf-8");

                            var response = await httpClient.SendAsync(request);
                            Console.WriteLine("All cleared!");
                            Console.ReadLine();
                            reindexSolr(solr);
                        }
                    }
                    break;
                case 1:
                    reindexSolr(solr);
                    break;
                case 2:
                    string toDelete = Console.ReadLine();

                    IndexProduct ipToDelete = new IndexProduct();
                    ipToDelete.SolrId = toDelete;
                    solr.Delete(ipToDelete);
                    solr.Commit();
                    Console.WriteLine("supprimé");
                    break;
            }
        }
    }
}
