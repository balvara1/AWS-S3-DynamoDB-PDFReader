using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _301128209alvarado_Lab2.Models
{
    // target table is Bookshelf
    [DynamoDBTable("Bookshelf")]
    public class BookItem
    {
        // this should match Partition Key in the table
        [DynamoDBHashKey]
        public string Username { get; set; }

        // this should march Sort Key in the table
        [DynamoDBRangeKey]
        public string ISBN { get; set; }
        public string Title { get; set; }
        public bool IsBookmarked { get; set; }
        public int Page { get; set; }
        public string Author { get; set; }
        public long TimeLastViewed { get; set; }

    }
}
