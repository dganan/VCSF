using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using weka.core.converters;
using weka.core;
using weka.filters.unsupervised.attribute;
using weka.classifiers.meta;
using weka.classifiers.functions;
using weka.classifiers;
using weka.classifiers.bayes;
using System.Configuration;
using weka.classifiers.trees;

namespace VCS
{
	//import weka.core.*;
	//import weka.core.converters.*;
	//import weka.classifiers.Classifier;
	//import weka.classifiers.functions.SMO;
	//import weka.classifiers.meta.FilteredClassifier;
	//import weka.classifiers.trees.*;
	//import weka.filters.*;
	//import weka.filters.unsupervised.attribute.*;

	//import java.io.*;

	///**
	// * Example class that converts HTML files stored in a directory structure into and ARFF file using the TextDirectoryLoader converter. It then applies the StringToWordVector to the data and feeds a J48 classifier with it.
	// * 
	// * @author FracPete (fracpete at waikato dot ac dot nz)
	// */
	public class ModelGenerator
	{
		public static void GenerateModel ()
		{
			// convert the directory into a dataset
			TextDirectoryLoader loader = new TextDirectoryLoader();
			loader.setDirectory(new java.io.File(ConfigurationManager.AppSettings["SpeechActClassificationDirectory"]));
			Instances instances = loader.getDataSet();

			StringToWordVector filter = new StringToWordVector();
			filter.setInputFormat(instances);
			//Instances dataFiltered = Filter.useFilter(dataRaw, filter);

			// STEP 6 : CREEM UN CLASSIFIER
			
			// SVM Software Vector Machine
			//Classifier cModel = (Classifier)new SMO();
			//(cModel as SMO).setOptions(weka.core.Utils.splitOptions("-M"));
			
			
			Classifier cModel = (Classifier)new J48();
					

			// Bayesian
			//Classifier cModel = (Classifier)new NaiveBayes();
			
			FilteredClassifier fClass = new FilteredClassifier();
			fClass.setFilter(filter);
			fClass.setClassifier(cModel);

			fClass.buildClassifier(instances);

			ArffSaver saver = new ArffSaver();
			saver.setInstances(instances);
			saver.setFile(new java.io.File(ConfigurationManager.AppSettings["SpeechActClassificationDirectory"] + "\\SpeechActClassification.arff"));
			////saver.setDestination(new File("./SpeechActClassification.arff"));   // **not** necessary in 3.5.4 and later
			saver.writeBatch();

			java.io.ObjectOutputStream oos = new java.io.ObjectOutputStream(
			new java.io.FileOutputStream(ConfigurationManager.AppSettings["SpeechActClassificationDirectory"] + "\\SpeechActClassification.model"));
			oos.writeObject(fClass);
			oos.flush();
			oos.close();
	    }
	}
}