using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using softserve.projectlabs.Shared.DTOs.Cart;

namespace softserve.projectlabs.Shared.DTOs.Cart
{
    public class CartDto
    {
        public string Id { get; set; }
        public int CustomerId { get; set; }
        public List<CartItemDto> Items { get; set; } = new();
    }
}
