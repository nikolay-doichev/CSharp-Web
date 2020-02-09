﻿using SIS.MvcFramework;
using SulsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SulsApp.Services
{
    public class UsersService : IUsersService
    {
        private ApplicationDbContext db;

        public UsersService(ApplicationDbContext db)
        {
            this.db = db;
        }
        public void ChangePassword(string username, string newPassword)
        {
            var user = this.db.Users.FirstOrDefault(x => x.Username == username);
            if (user == null)
            {
                return;
            }

            user.Password = this.Hash(newPassword);
            this.db.SaveChanges();
        }

        public int CountUsers()
        {
            return this.db.Users.Count();
        }

        public void CreateUser(string username, string email, string password)
        {
            var user = new User()
            {
                Email = email,
                Password = this.Hash(password),
                Username = username,
                Role = IdentityRole.User,
            };

            db.Users.Add(user);
            db.SaveChanges();
        }

        public string GetUserId(string username, string password)
        {
            var passwordHash = this.Hash(password);
            return this.db.Users
                .Where(x => x.Username == username && x.Password == passwordHash)
                .Select(x => x.Id)
                .FirstOrDefault();
        }

        private string Hash(string input)
        {
            var crypt = new SHA256Managed();
            StringBuilder hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(input));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }

            return hash.ToString();
        }
    }
}
