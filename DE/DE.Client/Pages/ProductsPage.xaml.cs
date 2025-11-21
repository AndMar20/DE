using DE.DataLayer.DTOs.DeProducts;
using DE.DataLayer.Services;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows.Controls;
using System.Windows;

namespace DE.Client.Pages
{
    public partial class ProductsPage : Page
    {
        private readonly DeProductsService _productsService;
        public ObservableCollection<DeProductDto> Products { get; set; } = new();

        public ProductsPage()
        {
            InitializeComponent();

            var httpClient = new HttpClient();
            _productsService = new DeProductsService(httpClient);

            DataContext = this;

            LoadProducts();

        }

        private async void LoadProducts()
        {
            try
            {
                var products = await _productsService.GetAllAsync();

                if (products != null)
                {
                    Products.Clear();
                    foreach (var product in products)
                    {
                        Products.Add(product);
                    }
                }

                else
                {
                    MessageBox.Show("Не удалось загрузить товары.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке товаров: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }
    }
}