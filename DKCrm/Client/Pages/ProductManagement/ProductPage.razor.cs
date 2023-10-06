using DKCrm.Shared.Models.Products;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using MudBlazor;

namespace DKCrm.Client.Pages.ProductManagement
{
    partial class ProductPage
    {
        private List<Product> Elements = new List<Product>();
        // private List<IdentityRole> Roles = new List<IdentityRole>();
        private List<Brand> Brands = new List<Brand>();
        private List<string> editEvents = new();
        private string searchString = "";
        private Product elementBeforeEdit;

        private HashSet<Product> selectedItems = new HashSet<Product>();
        private TableApplyButtonPosition applyButtonPosition = TableApplyButtonPosition.Start;
        private TableEditButtonPosition editButtonPosition = TableEditButtonPosition.Start;
        private TableEditTrigger editTrigger = TableEditTrigger.EditButton;


        private void OpenDialog() => _visibleRegisterDialog = true;
        private DialogOptions _registerDialogOptions = new() { FullWidth = true, CloseButton = true };
        private bool _visibleRegisterDialog;

        private Product _currentUser;
        private static List<string> _currentRoles;
        private DialogOptions _roleDialogOptions = new() { FullWidth = true, CloseButton = true };
        private bool _visibleRoleDialog;
        private string value { get; set; } = "Nothing selected";
        private async void OpenRoleDialog(Product user)
        {
            _currentUser = user;
           // _currentRoles = await UserManagerCustom.GetRoleFromUser(user.Id);
            value = _currentRoles.FirstOrDefault()!;
            _visibleRoleDialog = true;
        }

        protected override async Task OnInitializedAsync()
        {

            Elements = await ProductManager.GetProductsAsync();
            //Brands = await BrandManager.GetAsync();
            // Roles = await UserManagerCustom.GetAllRolesAsync();


        }

        private void AddEditionEvent(string message)
        {
            editEvents.Add(message);
            StateHasChanged();
        }

        private void BackupItem(object element)
        {
            elementBeforeEdit = new()
            { Id = ((Product)element).Id,
                Name = ((Product)element).Name,
                PartNumber = ((Product)element).PartNumber,
                Brand = ((Product)element).Brand,
            };
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
            };
            await ProductManager.UpdateProductAsync(elementBeforeEdit);
            AddEditionEvent($"RowEditCommit event: Changes to Element {((Product)element).PartNumber} committed");
        }

        private void ResetItemToOriginalValues(object element)
        {
            ((Product)element).Id = elementBeforeEdit.Id;
            ((Product)element).Name = elementBeforeEdit.Name;

            ((Product)element).Brand = elementBeforeEdit.Brand;
 
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
            _navigationManager.NavigateTo(_navigationManager.Uri, forceLoad: true);
        }

        private async Task AddProduct()
        {
            await ProductManager.AddProductAsync(new Product() { Name = "_Новый продукт" });
            _navigationManager.NavigateTo(_navigationManager.Uri, forceLoad:true);
        }
        private async Task AddBrand()
        {
            var brand = new Brand() { Name = value };
            await BrandManager.AddAsync(brand);
            Brands.Add(brand);
            _snackBar.Add("Изменения применены!");
            //_navigationManager.NavigateTo(_navigationManager.Uri, forceLoad: true);
        }
    }
}
