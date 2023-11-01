using DKCrm.Shared.Models.Products;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using MudBlazor;

namespace DKCrm.Client.Pages.ProductManagement
{
    partial class ProductPage
    {
        [Parameter]
        public Guid CategoryId { get; set; }

        private string? _currentCategoryName;
        private List<Product> Elements = new List<Product>();
        // private List<IdentityRole> Roles = new List<IdentityRole>();
        private List<Brand>? Brands { get; set; }
        private List<Category>? Categories { get; set; }
        private List<string> editEvents = new();
        private string searchString = "";
        private Product elementBeforeEdit;

        private HashSet<Product> selectedItems = new HashSet<Product>();
        private TableApplyButtonPosition applyButtonPosition = TableApplyButtonPosition.Start;
        private TableEditButtonPosition editButtonPosition = TableEditButtonPosition.Start;
        private TableEditTrigger editTrigger = TableEditTrigger.EditButton;



        private bool _visibleProductFilter;


        private string value { get; set; } = "Nothing selected";
       
        protected override async Task OnInitializedAsync()
        {

            Elements = await ProductManager.GetProductsAsync();

            //Brands = await BrandManager.GetAsync();
            // Roles = await UserManagerCustom.GetAllRolesAsync();


        }

        protected override async Task OnParametersSetAsync()
        {
            if (CategoryId != null && CategoryId != Guid.Empty)
            {
                var resultProducts = await CategoryManager.GetDetailsAsync(CategoryId);
                if (resultProducts.Products != null)
                    Elements = resultProducts.Products.ToList();
               
                _currentCategoryName = resultProducts.Name;
                
            }
            else
            {
                Elements = await ProductManager.GetProductsAsync();
                _currentCategoryName= null;
            }
        }

        private void AddEditionEvent(string message)
        {
            editEvents.Add(message);
            StateHasChanged();
        }

        private async void BackupItem(object element)
        {
            elementBeforeEdit = new()
            { Id = ((Product)element).Id,
                Name = ((Product)element).Name,
                PartNumber = ((Product)element).PartNumber,
                Brand = ((Product)element).Brand,
                Category = ((Product)element).Category,
            };
            Brands ??= await BrandManager.GetAsync();
            if (Categories==null)
            {
                Categories = await CategoryManager.GetAsync();
                Categories = Categories.Where(w => w.Children == null|| !w.Children.Any()).OrderBy(o=>o.Name).ToList();
            }
            
            AddEditionEvent($"RowEditPreview event: made a backup of Element {((Product)element).PartNumber}");
        }

        private async void ItemHasBeenCommitted(object element)
        {
            elementBeforeEdit = new()
            {
                Id = ((Product)element).Id,
                Name = ((Product)element).Name,
                PartNumber = ((Product)element).PartNumber,
                Brand = ((Product)element).Brand,
                Category = ((Product)element).Category,
            };
            await ProductManager.UpdateProductAsync(elementBeforeEdit);
            AddEditionEvent($"RowEditCommit event: Changes to Element {((Product)element).PartNumber} committed");
        }

        private void ResetItemToOriginalValues(object element)
        {
            ((Product)element).Id = elementBeforeEdit.Id;
            ((Product)element).Name = elementBeforeEdit.Name;

            ((Product)element).Brand = elementBeforeEdit.Brand;
            ((Product)element).Category = elementBeforeEdit.Category;
            AddEditionEvent($"RowEditCancel event: Editing of Element {((Product)element).PartNumber} canceled");
        }

        private bool FilterFunc(Product element)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (element.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.PartNumber.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            //if (element.Brand.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            //    return true;
            if ($"{element.Id}".Contains(searchString))
                return true;
            return false;
        }

        [Inject] private IDialogService DialogService { get; set; }
        private async void OnButtonDeleteClicked()
        {
            bool? result = await DialogService.ShowMessageBox(
                "Внимание",
                "Подтвердите удаление!",
                yesText: "Удалить!  ", cancelText: "  Отменить");

            if (result != null && (bool)result)
            {
                RemoveElement(selectedItems);
            }
            StateHasChanged();
        }
        private void RemoveElement(HashSet<Product> selectedItems)
        {
            var tempList = new List<Product>(selectedItems);
            ProductManager.RemoveRangeProductsAsync(tempList);
            foreach (var item in selectedItems)
            {
                Elements.Remove(item);
            }
            
            //_navigationManager.NavigateTo(_navigationManager.Uri, forceLoad: true);
        }

        private async Task AddProduct()
        {
            var createdProduct = new Product { Name = "_Новый продукт" };
            if (CategoryId!=null && CategoryId!= Guid.Empty)
            {
                createdProduct.CategoryId = CategoryId;
            }

            await ProductManager.AddProductAsync(createdProduct);
            Elements.Add(createdProduct);
        }
        private async Task AddBrand()
        {
            var brand = new Brand() { Name = value };
            await BrandManager.AddAsync(brand);
            Brands.Add(brand);
            _snackBar.Add("Изменения применены!");
            //_navigationManager.NavigateTo(_navigationManager.Uri, forceLoad: true);
        }
        Func<Brand, string> converter = p => p?.Name;
    }
}
