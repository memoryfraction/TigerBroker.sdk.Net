using System;
using System.Collections.Generic;
using System.Text;

namespace TigerBrokerLibrary.Model
{
    public class TigerAssetsRequest
    {
        public string tiger_id { get; set; }
        public string charset { get; set; }
        public string sign_type { get; set; }
        public string version { get; set; }
        public string timestamp { get; set; }
        public string method { get; set; }
        public string biz_content { get; set; }
        public string sign { get; set; }
    }
}
