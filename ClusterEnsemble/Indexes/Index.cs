using System;
using System.Collections.Generic;
using System.Text;

namespace ClusterEnsemble
{
    public abstract class Index
    {
        public Index()   { }

        public abstract double Process(Partition p);

        double[] lastEval;
        double entropy;
        double sum = 0;


        /// <summary>
        /// Suma de las evaluaciones de este indice en todas las particiones
        /// </summary>
        public double Sum
        {
            get { return sum; }
        }

        /// <summary>
        /// Entropia de la evaluaciojnes de este indice en todas las partitiones
        /// </summary>
        public double Entropy
        {
            get { return entropy; }
        }


        /// <summary>
        /// Evaluacion de los indices en el ultimo conjunto de particiones que se le paso al metodo NormIndexEvaluation
        /// </summary>
        public double[] LastEval
        {
            get { return lastEval; }
        }
        /// <summary>
        /// Para cada indice guarda el valor de evaluar este indice en todas las particiones. Devuelve el valor normalizado entre 0 y 1, esto se hace porq cada indice tiene un rango de valores diferentes, se normaliza para poder hacer la combinacion.
        /// </summary>
        /// <param name="partitions"></param>
        /// <returns></returns>
        public void NormIndexEvaluation(Partition[] partitions)
        {
            lastEval=new double[partitions.Length];
            double min = double.MaxValue;
            double max = double.MinValue;
            sum = 0;
            entropy = 0;
            
            for (int i = 0; i < lastEval.Length; i++)
            {
                lastEval[i] = this.Process(partitions[i]);
                if (lastEval[i] < min)
                    min = lastEval[i];
                if (lastEval[i] > max)
                    max = lastEval[i];
            }

            //Normalizacion y calculo de la suma de los valores

            for (int i = 0; i < lastEval.Length; i++)
            {
                if (min == max)
                    lastEval[i] = 0;
                else
                    lastEval[i] = (lastEval[i] - min) / (max - min);
                sum += lastEval[i];
            }


            //Calculo de la Entropia

            for (int i = 0; i < lastEval.Length; i++)
            {
                if(lastEval[i]!=0)
                    entropy += -1 * (lastEval[i] / sum) * Math.Log(lastEval[i] / sum, 2);
            }
        }


    }
}
