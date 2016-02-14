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
using ClusteringEnsembleSuite.Code;
using ClusterEnsemble;
using ClusteringEnsembleSuite.Code.DataStructures;
using ClusterEnsemble.Graphics;
using ClusteringEnsembleSuite.Code.Interfaces;

namespace ClusteringEnsembleSuite.Controls.Visualizers
{
    /// <summary>
    /// Interaction logic for OneEnsembleAlgVisualizer.xaml
    /// </summary>
    public partial class OneEnsembleAlgVisualizer : UserControl,IOneVisualizer
    {
       public event NewEnsembleAlgorithmEventHandler NewEnsembleAlgorithm;

        Tree ftree;
        public Tree Tree
        {
            get { return ftree; }
            set
            {
                try
                {
                    this.ftree = value;
                    UpdateTree(this.ftree);
                }
                catch (Exception _ex)
                {
                    GeneralTools.Tools.WriteToLog(_ex);
                }
            }
        }

        public OneEnsembleAlgVisualizer()
        {
            try
            {
                InitializeComponent();
                Init();
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        public OneEnsembleAlgVisualizer(Tree atree)
            : this()
        {
            try
            {
                this.Tree = atree;
                Init();
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        private void bt_select_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string _errors = "";
                if (!Enviroment.CanRunAlgorithm(out _errors, AlgorithmType.Ensemble))
                {
                    MessageBox.Show(_errors, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                bool _ok;

                _errors = VerifyErrors(out _ok);

                if (!_ok)
                {
                    MessageBox.Show(_errors, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (NewEnsembleAlgorithm != null)
                {
                    NewEnsembleAlgorithm(this, new NewEnsembleAlgorithmEventArgs(Tree));
                }
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        #region IOneVisualizer Members

        public List<InControl> InControls { get; set; }

        public StackPanel StackPanel
        {
            get
            {
                return this.sp;
            }
            set { }
        }

        public Button ButtonSelect
        {
            get
            {
                return this.bt_select;
            }
            set { }
        }

        public void Init()
        {
            try
            {
                Utils_IOneVisualizer.Init(this);
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        public void UpdateTree(Tree aTree)
        {
            try
            {
                Utils_IOneVisualizer.UpdateTree(this, aTree);
            }
            catch (Exception _ex)
            {
                GeneralTools.Tools.WriteToLog(_ex);
            }
        }

        public Grid CreateGrid(string aParameterName, ClusterEnsemble.DataStructures.Property aProperty)
        {
            try
            {
                return Utils_IOneVisualizer.CreateGrid(this, aParameterName, aProperty);
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
                return Utils_IOneVisualizer.VerifyErrors(this, Tree, out aOk);
            }
            catch (Exception _ex)
            {
                aOk = false;
                GeneralTools.Tools.WriteToLog(_ex);
                return "Global Error in Application ClusterEnsembleSuite... (control OneEnsembleAlgVisualizer)";
            }
        }

        #endregion
    }
}