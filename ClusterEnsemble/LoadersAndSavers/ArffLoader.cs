using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using weka.core.converters;
using weka.core;
using ClusterEnsemble.Graphics;

namespace ClusterEnsemble.LoadersAndSavers
{
    public class ArffLoader : ILoader
    {
        FileStream filestream;
        public ArffLoader()
        {
        }
        public ArffLoader(string filepath)
        {
            this.SourceFilePath = filepath;
        }

        #region ILoader Members

        public void SetSource(FileStream file)
        {
            if (file == null || !file.CanRead || Path.GetExtension(file.Name) != ".arff")
                throw new ArgumentException("Parametros Incorrectos en el metodo SetSource de ArffLOader.");
            this.filestream = file;
        }

        public void SetSource(string filepath)
        {
            if (!File.Exists(filepath) || Path.GetExtension(filepath) != ".arff")
                throw new ArgumentException("El Fichero: " + filepath + ", tiene formato incorrecto o no existe.");

            this.SourceFilePath = filepath;
        }

        public void ResetSource()
        {
            throw new NotImplementedException();
        }
       
        public bool TryLoad()
        {
            try
            {
                this.filestream = new FileStream(this.SourceFilePath, FileMode.Open);

                weka.core.converters.ArffLoader loader = new weka.core.converters.ArffLoader();
                loader.set_FilePath(SourceFilePath);
                Instances instances = loader.getDataSet();
                return true;
            }
            catch (Exception _ex)
            {
                return false;
            }
            finally
            {
                filestream.Close();
            }
        }
        public Set Load()
        {
            Set set = null;
            try
            {
                this.filestream = new FileStream(this.SourceFilePath, FileMode.Open);

                weka.core.converters.ArffLoader loader = new weka.core.converters.ArffLoader();
                loader.set_FilePath(SourceFilePath);
                Instances instances = loader.getDataSet();
                AttributeStats _ast =  instances.attributeStats(0);
                
                List<Attribute> attributes = new List<Attribute>();
                Attribute attrToAdd = null;
                //Obtener los atributos de la coleccion de elementos
                for (int i = 0; i < instances.numAttributes(); i++)
                {
                    attrToAdd = null;
                    if (instances.attribute(i).isNominal())
                    {
                        List<object> l = new List<object>();
                        for (int j = 0; j < instances.attribute(i).numValues(); j++)
                            l.Add(instances.attribute(i).value(j));

                        attrToAdd = new Attribute(instances.attribute(i).name(), l);
                        attrToAdd.AttributeType = AttributeType.Nominal;
                        attributes.Add(attrToAdd);
                    }
                    else if (instances.attribute(i).isNumeric())
                    {
                        attrToAdd = new Attribute(instances.attribute(i).name(), null);
                        attrToAdd.AttributeType = AttributeType.Numeric;
                        attributes.Add(attrToAdd);
                    }
                    else if (instances.attribute(i).isString())
                    {
                        attrToAdd = new Attribute(instances.attribute(i).name(), null);
                        attrToAdd.AttributeType = AttributeType.String;
                        attributes.Add(attrToAdd);
                    }
                    else if (instances.attribute(i).isDate())
                    {
                        attrToAdd = new Attribute(instances.attribute(i).name(), null);
                        attrToAdd.AttributeType = AttributeType.Date;
                        attributes.Add(attrToAdd);
                    }

                }
                Attributes attributes_values = new Attributes(attributes);

                List<Element> elements = new List<Element>();
                //Darle los valores a cada elemento de la coleccion
                for (int i = 0; i < instances.numInstances(); i++)
                {
                    List<object> realvalues = new List<object>();
                    for (int j = 0; j < instances.instance(i).numAttributes(); j++)
                    {
                        if (instances.instance(i).isMissing(j))
                            realvalues.Add(null);
                        else if (instances.attribute(j).isNumeric())
                            realvalues.Add(instances.instance(i).value(j));
                        else if (instances.attribute(j).isNominal() || instances.attribute(j).isString())
                            realvalues.Add(instances.instance(i).stringValue(j));
                        else if (instances.attribute(j).isDate())
                            realvalues.Add(DateTime.Parse(instances.instance(i).stringValue(j)));
                    }
                    Element e = new Element(realvalues);
                    e.Name = "E-" + i;
                    e.Index = i;
                    elements.Add(e);
                }

                set = new Set(instances.relationName(), elements, attributes_values);
                //Attributes[i].Missing = _missing;
                //Attributes[i].MissingPercent = 100 * _missing / ElementsCount;
                //Attributes[i].Distinct = _distinct;
                //Attributes[i].Unique = _unique;
                //Attributes[i].UniquePercent = 100 * _unique / ElementsCount;
                for (int i = 0; i < instances.numAttributes(); i++)
                {
                    set.Attributes[i].Missing = instances.attributeStats(i).missingCount;
                    set.Attributes[i].MissingPercent = 100 * set.Attributes[i].Missing / elements.Count;
                    set.Attributes[i].Distinct = instances.attributeStats(i).distinctCount;
                    set.Attributes[i].Unique = instances.attributeStats(i).uniqueCount;
                    set.Attributes[i].UniquePercent = 100*set.Attributes[i].Unique/elements.Count;                
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                filestream.Close();
            }
            return set;
        }


        public bool TryLoad(string filepath)
        {
            string tempfilepath = SourceFilePath;

            SourceFilePath = filepath;
            bool result = TryLoad();
            SourceFilePath = tempfilepath;

            return result;
        }

        public Set Load(string filepath)
        {

            string tempfilepath = SourceFilePath;

            SourceFilePath = filepath;
            Set result = Load();
            SourceFilePath = tempfilepath;

            return result;
        }

        public string SourceFilePath { get; set; }

        #endregion
    }
}
