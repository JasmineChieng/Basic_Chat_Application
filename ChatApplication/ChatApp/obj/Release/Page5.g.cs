﻿#pragma checksum "..\..\Page5.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "2D14169F818D33E25BB10A0D745AE90465BDCBB67D53D0065749D84A67D1A445"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using ChatApp;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace ChatApp {
    
    
    /// <summary>
    /// Page5
    /// </summary>
    public partial class Page5 : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 18 "..\..\Page5.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button viewMoreBtn;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\Page5.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox chatListBox;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\Page5.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox messageTB;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\Page5.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button sendBtn;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\Page5.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid sidePanel;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\Page5.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel sideStackPanel;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\Page5.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button leaveGroupBtn;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ChatApp;component/page5.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Page5.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.viewMoreBtn = ((System.Windows.Controls.Button)(target));
            
            #line 18 "..\..\Page5.xaml"
            this.viewMoreBtn.Click += new System.Windows.RoutedEventHandler(this.viewMoreBtn_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.chatListBox = ((System.Windows.Controls.ListBox)(target));
            return;
            case 3:
            this.messageTB = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.sendBtn = ((System.Windows.Controls.Button)(target));
            
            #line 38 "..\..\Page5.xaml"
            this.sendBtn.Click += new System.Windows.RoutedEventHandler(this.sendBtn_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.sidePanel = ((System.Windows.Controls.Grid)(target));
            return;
            case 6:
            
            #line 45 "..\..\Page5.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.closeBtn_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.sideStackPanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 8:
            this.leaveGroupBtn = ((System.Windows.Controls.Button)(target));
            
            #line 51 "..\..\Page5.xaml"
            this.leaveGroupBtn.Click += new System.Windows.RoutedEventHandler(this.leaveGroupBtn_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

