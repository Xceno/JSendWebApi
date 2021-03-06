﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using JSend.WebApi.Properties;
using JSend.WebApi.Responses;

namespace JSend.WebApi.Results
{
    /// <summary>
    /// Represents an action result that returns a <see cref="FailResponse"/> with status code <see cref="HttpStatusCode.BadRequest"/>.
    /// </summary>
    public class JSendBadRequestResult : IJSendResult<FailResponse>
    {
        private readonly JSendResult<FailResponse> _result;

        /// <summary>Initializes a new instance of <see cref="JSendBadRequestResult"/>.</summary>
        /// <param name="reason">The reason why the request could not be processed.</param>
        /// <param name="controller">The controller from which to obtain the dependencies needed for execution.</param>
        public JSendBadRequestResult(string reason, ApiController controller)
        {
            if (reason == null)
                throw new ArgumentNullException(nameof(reason));

            if (string.IsNullOrWhiteSpace(reason))
                throw new ArgumentException(StringResources.BadRequest_WhiteSpaceReason, nameof(reason));

            _result = new JSendResult<FailResponse>(HttpStatusCode.BadRequest, new FailResponse(reason), controller);
        }

        /// <summary>Gets the response to be formatted into the message's body.</summary>
        public FailResponse Response => _result.Response;

        /// <summary>Gets the HTTP status code for the response message.</summary>
        public HttpStatusCode StatusCode => _result.StatusCode;

        /// <summary>Gets the request message which led to this result.</summary>
        public HttpRequestMessage Request => _result.Request;

        /// <summary>Gets the reason why the request could not be processed.</summary>
        public string Reason => (string) _result.Response.Data;

        /// <summary>Creates an <see cref="HttpResponseMessage"/> asynchronously.</summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that, when completed, contains the <see cref="HttpResponseMessage"/>.</returns>
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            => _result.ExecuteAsync(cancellationToken);
    }
}
