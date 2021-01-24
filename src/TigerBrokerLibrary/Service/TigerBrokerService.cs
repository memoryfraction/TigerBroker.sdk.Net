using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TigerBrokerLibrary.Model;

namespace TigerBrokerLibrary.Service
{
    public class TigerBrokerService : ITigerBrokerService
    {
        public string GetContentTobeSigned(Dictionary<string, string> parameters)
        {
            var sb = new StringBuilder();
            foreach (var parameter in parameters.OrderBy(x => x.Key))
            {
                sb.Append("&" + GeneratePairString(parameter));
            }
            sb.Remove(0, 1);
            return sb.ToString();
        }

        private string GeneratePairString(KeyValuePair<string,string> pair)
        {
            var sb = new StringBuilder();
            sb.Append(pair.Key.ToString());
            sb.Append("=");
            sb.Append(pair.Value.ToString());
            return sb.ToString();
        }


        

        public string Sign(string contentToBeSigned, string privateKeyPkcs8)
        {
            RsaKeyParameters privateKeyParam = (RsaKeyParameters)PrivateKeyFactory.CreateKey(Convert.FromBase64String(privateKeyPkcs8));
            ISigner signer = SignerUtilities.GetSigner("SHA1WITHRSA");
            signer.Init(true, privateKeyParam);
            var dataByte = Encoding.GetEncoding("UTF-8").GetBytes(contentToBeSigned);
            signer.BlockUpdate(dataByte, 0, dataByte.Length);
            return Convert.ToBase64String(signer.GenerateSignature());
        }

        public bool Verify(string contentToBeVerified, string publicKeyPkcs8, string signature)
        {
            RsaKeyParameters publicKeyParam = (RsaKeyParameters)PublicKeyFactory.CreateKey(Convert.FromBase64String(publicKeyPkcs8));
            ISigner signer = SignerUtilities.GetSigner("SHA1WITHRSA");
            signer.Init(false, publicKeyParam);
            byte[] dataByte = Encoding.GetEncoding("UTF-8").GetBytes(contentToBeVerified);
            signer.BlockUpdate(dataByte, 0, dataByte.Length);
            byte[] signatureByte = Convert.FromBase64String(signature);
            return signer.VerifySignature(signatureByte);
        }



        public TigerAssetsModel ConverToTigerAssetsFromTigerAssetsResponse(TigerAssetsResponse tigerAssetsResponse)
        {
            if (tigerAssetsResponse == null)
                throw new ArgumentNullException();

            var tigerAssetsModel = new TigerAssetsModel();

            dynamic responseData = JsonConvert.DeserializeObject(tigerAssetsResponse.data);
            if (responseData != null)
            {
                dynamic dynamicResponse = responseData.items[0];
                var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(dynamicResponse.ToString());

                tigerAssetsModel.account = values["account"].ToString();
                tigerAssetsModel.accruedCash = Convert.ToDecimal(values["accruedCash"]);
                tigerAssetsModel.accruedDividend = Convert.ToDecimal(values["accruedDividend"]);
                tigerAssetsModel.availableFunds = Convert.ToDecimal(values["availableFunds"]);
                tigerAssetsModel.buyingPower = Convert.ToDecimal(values["buyingPower"]);
                tigerAssetsModel.capability = Convert.ToString(values["capability"]);
                tigerAssetsModel.cashValue = Convert.ToDecimal(values["cashValue"]);
                tigerAssetsModel.currency = Convert.ToString(values["currency"]);
                tigerAssetsModel.dayTradesRemaining = Convert.ToInt32(values["dayTradesRemaining"]);
                tigerAssetsModel.cushion = Convert.ToDouble(values["cushion"]);
                tigerAssetsModel.equityWithLoan = Convert.ToDecimal(values["equityWithLoan"]);
                tigerAssetsModel.excessLiquidity = Convert.ToDecimal(values["excessLiquidity"]);
                tigerAssetsModel.grossPositionValue = Convert.ToDecimal(values["grossPositionValue"]);
                tigerAssetsModel.initMarginReq = Convert.ToDecimal(values["initMarginReq"]);
                tigerAssetsModel.maintMarginReq = Convert.ToDecimal(values["maintMarginReq"]);
                tigerAssetsModel.netLiquidation = Convert.ToDecimal(values["netLiquidation"]);
            }
            return tigerAssetsModel;
        }

        public async Task<TigerAssetsResponse> GetTigerBrokerAssetsModelAsync(string requestUrl,  TigerAssetsRequest tigerAssetsRequest)
        {
            if (string.IsNullOrEmpty(requestUrl) || tigerAssetsRequest == null)
                throw new ArgumentNullException();

            var json = JsonConvert.SerializeObject(tigerAssetsRequest);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(requestUrl, data);
                dynamic dynamicResponse = JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);
                var tigerAssetsResponse = new TigerAssetsResponse() {
                    code = dynamicResponse.code,
                    data = dynamicResponse.data,
                    message = dynamicResponse.message,
                    sign = dynamicResponse.sign,
                    timestamp = dynamicResponse.timestamp,
                };
                return tigerAssetsResponse;
            }
        }

        public Dictionary<string, string> GetTigerBrokerRequestDictionary(string timeStampString, string account, string tiger_id, string charset = "UTF-8", string method = "assets", string sign_type = "RSA", string version = "1.0")
        {
            var dic = new Dictionary<string, string>();
            var biz_content = @"{""account"":" + "\"" + account + "\"" + "}";
            dic.Add("biz_content", biz_content);
            dic.Add("charset", charset);
            dic.Add("method", method);
            dic.Add("sign_type", sign_type);
            dic.Add("tiger_id", tiger_id);
            dic.Add("timestamp", timeStampString);
            dic.Add("version", version);
            return dic;
        }

        public TigerAssetsRequest GetTigerAssetsRequest(string account, string tiger_id, string timeStamp, string sign, string method = "assets", string charset = "UTF-8", string sign_type = "RSA", string version = "1.0")
        {
            var request = new TigerAssetsRequest();
            request.biz_content = GetbizContent(account);
            request.timestamp = timeStamp;
            request.tiger_id = tiger_id;
            request.sign = sign;
            request.method = method;
            request.charset = charset;
            request.sign_type = sign_type;
            request.version = version;
            return request;
        }

        /// <summary>
        /// TARGET： "biz_content":"{\"account\":\"U9563332\"}"
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        private string GetbizContent(string account)
        {
            var sb = new StringBuilder();
            sb.Append("{\"account\"" );
            sb.Append(":");
            sb.Append("\"");
            sb.Append(account);
            sb.Append("\"}");
            return sb.ToString();
        }

    
    }
}
