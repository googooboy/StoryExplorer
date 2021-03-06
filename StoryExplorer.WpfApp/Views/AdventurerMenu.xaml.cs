﻿using System;
using System.Windows;
using System.Windows.Media;
using StoryExplorer.DataModel;

namespace StoryExplorer.WpfApp
{
	/// <summary>
	/// Interaction logic for AdventurerMenu.xaml
	/// </summary>
	public partial class AdventurerMenu : Window
	{
		private MainWindow mainWindow;
		private bool goBack = false;

		public AdventurerMenu()
		{
			InitializeComponent();
		}

		public AdventurerMenu(MainWindow previous, Adventurer adventurer) : this()
		{
			mainWindow = previous;

			var viewModel = (AdventurerMenuViewModel)DataContext;
			viewModel.SelectedAdventurer = adventurer;

			adventurerName.Content = adventurer.Name;
			labelGender.Content = adventurer.Gender;
			labelHairColor.Content = adventurer.HairColor;
			labelHairStyle.Content = adventurer.HairStyle;
			labelSkinColor.Content = adventurer.SkinColor;
			labelEyeColor.Content = adventurer.EyeColor;
			labelPersonality.Content = adventurer.Personality;
			labelHeight.Content = adventurer.Height;
			labelCurrentRegion.Content = adventurer.CurrentRegion?.Name ?? "None";
			labelCreatedDate.Content = adventurer.Created.ToShortDateString();

			if (adventurer.Gender == Gender.Female)
				background.ImageSource = new ImageSourceConverter().ConvertFromString("pack://application:,,,/StoryExplorer.WpfApp;component/Images/silvan-tracker.jpg") as ImageSource;
		}

		private void Window_Closed(object sender, EventArgs e)
		{
			if (goBack)
			{
				mainWindow.Show();
			}
			else
			{
				mainWindow.Close();
			}
		}

		private void back_Click(object sender, RoutedEventArgs e)
		{
			goBack = true;
			Close();
		}

		private void changePassword_Click(object sender, RoutedEventArgs e)
		{
			changePasswordPanel.Visibility = Visibility.Visible;
			changePassword.IsEnabled = false;
			delete.IsEnabled = false;
			chooseRegion.IsEnabled = false;
			back.IsEnabled = false;
		}

		private void delete_Click(object sender, RoutedEventArgs e)
		{
			if (MessageBox.Show("Are you sure you want to delete this adventurer permanently?", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
			{
				var viewModel = (AdventurerMenuViewModel)DataContext;
				viewModel.SelectedAdventurer.Delete();
				mainWindow.refreshAdventurers();
				mainWindow.Show();
				goBack = true;
				Close();
			}
		}

		private void chooseRegion_Click(object sender, RoutedEventArgs e)
		{
			var viewModel = (AdventurerMenuViewModel)DataContext;
			Hide();
			var regionEntryWindow = new RegionEntry(this, viewModel.SelectedAdventurer);
			regionEntryWindow.Show();
		}

		private void checkPasswordFields()
		{
			var viewModel = (AdventurerMenuViewModel)DataContext;

			if (oldPassword.Password == viewModel.SelectedAdventurer.Password &&
				newPassword.Password == confirmPassword.Password)
			{
				commitPassword.IsEnabled = true;
			}
			else
			{
				commitPassword.IsEnabled = false;
			}
		}

		private void oldPassword_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
		{
			checkPasswordFields();
		}

		private void newPassword_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
		{
			checkPasswordFields();
		}

		private void confirmPassword_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
		{
			checkPasswordFields();
		}

		private void hideChangePasswordControls()
		{
			changePasswordPanel.Visibility = Visibility.Hidden;
			changePassword.IsEnabled = true;
			delete.IsEnabled = true;
			chooseRegion.IsEnabled = true;
			back.IsEnabled = true;
		}

		private void commitPassword_Click(object sender, RoutedEventArgs e)
		{
			var viewModel = (AdventurerMenuViewModel)DataContext;
			viewModel.SelectedAdventurer.Password = newPassword.Password;
			viewModel.SelectedAdventurer.Save();
			hideChangePasswordControls();
		}

		private void cancelPassword_Click(object sender, RoutedEventArgs e)
		{
			hideChangePasswordControls();
		}
	}
}
