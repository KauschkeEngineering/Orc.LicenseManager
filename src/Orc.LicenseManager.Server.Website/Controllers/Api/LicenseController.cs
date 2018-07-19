﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckLicenseController.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Server.Website.Controllers.Api
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Catel;
    using Models;
    using Server.Services;

    public class LicenseController : ApiController
    {
        private readonly ILicenseValidationService _licenseValidationService;

        public LicenseController(ILicenseValidationService licenseValidationService)
        {
            Argument.IsNotNull(() => licenseValidationService);

            _licenseValidationService = licenseValidationService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> Validate([FromBody]LicenseValidation licenseValidation)
        {
            Argument.IsNotNull(() => licenseValidation);

            var licenseValidationResult = await _licenseValidationService.ValidateLicenseAsync(licenseValidation.License);

            return new HttpResponseMessage
            {
                Content = new JsonContent(licenseValidationResult)
            };
        }
    }
}