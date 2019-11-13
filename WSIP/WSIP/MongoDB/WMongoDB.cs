using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSIP
{
    public class WMongoDB
    {
        private MongoClient dbconn;
        private IMongoDatabase db;
        private string mlabconn;    // = "mongodb://mlabConnectString";


        public WMongoDB(string connectString)
        {
            mlabconn = connectString;
        }

        public void TestMongoDBController(string DBName)
        {
            dbconn = new MongoClient(mlabconn);
            db = dbconn.GetDatabase(DBName);
        }

        private bool InsertOne(string catelog, BsonDocument doc)
        {
            try
            {
                var coll = db.GetCollection<BsonDocument>(catelog);
                coll.InsertOne(doc);
            }
            catch(Exception)
            {
                return false;
            }
            return true;
        }

        List<BsonDocument> Query(string catelog, BsonDocument docFilter)
        {
            var collection = db.GetCollection<BsonDocument>(catelog);
            List< BsonDocument> document = collection.Find(docFilter).ToList();

            return document;
        }

    }
}
