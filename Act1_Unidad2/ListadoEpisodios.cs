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
{
    public class ListadoEpisodios : INotifyPropertyChanged
    {
        private bool agregando;
        private bool editando;
        public ICommand VerAgregandoCommand { get; set; }
        public ICommand AgregarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand EditarCommand { get; set; }
        public ICommand VerEditarCommand { get; set; }
        public ListadoEpisodios()
        {
            Load();
            VerAgregandoCommand = new RelayCommand<string>(VerAgregar);
            AgregarCommand = new RelayCommand(Agregar);
            EliminarCommand = new RelayCommand(Eliminar);
            VerEditarCommand = new RelayCommand<string>(VerEditar);
            EditarCommand = new RelayCommand(Editar);
            

        }
        public bool Agregando
        {
            get { return agregando; }
            set { agregando = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Agregando"));
            }
        }
        private void VerAgregar(string ver)
        {
            Error = "";
            Agregando = bool.Parse(ver);
            if (Agregando == true)
            {
                Episodios = new Episodios();
            }
            //Agregando = false;
        }
        int posElementoEditar;
        private void VerEditar(string ver)
        {
            if (Episodio != null)
            {
                Error = " ";
                Editando = bool.Parse(ver);
                if (Editando == true)
                {
                    posElementoEditar = Episodio.IndexOf(Episodios);
                    Episodios clon = new Episodios()
                    {
                        Episodio = Episodios.Episodio,
                        Temporada = Episodios.Temporada,
                        Titulo = Episodios.Titulo,
                        TituloEspañol=Episodios.TituloEspañol,
                        Descripcion = Episodios.Descripcion
                    };
                    Episodios = clon;
                }
                else
                {
                    Episodios = Episodio[posElementoEditar];
                }
            }
        }
        public ObservableCollection<Episodios> Episodio { get; set; }
        private Episodios e;

        public Episodios Episodios
        {
            get { return e; }
            set
            {
                e = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Episodios"));
            }
        }
        private string error;

        public string Error
        {
            get { return error; }
            set {
                error = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Error")); ; }
        }

        public void Agregar()
        {
            Error = " ";
            if (string.IsNullOrWhiteSpace(Episodios.Episodio))
            {
                Error = "El numero del episodio esta vacio";
                return;
            }
            if (string.IsNullOrWhiteSpace(Episodios.Temporada))
            {
                Error = "El numero de la temporada esta vacia";
                return;
            }
            if (string.IsNullOrWhiteSpace(Episodios.Titulo))
            {
                Error = "El nombre del episodio esta vacio";
                return;
                
            }
            if (string.IsNullOrWhiteSpace(Episodios.TituloEspañol))
            {
                Error = "El nombre del episodio en español esta vacio";
                return;
            }
            if (string.IsNullOrWhiteSpace(Episodios.Descripcion))
            {
                Error = "La descripcion esta vacia";
                return;
            }
            
            //if (Episodio.Any(x => x.Episodio == Episodios.Episodio))
                Episodio.Add(Episodios);
            //Episodio.Add(Episodios);
            Save();
            Agregando = false;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Error"));
        }
        public void Eliminar()
        {      
                if (Episodios !=null)
                {
                    Episodio.Remove(Episodios);
                }
}
        public bool Editando
        {
            get { return editando; }
            set { editando = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Editando")); }
        }
        public void Editar()
        {
            Error = "";
            if (string.IsNullOrWhiteSpace(Episodios.Episodio))
            {
                Error = "El numero del episodio esta vacio";
                return;
            }
            if (string.IsNullOrWhiteSpace(Episodios.Temporada))
            {
                Error = "El numero de la temporada esta vacia";
                return;
            }
            if (string.IsNullOrWhiteSpace(Episodios.Titulo))
            {
                Error = "El nombre del episodio esta vacio";
                return;
            }
            if (string.IsNullOrWhiteSpace(Episodios.Descripcion))
            {
                Error = "La descripcion esta vacia";
                return;
            }
            if (string.IsNullOrWhiteSpace(Episodios.TituloEspañol))
            {
                Error = "El nombre del episodio en español esta vacio";
                return;
            }
            Episodio[posElementoEditar] = Episodios;
            Save();
            Editando = false;
        }
        private void Save()
        {
            FileStream fs = File.Create("Episodios.data");
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, Episodio);
            fs.Close();

        }

        private void Load()
        {
            try
            {
                FileStream fs = File.OpenRead("Episodios.data");
                BinaryFormatter bf = new BinaryFormatter();
                Episodio = (ObservableCollection < Episodios >)bf. Deserialize(fs);
                fs.Close();
            }
            catch(Exception)
            {
                Episodio = new ObservableCollection <Episodios>();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
