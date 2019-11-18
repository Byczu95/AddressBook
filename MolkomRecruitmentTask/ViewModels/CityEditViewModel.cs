using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MolkomRecruitmentTask.Models;

namespace MolkomRecruitmentTask.ViewModels
{
    class CityEditViewModel : ViewModelBase, ICloseable
    {
        public event EventHandler<EventArgs> RequestClose;

        /// <summary> Efekt kliknięcia przycisku "Zapisz"  </summary>
        public ICommand SaveButtonClick { get; set; }
        /// <summary> identyfikator edytowanego elementu. Dotyczy tylko edycji istniejącego wpisu. </summary>
        private int idOfEntity;

        #region Bindings

        private string _postalCodeTextBoxContent;
        /// <summary> TextBox z kodem pocztowym </summary>
        public string PostalCodeTextBoxContent
        {
            get
            {
                return _postalCodeTextBoxContent;
            }
            set
            {
                _postalCodeTextBoxContent = value;
                OnPropertyChanged("PostalCodeTextBoxContent");
            }
        }

        private string _cityNameTextBoxContent;
        /// <summary> TextBox z nazwą miejscowości </summary>
        public string CityNameTextBoxContent
        {
            get
            {
                return _cityNameTextBoxContent;
            }
            set
            {
                _cityNameTextBoxContent = value;
                OnPropertyChanged("CityNameTextBoxContent");
            }
        }

        private DateTime _dateOfCreation;
        /// <summary> Data utworzenia wpisu </summary>
        public DateTime DateOfCreation
        {
            get
            {
                return _dateOfCreation;
            }
            set
            {
                _dateOfCreation = value;
                OnPropertyChanged("DateOfCreation");
            }
        }

        private DateTime _dateOfUpdate;
        /// <summary> Data aktualizacji wpisu </summary>
        public DateTime DateOfUpdate
        {
            get
            {
                return _dateOfUpdate;
            }
            set
            {
                _dateOfUpdate = value;
                OnPropertyChanged("DateOfUpdate");
            }
        }

        #endregion

        /// <summary> Konstruktor uruchamiany w celu dodania nowego wpisu </summary>
        public CityEditViewModel(string connectionString)
        {
            this.SaveButtonClick = new GalaSoft.MvvmLight.Command.RelayCommand(() =>
            {
                try
                {
                    if (this.PostalCodeTextBoxContent == null || this.CityNameTextBoxContent == null) { MessageBox.Show("Wypełnij pole z kodem pocztowym i nazwą"); }
                    else
                    {
                        using (var ctx = new molkomEntities())
                        {
                            ctx.Cities.Add(GetCityObject());
                            ctx.SaveChanges();
                            GlobalVariables.refreshMainWindowFlag = 1;
                            RequestClose?.Invoke(this, EventArgs.Empty); 
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            });

            SetDefaultDateFormat();
        }

        /// <summary> Konstruktor uruchamiany w celu edycji istniejącego wpisu </summary>
        public CityEditViewModel(Cities entity, string connectionString)
        {
            this.idOfEntity = entity.id;
            SetDefaultDateFormat();
            LoadData(entity);

            this.SaveButtonClick = new GalaSoft.MvvmLight.Command.RelayCommand(() =>
            {
                try
                {
                    if (this.PostalCodeTextBoxContent == null || this.CityNameTextBoxContent == null) { MessageBox.Show("Wypełnij pole z kodem pocztowym i nazwą"); }
                    else
                    {
                        using (var ctx = new molkomEntities())
                        {
                            SaveEntry();
                            GlobalVariables.refreshMainWindowFlag = 1;
                            RequestClose?.Invoke(this, EventArgs.Empty);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            });
        }

        /// <summary> Ustawienie formatu kontrolek wyświetlających czas </summary>
        private void SetDefaultDateFormat()
        {
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy HH:mm";
            System.Threading.Thread.CurrentThread.CurrentCulture = ci;
        }

        /// <summary> Utworzenie obiektu miasta na podstawie wartości kontrolek widoku. Pobranie obiektu. </summary>
        private Cities GetCityObject()
        {
            Cities newAddress = new Cities
            {
                postalCode = PostalCodeTextBoxContent,
                name = CityNameTextBoxContent,
                dateOfCreation = System.DateTime.Now,
                dateOfUpdate = System.DateTime.Now
            };

            return newAddress;
        }

        /// <summary> Ustawienie kontrolek widoku na podstawie wartości pól edytowanego elementu </summary>
        private void LoadData(Cities entity)
        {
            try
            {

                if (entity.postalCode != null) { this.PostalCodeTextBoxContent = entity.postalCode; } else { this.PostalCodeTextBoxContent = ""; }
                if (entity.name != null) { this.CityNameTextBoxContent = entity.name; } else { this.CityNameTextBoxContent = ""; }

                if (entity.dateOfCreation != null) { this.DateOfCreation = Convert.ToDateTime(entity.dateOfCreation); } else { this.DateOfCreation = DateTime.Now.ToLocalTime(); }
                if (entity.dateOfUpdate != null) { this.DateOfUpdate = Convert.ToDateTime(entity.dateOfUpdate); } else { this.DateOfUpdate = DateTime.Now.ToLocalTime(); }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary> Zapisanie zmian edytowanego elementu do bazy danych </summary>
        public void SaveEntry()
        {
            try
            {
                using (var ctx = new molkomEntities())
                {
                    var address = ctx.Cities.FirstOrDefault(x => x.id == this.idOfEntity);

                    if (address != null)
                    {
                        address.postalCode = PostalCodeTextBoxContent;
                        address.name = CityNameTextBoxContent;
                        address.dateOfCreation = System.DateTime.Now;
                        address.dateOfUpdate = System.DateTime.Now;

                        ctx.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary> Ustawienie wartości flagi odświeżenia listy po zamknięciu okna widoku </summary>
        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            if (GlobalVariables.refreshMainWindowFlag != 0) { }
            else
            {
                GlobalVariables.refreshMainWindowFlag = 2;
            }
        }
    
    }
}
