﻿#pragma checksum "..\..\..\..\Controls\Visualizers\ClusterDetails.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A34FF1DDBB8FCDB6F97B728E404825CC"
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


namespace ClusteringEnsembleSuite.Controls.Visualizers {
    
    
    /// <summary>
    /// ClusterDatails
    /// </summary>
    public partial class ClusterDatails : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 8 "..\..\..\..\Controls\Visualizers\ClusterDetails.xaml"
        internal System.Windows.Controls.Button btPrev;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\..\..\Controls\Visualizers\ClusterDetails.xaml"
        internal System.Windows.Controls.Button btNext;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\..\Controls\Visualizers\ClusterDetails.xaml"
        internal System.Windows.Controls.TextBlock tb_AlgName;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\..\Controls\Visualizers\ClusterDetails.xaml"
        internal System.Windows.Controls.Button btSave;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\..\Controls\Visualizers\ClusterDetails.xaml"
        internal ClusteringEnsembleSuite.Controls.Visualizers.Assigment uctrl_Assigment;
        
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
            System.Uri resourceLocater = new System.Uri("/ClusteringEnsembleSuite;component/controls/visualizers/clusterdetails.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Controls\Visualizers\ClusterDetails.xaml"
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
            this.btPrev = ((System.Windows.Controls.Button)(target));
            
            #line 8 "..\..\..\..\Controls\Visualizers\ClusterDetails.xaml"
            this.btPrev.Click += new System.Windows.RoutedEventHandler(this.btPrev_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.btNext = ((System.Windows.Controls.Button)(target));
            
            #line 11 "..\..\..\..\Controls\Visualizers\ClusterDetails.xaml"
            this.btNext.Click += new System.Windows.RoutedEventHandler(this.btNext_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.tb_AlgName = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.btSave = ((System.Windows.Controls.Button)(target));
            
            #line 15 "..\..\..\..\Controls\Visualizers\ClusterDetails.xaml"
            this.btSave.Click += new System.Windows.RoutedEventHandler(this.btSave_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.uctrl_Assigment = ((ClusteringEnsembleSuite.Controls.Visualizers.Assigment)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
