using SolrNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolrCsharpTest
{
    class IndexProduct
    {
        [SolrUniqueKey("id")]
        public string SolrId { get; set; }
        [SolrField("productId")]
        public int Id { get; set; }
        [SolrField("productName")]
        public string ProductName { get; set; }
        [SolrField("description")]
        public string Description { get; set; }
        [SolrField("brandName")]
        public string BrandName { get; set; }
        [SolrField("categoryName")]
        public string CategoryName { get; set; }
        [SolrField("colorName")]
        public string ColorName { get; set; }
        [SolrField("sizeName")]
        public string SizeName { get; set; }
        [SolrField("ImageUrl")]
        public string imageUrl { get; set; }
    }
}
