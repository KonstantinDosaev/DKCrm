﻿namespace DKCrm.Shared.Models.OrderModels
{
    public class OrderFromListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Image { get; set; } = null!;
        public string Product { get; set; } = null!;
        public string? PartNumber { get; set; }
        public string? Brand { get; set; }
    }
}
