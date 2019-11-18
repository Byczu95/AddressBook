using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using MolkomRecruitmentTask.Commands;
using MolkomRecruitmentTask.Models;
using MolkomRecruitmentTask.Views;

namespace MolkomRecruitmentTask.ViewModels
{
    class MenuViewModel : ViewModelBase
    {
        /// <summary> Przycisk przejścia do listy adresów  </summary>
        public ICommand AdressBookButtonClick { get; set; }
        /// <summary> Przycisk przejścia do listy miast  </summary>
        public ICommand CitiesButtonClick { get; set; }
        /// <summary> Przycisk "Wyszukaj"  </summary>
        public ICommand SearchButtonClick { get; set; }
        /// <summary> Przycisk dodania nowego wpisu  </summary>
        public ICommand AddButtonClick { get; set; }
        /// <summary> Przycisk edycji istniejącego wpisu  </summary>
        public ICommand EditButtonClick { get; set; }
        /// <summary> Przycisk usunięcia istniejącego wpisu  </summary>
        public ICommand DeleteButtonClick { get; set; }

        /// <summary> Wartość connection stringa wykorzystywanego do połączenia z lokalną bazą danych  </summary>
        public string connectionString;

        /// <summary> Nazwa obecnie wyświetlanej w menu tabeli  </summary>
        public string currentTableName;
        /// <summary> Identyfikator wybranego obiektu. Pole "Id" z bazy danych.  </summary>
        private int idOfEditedItem;

        /// <summary> Tabela zawierająca elementy pobrane z bazy danych </summary>
        private DataTable dataTable;
        /// <summary> Widok, źródło listy elementów, które wyświetlić w menu głównym </summary>
        public DataView DisplaySource { get; set; }

        /// <summary> zawartość listy rozwijanej. Zawiera nazwy kolumn z listy z menu głównego, poza lp i id </summary>
        public ObservableCollection<string> SortComboBoxContent { get; set; }
        
        private int _sortSelectedIndex;
        /// <summary> Index elementu wybranego z listy rozwijanej kolumn, po której sortować tabelę </summary>
        public int SortSelectedIndex
        {
            get
            {
                return _sortSelectedIndex;
            }
            set
            {
                string column = "Column" + (value + 3).ToString(); //dodanie 3, ponieważ dodając wiersze do listy rozwijanej pomijam dwie pierwsze kolumny(Lp i Id)
                int newLp = 1;
                dataTable.DefaultView.Sort = column + " ASC";
                dataTable = dataTable.DefaultView.ToTable();
                foreach (DataRow elem in dataTable.DefaultView.Table.Rows)
                {
                    elem["Column1"] = newLp; //nadpisanie liczby porządkowej na nowo
                    newLp++;
                }
                DisplaySource = dataTable.DefaultView;
                _sortSelectedIndex = value;
                OnPropertyChanged("SortSelectedIndex");
                OnPropertyChanged("DisplaySource");
            }
        }

        private int _searchSelectedIndex;
        /// <summary> Index elementu wybranego z listy rozwijanej kolumn, po której filtrować tabelę </summary>
        public int SearchSelectedIndex
        {
            get
            {
                return _searchSelectedIndex;
            }
            set
            {
                _searchSelectedIndex = value;
                OnPropertyChanged("SearchSelectedIndex");
            }
        }
        
        private string _searchTextBoxContent;
        /// <summary> TextBox z wartością do wyszukania w tabeli </summary>
        public string SearchTextBoxContent
        {
            get
            {
                return _searchTextBoxContent;
            }
            set
            {
                _searchTextBoxContent = value;
                OnPropertyChanged("SearchTextBoxContent");
            }
        }

        private DataRowView _selectedListViewItem = null;
        /// <summary> wybrany element z listy elementów widoku </summary>
        public DataRowView SelectedListViewItem
        {
            get
            {
                return _selectedListViewItem;
            }
            set
            {
                _selectedListViewItem = value;
                OnPropertyChanged("SelectedListViewItem");
            }
        }

        #region Headers

        private string _header1 = null;
        /// <summary> Nazwa 1 kolumny tabeli </summary>
        public string Header1
        {
            get
            {
                return _header1;
            }
            set
            {
                _header1 = value;
                OnPropertyChanged("Header1");
            }
        }

