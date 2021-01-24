namespace TigerBrokerLibrary.Model
{
    public class TigerAssetsModel
    {
        public string account { get; set; }
        public decimal accruedCash { get; set; }
        public decimal accruedDividend { get; set; }
        public decimal availableFunds { get; set; }
        public decimal buyingPower { get; set; }
        public string capability { get; set; }
        public decimal cashValue { get; set; }
        public string currency { get; set; }
        public int dayTradesRemaining { get; set; }
        public double cushion { get; set; }
        public decimal equityWithLoan { get; set; }
        public decimal excessLiquidity { get; set; }
        public decimal grossPositionValue { get; set; }
        public decimal initMarginReq { get; set; }
        public decimal maintMarginReq { get; set; }
        public decimal netLiquidation { get; set; }

    }
}
