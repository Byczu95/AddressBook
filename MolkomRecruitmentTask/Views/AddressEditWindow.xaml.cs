using System;
using System.Windows;
using MolkomRecruitmentTask.ViewModels;
using MolkomRecruitmentTask.Models;

namespace MolkomRecruitmentTask.Views
{
    /// <summary>
    /// Widok edycji adresu
    /// </summary>
    public partial class AddressEditWindow : Window
    {
        /// <summary>
        /// Konstruktor uruchamiany w celu dodania nowego wpisu
        /// </summary>
        public AddressEditWindow(string connectionString)
        {
            try
            {
                InitializeComponent();
                AddressEditViewModel viewModel = new AddressEditViewModel(connectionString); //ViewModel z konstruktorem dla nowego wpisu
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
        public AddressEditWindow(Addresses entity, string connectionString)
        {
            try
            {
                InitializeComponent();
                AddressEditViewModel viewModel = new AddressEditViewModel(entity, connectionString);//ViewModel z konstruktorem dla istniejącego wpisu
                this.DataContext = viewModel;//przypisanie logiki widoku do odpowiedniej klasy ViewModel
                Closing += viewModel.OnWindowClosing;//przypisanie metody wywołanej po zamknięciu okna
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
