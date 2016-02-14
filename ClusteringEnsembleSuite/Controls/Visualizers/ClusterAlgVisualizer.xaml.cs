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
using ClusteringEnsembleSuite.Code.DataStructures;
using ClusterEnsemble.DataStructures;
using ClusterEnsemble.Reflection;
using ClusteringEnsembleSuite.Code;
using ClusterEnsemble;
using Telerik.Windows.Controls;
using ClusterEnsemble.Dissimilarities;

namespace ClusteringEnsembleSuite.Controls.Visualizers
{
    /// <summary>
    /// Interaction logic for ClusterAlgVisualizer.xaml
    /// </summary>
    public partial class ClusterAlgVisualizer : UserControl
    {
        public event NewClusterAlgorithmEventHandler NewClusterAlgorithm;
        public ClusterAlgVisualizer()
        {
            InitializeComponent();

            this.tv_ClusterAlgs.ItemsSource = new ListClusterTree();
            this.uctrl_OneClustAlgVis.NewClusterAlgorithm += new NewClusterAlgorithmEventHandler(NewClusterAlg);
           

        }
        /// <summary>
        /// Este metodo esta engachado al evento que lanza el AttributesVisualizer, cuando se chequea o se deschequea algun atributo
        /// para que este mismo control se actualice sobre las Disimilitud que debe mostrar, que coincida su tipo con la de los atributos chequeados
        /// </summary>
        public void CheckAttributes(object sender, CheckAttributeEnventArgs e)
        {
            List<Dissimilarity> _dissTemp = LoadDissimilarities();

            ElementType _elementType = Dissimilarity.GetAttributesType(e.SelectedAttributes);

            List<Dissimilarity> _source  =new List<Dissimilarity>();

            //en el caso que le ponga de ItemsSource (al listView de atributes en el control AttributesVisualizer) NULL, entonces e.SelectedAttributes esta vacio
            //por tanto en el metodo Dissimilarity.GetAttributesType va retornar Numeric, porque como la lista esta vacia no hay ninguno que no sea numeric, y eso
            //esta mal, verificar el metodo Dissimilarity.GetAttributesType para ver lo que acabo de decir, cuando la lista que le paso esta vacia
            if (e.SelectedAttributes.Count > 0)
            {
                foreach (Dissimilarity _diss in _dissTemp)
                    if (_diss.AdmissibleElementType == ElementType.Mixt || _diss.AdmissibleElementType == _elementType)
                        _source.Add(_diss);
            }
            this.cb_Dissimilarities.ItemsSource = null;
            this.cb_Dissimilarities.ItemsSource = _source;
            this.cb_Dissimilarities.DisplayMemberPath = "Name";
            if (Dissimilarities.Count > 0)
                this.cb_Dissimilarities.SelectedIndex = 0;
        }
        public List<Dissimilarity> Dissimilarities { get; set; }
        private void NewClusterAlg(object asender, NewClusterAlgorithmEventArgs aNewClusterAlg)
        {
            if (NewClusterAlgorithm != null)
                NewClusterAlgorithm(asender, aNewClusterAlg);
        }        

        private List<Dissimilarity> LoadDissimilarities()
        {
            ReflectionTools _rct = new ReflectionTools();
            List<Dissimilarity> _result = _rct.GetDissimilarities();

            Dissimilarities = _result;
            return _result;
        }

        private void cb_Dissimilarities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((RadComboBox)sender).SelectedIndex != -1)
            {
                Enviroment.Dissimilarity = (Dissimilarity)((RadComboBox)sender).SelectedItem;
            }

        }

        private void bt_Hidden_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        private void tv_ClusterAlgs_SelectedItemChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.tv_ClusterAlgs.SelectedItem != null)
            {
                Tree _tree = (Tree)((RadTreeView)sender).SelectedItem;

                this.uctrl_OneClustAlgVis.Tree = _tree;

                this.gb_Dissimilarities.Visibility = (_tree.Value.IsAbstract) ? Visibility.Hidden : Visibility.Visible;

            }
        }

    }
}
