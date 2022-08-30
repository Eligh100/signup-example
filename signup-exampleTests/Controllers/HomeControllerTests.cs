using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using signup_example.Controllers;
using signup_example.Models;
using signup_example.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace signup_example.Controllers.Tests
{
    [TestClass()]
    public class HomeControllerTests
    {
        private HomeController homeController;

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

        private static Response BAD_RESPONSE_FORM_INVALID = new Response
        {
            StatusCode = 400,
            ResponseMessage = "Form details are invalid"
        };

        private static Response BAD_RESPONSE_DUPLICATE = new Response
        {
            StatusCode = 400,
            ResponseMessage = "This email is already in use"
        };


        [TestInitialize()]
        public void SetupHomeController()
        {
            var logger = new Logger<HomeController>(new LoggerFactory());
            var passwordService = new PasswordService();
            var apiService = new ApiService();
            homeController = new HomeController(logger, passwordService, apiService);
        }

        [TestMethod()]
        public async Task TestEmail()
        {
            User goodEmail = new User
            {
                Email = GenerateRandomValidEmail(),
                Password = GOOD_PASSWORD
            };

            homeController.ViewData.ModelState.Clear();

            var result = await homeController.RegisterUser(goodEmail);
            AssertResponsesAreEqual(GOOD_RESPONSE, result);

            User emailTooLong = new User
            {
                Email = Guid.NewGuid().ToString() + "@testtesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttest.com",
                Password = GOOD_PASSWORD
            };

            homeController.ViewData.ModelState.Clear();
            MockModelState(emailTooLong, homeController);

            result = await homeController.RegisterUser(emailTooLong);
            AssertResponsesAreEqual(BAD_RESPONSE_FORM_INVALID, result);

            User emailMalformed = new User
            {
                Email = Guid.NewGuid().ToString() + ".com",
                Password = GOOD_PASSWORD
            };

            homeController.ViewData.ModelState.Clear();
            MockModelState(emailMalformed, homeController);

            result = await homeController.RegisterUser(emailTooLong);
            AssertResponsesAreEqual(BAD_RESPONSE_FORM_INVALID, result);
        }

        [TestMethod()]
        public async Task TestPassword()
        {
            User goodPassword = new User
            {
                Email = GenerateRandomValidEmail(),
                Password = GOOD_PASSWORD
            };

            homeController.ViewData.ModelState.Clear();

            var result = await homeController.RegisterUser(goodPassword);
            AssertResponsesAreEqual(GOOD_RESPONSE, result);

            User passwordTooShort = new User
            {
                Email = GenerateRandomValidEmail(),
                Password = "B0bby!!"
            };

            homeController.ViewData.ModelState.Clear();
            MockModelState(passwordTooShort, homeController);

            result = await homeController.RegisterUser(passwordTooShort);
            AssertResponsesAreEqual(BAD_RESPONSE_FORM_INVALID, result);

            User passwordNoCapitals = new User
            {
                Email = GenerateRandomValidEmail(),
                Password = "bobby123!"
            };

            homeController.ViewData.ModelState.Clear();
            MockModelState(passwordNoCapitals, homeController);

            result = await homeController.RegisterUser(passwordNoCapitals);
            AssertResponsesAreEqual(BAD_RESPONSE_FORM_INVALID, result);

            User passwordNoNumbers = new User
            {
                Email = GenerateRandomValidEmail(),
                Password = "Bobbyjones!"
            };

            homeController.ViewData.ModelState.Clear();
            MockModelState(passwordNoNumbers, homeController);

            result = await homeController.RegisterUser(passwordNoNumbers);
            AssertResponsesAreEqual(BAD_RESPONSE_FORM_INVALID, result);

            User passwordNoSpecialChars = new User
            {
                Email = GenerateRandomValidEmail(),
                Password = "Bobby1234"
            };

            homeController.ViewData.ModelState.Clear();
            MockModelState(passwordNoSpecialChars, homeController);

            result = await homeController.RegisterUser(passwordNoSpecialChars);
            AssertResponsesAreEqual(BAD_RESPONSE_FORM_INVALID, result);
        }

        [TestMethod()]
        public async Task TestExistingUser()
        {
            // Generate a UUID
            string testEmail = GenerateRandomValidEmail();

            // Add the user
            User testUser = new User
            {
                Email = testEmail,
                Password = GOOD_PASSWORD
            };

            var result = await homeController.RegisterUser(testUser);
            AssertResponsesAreEqual(GOOD_RESPONSE, result);

            // Now try and add the same user again
            result = await homeController.RegisterUser(testUser);
            AssertResponsesAreEqual(BAD_RESPONSE_DUPLICATE, result);
        }

        private string GenerateRandomValidEmail()
        {
            // Generate a UUID
            Guid myuuid = Guid.NewGuid();
            return myuuid.ToString() + "@gmail.com";
        }

        private void AssertResponsesAreEqual(Response expectedResponse, ActionResult<Response> result)
        {
            Response actualResponse = result.Value as Response;

            Assert.AreEqual(expectedResponse.StatusCode, actualResponse.StatusCode);
            Assert.AreEqual(expectedResponse.ResponseMessage, actualResponse.ResponseMessage);
        }

        private void MockModelState<TModel, TController>(TModel model, TController controller) where TController : ControllerBase
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(model, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            foreach (var validationResult in validationResults)
            {
                controller.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }
        }
    }
}