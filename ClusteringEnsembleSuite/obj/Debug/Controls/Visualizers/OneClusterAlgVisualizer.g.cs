﻿#pragma checksum "..\..\..\..\Controls\Visualizers\OneClusterAlgVisualizer.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "11C54A1A182FBF054FBB87D72C0B3544"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
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


namespace ClusteringEnsembleSuite.Controls.Visualizers {
    
    
    /// <summary>
    /// OneClusterAlgVisualizer
    /// </summary>
    public partial class OneClusterAlgVisualizer : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\..\..\Controls\Visualizers\OneClusterAlgVisualizer.xaml"
        internal System.Windows.Controls.StackPanel sp;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\..\Controls\Visualizers\OneClusterAlgVisualizer.xaml"
        internal System.Windows.Controls.Button bt_select;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ClusteringEnsembleSuite;component/controls/visualizers/oneclusteralgvisualizer.x" +
                    "aml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Controls\Visualizers\OneClusterAlgVisualizer.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.sp = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 2:
            this.bt_select = ((System.Windows.Controls.Button)(target));
            
            #line 16 "..\..\..\..\Controls\Visualizers\OneClusterAlgVisualizer.xaml"
            this.bt_select.Click += new System.Windows.RoutedEventHandler(this.bt_select_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

