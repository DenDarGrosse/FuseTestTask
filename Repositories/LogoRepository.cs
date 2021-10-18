using System.Linq;
using test_fuse.Models.Domains;
using test_fuse.Data;

namespace test_fuse.Repositories
{
    public class LogoRepository : ILogoRepository
    {
        private readonly ApplicationDbContext _context;

        public LogoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public string GetLogoURLByCurrencyId(int currencyId)
        {
            var logos = _context.Logos.Where(t => t.CurrencyId == currencyId);

            if (logos.Count() == 0)
            {
                return null;
            }
            else
            {
                return logos.Single().LogoURL;
            }
        }

        public void Save(Logo logo)
        {
            if (_context.Logos.Where(t => t.CurrencyId == logo.CurrencyId).Count()>1)
            {
                _context.Logos.Update(logo);
            }
            else
            {
                _context.Logos.Add(logo);
            }
            _context.SaveChanges();
        }
    }
}
