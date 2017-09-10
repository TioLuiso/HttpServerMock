﻿namespace HttpServerMock.Common.Model
{
    /// <summary>
    /// Available HTTP request methods.
    /// </summary>
    public enum Method
    {
        /// <summary>
        /// HTTP get method.
        /// </summary>
        GET,

        /// <summary>
        /// HTTP post method.
        /// </summary>
        POST,

        /// <summary>
        /// HTTP delete method.
        /// </summary>
        DELETE,

        /// <summary>
        /// HTTP put method.
        /// </summary>
        PUT,

        /// <summary>
        /// HTTP patch method.
        /// </summary>
        PATCH,

        /// <summary>
        /// HTTP options method.
        /// </summary>
        OPTIONS
    }
}