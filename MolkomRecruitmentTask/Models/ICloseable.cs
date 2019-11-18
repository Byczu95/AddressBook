using System;

namespace MolkomRecruitmentTask.Models
{
    /// <summary>
    /// Interfejs zawierający deklarację zdarzenia zamknięcia okna widoku.
    /// </summary>
    public interface ICloseable
    {
        /// <summary>
        /// zamknięcie okna widoku
        /// </summary>
        event EventHandler<EventArgs> RequestClose;
    }
}