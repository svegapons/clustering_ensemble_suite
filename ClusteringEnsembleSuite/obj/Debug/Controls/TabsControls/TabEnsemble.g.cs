﻿#pragma checksum "..\..\..\..\Controls\TabsControls\TabEnsemble.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "1C33EEE5A7339902DC28F8C2EB2B2448"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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


namespace ClusteringEnsembleSuite.Controls.TabsControls {
    
    
    /// <summary>
    /// TabEnsemble
    /// </summary>
    public partial class TabEnsemble : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 32 "..\..\..\..\Controls\TabsControls\TabEnsemble.xaml"
        internal System.Windows.Controls.TextBlock tb_SelectEnsembleAlg;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\..\Controls\TabsControls\TabEnsemble.xaml"
        internal System.Windows.Controls.Button bt_Run;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\..\Controls\TabsControls\TabEnsemble.xaml"
        internal ClusteringEnsembleSuite.Controls.Visualizers.ListClusterAlgVIsualizer uctrl_ListClusterAlgVisualizerClustering;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\..\Controls\TabsControls\TabEnsemble.xaml"
        internal System.Windows.Controls.GridSplitter gridSplitter1;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\..\Controls\TabsControls\TabEnsemble.xaml"
        internal ClusteringEnsembleSuite.Controls.Visualizers.ListClusterAlgVIsualizer uctrl_ListClusterAlgVisualizerEnsemble;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\..\..\Controls\TabsControls\TabEnsemble.xaml"
        internal System.Windows.Controls.GridSplitter gridSplitter2;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\..\..\Controls\TabsControls\TabEnsemble.xaml"
        internal System.Windows.Controls.TextBox tb_output;
        
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
            System.Uri resourceLocater = new System.Uri("/ClusteringEnsembleSuite;component/controls/tabscontrols/tabensemble.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Controls\TabsControls\TabEnsemble.xaml"
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
            this.tb_SelectEnsembleAlg = ((System.Windows.Controls.TextBlock)(target));
            
            #line 32 "..\..\..\..\Controls\TabsControls\TabEnsemble.xaml"
            this.tb_SelectEnsembleAlg.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.tb_SelectEnsembleAlg_MouseDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.bt_Run = ((System.Windows.Controls.Button)(target));
            
            #line 37 "..\..\..\..\Controls\TabsControls\TabEnsemble.xaml"
            this.bt_Run.Click += new System.Windows.RoutedEventHandler(this.bt_Run_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.uctrl_ListClusterAlgVisualizerClustering = ((ClusteringEnsembleSuite.Controls.Visualizers.ListClusterAlgVIsualizer)(target));
            return;
            case 4:
            this.gridSplitter1 = ((System.Windows.Controls.GridSplitter)(target));
            return;
            case 5:
            this.uctrl_ListClusterAlgVisualizerEnsemble = ((ClusteringEnsembleSuite.Controls.Visualizers.ListClusterAlgVIsualizer)(target));
            return;
            case 6:
            this.gridSplitter2 = ((System.Windows.Controls.GridSplitter)(target));
            return;
            case 7:
            this.tb_output = ((System.Windows.Controls.TextBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

