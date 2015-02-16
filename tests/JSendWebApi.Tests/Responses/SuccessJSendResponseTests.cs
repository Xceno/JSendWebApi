﻿using System;
using FluentAssertions;
using JSendWebApi.Responses;
using JSendWebApi.Tests.FixtureCustomizations;
using JSendWebApi.Tests.TestTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace JSendWebApi.Tests.Responses
{
    public class SuccessJSendResponseTests
    {
        [Theory, JSendAutoData]
        public void StatusIsSuccess(SuccessJSendResponse response)
        {
            // Exercise system and verify outcome
            response.Status.Should().Be("success");
        }

        [Theory, JSendAutoData]
        public void DataIsCorrectlyInitialized(Model model)
        {
            // Exercise system
            var response = new SuccessJSendResponse(model);
            // Verify outcome
            response.Data.Should().BeSameAs(model);
        }

        [Fact]
        public void DataIsNullByDefault()
        {
            // Exercise system
            var response = new SuccessJSendResponse();
            // Verify outcome
            response.Data.Should().BeNull();
        }

        [Theory, JSendAutoData]
        public void SerializesCorrectly(object data, [Greedy] SuccessJSendResponse response)
        {
            // Fixture setup
            var expectedSerializedResponse = new JObject
            {
                {"status", "success"},
                {"data", JObject.FromObject(data)}
            };

            // Exercise system
            var serializedResponse = JObject.FromObject(response);
            // Verify outcome
            JToken.DeepEquals(serializedResponse, expectedSerializedResponse)
                .Should().BeTrue();
        }

        [Theory, JSendAutoData]
        public void NullDataIsSerialized()
        {
            // Fixture setup
            var response = new SuccessJSendResponse();
            // Exercise system
            var serializedResponse = JObject.FromObject(response);
            // Verify outcome
            serializedResponse.Should().ContainKey("data");
            serializedResponse.Value<string>("data").Should().BeNull();
        }
    }
}
