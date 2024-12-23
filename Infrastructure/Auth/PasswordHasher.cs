﻿using Application;

namespace Infrastructure.Auth
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Generate(string password) =>
            BCrypt.Net.BCrypt.EnhancedHashPassword(password);

        public bool Verify(string password, string hashPassword) =>
            BCrypt.Net.BCrypt.EnhancedVerify(password, hashPassword);
    }
}
