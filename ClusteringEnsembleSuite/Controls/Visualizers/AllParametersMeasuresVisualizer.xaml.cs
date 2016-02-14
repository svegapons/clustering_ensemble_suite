using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

//Added
using ClusterEnsemble.DataStructures;
using ClusteringEnsembleSuite.Code.DataStructures;
using ClusterEnsemble.Graphics;
using Telerik.Windows.Controls;

namespace ClusteringEnsembleSuite.Controls.Visualizers
{
    /// <summary>
    /// Interaction logic for AllParametersMeasuresVisualizer.xaml
    /// </summary>
    public partial class AllParametersMeasuresVisualizer : UserControl
    {
        List<InControl> InTextBoxs { get; set; }

        public AllParametersMeasuresVisualizer()
        {
            InitializeComponent();
            Init();
        }
        public AllParametersMeasuresVisualizer(List<Tree> aTrees)
        {
            this.Trees = aTrees;
            Init();
        }
        private void Init()
        {
            try
            {
                this.InTextBoxs = new List<InControl>();
                this.sp.Children.Clear();
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        List<Tree> fTrees;
        public List<Tree> Trees
        {
            get { return fTrees; }
            set
            {
                try
                {
                    this.fTrees = value;
                    Update(this.fTrees);
                }
                catch (Exception _ex)
                {
                    GeneralTools.Tools.WriteToLog(_ex);
                }
            }
        }

        private void Update(List<Tree> aTrees)
        {
            try
            {
                Init();

                for (int i = 0; i < aTrees.Count; i++)
                {
                    if (!aTrees[i].Value.IsAbstract && aTrees[i].Value.InProperties != null && aTrees[i].Value.InProperties.Count > 0)
                    {
                        GroupBox _gbx = CreateGroupBox(aTrees[i]);
                        if (_gbx != null)
                            this.sp.Children.Add(_gbx);
                    }
                }
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        private GroupBox CreateGroupBox(Tree aTree)
        {
            try
            {
                GroupBox _result = new GroupBox();
                _result.Header = aTree.Value.Name;
                _result.Margin = new Thickness(10, 5, 10, 5);

                StackPanel _content = new StackPanel();
                _content.Orientation = Orientation.Vertical;

                foreach (Property _p in aTree.Value.InProperties)
                {
                    Grid _temp = CreateGrid(_p.Name, _p);
                    if (_temp != null)
                        _content.Children.Add(_temp);
                }

                _result.Content = _content;

                return _result;
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
                return null;
            }
        }

        private Grid CreateGrid(string aParameterName, Property aProperty)
        {
            try
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
                _tblk.Margin = new Thickness(5, 10, 2, 0);
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

                if (InTextBoxs != null)
                    InTextBoxs.Add(_InControl);


                return _result;
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
                return null;
            }
        }

        public string VerifyErrors(out bool aOk)
        {
            try
            {
                string _errors = "";
                aOk = true;
                for (int i = 0; i < this.InTextBoxs.Count; i++)
                {
                    object _value;
                    string _parameter = null;
                    Property _propertyIn = null;

                    if (InTextBoxs[i].ControlType == ControlType.TextBox)
                    {
                        _parameter = InTextBoxs[i].TextBox.Text;
                        _propertyIn = ((Property)InTextBoxs[i].TextBox.Tag);
                    }
                    else
                    {
                        _parameter = ((Value_Converter)InTextBoxs[i].ComboBox.SelectedItem).Value;
                        _propertyIn = (Property)InTextBoxs[i].ComboBox.Tag;

                        _propertyIn.Converter = ((Value_Converter)InTextBoxs[i].ComboBox.SelectedItem).Converter;
                    }

                    if (!_propertyIn.Converter.TryConvert(_parameter, out _value))
                    {
                        aOk = false;
                        _errors += "Parámetro: " + _propertyIn.Name + " incorrecto, de la Medida: " + _propertyIn.ClassName + "\r\n";
                    }
                    else
                        _propertyIn.Value = _value;
                }
                return _errors;
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
                aOk = false;
                return "Global Error in Application ClusterEnsembleSuite... (control: AllParametersMeasuresVisualizer)";
            }
        }


    }

    class TreeName_Property
    {
        public string TreeName { get; set; }
        public Property Property { get; set; }
    }

}
