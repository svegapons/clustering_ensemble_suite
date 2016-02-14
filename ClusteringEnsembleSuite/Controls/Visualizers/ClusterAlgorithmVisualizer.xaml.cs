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
using System.Windows.Shapes;
using ClusteringEnsembleSuite.Code.DataStructures;
using ClusteringEnsembleSuite.Code;
using Telerik.Windows.Controls;
using ClusterEnsemble.Proximities;
using ClusterEnsemble.Reflection;
using ClusterEnsemble.DataStructures;
using ClusterEnsemble;
using ClusterEnsemble.Clusters;

namespace ClusteringEnsembleSuite.Controls.Visualizers
{
    /// <summary>
    /// Interaction logic for ClusterAlgorithmVisualizer.xaml
    /// </summary>
    public partial class ClusterAlgorithmVisualizer : Window
    {
        public event NewClusterAlgorithmEventHandler NewClusterAlgorithm;
        public ClusterAlgorithmVisualizer()
        {
            try
            {
                InitializeComponent();

                this.tv_ClusterAlgs.ItemsSource = new ListClusterTree();
                this.uctrl_OneClustAlgVis.NewClusterAlgorithm += new NewClusterAlgorithmEventHandler(NewClusterAlg);
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        /// <summary>
        /// Este metodo esta engachado al evento que lanza el AttributesVisualizer, cuando se chequea o se deschequea algun atributo
        /// para que este mismo control se actualice sobre las Disimilitud que debe mostrar, que coincida su tipo con la de los atributos chequeados
        /// </summary>
        public void CheckAttributes(object sender, CheckAttributeEnventArgs e)
        {
            try
            {
                List<Proximity> _dissTemp = LoadProximities();

                ElementType _elementType = Proximity.GetAttributesType(e.SelectedAttributes);

                List<Proximity> _source = new List<Proximity>();

                //en el caso que le ponga de ItemsSource (al listView de atributes en el control AttributesVisualizer) NULL, entonces e.SelectedAttributes esta vacio
                //por tanto en el metodo Proximity.GetAttributesType va retornar Numeric, porque como la lista esta vacia no hay ninguno que no sea numeric, y eso
                //esta mal, verificar el metodo Proximity.GetAttributesType para ver lo que acabo de decir, cuando la lista que le paso esta vacia
                if (e.SelectedAttributes.Count > 0)
                {
                    foreach (Proximity _prox in _dissTemp)
                        if (_prox.AdmissibleElementType == ElementType.Mixt || _prox.AdmissibleElementType == _elementType)
                            _source.Add(_prox);
                }

                //Esto es lo que se utiliza en UpdateProximities, ya que ahora ademas de cumplir las restricciones de los attributes,
                //tambien hay que verificar que el algoritmo que se seleccione pueda trabajar con esa proximidad
                CurrentProximities = _source;

                this.cb_Proximities.ItemsSource = null;
                this.cb_Proximities.ItemsSource = _source;
                this.cb_Proximities.DisplayMemberPath = "Name";
                if (_source.Count > 0)
                    this.cb_Proximities.SelectedIndex = 0;
                else
                    Enviroment.Proximity = null;

                //Esto es porque si se me queda seleccionado el algoritmo Metis, cuando se actualizan las proximidades 
                //(si se selecciona o deseleccion un attribute)
                //se me quedan para el algoritmo Metis todas las que se puedan cargar, esto es porque no se selecciono un algoritmo,
                //entonces para que se actualicen debo seleccionar otro algoritmo.
                //this.tv_ClusterAlgs.ItemsSource = null;
                //this.tv_ClusterAlgs.ItemsSource = new ListClusterTree();
                this.tv_ClusterAlgs_SelectedItemChanged(null,null);
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        private void NewClusterAlg(object asender, NewClusterAlgorithmEventArgs aNewClusterAlg)
        {
            try
            {
                if (NewClusterAlgorithm != null)
                    NewClusterAlgorithm(asender, aNewClusterAlg);
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        private List<Proximity> LoadProximities()
        {
            try
            {
                ReflectionTools _rct = new ReflectionTools();
                List<Proximity> _result = _rct.GetProximities();

                return _result;
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
                return new List<Proximity>();
            }
        }

        private void cb_Proximities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (((RadComboBox)sender).SelectedIndex != -1)
                {
                    Enviroment.Proximity = (Proximity)((RadComboBox)sender).SelectedItem;
                }
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        //Se actualiza en el metodo CheckAttributes.
        List<Proximity> CurrentProximities { get; set; }
        private void UpdateProximities(Tree aTree)
        {
            try
            {
                if (!aTree.Value.IsAbstract)
                {
                    List<Proximity> _dissTemp = (CurrentProximities != null) ? CurrentProximities : new List<Proximity>();

                    ClusterAlgorithm _ClusterAlgorithm = ReflectionTools.GetInstance<ClusterAlgorithm>(aTree.Value.FullName);

                    List<Proximity> _source = new List<Proximity>();

                    foreach (Proximity _prox in _dissTemp)
                        if (_ClusterAlgorithm.ProximityType == ProximityType.Both || VerifyProximity(_ClusterAlgorithm.ProximityType, _prox))
                            _source.Add(_prox);

                    this.cb_Proximities.ItemsSource = null;
                    this.cb_Proximities.ItemsSource = _source;
                    this.cb_Proximities.DisplayMemberPath = "Name";
                    if (_source.Count > 0)
                        this.cb_Proximities.SelectedIndex = 0;
                    else
                        Enviroment.Proximity = null;

                    this.cb_Proximities.IsEnabled = _ClusterAlgorithm.ProximityType != ProximityType.None; 
                }
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }
        private bool VerifyProximity(ProximityType aProximityType, Proximity aProximity)
        {
            try
            {
                switch (aProximityType)
                {
                    case ProximityType.Similarity:
                        if (aProximity is Similarity)
                            return true;
                        break;
                    case ProximityType.Dissimilarity:
                        if (aProximity is Dissimilarity)
                            return true;
                        break;
                    case ProximityType.None:
                        return true;
                        break;

                    default:
                        return false;
                }
                return false;
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
                return false;
            }
        }
        private void tv_ClusterAlgs_SelectedItemChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.tv_ClusterAlgs.SelectedItem != null)
                {
                    Tree _tree = (Tree)this.tv_ClusterAlgs.SelectedItem;
                    //Tree _tree = (Tree)((RadTreeView)sender).SelectedItem;

                    UpdateProximities(_tree);

                    this.uctrl_OneClustAlgVis.Tree = _tree;

                    this.gb_Proximities.Visibility = (_tree.Value.IsAbstract) ? Visibility.Hidden : Visibility.Visible;

                }
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                this.Visibility = Visibility.Hidden;
                e.Cancel = true;                
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

    }
}