        private string _header2 = null;
        /// <summary> Nazwa 2 kolumny tabeli </summary>
        public string Header2
        {
            get
            {
                return _header2;
            }
            set
            {
                _header2 = value;
                OnPropertyChanged("Header2");
            }
        }
        private string _header3 = null;
        /// <summary> Nazwa 3 kolumny tabeli </summary>
        public string Header3
        {
            get
            {
                return _header3;
            }
            set
            {
                _header3 = value;
                OnPropertyChanged("Header3");
            }
        }

        private string _header4 = null;
        /// <summary> Nazwa 4 kolumny tabeli </summary>
        public string Header4
        {
            get
            {
                return _header4;
            }
            set
            {
                _header4 = value;
                OnPropertyChanged("Header4");
            }
        }

        private string _header5 = null;
        /// <summary> Nazwa 5 kolumny tabeli </summary>
        public string Header5
        {
            get
            {
                return _header5;
            }
            set
            {
                _header5 = value;
                OnPropertyChanged("Header5");
            }
        }

        private string _header6 = null;
        /// <summary> Nazwa 6 kolumny tabeli </summary>
        public string Header6
        {
            get
            {
                return _header6;
            }
            set
            {
                _header6 = value;
                OnPropertyChanged("Header6");
            }
        }

        private string _header7 = null;
        /// <summary> Nazwa 7 kolumny tabeli </summary>
        public string Header7
        {
            get
            {
                return _header7;
            }
            set
            {
                _header7 = value;
                OnPropertyChanged("Header7");
            }
        }

        private string _header8 = null;
        /// <summary> Nazwa 8 kolumny tabeli </summary>
        public string Header8
        {
            get
            {
                return _header8;
            }
            set
            {
                _header8 = value;
                OnPropertyChanged("Header8");
            }
        }

        private string _header9 = null;
        /// <summary> Nazwa 9 kolumny tabeli </summary>
        public string Header9
        {
            get
            {
                return _header9;
            }
            set
            {
                _header9 = value;
                OnPropertyChanged("Header9");
            }
        }
        #endregion

        #region WidthHeader

        private int _widthHeader1 = -1;
        /// <summary> Szerokość 1 kolumny tabeli </summary>
        public int WidthHeader1
        {
            get
            {
                return _widthHeader1;
            }
            set
            {
                _widthHeader1 = value;
                OnPropertyChanged("WidthHeader1");
            }
        }

        private int _widthHeader2 = -1;
        /// <summary> Szerokość 2 kolumny tabeli </summary>
        public int WidthHeader2
        {
            get
            {
                return _widthHeader2;
            }
            set
            {
                _widthHeader2 = value;
                OnPropertyChanged("WidthHeader2");
            }
        }

        private int _widthHeader3 = -1;
        /// <summary> Szerokość 3 kolumny tabeli </summary>
        public int WidthHeader3
        {
            get
            {
                return _widthHeader3;
            }
            set
            {
                _widthHeader3 = value;
                OnPropertyChanged("WidthHeader3");
            }
        }

        private int _widthHeader4 = -1;
        /// <summary> Szerokość 4 kolumny tabeli </summary>
        public int WidthHeader4
        {
            get
            {
                return _widthHeader4;
            }
            set
            {
                _widthHeader4 = value;
                OnPropertyChanged("WidthHeader4");
            }
        }

        private int _widthHeader5 = -1;
        /// <summary> Szerokość 5 kolumny tabeli </summary>
        public int WidthHeader5
        {
            get
            {
                return _widthHeader5;
            }
            set
            {
                _widthHeader5 = value;
                OnPropertyChanged("WidthHeader5");
            }
        }

        private int _widthHeader6 = -1;
        /// <summary> Szerokość 6 kolumny tabeli </summary>
        public int WidthHeader6
        {
            get
            {
                return _widthHeader6;
            }
            set
            {
                _widthHeader6 = value;
                OnPropertyChanged("WidthHeader6");
            }
        }

