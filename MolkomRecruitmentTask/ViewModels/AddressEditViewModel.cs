using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MolkomRecruitmentTask.Models;

namespace MolkomRecruitmentTask.ViewModels
{
    class AddressEditViewModel : ViewModelBase, ICloseable
    {
        public event EventHandler<EventArgs> RequestClose;

        /// <summary> Efekt kliknięcia przycisku "Zapisz"  </summary>
        public ICommand SaveButtonClick { get; set; }
        /// <summary> identyfikator edytowanego elementu. Dotyczy tylko edycji istniejącego wpisu. </summary>
        private int idOfEntity;

        #region Bindings

        /// <summary> zawartość listy rozwijanej miast. Zawiera nazwę i kod pocztwoy </summary>
        public ObservableCollection<string> CitiesComboBoxContent { get; set; }
        /// <summary> zawartość listy rozwijanej miesięcy </summary>
        public ObservableCollection<string> MonthOfBirthComboBoxContent { get; set; }

        private string _citySelectedItem;
        /// <summary> wybrane miasto z listy rozwijanej </summary>
        public string CitySelectedItem
        {
            get
            {
                return _citySelectedItem;
            }
            set
            {
                _citySelectedItem = value;
                OnPropertyChanged("CitySelectedItem");
            }
        }

        private string _citySelectedValue;
        /// <summary> wybrana wartość z rozwijanej miast </summary>
        public string CitySelectedValue
        {
            get
            {
                return _citySelectedValue;
            }
            set
            {
                _citySelectedValue = value;
                OnPropertyChanged("CitySelectedValue");
            }
        }

        private string _nameTextBoxContent;
        /// <summary> TextBox z imieniem </summary>
        public string NameTextBoxContent
        {
            get
            {
                return _nameTextBoxContent;
            }
            set
            {
                _nameTextBoxContent = value;
                OnPropertyChanged("NameTextBoxContent");
            }
        }

        private string _surnameTextBoxContent;
        /// <summary> TextBox z nazwiskiem </summary>
        public string SurnameTextBoxContent
        {
            get
            {
                return _surnameTextBoxContent;
            }
            set
            {
                _surnameTextBoxContent = value;
                OnPropertyChanged("SurnameTextBoxContent");
            }
        }

        private string _phoneTextBoxContent;
        /// <summary> TextBox z numerem telefonu </summary>
        public string PhoneTextBoxContent
        {
            get
            {
                return _phoneTextBoxContent;
            }
            set
            {
                _phoneTextBoxContent = value;
                OnPropertyChanged("PhoneTextBoxContent");
            }
        }

        private string _dayOfBirthTextBoxContent;
        /// <summary> TextBox z dniem urodzin. Przyjmuje tylko liczby z zakresu 1-31 </summary>
        public string DayOfBirthTextBoxContent
        {
            get
            {
                return _dayOfBirthTextBoxContent;
            }
            set
            {
                if (GlobalFunctions.IsNumeric(value) && Int32.Parse(value) > 0 && Int32.Parse(value) <= 31)
                {
                    _dayOfBirthTextBoxContent = value;
                    OnPropertyChanged("DayOfBirthTextBoxContent");
                }
                else { MessageBox.Show("Proszę wprowadzić liczbę z zakresu 1-31"); }
            }
        }

        private int _monthOfBirthSelectedIndex;
        /// <summary> Index elementu wybranego z listy rozwijanej miesięcy </summary>
        public int MonthOfBirthSelectedIndex
        {
            get
            {
                return _monthOfBirthSelectedIndex;
            }
            set
            {
                _monthOfBirthSelectedIndex = value;
                OnPropertyChanged("MonthOfBirthSelectedIndex");
            }
        }

