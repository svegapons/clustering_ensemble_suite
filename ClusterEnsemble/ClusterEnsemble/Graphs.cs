using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ClusterEnsemble.Clusters;
using ClusterEnsemble.Proximities;
using ClusterEnsemble.Graphics;
using System.IO;
using System.Diagnostics;

namespace ClusterEnsemble.ClusterEnsemble
{
    public class CSPA : Graphs
    {
        [In(typeof(IntGTZeroConverter))]
        public int ClustersCount { get; set; }

        public CSPA(Set set, List<Structuring> estr)
            : base(set, estr)
        { Name = "CSPA"; }
        public CSPA() : base() { Name = "CSPA"; }

        public override Structuring BuildStructuring()
        {
            try
            {
                if (Set == null || Structurings == null)
                    throw new NullReferenceException();

                int _current = 1, _max = Set.ElementsCount * Structurings.Count;
                if (IContainerProgressBar != null)
                {
                    IContainerProgressBar.ResetProgressBar(1, _max, true);
                    IContainerProgressBar.UpdateProgressBar(1, "Running CSPA algorithm...", true);
                }

                if (ClustersCount <= 0)
                    throw new Exception("La cantidad de clusters debe ser mayor que cero");
                else if (ClustersCount == 1)
                {
                    Dictionary<string, Cluster> dic_clus = new Dictionary<string, Cluster>();
                    string name = "C-0";
                    List<Element> temp = new List<Element>();

                    for (int i = 0; i < Set.ElementsCount; i++)
                        temp.Add(Set[i]);

                    dic_clus.Add(name, new Cluster(name) { Elements = temp });

                    Structuring = new Partition() { Clusters = dic_clus };

                    if (IContainerProgressBar != null)
                        IContainerProgressBar.FinishProgressBar();

                    return Structuring;
                }
                else
                {
                    #region Algorithm
                    //Calcular la matrix V x H
                    int dimH = 0;
                    foreach (Structuring s in Structurings)
                        dimH += s.ClustersCount;

                    byte[,] VxH = new byte[Set.ElementsCount, dimH];

                    //rellenar VxH
                    int currentColumn = 0;//cada columna representa un cluster
                    //Recordar tbn que los elementos tienen un indice que los representa
                    foreach (Structuring s in Structurings)
                    {
                        foreach (Cluster cluster in s.Clusters.Values)
                        {
                            foreach (Element e in cluster.Elements)
                            {
                                VxH[e.Index, currentColumn] = 1;

                                if (IContainerProgressBar != null)
                                    IContainerProgressBar.UpdateProgressBar(_current++, "Running CSPA algorithm...", false);

                            }
                            //Debo actualizar currentColumn ya que cada cluser representa una columna
                            currentColumn++;
                        }
                    }

                    //Construir CoAsociationMatrixDissToMetis para pasarselo como similitud entre elementos
                    //al algoritmo Metis
                    Similarity _sim = new CoAsociationMatrixDissToMetis(Set, VxH, Structurings.Count);

                    ClusterAlgorithm ca = new Metis(Set, _sim);

                    //Llamar al algoritmo Metis
                    ca.ClustersCount = ClustersCount;
                    ca.BuildStructuring();

                    Structuring = ca.Structuring;

                    if (IContainerProgressBar != null)
                        IContainerProgressBar.FinishProgressBar();

                    return Structuring;

                    #endregion
                }
            }
            catch
            {
                if (IContainerProgressBar != null)
                    IContainerProgressBar.ShowError("Error occurred in CSPA algorithm.");
                return null;
            }
        }

    }

    public class HGPA : Graphs
    {
        [In(typeof(IntGTZeroConverter))]
        public int ClustersCount { get; set; }
        public int UBfactor { get; private set; }

        public HGPA(Set set, List<Structuring> estr)
            : base(set, estr)
        { Name = "HGPA"; }
        public HGPA() : base() { Name = "HGPA"; }