        private int _widthHeader7 = -1;
        /// <summary> Szerokość 7 kolumny tabeli </summary>
        public int WidthHeader7
        {
            get
            {
                return _widthHeader7;
            }
            set
            {
                _widthHeader7 = value;
                OnPropertyChanged("WidthHeader7");
            }
        }

        private int _widthHeader8 = -1;
        /// <summary> Szerokość 8 kolumny tabeli </summary>
        public int WidthHeader8
        {
            get
            {
                return _widthHeader8;
            }
            set
            {
                _widthHeader8 = value;
                OnPropertyChanged("WidthHeader8");
            }
        }

        private int _widthHeader9 = -1;
        /// <summary> Szerokość 9 kolumny tabeli </summary>
        public int WidthHeader9
        {
            get
            {
                return _widthHeader9;
            }
            set
            {
                _widthHeader9 = value;
                OnPropertyChanged("WidthHeader9");
            }
        }
        #endregion

        public MenuViewModel()
        {
            try
            {
                this.dataTable = new DataTable();
                this.SortComboBoxContent = new ObservableCollection<string>();
                this.currentTableName = "";
                this.connectionString = "Data Source = localhost; Database = molkom; Trusted_Connection=True;"; //połączenie do lokalnej bazy danych

                this.AdressBookButtonClick = new RelayCommand(pars => LoadAdressBookDisplayWindow());
                this.CitiesButtonClick = new RelayCommand(pars => LoadCitiesDisplayWindow());

                this.SearchButtonClick = new RelayCommand(pars => Search());
                this.AddButtonClick = new RelayCommand(pars => AddEntry());
                this.EditButtonClick = new RelayCommand(pars => EditEntry());
                this.DeleteButtonClick = new RelayCommand(pars => DeleteEntry());

                AddColumnsToDataTable(9); 
                SetColumnsWidth();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Menu\n" + ex.Message);
            }
        }

        /// <summary> ustawienei szerokości kolumn listy widoku </summary>
        public void SetColumnsWidth()
        {
            if (this.currentTableName.Equals("Addresses"))
            {
                this.WidthHeader1 = 60;
                this.WidthHeader2 = 0;
                this.WidthHeader3 = 300;
                this.WidthHeader4 = 150;
                this.WidthHeader5 = 150;
                this.WidthHeader6 = 150;
                this.WidthHeader7 = 70;
                this.WidthHeader8 = 150;
                this.WidthHeader9 = 250;
            }
            else
            {
                this.WidthHeader1 = 60;
                this.WidthHeader2 = 0;
                this.WidthHeader3 = 200;
                this.WidthHeader4 = 370;
                this.WidthHeader5 = this.WidthHeader6 = this.WidthHeader7 = this.WidthHeader8 = this.WidthHeader9 = 0;
            }
        }

        /// <summary> pobranie identyfikatora wybranego elementu listy widoku </summary>
        public int getIdOfSelectedItem()
        {
            return Int32.Parse(SelectedListViewItem.Row[1].ToString());
        }

        private int _indexBind = -1;
        /// <summary> Indeks wybranego elementu z listy widoku </summary>
        public int IndexBind
        {
            get
            {
                return _indexBind;
            }
            set
            {
                _indexBind = value;
                OnPropertyChanged("IndexBind");
            }
        }

        /// <summary> dodanie kolumn do tabeli elementów </summary>
        private void AddColumnsToDataTable(int count)
        {
            for (int i = 1; i <= count; i++)
            {
                this.dataTable.Columns.Add("Column" + i.ToString());
            }
        }

        /// <summary> ustawienie nazw kolumn listy widoku </summary>
        private void FillHeadersNames(string header1, string header2, string header3, string header4, string header5, string header6, string header7, string header8, string header9)
        {
            this.Header1 = header1;
            this.Header2 = header2;
            this.Header3 = header3;
            this.Header4 = header4;
            this.Header5 = header5;
            this.Header6 = header6;
            this.Header7 = header7;
            this.Header8 = header8;
            this.Header9 = header9;

            LoadSortComboBoxContent(header1, header2, header3, header4, header5, header6, header7, header8, header9);
        }

