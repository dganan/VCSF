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
	public partial class CreateNewScenePage : Page
	{
		public CreateNewScenePage()
		{
			InitializeComponent();
		}

		// Executes when the user navigates to this page.
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			if (StoryBoardEditor.CheckEditingSLO(this.NavigationService.Navigate))
			{
				cbType.ItemsSource = SceneTypes.GetValues();
				cbType.SelectedIndex = 0;

				txtName.Focus();
			}
		}

		private void AcceptButton_Click(object sender, RoutedEventArgs e)
		{
			string error = StoryBoardEditor.CreateScene(txtName.Text, cbType.SelectedValue.ToString(), this.NavigationService.Navigate);

			if (error != null)
			{
				MessageBox.Show(error);
			}
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			this.NavigationService.Navigate(Pages.StoryBoardEditorPage);
		}
	}
}
