﻿

#pragma checksum "C:\Users\Gregory\GitHub Repositories\StrikeDistance\Windows\StrikeDistance-WindowsPhone\SettingsPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "26CA3961E8A179AADA80568B6DBDD1BE"
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
    partial class SettingsPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 46 "..\..\SettingsPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)(target)).Checked += this.unitDetails_Checked;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 47 "..\..\SettingsPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)(target)).Checked += this.convMath_Checked;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 48 "..\..\SettingsPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)(target)).Checked += this.calcMath_Checked;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


