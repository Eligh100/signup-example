using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using signup_example.Controllers;
using signup_example.Models;
using signup_example.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace signup_example.tests
{
    [TestClass]
    public class HomeControllerTest
    {
        private HomeController homeController;

        private static string GOOD_EMAIL = "GoodEmail@gmail.com";
        private static string GOOD_PASSWORD = "GoodPassword123!";

        private static Response GOOD_RESPONSE = new Response
        {
            StatusCode = 200,
            ResponseMessage = "Successfully created new user"
        };

        private static Response BAD_RESPONSE_GENERAL = new Response
        {
            StatusCode = 400,
            ResponseMessage = "Failed to create user"
        };

        private static Response BAD_RESPONSE_DUPLICATE = new Response
        {
            StatusCode = 400,
            ResponseMessage = "This email is already in use"
        };


        [TestInitialize]
        public void SetupHomeController()
        {
            var logger = new Logger<HomeController>(new LoggerFactory());
            var passwordService = new PasswordService();
            var apiService = new ApiService();
            homeController = new HomeController(logger, passwordService, apiService);
        }

        [TestMethod]
        public async Task TestEmail()
        {
            User goodEmail = new User
            {
                Email = GOOD_EMAIL,
                Password = GOOD_PASSWORD
            };

            var result = await homeController.RegisterUser(goodEmail);
            Assert.AreEqual(GOOD_RESPONSE, result);

            User emailTooLong = new User
            {
                Email = "test@testtesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttest.com",
                Password = GOOD_PASSWORD
            };

            result = await homeController.RegisterUser(emailTooLong);
            Assert.AreEqual(BAD_RESPONSE_GENERAL, result);

            User emailMalformed = new User
            {
                Email = "test.com",
                Password = GOOD_PASSWORD
            };

            result = await homeController.RegisterUser(emailTooLong);
            Assert.AreEqual(BAD_RESPONSE_GENERAL, result);
        }

        [TestMethod]
        public async Task TestPassword()
        {
            User goodPassword = new User
            {
                Email = GOOD_EMAIL,
                Password = GOOD_PASSWORD
            };

            var result = await homeController.RegisterUser(goodPassword);
            Assert.AreEqual(GOOD_RESPONSE, result);

            User passwordTooShort = new User
            {
                Email = GOOD_EMAIL,
                Password = "B0bby!!"
            };

            result = await homeController.RegisterUser(passwordTooShort);
            Assert.AreEqual(BAD_RESPONSE_GENERAL, result);

            User passwordNoCapitals = new User
            {
                Email = GOOD_EMAIL,
                Password = "bobby123!"
            };

            result = await homeController.RegisterUser(passwordNoCapitals);
            Assert.AreEqual(BAD_RESPONSE_GENERAL, result);

            User passwordNoNumbers = new User
            {
                Email = GOOD_EMAIL,
                Password = "Bobbyjones!"
            };

            result = await homeController.RegisterUser(passwordNoNumbers);
            Assert.AreEqual(BAD_RESPONSE_GENERAL, result);

            User passwordNoSpecialChars = new User
            {
                Email = GOOD_EMAIL,
                Password = "Bobby1234"
            };

            result = await homeController.RegisterUser(passwordNoSpecialChars);
            Assert.AreEqual(BAD_RESPONSE_GENERAL, result);
        }

        [TestMethod]
        public async Task TestExistingUser()
        {
            // Generate a UUID
            Guid myuuid = Guid.NewGuid();
            string testEmail = myuuid.ToString() + "@gmail.com";

            // Add the user
            User testUser = new User
            {
                Email = testEmail,
                Password = GOOD_PASSWORD
            };

            var result = await homeController.RegisterUser(testUser);
            Assert.AreEqual(GOOD_RESPONSE, result);

            // Now try and add the same user again
            result = await homeController.RegisterUser(testUser);
            Assert.AreEqual(BAD_RESPONSE_DUPLICATE, result);
        }
    }
}
