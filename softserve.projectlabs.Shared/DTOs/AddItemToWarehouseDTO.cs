﻿namespace softserve.projectlabs.Shared.DTOs
{
    public class AddItemToWarehouseDto
    {
        public int WarehouseId { get; set; }
        public int Sku { get; set; }
        public int CurrentStock { get; set; } = 1; 
    }
}
