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
using System.Windows.Navigation;

namespace VCS
{
	public partial class CharacterEditorPage : Page
	{
		byte[] photoImageBytes;

		public CharacterEditorPage()
		{
			InitializeComponent();

			ActivityValue.ValueChanged += new ValueChangedHandler(ActivityValue_ValueChanged);
			QualityValue.ValueChanged += new ValueChangedHandler(QualityValue_ValueChanged);
			PassivityValue.ValueChanged += new ValueChangedHandler(PassivityValue_ValueChanged);
			SocialNetworkValue.ValueChanged += new ValueChangedHandler(SocialNetworkValue_ValueChanged);
		}

		// Executes when the user navigates to this page.
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			if (CharacterEditor.CheckEditingCharacter(this.NavigationService.Navigate))
			{
				LoadData();
			}
		}

		private void AcceptButton_Click(object sender, RoutedEventArgs e)
		{
			string error = CharacterEditor.AcceptEdit(txtName.Text, rbAnimation.IsChecked.Value, photoImageBytes, cbAnimationAvatar.SelectedValue.ToString(), cbGender.SelectedValue.ToString(), cbAge.SelectedValue.ToString(), ActivityValue.Value, QualityValue.Value, PassivityValue.Value, SocialNetworkValue.Value, this.NavigationService.Navigate);

			if (error != null)
			{
				MessageBox.Show(error);
			}
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			CharacterEditor.CancelEdit(this.NavigationService.Navigate);
		}

		private void LoadData()
		{
			txtName.Text = CharacterEditor.EditingCharacter.Name ?? "";

			rbPhoto.IsChecked = !CharacterEditor.EditingCharacter.UseAnimatedAvatar;
			rbAnimation.IsChecked = CharacterEditor.EditingCharacter.UseAnimatedAvatar;

			photoImageBytes = CharacterEditor.EditingCharacter.PhotoAvatar;

			if (CharacterEditor.EditingCharacter.PhotoAvatar != null)
			{
				iPhoto.Source = CharacterEditor.EditingCharacter.PhotoAvatarImage;
			}

			cbGender.ItemsSource = Genders.GetValues();
			cbGender.SelectedValue = CharacterEditor.EditingCharacter.Gender.ToString();

			cbAge.ItemsSource = Ages.GetValues();
			cbAge.SelectedValue = CharacterEditor.EditingCharacter.Age.ToString();

			ReloadAvatars();

			cbAnimationAvatar.SelectedValue = CharacterEditor.EditingCharacter.AnimationAvatar.ToString();

			ActivityValue.Value = CharacterEditor.EditingCharacter.Activity;
			QualityValue.Value = CharacterEditor.EditingCharacter.Quality;
            PassivityValue.Value = CharacterEditor.EditingCharacter.Passivity;
            SocialNetworkValue.Value = CharacterEditor.EditingCharacter.SocialNetwork;

			UpdateWeights();

			UpdatePunctuation();
		}

		private void ReloadAvatars()
		{
			string current = (cbAnimationAvatar.SelectedValue != null ? cbAnimationAvatar.SelectedValue.ToString() : "");

			string[] avatars = AnimationAvatars.GetValues(cbGender.SelectedValue.ToString());

			cbAnimationAvatar.ItemsSource = avatars;

			if (avatars.Contains(current))
			{
				cbAnimationAvatar.SelectedValue = current;
			}
			else
			{
				cbAnimationAvatar.SelectedIndex = 0;
			}
		}

