using System.ComponentModel.DataAnnotations;

namespace test_fuse.Models.Domains
{
    public class Logo
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int CurrencyId { get; set; }

        [Required]
        public string LogoURL { get; set; }

        public Logo() { }

        public Logo(int currencyId, string logoUrl)
        {
            CurrencyId = currencyId;
            LogoURL = logoUrl;
        }
    }
}
