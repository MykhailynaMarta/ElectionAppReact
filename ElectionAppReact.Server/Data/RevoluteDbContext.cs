using ElectionAppReact.Server.Models;
using System;

namespace ElectionAppReact.Server.Data
{
    public static class BankSeedData
    {
        private static DateTime Utc(int y, int m, int d)
            => new DateTime(y, m, d, 0, 0, 0, DateTimeKind.Utc);

        //public static BankUser[] PrivatUsers => new[]
        //{
        //    new BankUser
        //    {
        //        Bank = "privat24",
        //        FullName = "Ivan Ivanov",
        //        BirthDate = Utc(1995, 5, 10),
        //        Address = "Kyiv",
        //        Password = "1234"
        //    }
        //};

        public static BankUser[] MonoUsers => new[]
        {
            new BankUser
            {
                Bank = "monobank",
                FullName = "Marta Mykh",
                BirthDate = Utc(2000, 1, 1),
                Address = "Lviv",
                Password = "pass"
            }
        };

        public static BankUser[] OschadUsers => new[]
        {
            new BankUser
            {
                Bank = "oschad",
                FullName = "Petro Petrov",
                BirthDate = Utc(1990, 2, 15),
                Address = "Odesa",
                Password = "oschad123"
            }
        };

        public static BankUser[] UniversalUsers => new[]
        {
            new BankUser
            {
                Bank = "universal",
                FullName = "Test User",
                BirthDate = Utc(1990, 1, 1),
                Address = "TestCity",
                Password = "test123"
            }
        };
        public static class AdminSeed
        {
            public static AdminUser[] Admins => new[]
            {
        new AdminUser { FullName = "Super Admin", Password = "admin123" }
    };
        }
    }
}