		private void ChoosePhotoButton_Click(object sender, RoutedEventArgs e)
		{
			// Create an instance of the open file dialog box.
			OpenFileDialog openFileDialog1 = new OpenFileDialog();

			// Set filter options and filter index.
			openFileDialog1.Filter = "Image Files (jpg or png)|*.jpg;*.png";
			openFileDialog1.FilterIndex = 1;

			openFileDialog1.Multiselect = false;

			// Call the ShowDialog method to show the dialog box.
			bool? userClickedOK = openFileDialog1.ShowDialog();

			// Process input if the user clicked OK.
			if (userClickedOK == true)
			{
				// Open the selected file to read.
				System.IO.Stream fileStream = openFileDialog1.File.OpenRead();

				if (fileStream.Length > (2 * 1024 * 1024))
				{
					MessageBox.Show("Image is too big!! Please select an image no greater than 2Mb.");
					return;
				}

				byte[] imageBytes = new byte[fileStream.Length];

				fileStream.Read(imageBytes, 0, (int)fileStream.Length);

				fileStream.Close();

				photoImageBytes = imageBytes;

				iPhoto.Source = imageBytes.ToBitmapImage();
			}
		}

		private void rbPhoto_Checked(object sender, RoutedEventArgs e)
		{
			PhotoPanel.Visibility = Visibility.Visible;
			AnimationPanel.Visibility = Visibility.Collapsed;
		}

		private void rbAnimation_Checked(object sender, RoutedEventArgs e)
		{
			PhotoPanel.Visibility = Visibility.Collapsed;
			AnimationPanel.Visibility = Visibility.Visible;
		}

		private void cbGender_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ReloadAvatars();
		}

		private void ActivityValue_ValueChanged()
		{
			ActivityColor.Fill = new SolidColorBrush(Character.ActivityValueToColor(ActivityValue.Value).ToColor());

			UpdatePunctuation();
		}

		private void QualityValue_ValueChanged()
		{
			QualityColor.Fill = new SolidColorBrush(Character.QualityValueToColor(QualityValue.Value).ToColor());

			UpdatePunctuation();
		}

		private void PassivityValue_ValueChanged()
		{
			PassivityColor.Fill = new SolidColorBrush(Character.PassivityValueToColor(PassivityValue.Value).ToColor());

			UpdatePunctuation();
		}

        private void SocialNetworkValue_ValueChanged()
		{
            SocialNetworkColor.Fill = new SolidColorBrush(Character.SocialNetworkValueToColor(SocialNetworkValue.Value).ToColor());

			//UpdatePunctuation();
		}
        
		private void UpdatePunctuation()
		{
			double? p = Character.IndicatorsToPunctuation(ActivityValue.Value, QualityValue.Value, PassivityValue.Value, SocialNetworkValue.Value);

			if (p != null)
			{
				PunctuationValue.Text = Math.Round(p.Value, 1).ToString();
			}
		}

		private void UpdateWeights()
		{
			ActivityWeight.Text = Character.ActivityWeight.ToString();
			QualityWeight.Text = Character.QualityWeight.ToString();
            PassivityWeight.Text = Character.PassivityWeight.ToString();
            SocialNetworkWeight.Text = Character.SocialNetworkWeight.ToString();
        }

		private void ChangeWeightsButton_Click(object sender, RoutedEventArgs e)
		{
			ActivityWeightValue.Value = Character.ActivityWeight;
			QualityWeightValue.Value = Character.QualityWeight;
            PassivityWeightValue.Value = Character.PassivityWeight;
            SocialNetworkWeightValue.Value = Character.SocialNetworkWeight;

			WeightsEditPanel.Visibility = Visibility.Visible;
		}

		private void WeightsEditAcceptButton_Click(object sender, RoutedEventArgs e)
		{
            string error = CharacterEditor.ChangeWeights(ActivityWeightValue.Value, QualityWeightValue.Value, PassivityWeightValue.Value, SocialNetworkWeightValue.Value);

			if (error != null)
			{
				MessageBox.Show(error);
			}
			else
			{
				UpdatePunctuation();

				UpdateWeights();

				ClearWeightsEditPanel();
			}
		}

		private void WeightsEditCancelButton_Click(object sender, RoutedEventArgs e)
		{
			ClearWeightsEditPanel();
		}

		private void ClearWeightsEditPanel()
		{
			ActivityWeightValue.Value = null;
			QualityWeightValue.Value = null;
            PassivityWeightValue.Value = null;
            SocialNetworkWeightValue.Value = null;

			WeightsEditPanel.Visibility = Visibility.Collapsed;
		}
	}
}