        private string _yearOfBirthTextBoxContent;
        /// <summary> TextBox z rokiem urodzin. </summary>
        public string YearOfBirthTextBoxContent
        {
            get
            {
                return _yearOfBirthTextBoxContent;
            }
            set
            {
                if (GlobalFunctions.IsNumeric(value) && Int32.Parse(value) > 0)
                {
                    _yearOfBirthTextBoxContent = value;
                    OnPropertyChanged("YearOfBirthTextBoxContent");
                }
                else { MessageBox.Show("Proszę wprowadzić liczbę"); }
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

        private Boolean _status;
        /// <summary> Checkbox ze statusem </summary>
        public Boolean Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }

        #endregion

        /// <summary> Konstruktor uruchamiany w celu dodania nowego wpisu </summary>
        public AddressEditViewModel(string connectionString)
        {
            this.SaveButtonClick = new GalaSoft.MvvmLight.Command.RelayCommand(() =>
            {
                try
                {
                    if (this.NameTextBoxContent == null || this.SurnameTextBoxContent == null) { MessageBox.Show("Wypełnij pole z imieniem i nazwiskiem"); }
                    else
                    {
                        using (var ctx = new molkomEntities())
                        {
                            ctx.Addresses.Add(GetAddressObject());
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

            this.Status = true;
            this.DateOfCreation = System.DateTime.Now;
            this.DateOfUpdate = System.DateTime.Now;
            this.CitiesComboBoxContent = new ObservableCollection<string>();
            this.MonthOfBirthComboBoxContent = new ObservableCollection<string>();
            SetDefaultDateFormat();
            LoadMonthsComboBoxContent();
            LoadCitiesComboBoxContent(connectionString);
        }

        /// <summary> Konstruktor uruchamiany w celu edycji istniejącego wpisu </summary>
        public AddressEditViewModel(Addresses entity, string connectionString)
        {
            this.idOfEntity = entity.id;
            this.CitiesComboBoxContent = new ObservableCollection<string>();
            this.MonthOfBirthComboBoxContent = new ObservableCollection<string>();
            SetDefaultDateFormat();
            LoadMonthsComboBoxContent();
            LoadCitiesComboBoxContent(connectionString);
            LoadData(entity);

            this.SaveButtonClick = new GalaSoft.MvvmLight.Command.RelayCommand(() =>
            {
                try
                {
                    if (this.NameTextBoxContent == null || this.SurnameTextBoxContent == null) { MessageBox.Show("Wypełnij pole z imieniem i nazwiskiem"); }
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
        public void SetDefaultDateFormat()
        {
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy HH:mm";
            System.Threading.Thread.CurrentThread.CurrentCulture = ci;
        }

        /// <summary> Utworzenie obiektu adresu na podstawie wartości kontrolek widoku. Pobranie obiektu. </summary>
        Addresses GetAddressObject()
        {
            Addresses newAddress = new Addresses();
            newAddress.name = this.NameTextBoxContent;
            newAddress.surname = this.SurnameTextBoxContent;
            newAddress.birthdate = new DateTime(Int32.Parse(this.YearOfBirthTextBoxContent), this.MonthOfBirthSelectedIndex +1, Int32.Parse(this.DayOfBirthTextBoxContent));
            newAddress.phoneNumber = this.PhoneTextBoxContent;
            newAddress.status = this.Status;
            if (GetIdOfSelectedCityItem() != 0) { newAddress.city = GetIdOfSelectedCityItem(); }
            newAddress.dateOfCreation = System.DateTime.Now;
            newAddress.dateOfUpdate = System.DateTime.Now;

            return newAddress;
        }

        /// <summary> Ustawienie kontrolek widoku na podstawie wartości pól edytowanego elementu </summary>
        private void LoadData(Addresses entity)
        {
            try
            {

                if (entity.name != null) { this.NameTextBoxContent = entity.name; } else { this.NameTextBoxContent = ""; }
                if (entity.surname != null) { this.SurnameTextBoxContent = entity.surname; } else { this.SurnameTextBoxContent = ""; }
                if (entity.phoneNumber != null) { this.PhoneTextBoxContent = entity.phoneNumber; } else { this.PhoneTextBoxContent = ""; }
                if (entity.status != null) { this.Status = Convert.ToBoolean(entity.status); } else { this.Status = false; }
                if (entity.birthdate != null)
                {

                    this.DayOfBirthTextBoxContent = Convert.ToDateTime(entity.birthdate).Day.ToString();
                    this.MonthOfBirthSelectedIndex = Convert.ToDateTime(entity.birthdate).Month - 1;
                    this.YearOfBirthTextBoxContent = Convert.ToDateTime(entity.birthdate).Year.ToString();
                }

                if (entity.city != null)
                {
                    using (var ctx = new molkomEntities())
                    {
                        var city = ctx.Cities.FirstOrDefault(x => x.id == entity.city);

                        if (city != null)
                        {
                            this.CitiesComboBoxContent.Add(city.postalCode+", " + city.name);
                            this.CitySelectedItem = city.postalCode + ", " + city.name;
                            this.CitySelectedValue = city.postalCode + ", " + city.name;
                        }
                    }
                }
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
                    var address = ctx.Addresses.FirstOrDefault(x => x.id == this.idOfEntity);

                    if (address != null)
                    {
                        address.name = this.NameTextBoxContent;
                        address.surname = this.SurnameTextBoxContent;
                        address.phoneNumber = this.PhoneTextBoxContent;
                        address.birthdate = new DateTime(Int32.Parse(this.YearOfBirthTextBoxContent), this.MonthOfBirthSelectedIndex + 1, Int32.Parse(this.DayOfBirthTextBoxContent));
                        address.status = this.Status;
                        if (GetIdOfSelectedCityItem() != 0) { address.city = GetIdOfSelectedCityItem(); }
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

        /// <summary> Uzupełnienie wartościami listy rozwijanej miast </summary>
        private void LoadCitiesComboBoxContent(string connectionString)  //połączyć po przecinku postalCode i nazwę a póżniej splitować
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand("SELECT postalCode, name FROM dbo.Cities ORDER BY postalCode", connection);
                SqlDataReader reader = adapter.SelectCommand.ExecuteReader();

                while (reader.Read())
                {
                    string content;
                    if (reader["postalCode"] != DBNull.Value || reader["name"] != DBNull.Value) { content = (string)reader["postalCode"] + ", " + (string)reader["name"]; } else { content = "-"; }
                    this.CitiesComboBoxContent.Add(content);
                }
                this.CitiesComboBoxContent.Add("");
                reader.Close();
                connection.Close();
                OnPropertyChanged("CitiesComboBoxContent");
            }
        }

        /// <summary> Uzupełnienie wartościami listy rozwijanej miesięcy </summary>
        private void LoadMonthsComboBoxContent()
        {
            this.MonthOfBirthComboBoxContent.Add("Styczeń");
            this.MonthOfBirthComboBoxContent.Add("Luty");
            this.MonthOfBirthComboBoxContent.Add("Marzec");
            this.MonthOfBirthComboBoxContent.Add("Kwiecień");
            this.MonthOfBirthComboBoxContent.Add("Maj");
            this.MonthOfBirthComboBoxContent.Add("Czerwiec");
            this.MonthOfBirthComboBoxContent.Add("Lipiec");
            this.MonthOfBirthComboBoxContent.Add("Sierpień");
            this.MonthOfBirthComboBoxContent.Add("Wrzesień");
            this.MonthOfBirthComboBoxContent.Add("Październik");
            this.MonthOfBirthComboBoxContent.Add("Listopad");
            this.MonthOfBirthComboBoxContent.Add("Grudzień");
            this.MonthOfBirthComboBoxContent.Add("");
            OnPropertyChanged("MonthOfBirthComboBoxContent");
            this.MonthOfBirthSelectedIndex = 12;
        }

        /// <summary> Pobranie identyfikatora miasta z listy rozwijanej </summary>
        private int GetIdOfSelectedCityItem()
        {
            int id = 0;

            if (this.CitySelectedItem != null && this.CitySelectedItem != "")
            {
                string postalCode = this.CitySelectedItem.Split(',')[0].ToString();
                using (var ctx = new molkomEntities())
                {
                    var _city = ctx.Cities.FirstOrDefault(x => x.postalCode.Equals(postalCode));

                    if (_city != null)
                    {
                        id = _city.id;
                    }
                }
            }
            return id;
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