﻿using System.Net;
using System.Net.Http.Json;
using DKCrm.Client.Constants;
using DKCrm.Shared.Models;
using DKCrm.Shared.Models.Products;
using DKCrm.Shared.Requests;

namespace DKCrm.Client.Services.ProductServices
{
    public class ProductManager : IProductManager
    {
        private readonly HttpClient _httpClient;

        public ProductManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<Product>> GetProductsAsync()
        {
           return await _httpClient.GetFromJsonAsync<List<Product>>("api/product") ?? throw new InvalidOperationException();
        }

        public async Task<SortPagedResponse<ProductsDto>> GetProductsBySortAsync(SortPagedRequest<FilterProductTuple> request)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/Product/get-sort-filtered/",request) ?? throw new InvalidOperationException();
            return await response.Content.ReadFromJsonAsync<SortPagedResponse<ProductsDto>>() ?? throw new InvalidOperationException();

        }

        public async Task<List<Product>> GetSearchProductAsync(string searchString)
        {
            return await _httpClient.GetFromJsonAsync<List<Product>>($"api/Product/{searchString}") ?? throw new InvalidOperationException();
        }

        public async Task<Product> GetProductDetailsAsync(Guid userId)
        {
            return await _httpClient.GetFromJsonAsync<Product>($"api/product/{userId}") ?? throw new InvalidOperationException();
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
           var result = await _httpClient.PutAsJsonAsync("api/product", product, JsonOptions.JsonIgnore);
            return result.StatusCode== HttpStatusCode.OK; 
        }

        public async Task<bool> UpdateRangeProductsAsync(IEnumerable<Product> products)
        {
           return (await _httpClient.PutAsJsonAsync("api/product/range", products, JsonOptions.JsonIgnore)).StatusCode== HttpStatusCode.OK;
        }    
        public async Task<bool> RemoveAsync(DeleteForGuidRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/Product/delete", request) ?? throw new InvalidOperationException();
            return await response.Content.ReadFromJsonAsync<int>() > 0 ;
        }

        public async Task AddProductAsync(Product product)
        {
            await _httpClient.PostAsJsonAsync($"api/product", product, JsonOptions.JsonIgnore);
        }

        public async Task RemoveRangeProductsAsync(IEnumerable<Product> products)
        {
             await _httpClient.PostAsJsonAsync($"api/product/removerange",products);
        }

        public async Task<byte[]> OutputProductListToExcelAsync(List<Guid> productsIds)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/Product/outputXl", productsIds) ?? throw new InvalidOperationException();
            return await response.Content.ReadFromJsonAsync<byte[]>() ?? throw new InvalidOperationException();

        }
        public async Task<List<string>> LoadProductsFromExcelAsync(byte[] excelBt)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/Product/loadFromXl", excelBt) ?? throw new InvalidOperationException();
            return await response.Content.ReadFromJsonAsync<List<string>>() ?? throw new InvalidOperationException();

        }
    }
}
