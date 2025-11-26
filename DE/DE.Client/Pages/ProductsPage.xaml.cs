using DE.DataLayer.DTOs.DeProducts;
using DE.DataLayer.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;
using System.Linq;

namespace DE.Client.Pages
{
    public partial class ProductsPage : Page, INotifyPropertyChanged
    {
        private readonly DeProductsService _productsService;
        private string _searchQuery = string.Empty;
        private string _selectedSortOption;
        private string _selectedDiscountFilter;
        private string _minPriceText = string.Empty;
        private string _maxPriceText = string.Empty;
        private decimal? _minPriceFilter;
        private decimal? _maxPriceFilter;
        private bool _showOnlyAvailable;
        private readonly ObservableCollection<CartItem> _cartItems = new ObservableCollection<CartItem>();

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<DeProductDto> Products { get; } = new ObservableCollection<DeProductDto>();
        public ICollectionView ProductsView { get; }

        public ReadOnlyObservableCollection<CartItem> CartItems { get; }

        public ObservableCollection<string> SortOptions { get; } = new ObservableCollection<string>()
        {
            "Без сортировки",
            "Цена по возрастанию",
            "Цена по убыванию",
            "Скидка по убыванию"
        };

        public ObservableCollection<string> DiscountFilters { get; } = new ObservableCollection<string>()
        {
            "Все скидки",
            "До 5%",
            "5% - 15%",
            "От 15%"
        };

        public int CartItemCount => _cartItems.Sum(c => c.Quantity);

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

        public int FilteredCount => ProductsView?.Cast<object>().Count() ?? 0;

        public string MinPriceFilterText
        {
            get => _minPriceText;
            set
            {
                if (_minPriceText == value)
                    return;

                _minPriceText = value;
                _minPriceFilter = TryParsePrice(value);
                OnPropertyChanged(nameof(MinPriceFilterText));
                ApplyFilters();
            }
        }

        public string MaxPriceFilterText
        {
            get => _maxPriceText;
            set
            {
                if (_maxPriceText == value)
                    return;

                _maxPriceText = value;
                _maxPriceFilter = TryParsePrice(value);
                OnPropertyChanged(nameof(MaxPriceFilterText));
                ApplyFilters();
            }
        }

        public bool OnlyShowAvailable
        {
            get => _showOnlyAvailable;
            set
            {
                if (_showOnlyAvailable == value)
                    return;

                _showOnlyAvailable = value;
                OnPropertyChanged(nameof(OnlyShowAvailable));
                ApplyFilters();
            }
        }

        public ProductsPage()
        {
            InitializeComponent();

            var httpClient = new HttpClient();
            _productsService = new DeProductsService(httpClient);

            CartItems = new ReadOnlyObservableCollection<CartItem>(_cartItems);
            _cartItems.CollectionChanged += (_, __) => OnPropertyChanged(nameof(CartItemCount));

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
                    foreach (var product in products.Where(p => p != null))
                    {
                        Products.Add(product!);
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

            bool discountMatches = SelectedDiscountFilter switch
            {
                "До 5%" => product.Discount < 5,
                "5% - 15%" => product.Discount >= 5 && product.Discount < 15,
                "От 15%" => product.Discount >= 15,
                _ => true
            };

            if (!discountMatches)
                return false;

            if (_minPriceFilter.HasValue && product.PriceWithDiscount < _minPriceFilter.Value)
                return false;

            if (_maxPriceFilter.HasValue && product.PriceWithDiscount > _maxPriceFilter.Value)
                return false;

            if (OnlyShowAvailable && !product.IsInStock)
                return false;

            return true;
        }

        private static decimal? TryParsePrice(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (decimal.TryParse(value, NumberStyles.Number, CultureInfo.CurrentCulture, out var parsed) && parsed >= 0)
                return parsed;

            if (decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out parsed) && parsed >= 0)
                return parsed;

            return null;
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

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button)
                return;

            var product = button.CommandParameter as DeProductDto ?? button.DataContext as DeProductDto;

            if (product is null)
                return;

            AddToCart(product);
        }

        private void AddToCart(DeProductDto product)
        {
            var existing = _cartItems.FirstOrDefault(c => c.Product.Id == product.Id);

            if (existing != null)
            {
                existing.Quantity += 1;
            }
            else
            {
                var newItem = new CartItem(product, 1);
                newItem.PropertyChanged += CartItem_PropertyChanged;
                _cartItems.Add(newItem);
            }

            OnPropertyChanged(nameof(CartItemCount));
        }

        private void CartItem_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CartItem.Quantity))
            {
                OnPropertyChanged(nameof(CartItemCount));
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class CartItem : INotifyPropertyChanged
    {
        private int _quantity;

        public CartItem(DeProductDto product, int quantity = 1)
        {
            Product = product;
            _quantity = quantity;
        }

        public DeProductDto Product { get; }

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity == value)
                    return;

                _quantity = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Quantity)));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
