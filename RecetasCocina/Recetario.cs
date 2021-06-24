using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Windows.Markup;
using System.ComponentModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using System.Runtime.Serialization.Formatters.Binary;

namespace RecetasCocina
{
    
    public class Recetario:INotifyPropertyChanged
    {

      
       
        public ICommand VerAgregandoCommand { get; set; }
        public ICommand AgregarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand EditarCommand { get; set; }
        public ICommand VerEditarCommand { get; set; }
      
      
        public Recetario()
        {
            Cargar();
            VerAgregandoCommand = new RelayCommand<string>(VerAgregar);
            AgregarCommand = new RelayCommand(Agregar);
            EliminarCommand = new RelayCommand(Eliminar);
            VerEditarCommand = new RelayCommand<string>(VerEditar);
            EditarCommand = new RelayCommand(Editar);
           
            
        }

        

        private bool agregando;
        private bool editando;
        public bool Agregando
        {
            get { return agregando; }
            set
            {
                agregando = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Agregando"));
            }
        }
        private void VerAgregar(string ver)
        {
            Error = "";
            Agregando = bool.Parse(ver);
            if (Agregando == true)
            {
                Receta = new Receta();
            }
            
        }
        int posElementoEditar;
        private void VerEditar(string ver)
        {
            if (Recetas != null)
            {
                Error = " ";
                Editando = bool.Parse(ver);
                if (Editando == true)
                {
                    posElementoEditar = Recetas.IndexOf(Receta);
                    Receta clon = new Receta()
                    {
                        Nombre = Receta.Nombre,
                        Ingrediente= Receta.Ingrediente,
                        Procedimiento=Receta.Procedimiento,
                        Imagen=Receta.Imagen
                    };
                    Receta = clon;
                }
                else
                {
                    Receta = Recetas[posElementoEditar];
                }
            }
        }
        public ObservableCollection<Receta> Recetas { get; set; }
       
        private Receta r;

        public Receta Receta
        {
            get { return r; }
            set
            {
                r = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Receta)));
            }
        }
        private string error;

        public string Error
        {
            get { return error; }
            set
            {
                error = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Error")); ;
            }
        }

        public void Agregar()
        {
            Error = " ";
            if (string.IsNullOrWhiteSpace(Receta.Nombre))
            {
                Error = "El campo nomnre esta vacio";
                return;
            }
            if (string.IsNullOrWhiteSpace(Receta.Ingrediente))
            {
                Error = "El campo ingredientes esta vacia";
                return;
            }
            if (string.IsNullOrWhiteSpace(Receta.Procedimiento))
            {
                Error = "El campo procedimiento esta vacio";
                return;

            }
            if (string.IsNullOrWhiteSpace(Receta.Imagen))
            {
                Error = "El campo imagen esta vacio";
                return;
            }


            Recetas.Add(Receta);
           
           
            Guardar();
            Agregando = false;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Error)));
        }
        private string nom;

        public string Nom
        {
            get { return nom; }
            set { nom = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Nom))); }
        }

      
       
        public void Eliminar()
        {
            if (Receta != null)
            {
                Recetas.Remove(Receta);
                Guardar();
            }
        }
        public bool Editando
        {
            get { return editando; }
            set
            {
                editando = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Editando"));
            }
        }
        public void Editar()
        {
            Error = " ";
            if (string.IsNullOrWhiteSpace(Receta.Nombre))
            {
                Error = "El campo nombre esta vacio";
                return;
            }
            if (string.IsNullOrWhiteSpace(Receta.Ingrediente))
            {
                Error = "El campo ingrediente esta vacia";
                return;
            }
            if (string.IsNullOrWhiteSpace(Receta.Procedimiento))
            {
                Error = "El campo procedimiento esta vacio";
                return;

            }
            if (string.IsNullOrWhiteSpace(Receta.Imagen))
            {
                Error = "El campo imagen esta vacio";
                return;
            }

            Recetas[posElementoEditar] = Receta;
            Guardar();
            Editando = false;
        }
       
        public event PropertyChangedEventHandler PropertyChanged;
        
        
      
        public void Guardar()
        {
            XmlWriter a = XmlWriter.Create("Recetas.xml");
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Receta>));
            serializer.Serialize(a, Recetas);
            a.Close();
        }

        public void Cargar()
        {
            try
            {
                XmlReader a = XmlReader.Create("Recetas.xml");
                XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Receta>));
                Recetas = (ObservableCollection<Receta>)serializer.Deserialize(a);
                a.Close();

            }
            catch (Exception)
            {

                Recetas = new ObservableCollection<Receta>();
            }
        }
    }
}
