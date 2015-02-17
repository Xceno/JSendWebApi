﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using JSendWebApi.Responses;

namespace JSendWebApi.Results
{
    public class JSendBadRequestResult : IHttpActionResult
    {
        private readonly JSendResult<FailJSendResponse> _result;

        public JSendBadRequestResult(JSendApiController controller, string reason)
        {
            if (reason == null)
                throw new ArgumentNullException("reason");

            if (string.IsNullOrWhiteSpace(reason))
                throw new ArgumentException("Reason cannot be an empty string.", "reason");

            _result = new JSendResult<FailJSendResponse>(controller, new FailJSendResponse(reason),
                HttpStatusCode.BadRequest);
        }

        public FailJSendResponse Response
        {
            get { return _result.Response; }
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return _result.ExecuteAsync(cancellationToken);
        }
    }
}
