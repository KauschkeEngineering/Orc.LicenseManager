﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlainTextFormatter.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Server.Website.Formatting
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    public class PlainTextFormatter : MediaTypeFormatter
    {
        public PlainTextFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/plain"));
        }

        #region Methods
        public override bool CanReadType(Type type)
        {
            if (type == typeof (string))
            {
                return true;
            }

            return false;
        }

        public override bool CanWriteType(Type type)
        {
            if (type == typeof(string))
            {
                return true;
            }

            return false;
        }

        public override async Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            using (var streamReader = new StreamReader(readStream))
            {
                return await streamReader.ReadToEndAsync();
            }
        }
        #endregion
    }
}