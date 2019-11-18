using System;
using System.Windows;
using MolkomRecruitmentTask.ViewModels;
using MolkomRecruitmentTask.Models;

namespace MolkomRecruitmentTask.Views
{
    /// <summary>
    /// Widok edycji miasta
    /// </summary>
    public partial class CityEditWindow : Window
    {
        /// <summary>
        /// Konstruktor uruchamiany w celu dodania nowego wpisu
        /// </summary>
        public CityEditWindow(string connectionString)
        {
            
            try
            {
                InitializeComponent();
                CityEditViewModel viewModel = new CityEditViewModel(connectionString); //ViewModel z konstruktorem dla nowego wpisu
                this.DataContext = viewModel; //przypisanie logiki widoku do odpowiedniej klasy ViewModel
                Closing += viewModel.OnWindowClosing; //przypisanie metody wywołanej po zamknięciu okna
                Loaded += (s, e) =>
                {
                    if (DataContext is ICloseable)
                    {
                        (DataContext as ICloseable).RequestClose += (_, __) => this.Close();
                    }
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            
        }

        /// <summary>
        /// Konstruktor uruchamiany w celu edycji istniejącego wpisu
        /// </summary>
        public CityEditWindow(Cities entity, string connectionString)
        {
            
            try
            {
                InitializeComponent();
                CityEditViewModel viewModel = new CityEditViewModel(entity, connectionString); //ViewModel z konstruktorem dla istniejącego wpisu
                this.DataContext = viewModel; //przypisanie logiki widoku do odpowiedniej klasy ViewModel
                Closing += viewModel.OnWindowClosing; //przypisanie metody wywołanej po zamknięciu okna
                Loaded += (s, e) =>
                {
                    if (DataContext is ICloseable)
                    {
                        (DataContext as ICloseable).RequestClose += (_, __) => this.Close();
                    }
                };

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
    }
}