        public override Structuring BuildStructuring()
        {
            try
            {
                if (Set == null || Structurings == null)
                    throw new NullReferenceException();

                int _max = Set.ElementsCount * Structurings.Count;
                if (IContainerProgressBar != null)
                {
                    IContainerProgressBar.ResetProgressBar(1, _max, true);
                    IContainerProgressBar.UpdateProgressBar(1, "Running HGPA algorithm...", true);
                }

                if (ClustersCount <= 0)
                    throw new Exception("La cantidad de clusters debe ser mayor que cero");
                else if (ClustersCount == 1)
                {
                    Dictionary<string, Cluster> dic_clus = new Dictionary<string, Cluster>();
                    string name = "C-0";
                    List<Element> temp = new List<Element>();

                    for (int i = 0; i < Set.ElementsCount; i++)
                        temp.Add(Set[i]);

                    dic_clus.Add(name, new Cluster(name) { Elements = temp });

                    Structuring = new Partition() { Clusters = dic_clus };

                    if (IContainerProgressBar != null)
                        IContainerProgressBar.FinishProgressBar();

                    return Structuring;
                }
                else
                {
                    UBfactor = 5;

                    string filename = BuildInputFile();

                    if (IContainerProgressBar != null)
                        IContainerProgressBar.UpdateProgressBar(_max, "Running HGPA algorithm...", true);

                    string parameters = filename + " " + ClustersCount + " " + UBfactor;
                    Utils.ExecuteMetisPackage(Utils.HMetisExecutableName, parameters);
                    Structuring = Utils.BuildStructuringFromOutputFile(filename, Set, ClustersCount, null);

                    if (IContainerProgressBar != null)
                        IContainerProgressBar.FinishProgressBar();

                    return Structuring;
                }
            }
            catch
            {
                if (IContainerProgressBar != null)
                    IContainerProgressBar.ShowError("Error occurred in CSPA algorithm.");
                return null;
            }
        }

        #region Private Members
        private string BuildInputFile()
        {
            int _current = 1;

            Random r = new Random(Environment.TickCount);
            string folderpath = Utils.ExesFolderPath;
            string filename = "inputhypergraph" + Utils.GetUniqueID + ".config";
            string filepath = folderpath + Path.DirectorySeparatorChar + filename;

            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }

            FileStream fs = new FileStream(filepath, FileMode.CreateNew, FileAccess.ReadWrite);
            StreamWriter sw = new StreamWriter(fs);

            //Calcular el numero de Hyperedges
            int hedges = 0;
            foreach (Structuring s in Structurings)
                hedges += s.ClustersCount;

            int vertices = Set.ElementsCount;

            //como los vertices y las hyperedges tienen el mismo peso se omite el parametro "fmt"

            //First Line
            string firsteline = hedges + " " + vertices;
            sw.WriteLine(firsteline);

            // |E|h - lines, where |E|h is the amount of hyperedges

            foreach (Structuring structuring in Structurings)
            {
                foreach (Cluster cluster in structuring.Clusters.Values)
                {
                    StringBuilder line = new StringBuilder();
                    foreach (Element element in cluster.Elements)
                    {
                        line.Append((element.Index + 1).ToString() + " ");

                        if (IContainerProgressBar != null)
                            IContainerProgressBar.UpdateProgressBar(_current++, "Running HGPA algorithm...", false);
                    }

                    sw.WriteLine(line);

                }
            }

            sw.Close();
            fs.Close();


