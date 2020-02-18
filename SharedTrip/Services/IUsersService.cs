using System;
using System.Collections.Generic;
using System.Text;

namespace SharedTrip.Services
{
    public interface IUsersService
    {
        string GetUserId(string username, string password);

        void Register(string username, string email, string password);

        bool UsernameExist(string username);

        bool EmailExists(string email);

        string GetUsername(string id);
        void AddUserToTrip(string tripId, string username, string password);
    }
}
