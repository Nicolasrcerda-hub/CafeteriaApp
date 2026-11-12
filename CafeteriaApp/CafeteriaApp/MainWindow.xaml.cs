using System;
using System.IO;
using System.Text.Json;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace CafeteriaApp
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.AppWindow.SetIcon("Assets/Cafeteria-heros.ico");
            this.Title = "Cafetería Hero´s";
        }

        private async void btnGenerar_Click(object sender, RoutedEventArgs e)
        {
            // 1. Validar nombre
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                await MostrarError("El nombre del cliente no puede estar vacío.");
                return;
            }

            // 2. Validar cantidad
            int cantidad;
            if (!int.TryParse(txtCantidad.Text, out cantidad) || cantidad <= 0)
            {
                await MostrarError("La cantidad debe ser un número entero mayor a 0.");
                return;
            }

            // 3. Obtener producto y precio según la opción elegida
            string producto = "";
            int precio = 0;

            if (cmbProductos.SelectedIndex == 0)
            {
                producto = "Café Americano";
                precio = 35;
            }
            else if (cmbProductos.SelectedIndex == 1)
            {
                producto = "Panini";
                precio = 55;
            }
            else
            {
                producto = "Jugo";
                precio = 30;
            }

            // 4. Calcular totales
            int subtotal = precio * cantidad;
            int total = subtotal;
            DateTime fecha = DateTime.Now;

            // 5. Mostrar el ticket
            txtResultado.Text =
                "===== CAFETERÍA RÁPIDA =====\n" +
                "Fecha: " + fecha.ToString("dd/MM/yyyy HH:mm") + "\n" +
                "Cliente: " + txtNombre.Text + "\n" +
                "----------------------------\n" +
                "Producto: " + producto + "\n" +
                "Precio: $" + precio + "\n" +
                "Cantidad: " + cantidad + "\n" +
                "Subtotal: $" + subtotal + "\n" +
                "TOTAL: $" + total + "\n" +
                "============================";

            // 6. Guardar en JSON (objeto anónimo, sin clases)
            var ticket = new
            {
                Cliente = txtNombre.Text,
                Producto = producto,
                Precio = precio,
                Cantidad = cantidad,
                Subtotal = subtotal,
                Total = total,
                Fecha = fecha
            };

            string json = JsonSerializer.Serialize(ticket, new JsonSerializerOptions { WriteIndented = true });
            string ruta = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "ticket.json");
            File.WriteAllText(ruta, json);
        }

        private async System.Threading.Tasks.Task MostrarError(string mensaje)
        {
            ContentDialog dialogo = new ContentDialog();
            dialogo.Title = "Error";
            dialogo.Content = mensaje;
            dialogo.CloseButtonText = "Aceptar";
            dialogo.XamlRoot = this.Content.XamlRoot;
            await dialogo.ShowAsync();
        }
    }
}
