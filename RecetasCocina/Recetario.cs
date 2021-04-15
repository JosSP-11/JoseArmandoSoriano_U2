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

namespace RecetasCocina
{
    public enum Vistas { Lista, Agregar, Eliminar, Editar }
    public class Recetario:INotifyPropertyChanged
    {

        private Vistas vistas = Vistas.Lista;

        public Vistas Vista
        {
            get { return vistas; }
            set
            {
                vistas = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Vista"));
            }
        }

        public ICommand AgregarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand EditarCommand { get; set; }
        public ICommand SeleccionarCommand { get; set; }
        public ICommand CambiarVistaCommand { get; set; }
        public ICommand MostrarEditarCommand { get; set; }
        public ICommand MostrarEliminarCommand { get; set; }
        


        public ObservableCollection<Receta> Recetas { get; set; }
        public  Recetario()
        {
            Cargar();
            AgregarCommand = new RelayCommand(Agregar);
            EliminarCommand = new RelayCommand(Eliminar);
            EditarCommand = new RelayCommand(Editar);
            CambiarVistaCommand = new RelayCommand<Vistas>(CambiarVista);
            MostrarEditarCommand = new RelayCommand<Receta>(MostrarEditar);
            MostrarEliminarCommand = new RelayCommand<Receta>(MostrarEliminar);
           
        }

        private void MostrarEliminar(Receta obj)
        {
            Receta = obj;
            CambiarVista(Vistas.Eliminar);
        }

        private void MostrarEditar(Receta obj)
        {      
            Receta = new Receta { Nombre = obj.Nombre, Ingrediente = obj.Ingrediente, Procedimiento = obj.Procedimiento, Imagen = obj.Imagen };
            pocision = Recetas.IndexOf(obj);
            CambiarVista(Vistas.Editar);
            Error = " ";
        }

        private void CambiarVista(Vistas obj)
        {
            
            Vista = obj;
            if (Vista==Vistas.Agregar)
            {
                Receta = new Receta();
                Error = " ";
            }
            
        }

        private Receta receta;

        public Receta Receta
        {
            get { return receta; }
            set { receta = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Receta")); }
        }

        private string error;

        public string Error
        {
            get { return error; }
            set { error = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Error")); }
        }


        public void Agregar()
        {// validacion
            if (receta != null)
            {
                Error = "";
                if (string.IsNullOrWhiteSpace(receta.Nombre))
                {
                    Error = "Escriba el nombre de la receta";
                    return;
                }
                if (string.IsNullOrWhiteSpace(receta.Ingrediente))
                {
                    Error = "Escriba los ingredientes de la receta";
                    return;
                }
                if (string.IsNullOrWhiteSpace(receta.Procedimiento))
                {
                    Error = "Escriba el procedimiento de la receta";
                    return;
                }
                if (string.IsNullOrWhiteSpace(receta.Imagen))
                {
                    Error = "Escriba la URL de la imagen";
                    return;
                }

                if (Recetas.Any(x => x.Nombre.ToUpper() == Receta.Nombre.ToUpper()))
                {
                    Error = "ya esta registrado una receta con el mismo nombre";
                }

                Recetas.Add(Receta);
                Guardar();
                CambiarVista(Vistas.Lista);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Error"));
            }
        }

        public int pocision;

        public event PropertyChangedEventHandler PropertyChanged;

        public void Editar()
        {
            if (receta != null)
            {
                Error = "";
                if (string.IsNullOrWhiteSpace(receta.Nombre))
                {
                    Error = "Escriba el nombre de la receta";
                    return;
                }
                if (string.IsNullOrWhiteSpace(receta.Ingrediente))
                {
                    Error = "Escriba los ingredientes de la receta";
                    return;
                }
                if (string.IsNullOrWhiteSpace(receta.Procedimiento))
                {
                    Error = "Escriba el procedimiento de la receta";
                    return;
                }
                if (string.IsNullOrWhiteSpace(receta.Imagen))
                {
                    Error = "Escriba la URL de la imagen";
                    return;
                }

                if (Recetas[pocision].Nombre != Receta.Nombre)
                {
                    if (Recetas.Any(x => x.Nombre.ToUpper() == Receta.Nombre.ToUpper()))
                    {
                        Error = "ya esta registrado una receta con el mismo nombre";
                    }
                }
                Recetas[pocision] = Receta;
                Guardar();
                CambiarVista(Vistas.Lista);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Error"));
            }
        }

       
        public void Eliminar()
        {
            if (Recetas.Remove(Receta))
            {
                Guardar();
            }
            CambiarVista(Vistas.Lista);
        }
        public void Guardar()
        {
            XmlWriter a = XmlWriter.Create("Recetas.xml");
            XmlSerializer serializer =new XmlSerializer(typeof(ObservableCollection<Receta>));
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
