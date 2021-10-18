namespace test_fuse.Models
{
    public class CryptoCurrency
    {
        public int id;
        public string name;
        public string symbol;
        public string logo;
        public double priceUSD;
        public double priceChange1h;
        public double priceChange24h;
        public double marketCup;

        public CryptoCurrency(
            int id,
            string name,
            string symbol,
            string logo,
            double priceUSD,
            double priceChange1h,
            double priceChange24h,
            double marketCup)
        {
            this.id = id;
            this.name = name;
            this.symbol = symbol;
            this.logo = logo;
            this.priceUSD = priceUSD;
            this.priceChange1h = priceChange1h;
            this.priceChange24h = priceChange24h;
            this.marketCup = marketCup;
        }
    }
}
