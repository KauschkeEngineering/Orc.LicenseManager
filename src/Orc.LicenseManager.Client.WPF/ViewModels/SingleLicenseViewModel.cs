﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SingleLicenseViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using Catel;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Services;
    using Models;
    using Services;

    /// <summary>
    /// View model for a single License.
    /// </summary>
    public class SingleLicenseViewModel : ViewModelBase
    {
        #region Constants
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region Fields
        private readonly INavigationService _navigationService;
        private readonly IProcessService _processService;

        private readonly ILicenseService _licenseService;

        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IViewModelFactory _viewModelFactory;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SingleLicenseViewModel" /> class.
        /// </summary>
        /// <param name="singleLicenseModel">The single license model.</param>
        /// <param name="navigationService">The navigation service.</param>
        /// <param name="processService">The process service.</param>
        /// <param name="licenseService">The license service.</param>
        /// <param name="uiVisualizerService">The uiVisualizer service.</param>
        /// <param name="viewModelFactory">The uiVisualizer service.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="singleLicenseModel" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="navigationService" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="processService" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="licenseService" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="uiVisualizerService" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="viewModelFactory" /> is <c>null</c>.</exception>
        public SingleLicenseViewModel(SingleLicenseModel singleLicenseModel, INavigationService navigationService, IProcessService processService,
            ILicenseService licenseService, IUIVisualizerService uiVisualizerService, IViewModelFactory viewModelFactory)
        {
            Argument.IsNotNull(() => singleLicenseModel);
            Argument.IsNotNull(() => navigationService);
            Argument.IsNotNull(() => processService);
            Argument.IsNotNull(() => licenseService);
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => viewModelFactory);

            _navigationService = navigationService;
            _processService = processService;
            _licenseService = licenseService;
            _uiVisualizerService = uiVisualizerService;
            _viewModelFactory = viewModelFactory;

            SingleLicenseModel = singleLicenseModel;
            XmlData = new ObservableCollection<XmlDataModel>();

            Paste = new Command(OnPasteExecute);
            ShowClipboard = new Command(OnShowClipboardExecute);
            PurchaseLinkClick = new Command(OnPurchaseLinkClickExecute);
            AboutSiteClick = new Command(OnAboutSiteClickExecute);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the Close command.
        /// </summary>
        /// <summary>
        /// Gets the WebsiteClick command.
        /// </summary>
        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool FailureOccurred { get; set; }

        /// <summary>
        /// Gets the failure message.
        /// </summary>
        /// <value>
        /// The failure message.
        /// </value>
        public string FailureMessage { get; private set; }

        /// <summary>
        /// Gets the failure solution.
        /// </summary>
        /// <value>
        /// The failure solution.
        /// </value>
        public string FailureSolution { get; private set; }

        /// <summary>
        /// Gets the PurchaseLinkClick command.
        /// </summary>
        public Command PurchaseLinkClick { get; private set; }

        /// <summary>
        /// Gets the AboutSiteClick command.
        /// </summary>
        public Command AboutSiteClick { get; private set; }

        [Model]
        [Catel.Fody.Expose("Title")]
        [Catel.Fody.Expose("PurchaseLink")]
        [Catel.Fody.Expose("AboutTitle")]
        [Catel.Fody.Expose("AboutSite")]
        [Catel.Fody.Expose("AboutText")]
        [Catel.Fody.Expose("AboutImage")]
        [Catel.Fody.Expose("Key")]
        private SingleLicenseModel SingleLicenseModel { get; set; }

        /// <summary>
        /// Gets the about image URI.
        /// </summary>
        /// <value>
        /// The about image URI.
        /// </value>
        public Uri AboutImageUri { get { return new Uri(SingleLicenseModel.AboutImage, UriKind.RelativeOrAbsolute);} }

        /// <summary>
        /// Gets the Paste command.
        /// </summary>
        public Command Paste { get; private set; }

        /// <summary>
        /// List of xml Data, only populated when license was valid.
        /// </summary>
        public ObservableCollection<XmlDataModel> XmlData { get; set; }

        /// <summary>
        /// Gets the ShowClipboard command.
        /// </summary>
        public Command ShowClipboard { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether [paste success].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [paste success]; otherwise, <c>false</c>.
        /// </value>
        public bool PasteSuccess { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [show failure].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show failure]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowFailure { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Method to invoke when the AboutSiteClick command is executed.
        /// </summary>
        private void OnAboutSiteClickExecute()
        {
            _processService.StartProcess(SingleLicenseModel.AboutSite);
        }

        /// <summary>
        /// Method to invoke when the PurchaseLinkClick command is executed.
        /// </summary>
        private void OnPurchaseLinkClickExecute()
        {
            _processService.StartProcess(SingleLicenseModel.PurchaseLink);
        }

        /// <summary>
        /// Initializes the view model. Normally the initialization is done in the constructor, but sometimes this must be delayed
        /// to a state where the associated UI element (user control, window, ...) is actually loaded.
        /// <para />
        /// This method is called as soon as the associated UI element is loaded.
        /// </summary>
        /// <remarks>
        /// It's not recommended to implement the initialization of properties in this method. The initialization of properties
        /// should be done in the constructor. This method should be used to start the retrieval of data from a web service or something
        /// similar.
        /// <para />
        /// During unit tests, it is recommended to manually call this method because there is no external container calling this method.
        /// </remarks>
        protected override async Task Initialize()
        {
            var pleaseWaitService = DependencyResolver.Resolve<IPleaseWaitService>(); // Once we ll verify online this will be useful
            pleaseWaitService.Show("Checking licenses");

            if (_licenseService.LicenseExists())
            {
                var licenseText = _licenseService.LoadLicense();
                var validationContext = _licenseService.ValidateLicense(licenseText);
                if (validationContext.HasErrors == false)
                {
                    FailureOccurred = false;
                }
                else
                {
                    FailureOccurred = true;
                }
            }
            else
            {
                FailureOccurred = true;
            }

            pleaseWaitService.Hide();
        }

        protected override async Task<bool> Save()
        {
            var validationContext = _licenseService.ValidateLicense(SingleLicenseModel.Key);
            if (validationContext.HasErrors)
            {
                return false;
            }

            _licenseService.SaveLicense(SingleLicenseModel.Key);

            return true;
        }

        protected override async Task<bool> Cancel()
        {
            Log.Debug("Closing application");

            _navigationService.CloseApplication();

            return true;
        }

        /// <summary>
        /// Method to invoke when the Paste command is executed. Validates the license and xml, 
        /// </summary>
        private void OnPasteExecute()
        {
            if (FailureOccurred == true)
            {
                XmlData.Clear();
                if (Clipboard.GetText() != string.Empty)
                {
                    SingleLicenseModel.Key = Clipboard.GetText();
                    string license = SingleLicenseModel.Key;
                    var xmlFirstError = _licenseService.ValidateXml(license).GetBusinessRuleErrors().FirstOrDefault();
                    if (xmlFirstError == null)
                    {
                        var normalFirstError = _licenseService.ValidateLicense(SingleLicenseModel.Key).GetBusinessRuleErrors().FirstOrDefault();
                        if (normalFirstError == null)
                        {
                            var xmlList = _licenseService.LoadXmlFromLicense(SingleLicenseModel.Key);
                            xmlList.ForEach(XmlData.Add);
                            FailureOccurred = false;
                            ShowFailure = false;
                        }
                        else
                        {
                            ShowFailure = true;
                            FailureMessage = normalFirstError.Message;
                            FailureSolution = normalFirstError.Tag as string;
                        }
                    }
                    else
                    {
                        ShowFailure = true;
                        FailureMessage = xmlFirstError.Message;
                        FailureSolution = xmlFirstError.Tag as string;
                    }
                }
                else
                {
                    ShowFailure = true;

                    // TODO: Read from resources (language service preferred)
                    FailureMessage = "No text was pasted into the windows.";
                    FailureSolution = "Please copy the license text into this window.";
                }
            }
        }

        /// <summary>
        /// Method to invoke when the ShowClipboard command is executed.
        /// </summary>
        private void OnShowClipboardExecute()
        {
            var vm = _viewModelFactory.CreateViewModel<ClipBoardViewModel>(null);
            _uiVisualizerService.ShowDialog(vm);
        }
        #endregion
    }
}