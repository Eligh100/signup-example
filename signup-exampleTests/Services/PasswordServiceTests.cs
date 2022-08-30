using Microsoft.VisualStudio.TestTools.UnitTesting;
using signup_example.Models;
using signup_example.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace signup_example.Services.Tests
{
    [TestClass()]
    public class PasswordServiceTests
    {
        private IPasswordService passwordService;

        [TestInitialize()]
        public void SetupPasswordService()
        {
            passwordService = new PasswordService();
        }

        [TestMethod()]
        public void TestSaltAndHashPassword()
        {
            string testPassword = "TestPassword2022!";

            Password testPasswordObj = passwordService.SaltAndHashPassword(testPassword);

            string hashedAndSaltedPassword = testPasswordObj.HashedPassword;
            string salt = testPasswordObj.Salt;

            Assert.AreNotEqual(hashedAndSaltedPassword, testPassword);
            Assert.AreEqual(44, hashedAndSaltedPassword.Length);

            // Test whether hashing the same password with the same salt yields same hashed password
            string preHashedPassword = salt + testPassword;
            string hashedAndSaltedPassword2 = IPasswordService.HashPassword(preHashedPassword);

            Assert.AreEqual(hashedAndSaltedPassword2, hashedAndSaltedPassword);
        }

        [TestMethod()]
        public void TestSaltUniqueness()
        {
            string samePassword = "test";

            Password testPassword = passwordService.SaltAndHashPassword(samePassword);
            string testSalt1 = testPassword.Salt;

            testPassword = passwordService.SaltAndHashPassword(samePassword);
            string testSalt2 = testPassword.Salt;

            testPassword = passwordService.SaltAndHashPassword(samePassword);
            string testSalt3 = testPassword.Salt;

            Assert.AreNotEqual(testSalt1, testSalt2);
            Assert.AreNotEqual(testSalt2, testSalt3);
            Assert.AreNotEqual(testSalt3, testSalt1);
        }
    }
}