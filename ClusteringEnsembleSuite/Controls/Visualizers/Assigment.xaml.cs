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
using ClusterEnsemble;

namespace ClusteringEnsembleSuite.Controls.Visualizers
{
    /// <summary>
    /// Interaction logic for Assigment.xaml
    /// </summary>
    public partial class Assigment : UserControl
    {
        public Assigment()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        public void PaintClusters(Structuring aStructuring)
        {
            try
            {
                Grid _grid = new Grid();

                RowDefinition _rd1 = new RowDefinition();
                _rd1.Height = new GridLength(2, GridUnitType.Star);
                RowDefinition _rd2 = new RowDefinition();
                _rd2.Height = new GridLength(2, GridUnitType.Star);
                _grid.RowDefinitions.Add(_rd1);
                _grid.RowDefinitions.Add(_rd2);

                int _RealClusters = aStructuring.ClustersCount + ((aStructuring.HaveUnassignedElements()) ? 1 : 0);
                int _columns = (_RealClusters % 2 == 0) ? _RealClusters / 2 : (_RealClusters / 2) + 1;

                for (int i = 0; i < _columns; i++)
                {
                    ColumnDefinition _cd = new ColumnDefinition();
                    _cd.Width = new GridLength(_columns, GridUnitType.Star);
                    _grid.ColumnDefinitions.Add(_cd);
                }
                int _row = 0;
                int _column = 0;
                foreach (Cluster _cluster in aStructuring.Clusters.Values)
                {
                    Border b = new Border();
                    b.HorizontalAlignment = HorizontalAlignment.Left;
                    b.VerticalAlignment = VerticalAlignment.Top;
                    b.Margin = new Thickness(5);
                    b.BorderThickness = new Thickness(1);
                    //B7D6F5
                    SolidColorBrush _mySolidColorBrush = new SolidColorBrush();
                    _mySolidColorBrush.Color = Color.FromArgb(255, 183, 214, 245);
                    b.Background = _mySolidColorBrush;
                    b.CornerRadius = new CornerRadius(10);
                    b.Tag = _cluster;

                    Grid.SetRow(b, _row);
                    Grid.SetColumn(b, _column);
                    _grid.Children.Add(b);

                    Grid _cElements = new Grid();
                    _cElements.Margin = new Thickness(5);
                    RowDefinition _rdTemp1 = new RowDefinition();
                    _rdTemp1.Height = new GridLength(0, GridUnitType.Auto);
                    RowDefinition _rdTemp2 = new RowDefinition();
                    _rdTemp2.Height = new GridLength(1, GridUnitType.Star);
                    _cElements.RowDefinitions.Add(_rdTemp1);
                    _cElements.RowDefinitions.Add(_rdTemp2);

                    b.Child = _cElements;

                    //Fila 0- Nombre del Cluster
                    TextBlock _tblk = new TextBlock();
                    _tblk.Text = _cluster.Name + " (" + _cluster.ElementsCount + ")";
                    _tblk.FontWeight = FontWeights.Bold;
                    _tblk.HorizontalAlignment = HorizontalAlignment.Center;
                    Grid.SetColumn(_tblk, 0);
                    Grid.SetRow(_tblk, 0);
                    _cElements.Children.Add(_tblk);

                    //Fila 1- Los elementos
                    ElementsVisualizer _ev = new ElementsVisualizer();
                    _ev.Elements = _cluster.Elements;
                    Grid.SetColumn(_ev, 0);
                    Grid.SetRow(_ev, 1);
                    _cElements.Children.Add(_ev);

                    _column++;
                    if (_column == _columns)
                    {
                        _column = 0;
                        _row++;
                    }
                }

                if (aStructuring.HaveUnassignedElements())
                {
                    Border b = new Border();
                    b.HorizontalAlignment = HorizontalAlignment.Left;
                    b.VerticalAlignment = VerticalAlignment.Top;
                    b.Margin = new Thickness(5);
                    b.BorderThickness = new Thickness(1);
                    b.Background = new SolidColorBrush(Colors.LightBlue);
                    b.CornerRadius = new CornerRadius(10);
                    b.Tag = aStructuring.UnassignedElements;

                    Grid.SetRow(b, _row);
                    Grid.SetColumn(b, _column);
                    _grid.Children.Add(b);

                    Grid _cElements = new Grid();
                    _cElements.Margin = new Thickness(5);
                    RowDefinition _rdTemp1 = new RowDefinition();
                    _rdTemp1.Height = new GridLength(0, GridUnitType.Auto);
                    RowDefinition _rdTemp2 = new RowDefinition();
                    _rdTemp2.Height = new GridLength(1, GridUnitType.Star);
                    _cElements.RowDefinitions.Add(_rdTemp1);
                    _cElements.RowDefinitions.Add(_rdTemp2);

                    b.Child = _cElements;

                    //Fila 0- Nombre del Cluster
                    TextBlock _tblk = new TextBlock();
                    _tblk.Text = "Unnassigned Elements (" + aStructuring.UnassignedElements.Count + ")";
                    _tblk.FontStyle = FontStyles.Italic;
                    _tblk.FontWeight = FontWeights.Bold;
                    _tblk.HorizontalAlignment = HorizontalAlignment.Center;
                    Grid.SetColumn(_tblk, 0);
                    Grid.SetRow(_tblk, 0);
                    _cElements.Children.Add(_tblk);

                    //Fila 1- Los elementos
                    ElementsVisualizer _ev = new ElementsVisualizer();
                    _ev.Elements = aStructuring.UnassignedElements;
                    Grid.SetColumn(_ev, 0);
                    Grid.SetRow(_ev, 1);
                    _cElements.Children.Add(_ev);
                }

                spMain.Children.Add(_grid);
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
            #region OLD CODE

            #region Painting All Clusters
            //foreach (Cluster _cluster in aStructuring.Clusters.Values)
            //{
            //    Border b = new Border();
            //    b.HorizontalAlignment = HorizontalAlignment.Left;
            //    b.VerticalAlignment = VerticalAlignment.Top;
            //    b.Margin = new Thickness(0, 0, 5, 0);
            //    b.BorderThickness = new Thickness(1);
            //    b.Background = new SolidColorBrush(Colors.Beige);
            //    b.CornerRadius = new CornerRadius(10);
            //    b.Tag = _cluster;
            //    b.MouseDown += GroupClick;

            //    WrapPanel sp = new WrapPanel();
            //    sp.Orientation = Orientation.Vertical;


            //    b.Child = sp;
            //    spMain.Children.Add(b);

            //    ElementsVisualizer _ev = new ElementsVisualizer();
            //    _ev.Elements = _cluster.Elements;
            //    sp.Children.Add(_ev);

            //    #region Painting All Elements of this Cluster
            //    //foreach (Element e in _cluster.Elements)
            //    //{
            //    //    Border subB = new Border();
            //    //    subB.HorizontalAlignment = HorizontalAlignment.Left;
            //    //    subB.VerticalAlignment = VerticalAlignment.Top;
            //    //    subB.Margin = new Thickness(5, 3, 5, 0);
            //    //    subB.Padding = new Thickness(5, 2, 5, 2);
            //    //    subB.BorderThickness = new Thickness(1);

            //    //    //subB.Background = new SolidColorBrush(colors[ZooEnvironment.RealPartition.GetCluster(e)[0]]);

            //    //    subB.CornerRadius = new CornerRadius(5);

            //    //    TextBlock tb = new TextBlock();
            //    //    tb.HorizontalAlignment = HorizontalAlignment.Center;

            //    //    if (!e.IsMissing(0))
            //    //        tb.Text = e.Values[0].ToString();
            //    //    else
            //    //        tb.Text = "?";

            //    //    subB.Child = tb;
            //    //    sp.Children.Add(subB);
            //    //}
            //    #endregion
            //}
            #endregion

            #region Painting All Unassigned Elements
            //if (aStructuring.HaveUnassignedElements())
            //{
            //    Border b = new Border();
            //    b.HorizontalAlignment = HorizontalAlignment.Left;
            //    b.VerticalAlignment = VerticalAlignment.Top;
            //    b.Margin = new Thickness(0, 0, 5, 0);
            //    b.BorderThickness = new Thickness(1);
            //    b.Background = new SolidColorBrush(Colors.LightBlue);
            //    b.CornerRadius = new CornerRadius(10);
            //    b.Tag = aStructuring.UnassignedElements;
            //    b.MouseDown += GroupClick;

            //    WrapPanel sp = new WrapPanel();
            //    sp.Orientation = Orientation.Vertical;


            //    b.Child = sp;
            //    spMain.Children.Add(b);

            //    #region Painting All Unassigned Elements
            //    foreach (Element e in aStructuring.UnassignedElements)
            //    {
            //        Border subB = new Border();
            //        subB.HorizontalAlignment = HorizontalAlignment.Left;
            //        subB.VerticalAlignment = VerticalAlignment.Top;
            //        subB.Margin = new Thickness(5, 3, 5, 0);
            //        subB.Padding = new Thickness(5, 2, 5, 2);
            //        subB.BorderThickness = new Thickness(1);

            //        subB.CornerRadius = new CornerRadius(5);

            //        TextBlock tb = new TextBlock();
            //        tb.HorizontalAlignment = HorizontalAlignment.Center;

            //        if (!e.IsMissing(0))
            //            tb.Text = e.Values[0].ToString();
            //        else
            //            tb.Text = "?";

            //        subB.Child = tb;
            //        sp.Children.Add(subB);
            //    }

            //    #endregion
            //}
            #endregion

            #endregion
        }

        public void Clear()
        {
            try
            {
                spMain.Children.Clear();
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        private void diffControl_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                //contentControl1.Visibility = Visibility.Hidden;
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

    }
}
