using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClusterEnsemble.Proximities
{

    public abstract class Proximity
    {
        public string Name { get; set; }
        public Property Properties { get; set; }
        public ElementType AdmissibleElementType { get; set; }
        public List<Attribute> AttributesToCalculateProximity { get; set; }
        public abstract double CalculateProximity(Element ei, Element ej);

        /// <summary>
        /// Verifica que se puede ejecutar el metodo CalculateProximity
        /// </summary>
        /// <param name="ei"></param>
        /// <param name="ej"></param>
        protected void Verify(Element ei, Element ej)
        {
            string _className = this.GetType().Name;

            if (AttributesToCalculateProximity == null)
                throw new Exception("La lista AttributesToCalculateProximity no puede ser NULL, error en la clase " + _className);

            ElementType _elementType = AttributesType();
            if (AdmissibleElementType != ElementType.Mixt)
            {
                if (_elementType != AdmissibleElementType)
                    throw new Exception("El tipo de los atributos a calcular proximidad no coincide con los admisibles por esta disimilitud, error en la clase " + _className);
            }
            if (ei == null || ej == null)
                throw new ArgumentNullException("Parametros incorrectos en el metodo CalculateProximity, error en la clase " + _className);
            if (AttributesToCalculateProximity == null)
                throw new Exception("La lista AttributesToCalculateProximity no puede ser NULL, error en la clase " + _className);

            if (!ei.Attributes.Equals(ej.Attributes))
                throw new ArgumentException("Los elementos no tienen los atributos iguales, error en la clase " + _className);

            Attributes attributes = ei.Attributes;
            //Validate 
            if (AttributesToCalculateProximity.Count == 0)
                throw new Exception("La lista AttributesToCalculateProximity no puede tener cero elementos, error en la clase " + _className);
            for (int i = 0; i < AttributesToCalculateProximity.Count; i++)
                if (!attributes.ContainsAttribute(AttributesToCalculateProximity[i].Name))
                    throw new Exception("Atributos Incorrectos, error en la clase " + _className);
        }
        private ElementType AttributesType()
        {
            string _className = this.GetType().Name;

            if (AttributesToCalculateProximity == null)
                throw new NullReferenceException("AttributesToCalculateProximity no puede ser null, error en la clase " + _className);
            ElementType _result;

            if (!AttributesToCalculateProximity.Exists(a => a.AttributeType != AttributeType.Numeric))
                _result = ElementType.Numeric;
            else if (!AttributesToCalculateProximity.Exists(a => a.AttributeType != AttributeType.Nominal))
                _result = ElementType.Nominal;
            else if (!AttributesToCalculateProximity.Exists(a => a.AttributeType != AttributeType.String))
                _result = ElementType.String;
            else if (!AttributesToCalculateProximity.Exists(a => a.AttributeType != AttributeType.Date))
                _result = ElementType.Date;
            else //Todos no son iguales 
                _result = ElementType.Mixt;

            return _result;
        }
        public static ElementType GetAttributesType(List<Attribute> AttributesToCalculateProximity)
        {
            if (AttributesToCalculateProximity == null)
                throw new ArgumentNullException("AttributesToCalculateProximity no puede ser null, parametro incorrecto, metodo static de la clase Proximity");
            ElementType _result;

            if (!AttributesToCalculateProximity.Exists(a => a.AttributeType != AttributeType.Numeric))
                _result = ElementType.Numeric;
            else if (!AttributesToCalculateProximity.Exists(a => a.AttributeType != AttributeType.Nominal))
                _result = ElementType.Nominal;
            else if (!AttributesToCalculateProximity.Exists(a => a.AttributeType != AttributeType.String))
                _result = ElementType.String;
            else if (!AttributesToCalculateProximity.Exists(a => a.AttributeType != AttributeType.Date))
                _result = ElementType.Date;
            else //Todos no son iguales 
                _result = ElementType.Mixt;

            return _result;
        }
    }
    /// <summary>
    /// Interface para calcula la disimilitud entre cualquier par de elementos.
    /// </summary>
    public abstract class Dissimilarity:Proximity
    {
        //Preguntar cuales son las propieades que debe cumplir las disimilariades.
    }
    public abstract class Similarity : Proximity
    { }


    public class AngleCosine : Dissimilarity
    {
        public AngleCosine()
        {
            Properties = Property.Simetric;
            Name = "Angle Cosine [diss]";
            AdmissibleElementType = ElementType.Numeric;
        }

        public override double CalculateProximity(Element ei, Element ej)
        {
            base.Verify(ei, ej);

            double esc_prod = EscalarProduct(ei, ej);
            double ei_norm = Norm(ei);
            double ej_norm = Norm(ej);

            return 1 - (esc_prod / (ei_norm * ej_norm));
        }

        #region Private Members
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ei">Tiene que ser de tipo Numerico</param>
        /// <param name="ej">Tiene que ser de tipo Numerico</param>
        /// <returns></returns>
        private double EscalarProduct(Element ei, Element ej)
        {
            double result = 0;
            for (int i = 0; i < ei.ValuesCount; i++)
                if (AttributesToCalculateProximity.Contains(ei.Attributes[i]) && !ei.IsMissing(i) && !ej.IsMissing(i))
                    result += ((double)ei[i]) * ((double)ej[i]);

            return result;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e">El elemento tiene que ser de tipo Numerico</param>
        /// <returns></returns>
        private double Norm(Element e)
        {
            double norm = 0;
            for (int i = 0; i < e.ValuesCount; i++)
                if (AttributesToCalculateProximity.Contains(e.Attributes[i]) && !e.IsMissing(i))
                {
                    double c = (double)e[i];
                    norm += c * c;
                }

            return Math.Sqrt(norm);
        }
        #endregion
    }
    public class AngleCosineSim : Similarity
    {
        public AngleCosineSim()
        {
            Properties = Property.Simetric;
            Name = "Angle Cosine [sim]";
            AdmissibleElementType = ElementType.Numeric;
        }

        public override double CalculateProximity(Element ei, Element ej)
        {
            base.Verify(ei, ej);

            double esc_prod = EscalarProduct(ei, ej);
            double ei_norm = Norm(ei);
            double ej_norm = Norm(ej);

            return (esc_prod / (ei_norm * ej_norm));
        }

        #region Private Members
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ei">Tiene que ser de tipo Numerico</param>
        /// <param name="ej">Tiene que ser de tipo Numerico</param>
        /// <returns></returns>
        private double EscalarProduct(Element ei, Element ej)
        {
            double result = 0;
            for (int i = 0; i < ei.ValuesCount; i++)
                if (AttributesToCalculateProximity.Contains(ei.Attributes[i]) && !ei.IsMissing(i) && !ej.IsMissing(i))
                    result += ((double)ei[i]) * ((double)ej[i]);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e">El elemento tiene que ser de tipo Numerico</param>
        /// <returns></returns>
        private double Norm(Element e)
        {
            double norm = 0;
            for (int i = 0; i < e.ValuesCount; i++)
                if (AttributesToCalculateProximity.Contains(e.Attributes[i]) && !e.IsMissing(i))
                {
                    double c = (double)e[i];
                    norm += c * c;
                }

            return Math.Sqrt(norm);
        }
        #endregion
    }

    public class EuclideanDistance : Dissimilarity
    {
        double[] normvalues;

        public EuclideanDistance()
        {
            Properties = Property.Simetric;
            Name = "Euclidean Distance";
            AdmissibleElementType = ElementType.Mixt;
        }

        public override double CalculateProximity(Element ei, Element ej)
        {
            base.Verify(ei, ej);
            if (ei.Set != null && ei.Set.NormValues!=null)
            normvalues = ei.Set.NormValues;
            
            double result = 0;
            for (int i = 0; i < ei.ValuesCount; i++)
                if (AttributesToCalculateProximity.Contains(ei.Attributes[i]) && !ei.IsMissing(i) && !ej.IsMissing(i))
                    result += Math.Pow(CalculateDiff(ei, ej, ei.Attributes[i], i), 2);

            return Math.Sqrt(result);
        }

        private double CalculateDiff(Element ei, Element ej, Attribute aAtrribute, int aIndex)
        {
            double _result = 0;
            switch (aAtrribute.AttributeType)
            {
                case AttributeType.String:
                case AttributeType.Nominal:
                    _result = ((string)ei[aIndex] == (string)ej[aIndex]) ? 0 : 1;
                    break;
                case AttributeType.Numeric:
                    if (normvalues != null && normvalues[aIndex] > 0)
                        _result = ((double)ei[aIndex] - (double)ej[aIndex]) / normvalues[aIndex];
                    else
                        _result = ((double)ei[aIndex] - (double)ej[aIndex]);

                    break;
                case AttributeType.Date:
                    _result = ((DateTime)ei[aIndex] == (DateTime)ej[aIndex]) ? 0 : 1;
                    break;
                default:
                    throw new Exception("El tipo del atributo no esta implementado, error en la clase EuclideanDistance");
            }

            return _result;
        }
        
    }

    public class ManhatanDistance : Dissimilarity
    {
        double[] normvalues;

        public ManhatanDistance()
        {            
            Name = "Manhatan Distance";
            AdmissibleElementType = ElementType.Mixt;
        }

        public override double CalculateProximity(Element ei, Element ej)
        {
            base.Verify(ei, ej);
            if (ei.Set != null && ei.Set.NormValues != null)
                normvalues = ei.Set.NormValues;

            double result = 0;
            for (int i = 0; i < ei.ValuesCount; i++)
                if (AttributesToCalculateProximity.Contains(ei.Attributes[i]) && !ei.IsMissing(i) && !ej.IsMissing(i))
                    result += Math.Abs(CalculateDiff(ei, ej, ei.Attributes[i], i));

            return result;
        }

        private double CalculateDiff(Element ei, Element ej, Attribute aAtrribute, int aIndex)
        {
            double _result = 0;
            switch (aAtrribute.AttributeType)
            {
                case AttributeType.String:
                case AttributeType.Nominal:
                    _result = ((string)ei[aIndex] == (string)ej[aIndex]) ? 0 : 1;
                    break;
                case AttributeType.Numeric:
                    if (normvalues != null && normvalues[aIndex] > 0)
                        _result = ((double)ei[aIndex] - (double)ej[aIndex]) / normvalues[aIndex];
                    else
                        _result = ((double)ei[aIndex] - (double)ej[aIndex]);

                    break;
                case AttributeType.Date:
                    _result = ((DateTime)ei[aIndex] == (DateTime)ej[aIndex]) ? 0 : 1;
                    break;
                default:
                    throw new Exception("El tipo del atributo no esta implementado, error en la clase EuclideanDistance");
            }

            return _result;
        }
    }

    public class Chebyshev : Dissimilarity
    {
        double[] normvalues;

        public Chebyshev()
        {            
            Name = "Chebyshev";
            AdmissibleElementType = ElementType.Mixt;
        }

        public override double CalculateProximity(Element ei, Element ej)
        {
            base.Verify(ei, ej);
            if (ei.Set != null && ei.Set.NormValues != null)
                normvalues = ei.Set.NormValues;

            double result = double.MinValue;
            for (int i = 0; i < ei.ValuesCount; i++)
                if (AttributesToCalculateProximity.Contains(ei.Attributes[i]) && !ei.IsMissing(i) && !ej.IsMissing(i))
                {
                    double temp = Math.Abs(CalculateDiff(ei, ej, ei.Attributes[i], i));
                    if (temp > result)
                        result = temp;
                }

            return result;
        }

        private double CalculateDiff(Element ei, Element ej, Attribute aAtrribute, int aIndex)
        {
            double _result = 0;
            switch (aAtrribute.AttributeType)
            {
                case AttributeType.String:
                case AttributeType.Nominal:
                    _result = ((string)ei[aIndex] == (string)ej[aIndex]) ? 0 : 1;
                    break;
                case AttributeType.Numeric:
                    if (normvalues != null && normvalues[aIndex] > 0)
                        _result = ((double)ei[aIndex] - (double)ej[aIndex]) / normvalues[aIndex];
                    else
                        _result = ((double)ei[aIndex] - (double)ej[aIndex]);

                    break;
                case AttributeType.Date:
                    _result = ((DateTime)ei[aIndex] == (DateTime)ej[aIndex]) ? 0 : 1;
                    break;
                default:
                    throw new Exception("El tipo del atributo no esta implementado, error en la clase EuclideanDistance");
            }

            return _result;
        }
    }

    public class Minkowski : Dissimilarity
    {
        double[] normvalues;

        public Minkowski()
        {            
            Name = "Minkowski [m=4]";
            AdmissibleElementType = ElementType.Mixt;
        }

        public override double CalculateProximity(Element ei, Element ej)
        {
            base.Verify(ei, ej);
            if(ei.Set!=null && ei.Set.NormValues !=null)
            normvalues = ei.Set.NormValues;

            double result = 0;
            for (int i = 0; i < ei.ValuesCount; i++)
                if (AttributesToCalculateProximity.Contains(ei.Attributes[i]) && !ei.IsMissing(i) && !ej.IsMissing(i))
                    result += Math.Pow(CalculateDiff(ei, ej, ei.Attributes[i], i), 4);

            return Math.Pow(result, 1.0 / 4);
        }

        private double CalculateDiff(Element ei, Element ej, Attribute aAtrribute, int aIndex)
        {
            double _result = 0;
            switch (aAtrribute.AttributeType)
            {
                case AttributeType.String:
                case AttributeType.Nominal:
                    _result = ((string)ei[aIndex] == (string)ej[aIndex]) ? 0 : 1;
                    break;
                case AttributeType.Numeric:
                    if (normvalues != null && normvalues[aIndex] > 0)
                        _result = ((double)ei[aIndex] - (double)ej[aIndex]) / normvalues[aIndex];
                    else
                        _result = ((double)ei[aIndex] - (double)ej[aIndex]);

                    break;
                case AttributeType.Date:
                    _result = ((DateTime)ei[aIndex] == (DateTime)ej[aIndex]) ? 0 : 1;
                    break;
                default:
                    throw new Exception("El tipo del atributo no esta implementado, error en la clase EuclideanDistance");
            }

            return _result;
        }
    }

    /// <summary>
    /// Esta clase se utiliza en la heuristica de hipergrafo CSPA 
    /// para Cluster Ensemble, como una medida de disimilitud que se le pasa al Metis.
    /// </summary>
    class CoAsociationMatrixDissToMetis : Similarity
    {
        private Set Set { get; set; }
        private byte[,] VxH { get; set; }
        private int StructuringsCount { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="set">El conjunto sobre el cual se esta trabajando</param>
        /// <param name="VxH">Matriz binaria, de dimension cantidad de vertices por cantidad de labels total</param>
        /// <param name="structuringsCount">Cantidad de estructuraciones sobre las que se quiere hacer Cluster Ensemble</param>
        public CoAsociationMatrixDissToMetis(Set set, byte[,] VxH,int structuringsCount)
        {
            if (set == null || VxH == null || !(structuringsCount > 0) || set.ElementsCount != VxH.GetLength(0))
                throw new ArgumentNullException("Argumentos Incorrectos en la construccion de CoAsociationMatrixDiss: esta clase se utiliza en la heuristica de grafo CSPA");

            this.Set = set;
            this.VxH = VxH;
            this.StructuringsCount = structuringsCount;
            Name = "CoAsociationMatrixDissToMetis";
        }

        /// <summary>
        /// La disimilitud es la cantidad de clusters en que cayeron juntos(que a lo sumo en cada estructuracion es 1) entre la cantidad total de estructuraciones
        /// </summary>
        /// <param name="ei">Elemento ei</param>
        /// <param name="ej">Elemento ej</param>
        /// <returns></returns>
        public override double CalculateProximity(Element ei, Element ej)
        {
            int sameCluster = 0;

            //Contar en la cantidad de particiones en que cayeron en el mismo cluster ei and ej
            for (int i = 0; i < VxH.GetLength(1); i++)
                if (VxH[ei.Index, i] == 1 && VxH[ej.Index, i] == 1)
                    sameCluster++;
            return ((double)sameCluster) / StructuringsCount;
        }
    }

    /// <summary>
    /// Esta clase se utiliza en la heuristica de hipergrafo MCLA 
    /// para Cluster Ensemble, como una medida de disimilitud que se le pasa al Metis.
    /// </summary>
    class BinaryJaccardMeasure : Similarity
    {
        private byte[,] Metagraph { get; set; }
        
        public BinaryJaccardMeasure(byte[,] metagraph)
        {
            if (metagraph == null)
                throw new ArgumentNullException();
            this.Metagraph = metagraph;
            Name = "BinaryJaccardMeasure";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hi">Hyperedges ha</param>
        /// <param name="hj">Hyperedges hb</param>
        /// <returns></returns>
        public override double CalculateProximity(Element ha, Element hb)
        {
            int ha_hb = MultiplyVector(ha.Index, hb.Index);
            int ha_ha = SquareNorm(ha.Index);
            int hb_hb = SquareNorm(hb.Index);
            int total = ha_ha + hb_hb - ha_hb;

            return ((double)ha_hb) / ((double)total);
        }

        #region Private Members

        private int MultiplyVector(int a, int b)
        {
            int result = 0;
            for (int row = 0; row < Metagraph.GetLength(0); row++)
                result += Metagraph[row,a]*Metagraph[row,b];
            
            return result;
        }

        //Se puede utilizar el metodo MultiplyVector y los parametros son iguales
        //pero seria mas ineficiente ya que estaria multiplicando por gusto en cada iteracion.
        private int SquareNorm(int a)
        {
            int result = 0;
            for (int row = 0; row < Metagraph.GetLength(0); row++)
                result += Metagraph[row, a];

            return result;
        }

        #endregion

    }

    class SimilarityWSPA : Similarity
    {
        private double[,] Similarities { get; set; }
        
        public SimilarityWSPA(double[,] aSimilarities)
        {
            if (aSimilarities == null)
                throw new ArgumentNullException();
            this.Similarities = aSimilarities;
            Name = "SimilarityWSPA";
        }
        public override double CalculateProximity(Element ei, Element ej)
        {
            return Similarities[ei.Index, ej.Index];
        }
    }

    class SimilarityWBPA : Similarity
    {
        double[,] A { get; set; }
        int OriginalElementsCount { get; set; }
        int ClusterCount { get; set; }
        int PartitionsCount { get; set; }

        public SimilarityWBPA(double[,] aA, int aOriginalElementsCount, int aClusterCount, int aPartitionsCount)
        {
            if (aA == null)
                throw new ArgumentNullException();
            this.A = aA;
            this.OriginalElementsCount = aOriginalElementsCount;
            this.ClusterCount = aClusterCount;
            this.PartitionsCount = aPartitionsCount;
            Name = "SimilarityWBPA";
        }
        public override double CalculateProximity(Element ei, Element ej)
        {
            int q_m = ClusterCount * PartitionsCount;
            //Ambos son clusters
            if (ei.Index < q_m && ej.Index < q_m)
                return 0;
            //Ambos son elementos
            else if (ei.Index >= q_m && ej.Index >= q_m)
                return 0;
            //Elemento Cluster
            else if (ei.Index >= q_m && ej.Index < q_m)
                return A[ei.Index - q_m, ej.Index];
            //Cluster Elemento
            else if (ei.Index < q_m && ej.Index >= q_m)
                return CalculateProximity(ej, ei);

            return -1;
        }
    }

    



}
