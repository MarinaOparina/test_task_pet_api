using RestSharp;
using NUnit.Framework;
using RestSharp.Serialization.Json;
using System.Collections.Generic;

namespace TestProject
{
    [TestFixture]
    public class PetApiTest
    {
        public static string URL = "http://petstore.swagger.io/v2/pet";
        public static int test_ID = 0;

        [TestCase(100, "Test100"), Order(1)]
        public void PostNewPet(int id, string petName)
        {
            test_ID = id;
            var client = new RestClient(URL);
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");

            var body = "{ \"id\": " + id + ", \"category\": { \"id\": 0, \"name\": \"string\" }, \"name\": \"" + petName + "\", \"photoUrls\": [ \"string\" ], \"tags\": [ { \"id\": 0, \"name\": \"string\" } ], \"status\": \"available\"}";
            request.AddJsonBody(body);
            var responce = client.Execute(request);

            var deser = new JsonDeserializer();
            var obj = deser.Deserialize<Dictionary<string, string>>(responce);

            Assert.AreEqual("OK", responce.StatusCode.ToString());
            Assert.AreEqual(id.ToString(), obj["id"]);
            Assert.AreEqual(petName, obj["name"]);
        }

        [TestCase(), Order(2)]
        public static void GetPositiveTest()
        {
            var id = test_ID;
            var url = $"{URL}/{id}";
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);

            var responce = client.Execute(request);

            var deser = new JsonDeserializer();
            var obj = deser.Deserialize<Dictionary<string, string>>(responce);

            Assert.AreEqual("OK", responce.StatusCode.ToString());
            Assert.AreEqual(id.ToString(), obj["id"]);
            Assert.AreEqual("Test100", obj["name"]);

        }

        [TestCase]
        public static void GetNegativeTest()
        {
            var url = $"http://petstore.swagger.io/v2/pet/1534876";
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);

            var responce = client.Execute(request);

            var errorText = "{\"code\":1,\"type\":\"error\",\"message\":\"Pet not found\"}";

            Assert.AreEqual(errorText,responce.Content.ToString());
            Assert.AreEqual("NotFound", responce.StatusCode.ToString());
        }

        [TestCase()]
        public static void DeleteTest()
        {
            var id = test_ID;
            var url = $"{URL}/{id}";
            var client = new RestClient(url);
            var request = new RestRequest(Method.DELETE);

            var responce = client.Execute(request);

            Assert.AreEqual("OK", responce.StatusCode.ToString());
        }

    }
}
