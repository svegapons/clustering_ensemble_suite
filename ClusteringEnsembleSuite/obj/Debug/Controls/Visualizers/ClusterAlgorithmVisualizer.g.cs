﻿#pragma checksum "..\..\..\..\Controls\Visualizers\ClusterAlgorithmVisualizer.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "D33DF5F450E056C9677BF68917C8BCF5"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using ClusterEnsemble.DataStructures;
using ClusteringEnsembleSuite.Controls.Visualizers;
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
using Telerik.Windows.Controls;


namespace ClusteringEnsembleSuite.Controls.Visualizers {
    
    
    /// <summary>
    /// ClusterAlgorithmVisualizer
    /// </summary>
    public partial class ClusterAlgorithmVisualizer : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 25 "..\..\..\..\Controls\Visualizers\ClusterAlgorithmVisualizer.xaml"
        internal Telerik.Windows.Controls.RadTreeView tv_ClusterAlgs;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\..\Controls\Visualizers\ClusterAlgorithmVisualizer.xaml"
        internal System.Windows.Controls.GroupBox gb_Proximities;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\..\Controls\Visualizers\ClusterAlgorithmVisualizer.xaml"
        internal Telerik.Windows.Controls.RadComboBox cb_Proximities;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\..\Controls\Visualizers\ClusterAlgorithmVisualizer.xaml"
        internal ClusteringEnsembleSuite.Controls.Visualizers.OneClusterAlgVisualizer uctrl_OneClustAlgVis;
        
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
            System.Uri resourceLocater = new System.Uri("/ClusteringEnsembleSuite;component/controls/visualizers/clusteralgorithmvisualize" +
                    "r.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Controls\Visualizers\ClusterAlgorithmVisualizer.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 9 "..\..\..\..\Controls\Visualizers\ClusterAlgorithmVisualizer.xaml"
            ((ClusteringEnsembleSuite.Controls.Visualizers.ClusterAlgorithmVisualizer)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.Window_Closing);
            
            #line default
            #line hidden
            return;
            case 2:
            this.tv_ClusterAlgs = ((Telerik.Windows.Controls.RadTreeView)(target));
            
            #line 25 "..\..\..\..\Controls\Visualizers\ClusterAlgorithmVisualizer.xaml"
            this.tv_ClusterAlgs.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.tv_ClusterAlgs_SelectedItemChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.gb_Proximities = ((System.Windows.Controls.GroupBox)(target));
            return;
            case 4:
            this.cb_Proximities = ((Telerik.Windows.Controls.RadComboBox)(target));
            
            #line 29 "..\..\..\..\Controls\Visualizers\ClusterAlgorithmVisualizer.xaml"
            this.cb_Proximities.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.cb_Proximities_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.uctrl_OneClustAlgVis = ((ClusteringEnsembleSuite.Controls.Visualizers.OneClusterAlgVisualizer)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

