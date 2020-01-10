using System.Threading.Tasks;
namespace Fleet_App.Common.Services
{
    public interface IGeolocatorService
    {
        double GRXX { get; set; }
        double GRYY { get; set; }
        Task GetLocationAsync();
    }
}