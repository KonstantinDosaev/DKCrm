﻿using DKCrm.Shared.Models.OrderModels;

namespace DKCrm.Client.Services.OrderServices
{
    public interface IExportedOrderManager
    {
        Task<List<ExportedOrder>> GetAsync();
        Task<ExportedOrder> GetDetailsAsync(Guid id);
        Task<bool> UpdateAsync(ExportedOrder item);
        Task<bool> AddAsync(ExportedOrder item);
        Task<bool> RemoveRangeAsync(IEnumerable<ExportedOrder> items);
        Task<bool> RemoveAsync(Guid id);
    }
}