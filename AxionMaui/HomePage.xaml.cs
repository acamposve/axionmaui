using AxionMaui.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http.Json;

namespace AxionMaui;

public partial class HomePage : ContentPage, INotifyPropertyChanged
{
    public string FullName { get; set; }

    private readonly HttpClient _httpClient = new HttpClient();

    public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();
    public ObservableCollection<ProductFeatured> FeaturedProducts { get; set; } = new ObservableCollection<ProductFeatured>();
    private string featuredLabel = "Destacados";

    public string FeaturedLabel
    {
        get => featuredLabel;
        set
        {
            if (featuredLabel != value)
            {
                featuredLabel = value;
                OnPropertyChanged(nameof(FeaturedLabel)); // Notifica a la vista que la propiedad ha cambiado
            }
        }
    }
    public HomePage(string fullName)
	{
		InitializeComponent();
        FullName = fullName;

        BindingContext = this; // Establece el contexto de enlace

        LoadDataAsync();
    }

    private async void LoadDataAsync()
    {
        await LoadProducts();
        // Cambia el texto del Label después de cargar los productos
        FeaturedLabel = "Destacados (cargados)";
        await LoadFeaturedProducts();
    }


    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private async Task LoadFeaturedProducts()
    {
        try
        {
            // Realizar la llamada a la API y obtener la lista de productos
            var productsFeatured = await _httpClient.GetFromJsonAsync<List<ProductFeatured>>("https://amontefusco-002-site1.ktempurl.com/api/Products/all");

            // Llenar la colección Products con los datos obtenidos
            if (productsFeatured != null)
            {
                foreach (var productFeatured in productsFeatured.Take(3)) // Seleccionar solo los primeros 4 productos
                {
                    // Actualizar URL de imagen
                    productFeatured.ImageUrl = $"https://amontefusco-002-site1.ktempurl.com{productFeatured.Image}";
                    FeaturedProducts.Add(productFeatured);
                }
            }
        }
        catch (Exception ex)
        {
            // Manejar excepciones de la llamada a la API
            await DisplayAlert("Error", "No se pudieron cargar los productos.", "OK");
        }
    }



    private async Task LoadProducts()
    {
        try
        {
            // Realizar la llamada a la API y obtener la lista de productos
            var products = await _httpClient.GetFromJsonAsync<List<Product>>("https://amontefusco-002-site1.ktempurl.com/api/Products/all");

            // Llenar la colección Products con los datos obtenidos
            if (products != null)
            {
                foreach (var product in products.Take(4)) // Seleccionar solo los primeros 4 productos
                {
                    // Actualizar URL de imagen
                    product.ImageUrl = $"https://amontefusco-002-site1.ktempurl.com{product.Image}";
                    Products.Add(product);
                }
            }
        }
        catch (Exception ex)
        {
            // Manejar excepciones de la llamada a la API
            await DisplayAlert("Error", "No se pudieron cargar los productos.", "OK");
        }
    }
    private void OnViewCategoriesClicked(object sender, EventArgs e)
    {
        // Navega a la página de categorías aquí
        // Ejemplo: await Navigation.PushAsync(new CategoriesPage());
    }
}