using DE.DataLayer.DTOs.DeProducts;
using DE.DataLayer.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;
using System.Globalization;
using System.Windows.Media;

namespace DE.Client.Pages
{
    public partial class ProductsPage : Page, INotifyPropertyChanged
    {
        private readonly DeProductsService _productsService;
        private string _searchQuery = string.Empty;
        private string _selectedSortOption;
        private string _selectedDiscountFilter;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<DeProductDto> Products { get; } = new();
        public ICollectionView ProductsView { get; }

        public ObservableCollection<string> SortOptions { get; } = new()
        {
            "Без сортировки",
            "Цена по возрастанию",
            "Цена по убыванию",
            "Скидка по убыванию"
        };

        public ObservableCollection<string> DiscountFilters { get; } = new()
        {
            "Все скидки",
            "До 5%",
            "5% - 15%",
            "От 15%"
        };

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                if (_searchQuery == value)
                    return;
                _searchQuery = value;
                OnPropertyChanged(nameof(SearchQuery));
                ApplyFilters();
            }
        }

        public string SelectedSortOption
        {
            get => _selectedSortOption;
            set
            {
                if (_selectedSortOption == value)
                    return;
                _selectedSortOption = value;
                OnPropertyChanged(nameof(SelectedSortOption));
                ApplySort();
            }
        }

        public string SelectedDiscountFilter
        {
            get => _selectedDiscountFilter;
            set
            {
                if (_selectedDiscountFilter == value)
                    return;
                _selectedDiscountFilter = value;
                OnPropertyChanged(nameof(SelectedDiscountFilter));
                ApplyFilters();
            }
        }

        public int FilteredCount => ProductsView?.Count ?? 0;

        public ProductsPage()
        {
            InitializeComponent();

            var httpClient = new HttpClient();
            _productsService = new DeProductsService(httpClient);

            ProductsView = CollectionViewSource.GetDefaultView(Products);
            ProductsView.Filter = FilterProducts;

            _selectedSortOption = SortOptions[0];
            _selectedDiscountFilter = DiscountFilters[0];

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

                ApplySort();
                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ошибка при загрузке товаров: {ex.Message}",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }


        }

        private bool FilterProducts(object obj)
        {
            if (obj is not DeProductDto product)
                return false;

            if (!string.IsNullOrWhiteSpace(SearchQuery))
            {
                bool matchesName = product.ProductName.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase);
                bool matchesDescription = product.Description?.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) == true;

                if (!matchesName && !matchesDescription)
                    return false;
            }

            return SelectedDiscountFilter switch
            {
                "До 5%" => product.Discount < 5,
                "5% - 15%" => product.Discount >= 5 && product.Discount < 15,
                "От 15%" => product.Discount >= 15,
                _ => true
            };
        }

        private void ApplyFilters()
        {
            ProductsView?.Refresh();
            OnPropertyChanged(nameof(FilteredCount));
        }

        private void ApplySort()
        {
            if (ProductsView == null)
                return;

            ProductsView.SortDescriptions.Clear();

            switch (SelectedSortOption)
            {
                case "Цена по возрастанию":
                    ProductsView.SortDescriptions.Add(new SortDescription(nameof(DeProductDto.Price), ListSortDirection.Ascending));
                    break;
                case "Цена по убыванию":
                    ProductsView.SortDescriptions.Add(new SortDescription(nameof(DeProductDto.Price), ListSortDirection.Descending));
                    break;
                case "Скидка по убыванию":
                    ProductsView.SortDescriptions.Add(new SortDescription(nameof(DeProductDto.Discount), ListSortDirection.Descending));
                    break;
            }

            ApplyFilters();
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class DiscountToBackgroundConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is double discount && discount >= 15)
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2E8B57"));
            }

            return Brushes.White;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DiscountToForegroundConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is double discount && discount >= 15)
            {
                return Brushes.White;
            }

            return Brushes.Black;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
