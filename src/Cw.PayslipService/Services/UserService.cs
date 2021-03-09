using Cw.PayslipService.Interfaces;
using Cw.PayslipService.Models;
using Cw.Platform.Jwt;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace Cw.PayslipService.Services
{
    public class UserService: IUserService
    {
        private readonly ServiceContext _context;
        private readonly AppSettings _appSettings;

        public UserService(ServiceContext context, IOptions<AppSettings> appSettings)
        {
            if (appSettings is null)
            {
                throw new ArgumentNullException(nameof(appSettings));
            }

            _context = context;
            _appSettings = appSettings.Value;
        }

        public User GetUserById(int id)
        {
            return _context.Users.SingleOrDefault(x => x.Id == id);
        }

        public string ValidateCredentials(string userName, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == userName && u.Password == password);
            if(user == null)
            {
                return null;
            }
            else
            {
                return JwtProvider.GenerateJWTToken(user.Id, user.IsAdmin, _appSettings.Secret);
            }
        }
    }
}
