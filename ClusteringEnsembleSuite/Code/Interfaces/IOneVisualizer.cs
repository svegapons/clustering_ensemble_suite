using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Added
using ClusterEnsemble.DataStructures;
using System.Windows.Controls;
using ClusteringEnsembleSuite.Code.DataStructures;
using System.Windows;
using System.Windows.Media;
using ClusterEnsemble.Graphics;
using Telerik.Windows.Controls;

namespace ClusteringEnsembleSuite.Code.Interfaces
{
    /// <summary>
    /// Esta interface la deben implementar todos los UserControls que visualicen las propiedades de
    /// un algoritmo en particular, ya sea de Clustering o de Ensemble
    /// </summary>
    public interface IOneVisualizer
    {
        //Properties
        List<InControl> InControls { get; set; }
        StackPanel StackPanel { get; set; }
        Button ButtonSelect { get; set; }
        
        //Methods
        void Init();
        void UpdateTree(Tree aTree);
        Grid CreateGrid(string aParameterName, Property aProperty);
        string VerifyErrors(out bool aOk);
    }

    public class Utils_IOneVisualizer
    {
        public static void Init(IOneVisualizer aIOV)
        {
            aIOV.InControls = new List<InControl>();
            aIOV.StackPanel.Children.Clear();
        }
        public static void UpdateTree(IOneVisualizer aIOV, Tree aTree)
        {
            List<ClusterEnsemble.DataStructures.Property> _parameters = aTree.Value.InProperties;
            aIOV.Init();

            if (!aTree.Value.IsAbstract && _parameters != null)
            {
                for (int i = 0; i < _parameters.Count; i++)
                {
                    Grid _grid = aIOV.CreateGrid(_parameters[i].Name, _parameters[i]);
                    if (_grid != null)
                        aIOV.StackPanel.Children.Add(_grid);
                }
                aIOV.ButtonSelect.Visibility = Visibility.Visible;
            }
            else
                aIOV.ButtonSelect.Visibility = Visibility.Hidden;
        }
        public static Grid CreateGrid(IOneVisualizer aIOV, string aParameterName, Property aProperty)
        {
            Grid _result = new Grid();
            _result.Width = double.NaN;
            ColumnDefinition _cd1 = new ColumnDefinition();
            //cd1.Width = new GridLength(1,GridUnitType.Auto);
            _result.ColumnDefinitions.Add(_cd1);

            ColumnDefinition _cd2 = new ColumnDefinition();
            //cd2.Width = new GridLength(1, GridUnitType.Auto);
            _result.ColumnDefinitions.Add(_cd2);

            TextBlock _tblk = new TextBlock();
            _tblk.FontFamily = new FontFamily("Segoe UI");
            _tblk.FontWeight = FontWeights.Bold;
            //tblk.FontSize = 14;
            _tblk.Text = aParameterName;
            _tblk.Margin = new Thickness(10, 10, 5, 0);
            Grid.SetColumn(_tblk, 0);

            _result.Children.Add(_tblk);

            InControl _InControl = new InControl();

            if (!aProperty.IsMultipleSelection)
            {
                TextBox _tbx = new TextBox();
                _tbx.Tag = aProperty;
                _tbx.HorizontalAlignment = HorizontalAlignment.Right;
                _tbx.Width = 50;
                _tbx.Text = "1";
                _tbx.Margin = new Thickness(0, 10, 10, 0);
                Grid.SetColumn(_tbx, 1);

                _InControl.TextBox = _tbx;
                _InControl.ControlType = ControlType.TextBox;

                _result.Children.Add(_tbx);
            }
            else
            {
                RadComboBox _combbx = new RadComboBox();
                StyleManager.SetTheme(_combbx, new Office_BlueTheme());
                _combbx.ItemsSource = aProperty.Value_Converters;
                _combbx.DisplayMemberPath = "Value";
                _combbx.SelectedIndex = 0;
                _combbx.Tag = aProperty;
                _combbx.Margin = new Thickness(0, 10, 10, 0);
                _combbx.HorizontalAlignment = HorizontalAlignment.Right;
                Grid.SetColumn(_combbx, 1);

                _InControl.ComboBox = _combbx;
                _InControl.ControlType = ControlType.ComboBox;

                _result.Children.Add(_combbx);
            }

            if (aIOV.InControls != null)
                aIOV.InControls.Add(_InControl);


            return _result;

        }
        public static string VerifyErrors(IOneVisualizer aIOV, Tree aTree, out bool aOk)
        {
            string _errors = "";
            aOk = true;
            for (int i = 0; i < aIOV.InControls.Count; i++)
            {
                object _value;
                string _parameter = null;
                ClusterEnsemble.DataStructures.Property _propertyIn = null;

                if (aIOV.InControls[i].ControlType == ControlType.TextBox)
                {
                    _parameter = aIOV.InControls[i].TextBox.Text;
                    _propertyIn = (ClusterEnsemble.DataStructures.Property)aIOV.InControls[i].TextBox.Tag;
                }
                else
                {
                    _parameter = ((Value_Converter)aIOV.InControls[i].ComboBox.SelectedItem).Value;
                    _propertyIn = (ClusterEnsemble.DataStructures.Property)aIOV.InControls[i].ComboBox.Tag;

                    _propertyIn.Converter = ((Value_Converter)aIOV.InControls[i].ComboBox.SelectedItem).Converter;
                }

                if (!_propertyIn.Converter.TryConvert(_parameter, out _value))
                {
                    aOk = false;
                    _errors += "Wrong Parameter: " + _propertyIn.Name + ", from algorithm: " + aTree.Value.Name + "\r\n";
                }
                else
                    _propertyIn.Value = _value;
            }

            return _errors;
        }
    }


}
