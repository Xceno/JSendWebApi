﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Web.Http;
using JSend.WebApi.Responses;

namespace JSend.WebApi.FunctionalTests
{
    [RoutePrefix("users")]
    public class UsersController : JSendApiController
    {
        public static readonly User TestUser = new User {Username = "DCastro"};
        public static readonly string TestLocation = "http://localhost/users/dummy-location/5";

        public static readonly string ErrorMessage = "dummy error message";
        public static readonly int ErrorCode = 80;
        public static readonly object ErrorData = DateTime.UtcNow;

        public static readonly Exception TestException = new InvalidOperationException("dummy exception message");

        public static readonly string ModelErrorKey = "Username";
        public static readonly string ModelErrorValue = "Invalid Username";

        public static readonly string AuthenticationHeader = "dummy-authentication-header";

        /// <summary>
        /// Dummy action to redirect to.
        /// </summary>
        [Route("dummy-location/{id:int}", Name = "DummyLocation"), HttpGet]
        public void DummyLocation(int id)
        {
        }

        [Route("ok"), HttpGet]
        public IHttpActionResult OkAction() => JSendOk();

        [Route("ok-with-user"), HttpGet]
        public IHttpActionResult OkWithDataAction() => JSendOk(TestUser);

        [Route("created-with-string"), HttpGet]
        public IHttpActionResult CreatedWithStringAction() => JSendCreated(TestLocation, TestUser);

        [Route("created-with-uri"), HttpGet]
        public IHttpActionResult CreatedWithUriAction() => JSendCreated(new Uri(TestLocation), TestUser);

        [Route("created-at-route-with-object"), HttpGet]
        public IHttpActionResult CreatedAtRouteWithObjectAction()
            => JSendCreatedAtRoute("DummyLocation", new {id = 5}, TestUser);

        [Route("created-at-route-with-dictionary"), HttpGet]
        public IHttpActionResult CreatedAtRouteWithDictionaryAction()
        {
            var routeValues = new Dictionary<string, object>
            {
                {"id", 5}
            };
            return JSendCreatedAtRoute("DummyLocation", routeValues, TestUser);
        }

        [Route("redirect-with-string"), HttpGet]
        public IHttpActionResult RedirectWithStringAction() => JSendRedirect(TestLocation);

        [Route("redirect-with-uri"), HttpGet]
        public IHttpActionResult RedirectWithUriAction() => JSendRedirect(new Uri(TestLocation));

        [Route("redirect-to-route-with-object"), HttpGet]
        public IHttpActionResult RedirectToRouteWithObjectAction()
            => JSendRedirectToRoute("DummyLocation", new {id = 5});

        [Route("redirect-to-route-with-dictionary"), HttpGet]
        public IHttpActionResult RedirectToRouteWithDictionaryAction()
        {
            var routeValues = new Dictionary<string, object>
            {
                {"id", 5}
            };
            return JSendRedirectToRoute("DummyLocation", routeValues);
        }

        [Route("badrequest-with-reason"), HttpGet]
        public IHttpActionResult BadRequestWithReasonAction() => JSendBadRequest(ErrorMessage);

        [Route("badrequest-with-modelstate"), HttpGet]
        public IHttpActionResult BadRequestWithModelStateAction()
        {
            ModelState.AddModelError(ModelErrorKey, ModelErrorValue);
            return JSendBadRequest(ModelState);
        }

        [Route("unauthorized"), HttpGet]
        public IHttpActionResult UnauthorizedAction()
            => JSendUnauthorized(new AuthenticationHeaderValue(AuthenticationHeader));

        [Route("notfound"), HttpGet]
        public IHttpActionResult NotFoundAction() => JSendNotFound();

        [Route("notfound-with-reason"), HttpGet]
        public IHttpActionResult NotFoundWithReasonAction() => JSendNotFound(ErrorMessage);

        [Route("internal-server-error"), HttpGet]
        public IHttpActionResult InternalServerErrorAction()
            => JSendInternalServerError(ErrorMessage, ErrorCode, ErrorData);

        [Route("internal-server-error-with-exception"), HttpGet]
        public IHttpActionResult InternalServerErrorWithExceptionAction() => JSendInternalServerError(TestException);

        [Route("jsend"), HttpGet]
        public IHttpActionResult JSendAction()
        {
            var response = new SuccessResponse();

            return JSend(HttpStatusCode.Gone, response);
        }

        [Route("jsend-success"), HttpGet]
        public IHttpActionResult JSendSuccessAction() => JSendSuccess(HttpStatusCode.Gone, TestUser);

        [Route("jsend-fail"), HttpGet]
        public IHttpActionResult JSendFailAction() => JSendFail(HttpStatusCode.Gone, ErrorMessage);

        [Route("jsend-error"), HttpGet]
        public IHttpActionResult JSendErrorAction()
            => JSendError(HttpStatusCode.Gone, ErrorMessage, ErrorCode, ErrorData);

        [Route("void"), HttpGet]
        public void VoidAction()
        {
            
        }

        [Route("value"), HttpGet]
        public User ValueAction() => TestUser;

        [Route("exception"), HttpGet]
        public void ExceptionAction()
        {
            throw TestException;
        }

        [Route("exception-without-details"), HttpGet]
        public void ExceptionWithoutDetailsAction()
        {
            Configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Never;
            throw TestException;
        }
    }
}