        /// <summary> dodanie wartości do listy rozwijanych dla sortowania i filtrowania </summary>
        private void LoadSortComboBoxContent(string header1, string header2, string header3, string header4, string header5, string header6, string header7, string header8, string header9)
        {
            this.SortComboBoxContent.Clear();
            string[] headers = new string[] { header1, header2, header3, header4, header5, header6, header7, header8, header9 };
            foreach (string header in headers)
            {
                if (!(header.Equals("") || header == null) && !header.Equals("Id") && !header.Equals("Lp"))
                {
                    SortComboBoxContent.Add(header);
                }
            }
            OnPropertyChanged("SortComboBoxContent");
        }

        /// <summary> Pobranie tabeli adresów z bazy danych i dodanie elementów do wyświetlanej listy elementów w widoku </summary>
        public void LoadAdressBookDisplayWindow()
        {
            try
            {
                this.currentTableName = "Addresses";
                SetColumnsWidth();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    this.dataTable.Clear();
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = new SqlCommand("SELECT A.*, C.postalCode, C.name as cityName FROM dbo." + "Addresses A " +
                                                            "Left join Cities C on  A.city = C.id " +
                                                            " ORDER BY A.surname, A.name", connection);

                    SqlDataReader reader = adapter.SelectCommand.ExecuteReader();
                    int counter = 0;

                    FillHeadersNames("Lp", "Id", "Nazwisko", "Imię", "Data urodzenia", "Numer telefonu", "Status", "Kod pocztowy", "Miejscowość");



                    while (reader.Read())
                    {
                        counter++;
                        string c2, c3, c4, c5, c6, c7, c8, c9;

                        if (reader["id"] != DBNull.Value) { c2 = ((int)reader["id"]).ToString(); } else { c2 = "-"; }
                        if (reader["surname"] != DBNull.Value) { c3 = (string)reader["surname"]; } else { c3 = "-"; }
                        if (reader["name"] != DBNull.Value) { c4 = (string)reader["name"]; } else { c4 = "-"; }
                        if (reader["birthdate"] != DBNull.Value)
                        {
                            c5 = Convert.ToDateTime(reader["birthdate"]).ToShortDateString();
                        }
                        else { c5 = "-"; }
                        if (reader["phoneNumber"] != DBNull.Value) { c6 = (string)reader["phoneNumber"]; ; } else { c6 = "-"; }
                        if (reader["status"] != DBNull.Value && (bool)reader["status"])
                        {
                            c7 = "A";
                        }
                        else { c7 = "N"; }
                        if (reader["postalCode"] != DBNull.Value) { c8 = (string)reader["postalCode"]; } else { c8 = "-"; }
                        if (reader["cityName"] != DBNull.Value) { c9 = (string)reader["cityName"]; } else { c9 = "-"; }

                        this.dataTable.Rows.Add(new object[] { counter, c2, c3, c4, c5, c6, c7, c8, c9 });
                    }

                    reader.Close();
                    connection.Close();
                    this.DisplaySource = this.dataTable.DefaultView;
                    OnPropertyChanged("DisplaySource");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary> Pobranie tabeli miast z bazy danych i dodanie elementów do wyświetlanej listy elementów w widoku </summary>
        public void LoadCitiesDisplayWindow()
        {
            try
            {
                this.currentTableName = "Cities";
                SetColumnsWidth();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    this.dataTable.Clear();
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = new SqlCommand("SELECT * FROM dbo." + "Cities " +
                                                            " ORDER BY name", connection);

                    SqlDataReader reader = adapter.SelectCommand.ExecuteReader();
                    int counter = 0;

                    FillHeadersNames("Lp", "Id", "Kod pocztowy", "Miejscowość", "", "", "", "", "");



                    while (reader.Read())
                    {
                        counter++;
                        string c2, c3, c4;
                        if (reader["id"] != DBNull.Value) { c2 = ((int)reader["id"]).ToString(); } else { c2 = "-"; }
                        if (reader["postalCode"] != DBNull.Value) { c3 = (string)reader["postalCode"]; } else { c3 = "-"; }
                        if (reader["name"] != DBNull.Value) { c4 = (string)reader["name"]; } else { c4 = "-"; }

                        dataTable.Rows.Add(new object[] { counter, c2, c3, c4, "", "", "", "", "" });
                    }

                    reader.Close();
                    connection.Close();
                    this.DisplaySource = this.dataTable.DefaultView;
                    OnPropertyChanged("DisplaySource");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary> Filtrowanie elementów tabeli na podstawie wybranej kolumny i wartości z textBoxa </summary>
        public void Search()
        {
            DataTable filteredTable = dataTable;
            string column = "Column" + (SearchSelectedIndex + 3).ToString();
            int newLp = 1;

            filteredTable.DefaultView.RowFilter = column + " LIKE '%" + SearchTextBoxContent + "%'";
            filteredTable = filteredTable.DefaultView.ToTable();
            foreach (DataRow elem in filteredTable.DefaultView.Table.Rows)
            {
                elem["Column1"] = newLp;
                newLp++;
            }
            this.DisplaySource = filteredTable.DefaultView;
            OnPropertyChanged("DisplaySource");
        }

        /// <summary> Uruchomienie nowego okna w celu dodania nowego elementu do bazy danych i listy </summary>
        public void AddEntry()
        {
            try
            {
                if (this.currentTableName.Equals("Addresses"))
                {
                    this.IndexBind = -1;
                    this.SelectedListViewItem = null;

                    Thread newWindowThread = new Thread(new ThreadStart(() =>
                    {
                        SynchronizationContext.SetSynchronizationContext(
                        new DispatcherSynchronizationContext(
                            Dispatcher.CurrentDispatcher));

                        Window tempWindow = new AddressEditWindow(this.connectionString);

                        tempWindow.Closed += (s, e) =>
                         Dispatcher.CurrentDispatcher.BeginInvokeShutdown(DispatcherPriority.Background);

                        tempWindow.Show();
                        System.Windows.Threading.Dispatcher.Run();

                    }));

                    newWindowThread.SetApartmentState(ApartmentState.STA);
                    newWindowThread.IsBackground = true;
                    newWindowThread.Start();

                    while (true)
                    {
                        if (GlobalVariables.refreshMainWindowFlag == 1)
                        {
                            LoadAdressBookDisplayWindow();
                            break;
                        }
                        else if (GlobalVariables.refreshMainWindowFlag == 2)
                        {
                            break;
                        }
                        OnPropertyChanged("DisplaySource");
                        
                    }
                    LoadAdressBookDisplayWindow();
                    OnPropertyChanged("DisplaySource");
                    
                    GlobalVariables.refreshMainWindowFlag = 0;
                }
                else if (currentTableName.Equals("Cities"))
                {
                    this.IndexBind = -1;
                    this.SelectedListViewItem = null;

                    Thread newWindowThread = new Thread(new ThreadStart(() =>
                    {
                        SynchronizationContext.SetSynchronizationContext(
                        new DispatcherSynchronizationContext(
                            Dispatcher.CurrentDispatcher));

                        Window tempWindow = new CityEditWindow(this.connectionString);

                        tempWindow.Closed += (s, e) =>
                         Dispatcher.CurrentDispatcher.BeginInvokeShutdown(DispatcherPriority.Background);

                        tempWindow.Show();
                        System.Windows.Threading.Dispatcher.Run();

                    }));

                    newWindowThread.SetApartmentState(ApartmentState.STA);
                    newWindowThread.IsBackground = true;
                    newWindowThread.Start();

                    while (true)
                    {
                        if (GlobalVariables.refreshMainWindowFlag == 1)
                        {
                            LoadCitiesDisplayWindow();
                            break;
                        }
                        else if (GlobalVariables.refreshMainWindowFlag == 2)
                        {
                            break;
                        }
                        OnPropertyChanged("DisplaySource");
                        
                    }
                    LoadCitiesDisplayWindow();
                    OnPropertyChanged("DisplaySource");
                    
                    GlobalVariables.refreshMainWindowFlag = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary> Uruchomienie nowego okna w celu edycji istniejącego elementu i odświeżenie listy elementów </summary>
        public void EditEntry()
        {
            try
            {
                this.idOfEditedItem = getIdOfSelectedItem();

                if (this.currentTableName.Equals("Addresses"))
                {
                    using (var ctx = new molkomEntities())
                    {
                        var address = ctx.Addresses.FirstOrDefault(x => x.id.Equals(idOfEditedItem));

                        if (address != null)
                        {
                            Thread newWindowThread = new Thread(new ThreadStart(() =>
                            {
                                SynchronizationContext.SetSynchronizationContext(
                                new DispatcherSynchronizationContext(
                                    Dispatcher.CurrentDispatcher));

                                Window tempWindow = new AddressEditWindow(address, this.connectionString);

                                tempWindow.Closed += (s, e) =>
                                 Dispatcher.CurrentDispatcher.BeginInvokeShutdown(DispatcherPriority.Background);

                                tempWindow.Show();
                                System.Windows.Threading.Dispatcher.Run();

                            }));

                            newWindowThread.SetApartmentState(ApartmentState.STA);
                            newWindowThread.IsBackground = true;
                            newWindowThread.Start();

                            while (true)
                            {
                                if (GlobalVariables.refreshMainWindowFlag == 1)
                                {
                                    LoadAdressBookDisplayWindow();
                                    break;
                                }
                                else if (GlobalVariables.refreshMainWindowFlag == 2)
                                {
                                    break;
                                }
                            }
                            OnPropertyChanged("DisplaySource");
                            
                            GlobalVariables.refreshMainWindowFlag = 0;
                        }
                    }
                }
                else if (this.currentTableName.Equals("Cities"))
                {
                    using (var ctx = new molkomEntities())
                    {
                        var city = ctx.Cities.FirstOrDefault(x => x.id.Equals(idOfEditedItem));

                        if (city != null)
                        {
                            Thread newWindowThread = new Thread(new ThreadStart(() =>
                            {
                                SynchronizationContext.SetSynchronizationContext(
                                new DispatcherSynchronizationContext(
                                    Dispatcher.CurrentDispatcher));

                                Window tempWindow = new CityEditWindow(city, this.connectionString);

                                tempWindow.Closed += (s, e) =>
                                 Dispatcher.CurrentDispatcher.BeginInvokeShutdown(DispatcherPriority.Background);

                                tempWindow.Show();
                                System.Windows.Threading.Dispatcher.Run();

                            }));

                            newWindowThread.SetApartmentState(ApartmentState.STA);
                            newWindowThread.IsBackground = true;
                            newWindowThread.Start();

                            while (true)
                            {
                                if (GlobalVariables.refreshMainWindowFlag == 1)
                                {
                                    LoadCitiesDisplayWindow();
                                    break;
                                }
                                else if (GlobalVariables.refreshMainWindowFlag == 2)
                                {
                                    break;
                                }
                            }
                            OnPropertyChanged("DisplaySource");
                            
                            GlobalVariables.refreshMainWindowFlag = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary> Usunięcie wybranego elementu z bazy danych i odświeżenie listy elementów </summary>
        public void DeleteEntry()
        {
            if (getIdOfSelectedItem() != 0)
            {
                string _currentTableName = this.currentTableName;

                try
                {
                    using (SqlConnection connection = new SqlConnection(this.connectionString))
                    {
                        connection.Open();
                        SqlCommand command;

                        command = new SqlCommand("DELETE FROM dbo." + _currentTableName + " " + "WHERE id = '" + getIdOfSelectedItem() + "'", connection);
                        command.ExecuteNonQuery();

                        int indexOfDeletedItem = -1;
                        for (int i = 0; i <= this.dataTable.Rows.Count - 1; i++)
                        {
                            if (i == IndexBind)
                            {
                                indexOfDeletedItem = i;
                                DataRow dr = this.dataTable.Rows[i];
                                dr.Delete();
                            }
                        }

                        if (this.dataTable.Rows.Count >= 1 && indexOfDeletedItem != this.dataTable.Rows.Count + 1)
                        {
                            for (int i = 0; i <= dataTable.Rows.Count - 1; i++)
                            {
                                if (i >= indexOfDeletedItem)
                                {
                                    DataRow dr = this.dataTable.Rows[i];
                                    dr.BeginEdit();
                                    dr[0] = Int32.Parse(dr[0].ToString()) - 1;
                                    dr.EndEdit();
                                }
                            }
                        }

                        connection.Close();
                        this.DisplaySource = this.dataTable.DefaultView;
                        OnPropertyChanged("DisplaySource");
                        
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
