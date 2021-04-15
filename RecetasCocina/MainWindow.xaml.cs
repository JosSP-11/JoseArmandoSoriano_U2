using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RecetasCocina
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
           

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Recetarios.CambiarVistaCommand.Execute(Vistas.Agregar);
        }

        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {           
            if(List.SelectedItem!=null)
            {
            Receta r = (Receta)List.SelectedItem;
             Nom.Text = r.Nombre;
            Ing.Text= r.Ingrediente;
            Pro.Text = r.Procedimiento;
              imagen.Text =  r.Imagen;
            
              
                
            }
            
        }

       

        
    }
}
