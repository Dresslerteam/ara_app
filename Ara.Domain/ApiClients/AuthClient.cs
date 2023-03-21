using Ara.Domain.ApiClients.Dtos;
using Ara.Domain.ApiClients.Dtos.Common;
using Ara.Domain.ApiClients.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ara.Domain.ApiClients
{
    public class AuthClient : IAuthClient
    {
        private UserDto _currentUser;
        public UserDto GetCurrentUserInfo()
        {
            return _currentUser;
        }

        public UserDto Login(string email, string password = null)
        {
            var currentUser = this.GetAllUsers().FirstOrDefault(u => u.Email == email);
            _currentUser = currentUser ?? new UserDto();
            return _currentUser;
        }

        public void LogOut()
        {
            _currentUser = null;
        }

        public List<UserDto> GetAllUsers()
        {
            var list = new List<UserDto>()
            {
                new UserDto(){ Id = "1", FirstName = "Tato", LastName = "Khorava", Email = "tato@khorava.com"},
                new UserDto(){ Id = "2", FirstName = "Patrick", LastName = "Donoghue", Email = "patrick@donoghue.com"},
                new UserDto(){ Id = "3", FirstName = "Pat", LastName = "Florida", Email = "pat@florida.com"},
                new UserDto(){ Id = "4", FirstName = "Oleksii", LastName = "Vinda", Email = "oleksii@vinda.com"},
                new UserDto(){ Id = "5", FirstName = "Den", LastName = "Tkachenko", Email = "den@tkachenko.com"}
            };

            return list;
        }
    }
}