            return filename;
        }
        #endregion
    }

    public class MCLA : Graphs
    {
        [In(typeof(IntGTZeroConverter))]
        public int ClustersCount { get; set; }

        public MCLA(Set set, List<Structuring> estr)
            : base(set, estr)
        { Name = "MCLA"; }
        public MCLA() : base() { Name = "MCLA"; }

        public override Structuring BuildStructuring()
        {
            try
            {
                if (Set == null || Structurings == null)
                    throw new NullReferenceException();

                int _current = 1, _maxpb = Set.ElementsCount * Structurings.Count;
                if (IContainerProgressBar != null)
                {
                    IContainerProgressBar.ResetProgressBar(1, _maxpb, true);
                    IContainerProgressBar.UpdateProgressBar(1, "Running HGPA algorithm...", true);
                }

                if (ClustersCount <= 0)
                    throw new Exception("La cantidad de clusters debe ser mayor que cero");
                else if (ClustersCount == 1)
                {
                    Dictionary<string, Cluster> dic_clus = new Dictionary<string, Cluster>();
                    string name = "C-0";
                    List<Element> temp = new List<Element>();

                    for (int i = 0; i < Set.ElementsCount; i++)
                        temp.Add(Set[i]);

                    dic_clus.Add(name, new Cluster(name) { Elements = temp });

                    Structuring = new Partition() { Clusters = dic_clus };

                    if (IContainerProgressBar != null)
                        IContainerProgressBar.FinishProgressBar();

                    return Structuring;
                }
                else
                {
                    #region Algorithm
                    //Construir un conjunto de elementos donde ahora cada elemento es un label, 
                    //o lo que es lo mismo una hyperedge. Este seria el meta-grafo
                    Set hyperedgeSet = new Set("HyperEdge Set");

                    int dimH = 0;
                    foreach (Structuring s in Structurings)
                        dimH += s.ClustersCount;

                    byte[,] metagraph = new byte[Set.ElementsCount, dimH];

                    //Ahora por cada label annadir un nuevo elemento al conjunto, recordar que cada label
                    //seria cada hyperedges, lo que ahora cada hyperedges estaria representada por la clase Element
                    //pero la lista de Values seria el vector binario o mejor dicho los elementos que pertenecen
                    //a la hyperedges.
                    //La matriz metagraph es la representacion en vectores binarios del conjunto hyperedges.

                    // Ojo esto ya  ///////Es mejor representar cada hyperedges de esta forma es decir con los elementos que pertenecen
                    // no se hace   ///////a dicha hyperedges ya que si se representa por el vector binario entonces necesitariamos mucha memoria.
                    int index = 0;
                    foreach (Structuring structuring in Structurings)
                    {
                        foreach (Cluster cluster in structuring.Clusters.Values)
                        {
                            Element element = new Element();
                            element.Index = index;
                            element.Name = "hyperedge-" + index;
                            foreach (Element temp in cluster.Elements)
                            {
                                metagraph[temp.Index, index] = 1;

                                if (IContainerProgressBar != null)
                                    IContainerProgressBar.UpdateProgressBar(_current++, "Running HGPA algorithm...", false);
                            }

                            hyperedgeSet.AddElement(element);
                            index++;
                        }
                    }

                    if (IContainerProgressBar != null)
                        IContainerProgressBar.UpdateProgressBar(_maxpb, "Running HGPA algorithm...", true);

                    Similarity _sim = new BinaryJaccardMeasure(metagraph);

                    ClusterAlgorithm ca = new Metis(hyperedgeSet, _sim);

                    ca.ClustersCount = ClustersCount;
                    ca.BuildStructuring();

                    Partition metaClusters = (Partition)ca.Structuring;

                    //Asignar cada elemento original a su metacluster correspondiente

                    //double[] va a tener dimension igual a la cantidad de elementos iniciales y en cada posicion va a estar un numero entre 0 y 1
                    List<double[]> _meta_hyperedges = new List<double[]>();
                    foreach (Cluster _c in metaClusters.Clusters.Values)
                    {
                        double[] _meta_hyperedge = new double[metagraph.GetLength(0)];
                        foreach (Element _e in _c.Elements)
                        {
                            //_e.Index es la columna de la matrix metgraph, que dicha columna representa a ese elemento
                            for (int row = 0; row < metagraph.GetLength(0); row++)
                            {
                                _meta_hyperedge[row] += metagraph[row, _e.Index];
                            }
                        }

                        //LLevar cada posicion a un valor enre 0 y 1
                        for (int i = 0; i < _meta_hyperedge.Length; i++)
                            _meta_hyperedge[i] = _meta_hyperedge[i] / _c.ElementsCount;

                        _meta_hyperedges.Add(_meta_hyperedge);
                    }

                    //Construir la particion final
                    Dictionary<string, Cluster> _dic_clusters = new Dictionary<string, Cluster>();

                    for (int i = 0; i < Set.ElementsCount; i++)
                    {
                        Element _e = Set[i];

                        double _max = -1;
                        int _meta_hyperedge = -1;
                        for (int j = 0; j < _meta_hyperedges.Count; j++)
                        {
                            if (_meta_hyperedges[j][_e.Index] > _max)
                            {
                                _max = _meta_hyperedges[j][_e.Index];
                                _meta_hyperedge = j;
                            }
                        }

                        string _nameOfCluster = "C-" + _meta_hyperedge;
                        if (_dic_clusters.ContainsKey(_nameOfCluster))
                        {
                            _dic_clusters[_nameOfCluster].AddElement(_e);
                        }
                        else
                        {
                            Cluster c = new Cluster(_nameOfCluster);
                            c.AddElement(_e);
                            _dic_clusters.Add(_nameOfCluster, c);
                        }
                    }


                    Structuring = new Partition() { Clusters = _dic_clusters };

                    if (IContainerProgressBar != null)
                        IContainerProgressBar.FinishProgressBar();

                    return Structuring;

                    #endregion
                }
            }
            catch
            {
                if (IContainerProgressBar != null)
                    IContainerProgressBar.ShowError("Error occurred in MCLA algorithm.");
                return null;
            }
        }

    }

    #region Algoritmos localmente adaptativos (LAC)

    public class WBPA : Graphs
    {
        [In(typeof(IntGTZeroConverter))]
        public int ClustersCount { get; set; }

        public WBPA() : base() { Name = "WBPA"; }
        public WBPA(Set set, List<Structuring> estr)
            : base(set, estr)
        { Name = "WBPA"; }

        public override Structuring BuildStructuring()
        {
            try
            {
                if (IContainerProgressBar != null)
                {
                    IContainerProgressBar.ResetProgressBar(1, 1, true);
                    IContainerProgressBar.UpdateProgressBar(1, "Running WBPA algorithm...", true);
                }

                WSPA.VerifyPartitions(Structurings);
                List<double[,]> MatrixsOf_dit = new List<double[,]>();
                double[,] Max_row = new double[Set.ElementsCount, StructuringsCount];
                List<double[,]> MatrixsObjectRepresentation = new List<double[,]>();

                int q_m = StructuringsCount * Structurings[0].ClustersCount;
                double[,] A = new double[Set.ElementsCount, q_m];

                for (int i = 0; i < StructuringsCount; i++)
                {
                    double[,] Matrix = new double[Set.ElementsCount, Structurings[i].ClustersCount];
                    MatrixsOf_dit.Add(Matrix);

                    double[,] MatrixRepresentation = new double[Set.ElementsCount, Structurings[i].ClustersCount];
                    MatrixsObjectRepresentation.Add(MatrixRepresentation);

                    for (int j = 0; j < Set.ElementsCount; j++)
                    {
                        int k = 0;
                        double _max = double.MinValue;
                        //Aqui se llena la matrix por fila
                        foreach (var cluster in Structurings[i].Clusters.Values)
                        {
                            Matrix[j, k] = WSPA.Calculate_dit(Set[j], cluster);

                            if (Matrix[j, k] > _max)
                                _max = Matrix[j, k];

                            if (j > 0)
                            {
                                int _q = Structurings[i].ClustersCount;
                                double _numerator = Max_row[j - 1, i] - Matrix[j - 1, k] + 1;
                                double _denominator = _q * Max_row[j - 1, i] + _q - WSPA.SumRow(Matrix, j - 1);
                                MatrixRepresentation[j - 1, k] = _numerator / _denominator;
                                A[j - 1, i * _q + k] = MatrixRepresentation[j - 1, k];
                            }

                            k++;
                        }

                        Max_row[j, i] = _max;
                    }

                    //Calcula la ultima fila de la representacion, es decir del ultimo objeto
                    for (int k = 0; k < Structurings[i].ClustersCount; k++)
                    {
                        int j = Set.ElementsCount;
                        int _q = Structurings[i].ClustersCount;
                        double _numerator = Max_row[j - 1, i] - Matrix[j - 1, k] + 1;
                        double _denominator = _q * Max_row[j - 1, i] + _q - WSPA.SumRow(Matrix, j - 1);
                        MatrixRepresentation[j - 1, k] = _numerator / _denominator;
                        A[j - 1, i * _q + k] = MatrixRepresentation[j - 1, k];
                    }
                }

                int _ElementsCount = Set.ElementsCount;

                for (int i = 0; i < q_m; i++)
                {
                    Element _element = new Element();
                    _element.Name = "Element cluster - " + i;
                    _element.Index = i;
                    _element.Attributes = Set.Attributes;

                    Set.Elements.Insert(0, _element);
                }
                //Actualizar los indices de los elementos originales
                for (int i = q_m; i < Set.ElementsCount; i++)
                    Set[i].Index = i;

                SimilarityWBPA _sim = new SimilarityWBPA(A, _ElementsCount, Structurings[0].ClustersCount, StructuringsCount);

                Metis _metis = new Metis(Set, _sim);
                _metis.IContainerProgressBar = IContainerProgressBar;
                _metis.ClustersCount = ClustersCount;

                Structuring _StructTemp = _metis.BuildStructuring();



                Dictionary<string, Cluster> dic_clusters = new Dictionary<string, Cluster>();
                foreach (var cluster in _StructTemp.Clusters.Values)
                {

                    for (int i = 0; i < cluster.ElementsCount; i++)
                        if (cluster.Elements[i].Index < q_m)
                        {
                            cluster.Elements.RemoveAt(i);
                            i--;
                        }
                    if (cluster.ElementsCount > 0)
                        dic_clusters.Add(cluster.Name, cluster);
                }

                Structuring = new Partition() { Clusters = dic_clusters };

                //Borrar los elementos annadidos en el conjunto
                for (int i = 0; i < q_m; i++)
                    Set.Elements.RemoveAt(0);

                //Actualizar los indices de los elementos que estan en el SET, que eran los originales
                for (int i = 0; i < Set.ElementsCount; i++)
                    Set[i].Index = i;

                return Structuring;
            }
            catch (Exception _ex)
            {
                if (IContainerProgressBar != null)
                    IContainerProgressBar.ShowError("Error occurred in WBPA algorithm.\n" + _ex.Message);
                return null;
            }
        }
    }

    public class WSPA : Graphs
    {
        [In(typeof(IntGTZeroConverter))]
        public int ClustersCount { get; set; }

        public WSPA() : base() { Name = "WSPA"; }
        public WSPA(Set set, List<Structuring> estr)
            : base(set, estr)
        { Name = "WSPA"; }

        public override Structuring BuildStructuring()
        {
            try
            {
                if (IContainerProgressBar != null)
                {
                    IContainerProgressBar.ResetProgressBar(1, 1, true);
                    IContainerProgressBar.UpdateProgressBar(1, "Running WSPA algorithm...", true);
                }

                VerifyPartitions(Structurings);
                List<double[,]> MatrixsOf_dit = new List<double[,]>();
                double[,] Max_row = new double[Set.ElementsCount, StructuringsCount];
                List<double[,]> MatrixsObjectRepresentation = new List<double[,]>();


                for (int i = 0; i < StructuringsCount; i++)
                {
                    double[,] Matrix = new double[Set.ElementsCount, Structurings[i].ClustersCount];
                    MatrixsOf_dit.Add(Matrix);

                    double[,] MatrixRepresentation = new double[Set.ElementsCount, Structurings[i].ClustersCount];
                    MatrixsObjectRepresentation.Add(MatrixRepresentation);

                    for (int j = 0; j < Set.ElementsCount; j++)
                    {
                        int k = 0;
                        double _max = double.MinValue;
                        //Aqui se llena la matrix por fila
                        foreach (var cluster in Structurings[i].Clusters.Values)
                        {
                            Matrix[j, k] = Calculate_dit(Set[j], cluster);

                            if (Matrix[j, k] > _max)
                                _max = Matrix[j, k];

                            if (j > 0)
                            {
                                int _q = Structurings[i].ClustersCount;
                                double _numerator = Max_row[j - 1, i] - Matrix[j - 1, k] + 1;
                                double _denominator = _q * Max_row[j - 1, i] + _q - SumRow(Matrix, j - 1);
                                MatrixRepresentation[j - 1, k] = _numerator / _denominator;
                            }

                            k++;
                        }

                        Max_row[j, i] = _max;
                    }

                    //Calcula la ultima fila de la representacion, es decir del ultimo objeto
                    for (int k = 0; k < Structurings[i].ClustersCount; k++)
                    {
                        int j = Set.ElementsCount;
                        int _q = Structurings[i].ClustersCount;
                        double _numerator = Max_row[j - 1, i] - Matrix[j - 1, k] + 1;
                        double _denominator = _q * Max_row[j - 1, i] + _q - SumRow(Matrix, j - 1);
                        MatrixRepresentation[j - 1, k] = _numerator / _denominator;
                    }


                }


                double[,] _Similarities = new double[Set.ElementsCount, Set.ElementsCount];
                double m = StructuringsCount;
                for (int i = 0; i < Set.ElementsCount; i++)
                {
                    for (int j = 0; j < Set.ElementsCount; j++)
                    {
                        double _Sij = 0;
                        for (int k = 0; k < MatrixsObjectRepresentation.Count; k++)
                        {
                            double[,] A = MatrixsObjectRepresentation[k];

                            if (i != j)
                                _Sij += EscalarProduct(A, i, j) / (Norm(A, i) * Norm(A, j));
                        }
                        _Similarities[i, j] = _Sij / m;
                    }
                }

                SimilarityWSPA _sim = new SimilarityWSPA(_Similarities);

                Metis _metis = new Metis(Set, _sim);
                _metis.IContainerProgressBar = IContainerProgressBar;
                _metis.ClustersCount = ClustersCount;

                Structuring = _metis.BuildStructuring();

                return Structuring;
            }
            catch (Exception _ex)
            {
                if (IContainerProgressBar != null)
                    IContainerProgressBar.ShowError("Error occurred in WSPA algorithm.\n" + _ex.Message);
                return null;
            }
        }

        //Recordar que ya debe estar actualizado el centroide, 
        //ya que las particiones tienen que ser generadas solamente por
        //el algoritmo LAC o algun otro que tenga la misma salida.
        internal static double Calculate_dit(Element element, Cluster cluster)
        {
            double _result = 0;
            for (int i = 0; i < element.Attributes.ValuesCount; i++)
                _result += cluster.Weights[i] * Math.Pow((double)element[i] - (double)cluster.Centroid[i], 2);

            return Math.Sqrt(_result);
        }

        internal static void VerifyPartitions(List<Structuring> Partitions)
        {
            if (Partitions.Count > 0)
            {
                int _initialClusters = Partitions[0].ClustersCount;
                //Todas deben ser generadas por el LAC
                //Todas las particiones deben tener la misma cantidad de clusters
                foreach (var item in Partitions)
                    if (!(item is WeightedPartition))
                        throw new Exception("All partitions must be generated with LAC algorithm.");
                    else if (item.ClustersCount != _initialClusters)
                        throw new Exception("All partitions must have the same ClustersCount.");

            }
        }

        internal static double SumRow(double[,] Matrix, int row)
        {
            double _result = 0;
            for (int i = 0; i < Matrix.GetLength(1); i++)
                _result += Matrix[row, i];
            return _result;
        }


        private double Norm(double[,] A, int i)
        {
            double _result = 0;
            for (int j = 0; j < A.GetLength(1); j++)
                _result += Math.Pow(A[i, j], 2);

            return Math.Sqrt(_result);
        }

        private double EscalarProduct(double[,] A, int rowi, int rowj)
        {
            double _result = 0;
            for (int j = 0; j < A.GetLength(1); j++)
                _result += A[rowi, j] * A[rowj, j];

            return _result;
        }
    }

    #endregion
}
