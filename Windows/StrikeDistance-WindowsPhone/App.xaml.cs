﻿using SDEngine.Memory;
using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Phone.UI.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

namespace StrikeDistance_WindowsPhone
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
        private TransitionCollection transitions;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
            HardwareButtons.BackPressed += (s, e) =>
            {
                var frame = Window.Current.Content as Frame;
                if (frame != null && frame.CanGoBack)
                {
                    e.Handled = true;
                    frame.GoBack();
                }
            };
            UnhandledException += (s, e) => {
                e.Handled = true;
            };
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                DebugSettings.EnableFrameRateCounter = false;
            }
#endif
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                rootFrame = new Frame();
                rootFrame.CacheSize = 1;
                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                }
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                if (rootFrame.ContentTransitions != null)
                {
                    transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        transitions.Add(c);
                    }
                }
                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += RootFrame_FirstNavigated;
                if (!Manager.AutoGet) {
                    int lc = SDEngine.Memory.Utility.UtilityMethods.Get("LaunchCount", 0);
                    SDEngine.Memory.Utility.UtilityMethods.Set("LaunchCount", lc + 1);
                    if (!rootFrame.Navigate(typeof(CalculatorPage), (lc == 5 || lc == 10))) {
                        throw new Exception("Failed to create initial page");
                    }
                }
                else {
                    if (!rootFrame.Navigate(typeof(LoadingPage), e.Arguments)) {
                        throw new Exception("Failed to create initial page");
                    }
                }
            }
            Window.Current.Activate();
        }

        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the navigation event.</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.Navigated -= RootFrame_FirstNavigated;
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }
    }
}