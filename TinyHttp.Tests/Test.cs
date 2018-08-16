using System.Threading.Tasks;

namespace TinyHttp
{
    public class Test
    {
        public async Task MakeTest()
        {
            IFluent client = null;

            await client.AddHeader("Test", "Test").
                SerializeWith(new TinyJsonSerializer()).
                DeserializeWith(new TinyJsonDeserializer()).
                PostAsync<TestPoco>(null);

            await client.AddFormParameter("resource", "resource").
                AddFormParameter("password", "password").
                AddFormParameter("client_id", "clientId").
                AddFormParameter("username", "username").
                AddFormParameter("password", "password").
                PostAsync<TestPoco>("route");
        }
    }
}