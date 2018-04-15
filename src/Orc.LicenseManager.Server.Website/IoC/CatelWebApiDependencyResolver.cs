﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CatelWebApiDependencyResolver.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Server.Website.IoC
{
    using System;
    using System.Collections.Generic;
    using Catel;
    using Catel.IoC;

    public class CatelWebApiDependencyResolver : CatelDependencyResolver, System.Web.Http.Dependencies.IDependencyResolver
    {
        #region Fields
        private readonly IServiceLocator _serviceLocator;
        private readonly ITypeFactory _typeFactory;
        #endregion

        #region Constructors
        public CatelWebApiDependencyResolver()
            : this(ServiceLocator.Default)
        {
        }

        public CatelWebApiDependencyResolver(IServiceLocator serviceLocator)
            : base(serviceLocator)
        {
            Argument.IsNotNull(() => serviceLocator);

            _serviceLocator = serviceLocator;
            _typeFactory = serviceLocator.ResolveType<ITypeFactory>();
        }
        #endregion

        #region IDependencyResolver Members
        public System.Web.Http.Dependencies.IDependencyScope BeginScope()
        {
            // This resolver does not support child scopes, so we simply return 'this'.
            return this;
        }

        public void Dispose()
        {
            // nothing to dispose
        }
        #endregion
    }
}