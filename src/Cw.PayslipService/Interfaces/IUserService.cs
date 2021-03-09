using Cw.PayslipService.Models;

namespace Cw.PayslipService.Interfaces
{
    public interface IUserService
    {
        string ValidateCredentials(string userName, string password);
        User GetUserById(int id);
    }
}
