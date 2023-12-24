using DKCrm.Shared.Models.Products;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using MudBlazor;

namespace DKCrm.Client.Pages.ProductManagement
{
    partial class ProductPage
    {
        [Parameter] public Guid? ChapterId { get; set; }

        private string? _currentCategoryName;
        //private List<Product>? Elements { get; set; }

        //private List<Brand>? Brands { get; set; }
        //private List<Category>? Categories { get; set; }
        
        private Product EditedProduct = null!;
        
        public Guid ProductId;

        private HashSet<ProductsDto> selectedItems = new HashSet<ProductsDto>();
        private TableApplyButtonPosition applyButtonPosition = TableApplyButtonPosition.Start;
      
        private StorageSettings? _storageSettings;
        private bool _visibleProductFilter;
    


        private string value { get; set; } = "Nothing selected";

        protected override async Task OnParametersSetAsync()
        {
            if (table != null) await table.ReloadServerData();
        }
        private async Task<List<Product>> GetProductList()
        {
            _currentCategoryName = null;
            return await ProductManager.GetProductsAsync();
        }
        //private async Task<List<Product>> GetProductListByCategory()
        //{
        //    return await ProductManager.GetProductsByCategoryAsync(CategoryId);
        //}
       

        //private bool FilterFunc(Product element)
        //{
        //    if (string.IsNullOrWhiteSpace(_searchString))
        //        return true;
        //    if (element.Name.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
        //        return true;
        //    if (element.PartNumber != null && element.PartNumber.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
        //        return true;
        //    if (element.Brand != null && element.Brand.Name.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
        //        return true;
        //    //if (element.Brand.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
        //    //    return true;
        //    if ($"{element.Id}".Contains(_searchString))
        //        return true;
        //    return false;
        //}

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
        private async void RemoveElement(HashSet<ProductsDto> selectedItems)
        {
            var tempList = new List<ProductsDto>(selectedItems);
            foreach (var item in tempList)
            {
                await ProductManager.RemoveAsync(item.Id);
            }
         

            await table!.ReloadServerData();
        }

        
    }
}
