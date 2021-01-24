using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading.Tasks;
using TigerBrokerLibrary.Service;

namespace TigerBrokerLibrary.UnitTest
{
    [TestClass]
    public class ITigertBrokerServiceIntegrationTests
    {

        private readonly ITigerBrokerService _tigerBrokerService;
        private IConfigurationRoot config;
        public ITigertBrokerServiceIntegrationTests()
        {
            _tigerBrokerService = new TigerBrokerService();

            config = new ConfigurationBuilder()
                .AddJsonFile("Data/client-secrets.json")
                .Build();
        }


        [TestMethod]
        public async Task GetTigerBrokerAssetsResponse()
        {
            //1 Generate Dictionary
            //  Convert time zone to the one in Chinese
            var chinaZone = TimeZoneInfo.FindSystemTimeZoneById("China Standard Time");
            var dtChina = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow.Date, chinaZone);
            var timeStamp = dtChina.ToString("yyyy-MM-dd HH:mm:ss");

            var url = "https://openapi.itiger.com/gateway";
            var account = config["account"];
            var charset = "UTF-8";
            var method = "assets";
            var sign_type = "RSA";
            var tiger_id = config["tiger_id"];
            var version = "1.0";
            var paramsDic = _tigerBrokerService.GetTigerBrokerRequestDictionary(
                timeStampString: timeStamp, 
                account: account, 
                charset: charset, 
                method: method,
                sign_type : sign_type,
                tiger_id : tiger_id, 
                version : version);

            //2 Generate Sign
            //2.1 generate content to be signed
            var contentTobeSigned = _tigerBrokerService.GetContentTobeSigned(paramsDic);

            // 2.2 Sign
            var privateKey = File.ReadAllText(@"Data\TigerBrokerPrivateKey.txt");
            var sign = _tigerBrokerService.Sign(contentTobeSigned, privateKey);


            //3 Generate Request
            var tigerBrokerRequest = _tigerBrokerService.GetTigerAssetsRequest(
                account: account,
                tiger_id: tiger_id,
                timeStamp: timeStamp,
                sign: sign,
                method: method,
                charset: charset,
                sign_type:sign_type,
                version:version
                ); 

            //4 Send Request and get Response
            var tigerResponse = await _tigerBrokerService.GetTigerBrokerAssetsModelAsync(url,tigerBrokerRequest);
            var tigerAssets = _tigerBrokerService.ConverToTigerAssetsFromTigerAssetsResponse(tigerResponse);

            //6 Assert
            Assert.IsNotNull(tigerAssets);
        }
    }
}
