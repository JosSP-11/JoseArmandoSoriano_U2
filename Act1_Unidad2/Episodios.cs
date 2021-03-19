using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Input;
using System.Linq;

namespace Act1_Unidad2
{[Serializable]
   public class Episodios
    {
        public string Episodio { get; set; }
        public string Temporada { get; set; }
        public string Titulo { get; set; }
        public string TituloEspañol { get; set; }
        public string Descripcion { get; set; }
        
    }
}
