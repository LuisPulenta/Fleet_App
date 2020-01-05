using System.Threading.Tasks;
using Fleet_App.Common.Models;
namespace Fleet_App.Common.Services
{
    public interface IApiService
    {
        Task<Response<UserResponse>> GetUserByEmailAsync(
            string urlBase,
            string servicePrefix,
            string controller,
            string email);
       
        Task<bool> CheckConnectionAsync(string url);
    }
}