using Microsoft.EntityFrameworkCore;
using Fleet_App.Web.Data.Entities;
namespace Fleet_App.Web.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<SubContratistasUsrWeb> SubContratistasUsrWebs { get; set; }
    }
}
