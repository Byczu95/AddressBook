using System.ComponentModel;

namespace MolkomRecruitmentTask.ViewModels
{
    /// <summary>
    /// Implementacja interfejsu INotifyPropertyChanged dla wszystkich klas ViewModel
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Służy do signalizowania widoku o potrzebie odświeżenia danych
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Metoda wywoływana w celu odświeżenia właściwości widoku
        /// </summary>
        /// <param name="propName"> nazwa właściwości </param>
        protected virtual void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
