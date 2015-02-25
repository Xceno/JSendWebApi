﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using FluentAssertions;
using JSendWebApi.Properties;
using JSendWebApi.Responses;
using JSendWebApi.Results;
using JSendWebApi.Tests.FixtureCustomizations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace JSendWebApi.Tests.Results
{
    public class JSendExceptionResultTests
    {
        [Theory, JSendAutoData]
        public void IsHttpActionResult(JSendExceptionResult result)
        {
            // Exercise system and verify outcome
            result.Should().BeAssignableTo<IHttpActionResult>();
        }

        [Theory, JSendAutoData]
        public void ConstructorThrowsWhenControllerIsNull(Exception ex, string message, int? code, object data)
        {
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new JSendExceptionResult(ex, message, code, data, null));
        }

        [Theory, JSendAutoData]
        public void ConstructorsThrowWhenExceptionIsNull(
            JSendApiController controller, bool includeErrorDetail, JsonSerializerSettings settings, Encoding encoding,
            HttpRequestMessage request, string message, int? code, object data)
        {
            // Fixture setup
            Exception ex = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(
                () => new JSendExceptionResult(ex, message, code, data, includeErrorDetail, settings, encoding, request));
            Assert.Throws<ArgumentNullException>(() => new JSendExceptionResult(ex, message, code, data, controller));
        }

        [Theory, JSendAutoData]
        public void StatusCodeIs500(JSendExceptionResult result)
        {
            // Exercise system and verify outcome
            result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Theory, JSendAutoData]
        public void ExceptionIsCorrectlyInitialized(JSendApiController controller, Exception ex, string message,
            int? code, object data)
        {
            // Exercise system
            var result = new JSendExceptionResult(ex, message, code, data, controller);
            // Verify outcome
            result.Exception.Should().Be(ex);
        }

        [Theory, JSendAutoData]
        public void ResponseIsError(JSendExceptionResult result)
        {
            // Exercise system and verify outcome
            result.Response.Should().BeAssignableTo<ErrorResponse>();
        }

        [Theory, JSendAutoData]
        public async Task ResponseIsSerializedIntoBody(JSendExceptionResult result)
        {
            // Fixture setup
            var serializedResponse = JsonConvert.SerializeObject(result.Response);
            // Exercise system
            var httpResponseMessage = await result.ExecuteAsync(new CancellationToken());
            // Verify outcome
            var content = await httpResponseMessage.Content.ReadAsStringAsync();
            content.Should().Be(serializedResponse);
        }

        [Theory, JSendAutoData]
        public void ResponseMessageIsSetToMessage_When_MessageIsNotNull(
            [Frozen] string message, JSendExceptionResult result)
        {
            // Exercise system and verify outcome
            result.Response.Message.Should().Be(message);
        }

        [Theory, JSendAutoData]
        public void ResponseMessageIsSetToExceptionMessage_When_MessageIsNull_And_ControllerIsConfiguredToIncludeErrorDetails(
            JSendApiController controller, Exception ex, int? code, object data)
        {
            // Fixture setup
            controller.RequestContext.IncludeErrorDetail = true;
            // Exercise system
            var result = new JSendExceptionResult(ex, null, code, data, controller);
            // Verify outcome
            result.Response.Message.Should().Be(ex.Message);
        }

        [Theory, JSendAutoData]
        public void ResponseMessageIsSetToDefaultMessage_When_MessageIsNull_And_ControllerIsConfiguredToNotIncludeErrorDetails(
            JSendApiController controller, Exception ex, int? code, object data)
        {
            // Fixture setup
            controller.RequestContext.IncludeErrorDetail = false;
            // Exercise system
            var result = new JSendExceptionResult(ex, null, code, data, controller);
            // Verify outcome
            result.Response.Message.Should().Be(StringResources.DefaultErrorMessage);
        }

        [Theory, JSendAutoData]
        public void ResponseDataIsSetToData_When_DataIsNotNull(
            JSendApiController controller, Exception ex, string message, int? code, object data)
        {
            // Exercise system 
            var result = new JSendExceptionResult(ex, message, code, data, controller);
            // Verify outcome
            result.Response.Data.Should().BeSameAs(data);
        }

        [Theory, JSendAutoData]
        public void ResponseDataIsSetToStringifiedException_When_DataIsNull_And_ControllerIsConfiguredToIncludeErrorDetails(
            JSendApiController controller, Exception ex, string message, int? code)
        {
            // Fixture setup
            controller.RequestContext.IncludeErrorDetail = true;
            // Exercise system
            var result = new JSendExceptionResult(ex, message, code, null, controller);
            // Verify outcome
            result.Response.Data.Should().Be(ex.ToString());
        }

        [Theory, JSendAutoData]
        public void ResponseDataIsSetToNull_When_DataIsNull_And_ControllerIsConfiguredToNotIncludeErrorDetails(
            JSendApiController controller, Exception ex, string message, int? code)
        {
            // Fixture setup
            controller.RequestContext.IncludeErrorDetail = false;
            // Exercise system
            var result = new JSendExceptionResult(ex, message, code, null, controller);
            // Verify outcome
            result.Response.Data.Should().BeNull();
        }

        [Theory, JSendAutoData]
        public void ResponseCodeIsCorrectlySet([Frozen] int? code, JSendExceptionResult result)
        {
            // Exercise system and verify outcome
            result.Response.Code.Should().HaveValue()
                .And.Be(code);
        }

        [Theory, JSendAutoData]
        public async Task SetsStatusCode(JSendExceptionResult result)
        {
            // Exercise system
            var message = await result.ExecuteAsync(new CancellationToken());
            // Verify outcome
            message.StatusCode.Should().Be(result.StatusCode);
        }

        [Theory, JSendAutoData]
        public async Task SetsCharSetHeader(IFixture fixture)
        {
            // Fixture setup
            var encoding = Encoding.ASCII;
            fixture.Inject(encoding);

            var result = fixture.Create<JSendExceptionResult>();
            // Exercise system
            var message = await result.ExecuteAsync(new CancellationToken());
            // Verify outcome
            message.Content.Headers.ContentType.CharSet.Should().Be(encoding.WebName);
        }

        [Theory, JSendAutoData]
        public async Task SetsContentTypeHeader(JSendExceptionResult result)
        {
            // Exercise system
            var message = await result.ExecuteAsync(new CancellationToken());
            // Verify outcome
            message.Content.Headers.ContentType.MediaType.Should().Be("application/json");
        }
    }
}
