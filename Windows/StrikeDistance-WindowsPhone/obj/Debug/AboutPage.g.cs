﻿

#pragma checksum "C:\Users\devgr\GitHub Repositories\StrikeDistance\Windows\StrikeDistance-WindowsPhone\AboutPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "0B5D999CA50F3B914BEBFD9314F08E17"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StrikeDistance_WindowsPhone
{
    partial class AboutPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 32 "..\..\AboutPage.xaml"
                ((global::Windows.UI.Xaml.Controls.WebView)(target)).NavigationCompleted += this.web_NavigationCompleted;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 38 "..\..\AboutPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.raindrop_Click;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


