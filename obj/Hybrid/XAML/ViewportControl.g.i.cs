﻿#pragma checksum "..\..\..\XAML\ViewportControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F4D5B08420CFEC25CCB59231A70552F2"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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


namespace AdnCuiSamples {
    
    
    /// <summary>
    /// ViewportControl
    /// </summary>
    public partial class ViewportControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 8 "..\..\..\XAML\ViewportControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal AdnCuiSamples.ViewportControl UserControl;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\..\XAML\ViewportControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid LayoutRoot;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\..\XAML\ViewportControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox listSceneNodes;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\XAML\ViewportControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button buttonAllNodes;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\XAML\ViewportControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button buttonLights;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\XAML\ViewportControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button buttonSelectNode;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\XAML\ViewportControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button buttonClearSelection;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\XAML\ViewportControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button buttonMinMax;
        
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
            System.Uri resourceLocater = new System.Uri("/AdnCuiSample;component/xaml/viewportcontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\XAML\ViewportControl.xaml"
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
            this.UserControl = ((AdnCuiSamples.ViewportControl)(target));
            return;
            case 2:
            this.LayoutRoot = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.listSceneNodes = ((System.Windows.Controls.ListBox)(target));
            return;
            case 4:
            this.buttonAllNodes = ((System.Windows.Controls.Button)(target));
            
            #line 13 "..\..\..\XAML\ViewportControl.xaml"
            this.buttonAllNodes.Click += new System.Windows.RoutedEventHandler(this.buttonAllNodes_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.buttonLights = ((System.Windows.Controls.Button)(target));
            
            #line 14 "..\..\..\XAML\ViewportControl.xaml"
            this.buttonLights.Click += new System.Windows.RoutedEventHandler(this.buttonLights_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.buttonSelectNode = ((System.Windows.Controls.Button)(target));
            
            #line 15 "..\..\..\XAML\ViewportControl.xaml"
            this.buttonSelectNode.Click += new System.Windows.RoutedEventHandler(this.buttonSelect_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.buttonClearSelection = ((System.Windows.Controls.Button)(target));
            
            #line 16 "..\..\..\XAML\ViewportControl.xaml"
            this.buttonClearSelection.Click += new System.Windows.RoutedEventHandler(this.buttonClearSelect_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.buttonMinMax = ((System.Windows.Controls.Button)(target));
            
            #line 17 "..\..\..\XAML\ViewportControl.xaml"
            this.buttonMinMax.Click += new System.Windows.RoutedEventHandler(this.buttonMinMax_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

