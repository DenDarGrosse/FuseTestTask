using test_fuse.Models.Domains;

namespace test_fuse.Repositories
{
    public interface ILogoRepository
    {
        string GetLogoURLByCurrencyId(int logoId);
        void Save(Logo logo);
    }
}
