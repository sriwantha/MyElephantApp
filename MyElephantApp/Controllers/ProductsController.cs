using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Util;
using Amazon.XRay.Recorder.Core;
using Amazon.XRay.Recorder.Handlers.AwsSdk;
using MyElephantApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyElephantApp.Controllers
{
    public class ProductsController : ApiController
    {
        private static readonly Lazy<AmazonDynamoDBClient> dynamoDBClient = new Lazy<AmazonDynamoDBClient>(() =>
        {
            var client = new AmazonDynamoDBClient(EC2InstanceMetadata.Region ?? RegionEndpoint.USEast1);
            new AWSSdkTracingHandler(AWSXRayRecorder.Instance).AddEventHandler(client);
            return client;
        });

        Product[] products = new Product[]
        {
            new Product { Id = 1, Name = "Tomato Soup", Category = "Groceries", Price = 1 },
            new Product { Id = 2, Name = "Yo-yo", Category = "Toys", Price = 3.75M },
            new Product { Id = 3, Name = "Hammer", Category = "Hardware", Price = 16.99M }
        };

        public IEnumerable<Product> GetAllProducts()
        {
            return products;
        }

        public IHttpActionResult GetProduct(int id)
        {
            AWSXRayRecorder recorder = new AWSXRayRecorder();
            Product product = null;
            if (id <= 10)
            {
                product = products.FirstOrDefault((p) => p.Id == id);
            }
            else
            {
                try
                {
                    product = GetItem(id);
                }catch(Exception ex)
                {
                    recorder.AddException(ex);
                    throw;
                }
            }
            if (product == null)
            {
                recorder.AddAnnotation("MissingItem", id);
                return NotFound();
            }

            return Ok(product);
        }

        private Product GetItem(int id)
        {
            AmazonDynamoDBClient client = dynamoDBClient.Value;

            Amazon.DynamoDBv2.DocumentModel.Table table = Amazon.DynamoDBv2.DocumentModel.Table.LoadTable(client, "Product");

            Document record = table.GetItem(id.ToString());

            Product p = new Product();
            p.Id = id;
            p.Name = record["Name"];
            p.Price = record["Price"].AsDecimal();
            p.Category = record["Category"];
            return p;
        }
    }
}
