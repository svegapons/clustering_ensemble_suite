﻿#pragma checksum "..\..\..\..\Controls\Visualizers\ListSetsVisualizer.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "6E2A563DBDC70075FFA344175DDE601C"
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
    /// ListSetsVisualizer
    /// </summary>
    public partial class ListSetsVisualizer : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 6 "..\..\..\..\Controls\Visualizers\ListSetsVisualizer.xaml"
        internal System.Windows.Controls.GroupBox gb;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\..\Controls\Visualizers\ListSetsVisualizer.xaml"
        internal System.Windows.Controls.ListView lv_Sets;
        
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
            System.Uri resourceLocater = new System.Uri("/ClusteringEnsembleSuite;component/controls/visualizers/listsetsvisualizer.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Controls\Visualizers\ListSetsVisualizer.xaml"
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
            this.gb = ((System.Windows.Controls.GroupBox)(target));
            return;
            case 2:
            this.lv_Sets = ((System.Windows.Controls.ListView)(target));
            
            #line 13 "..\..\..\..\Controls\Visualizers\ListSetsVisualizer.xaml"
            this.lv_Sets.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.lv_Sets_SelectionChanged);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

