using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace VCS
{
	public partial class GenevaEmotionWheelControl : UserControl
	{
		public event EmotionSelectedHandler EmotionSelected;

		private Ellipse [,] ellipses;
		private Ellipse centerEllipse;

		private int ellipseLevels = 4;

		private double[] ellipseSizes = new double[] { 45, 35, 25, 15 };
		private double[] ellipseSeparation = new double[] { 155, 110, 75, 50 };

		private Color[] ellipseColors = new Color[] { 
		
			Color.FromArgb(255, 51, 255, 0),
			Color.FromArgb(255, 51, 153, 51),
			Color.FromArgb(255, 0, 102, 51),
			Color.FromArgb(255, 51, 153, 153),
			Color.FromArgb(255, 51, 51, 255),
			Color.FromArgb(255, 0, 51, 153),
			Color.FromArgb(255, 0, 0, 102),
			Color.FromArgb(255, 0, 0, 51),
			Color.FromArgb(255, 102, 102, 204),
			Color.FromArgb(255, 255, 51, 255),
			Color.FromArgb(255, 255, 0, 153),
			Color.FromArgb(255, 255, 51, 0),
			Color.FromArgb(255, 255, 153, 153),
			Color.FromArgb(255, 255, 153, 51),
			Color.FromArgb(255, 255, 255, 0),
			Color.FromArgb(255, 102, 255, 51),
		};

		private string[] emotionalTags = new string[] { 
		
			"Relief",
			"Hope",
			"Interest",
			"Surprise",
			"Sadness",
			"Fear",
			"Shame",
			"Guilt",
			"Envy",
			"Disgust",
			"Contempt",
			"Anger",
			"Pride",
			"Elation",
			"Joyful",
			"Satisfaction",
		};

		private int size = 400;
		private int midsize = 200;

		private int emotionalCategories = 16;
	
		private double offOpacity = 0.5;
		private double onOpacity = 1;

		private double offThickness = 1;
		private double onThickness = 4;

		public GenevaEmotionWheelControl()
		{
			InitializeComponent();

			PaintControl();
		}

		private void PaintControl()
		{
			ellipses = new Ellipse[ellipseLevels, emotionalCategories];

			double angle = 360 / (double)emotionalCategories;

			double currentAngle = angle / 2.0;

			for (int i = 0; i < ellipseLevels; i++)
			{
				for (int j = 0; j < emotionalCategories; j++)
				{
					ellipses[i,j] = PaintEllipse(ellipseSizes[i], ellipseSeparation[i], currentAngle, ellipseColors[j], emotionalTags[j] + "_" + i);

					currentAngle += angle;
				}
			}

			centerEllipse = PaintEllipse(40, 0, 0, Colors.White, "None");


			currentAngle = angle / 2.0;

			for (int k = 0; k < emotionalCategories; k++)
			{
				TextBlock label = new TextBlock();

				label.Text = emotionalTags[k];

				TransformGroup group = new TransformGroup();
				group.Children.Add(new TranslateTransform() { X = midsize - (label.ActualWidth / 2), Y = midsize - (label.ActualHeight / 2) });

				if (currentAngle >= 0 && currentAngle <= 180)
				{
					group.Children.Add(new RotateTransform() { CenterX = midsize, CenterY = midsize, Angle = -90 });
				}
				else
				{
					group.Children.Add(new RotateTransform() { CenterX = midsize, CenterY = midsize, Angle = 90 });
				}

				group.Children.Add(new TranslateTransform() { X = 190 });
				group.Children.Add(new RotateTransform() { CenterX = midsize, CenterY = midsize, Angle = currentAngle });

				currentAngle += angle;

				label.RenderTransform = group;

				LayoutRoot.Children.Add(label);
			}
		}

		private Ellipse PaintEllipse(double size, double separation, double angle, Color color, string tag)
		{
			Ellipse ell = new Ellipse();

			ell.Fill = new SolidColorBrush(color);
			ell.Stroke = new SolidColorBrush(Colors.Black);
			ell.StrokeThickness = offThickness;

			ell.Width = size;
			ell.Height = size;

			ell.Tag = tag;

			ell.Opacity = offOpacity;

			ell.MouseLeftButtonUp += new MouseButtonEventHandler(ellipse_MouseLeftButtonUp);

			TransformGroup group = new TransformGroup();
			group.Children.Add(new TranslateTransform() { X = midsize - (size / 2), Y = midsize - (size / 2) });
			group.Children.Add(new TranslateTransform() { X = separation });
			group.Children.Add(new RotateTransform() { CenterX = midsize, CenterY = midsize, Angle = angle });

			ell.RenderTransform = group;

			LayoutRoot.Children.Add(ell);

			return ell;
		}

		void ellipse_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			CleanSelection();

			Ellipse ell = sender as Ellipse;

			ell.StrokeThickness = onThickness;
			ell.Opacity = onOpacity;

			string [] tags = ell.Tag.ToString().Split('_');

			int intensity = 1;

			Emotion emotion = (Emotion)Enum.Parse(typeof(Emotion), tags[0], true);

			if (tags.Length > 1)
			{
				intensity = Int32.Parse(tags[1]);
			}

			if (EmotionSelected != null)
			{
				EmotionSelected(new EmotionalState () { Emotion = emotion, Intensity = intensity });
			}
		}

		private void CleanSelection()
		{
			for (int i = 0; i < ellipseLevels; i++)
			{
				for (int j = 0; j < emotionalCategories; j++)
				{
					ellipses[i, j].StrokeThickness = offThickness;
					ellipses[i, j].Opacity = offOpacity;
				}
			}

			centerEllipse.StrokeThickness = offThickness;
			centerEllipse.Opacity = offOpacity;
		}
	}
}
