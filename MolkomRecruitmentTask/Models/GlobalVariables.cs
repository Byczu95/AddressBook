using System;

namespace MolkomRecruitmentTask.Models
{
    class GlobalVariables
    {
        /// <summary>
        /// Flaga w celu sprawdzenia co zrobić z listą w menu głównym po zamknięciu okna edycji.
        ///  Wartości: 0 - nie odświeżać okna,  1 - odświeżać okno, dodano wpis, 2 - zamknięto edycję wpisu, nie odświeżać widoku 
        /// </summary>
        public static int refreshMainWindowFlag;
    }
}