using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TigerBrokerLibrary.Model;

namespace TigerBrokerLibrary.Service
{
    public interface ITigerBrokerService
    {
        Dictionary<string, string> GetTigerBrokerRequestDictionary(
            string timeStampString,
            string account,
            string tiger_id,
            string charset = "UTF-8",
            string method = "assets",
            string sign_type = "RSA",
            string version = "1.0"
            );

        /// <summary>
        /// generate content to be signed follow tiger's rule
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        string GetContentTobeSigned(Dictionary<string, string> parameters);


        string Sign(string contentToBeSigned, string privateKeyPkcs8);


        bool Verify(string contentToBeVerified, string publicKeyPkcs8, string signature);


        TigerAssetsRequest GetTigerAssetsRequest(
            string account,
            string tiger_id,
            string timeStamp,
            string sign,
            string method = "assets", 
            string charset = "UTF-8",
            string sign_type = "RSA",
            string version = "1.0"
            );

        Task<TigerAssetsResponse> GetTigerBrokerAssetsModelAsync(string requestUrl, TigerAssetsRequest tigerAssetsRequest);

        TigerAssetsModel ConverToTigerAssetsFromTigerAssetsResponse(TigerAssetsResponse tigerAssetsResponse);

    }
}
