using System;
using System.Windows;
using MolkomRecruitmentTask.ViewModels;
using MolkomRecruitmentTask.Models;

namespace MolkomRecruitmentTask.Views
{
    /// <summary>
    /// Widok menu głównego
    /// </summary>
    public partial class MenuWindow : Window
    {
        public MenuWindow()
        {
            try
            {
                InitializeComponent();
                MenuViewModel viewModel = new MenuViewModel();
                this.DataContext = viewModel;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}


