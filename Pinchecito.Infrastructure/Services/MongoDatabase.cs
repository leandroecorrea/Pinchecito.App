using MongoDB.Driver;

namespace Pinchecito.Infrastructure.Services
{
    public class MongoDatabase
    {
        public IMongoDatabase GetDatabase()
        {
            //mongodb+srv://myAtlasDBUser:leandrocapo@myatlasclusteredu.vi0wjyp.mongodb.net/test
            //var mongoURL = new MongoUrl("mongodb+srv://myAtlasDBUser:leandrocapo@myatlasclusteredu.vi0wjyp.mongodb.net/?retryWrites=true&w=majority");
            var mongoURL = new MongoUrl("mongodb://myAtlasDBUser:leandrocapo@ac-dydj4hq-shard-00-00.vi0wjyp.mongodb.net:27017,ac-dydj4hq-shard-00-01.vi0wjyp.mongodb.net:27017,ac-dydj4hq-shard-00-02.vi0wjyp.mongodb.net:27017/?ssl=true&replicaSet=atlas-9rm0mc-shard-0&authSource=admin&retryWrites=true&w=majority");
            var client = new MongoClient(mongoURL);            
            return client.GetDatabase("PinchecitoDB");
        }
    }
}