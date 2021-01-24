using System;
using System.Collections.Generic;
using System.Text;

namespace TigerBrokerLibrary.Model
{
    public class TigerAssetsResponse
    {
        public string code { get; set; }
        public string data { get; set; }
        public string message { get; set; }
        public string sign { get; set; }
        public string timestamp { get; set; }
    }
}
