using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductSpecification : BaseSpecification<Product>
    {
        public ProductSpecification(string? brand, string? type, string? sort) :
            base(x => (string.IsNullOrEmpty(brand) || x.Brand == brand) &&
            (string.IsNullOrEmpty(type) || x.Type == type)
        )
        {
            switch (sort)
            {
                case "priceAsc":
                    AddOrderBy(x => (double)x.Price);
                    break;
                case "priceDesc":
                    AddOrderByDescending(x => (double)x.Price);
                    break;
                default:
                    AddOrderBy(x => x.Name);
                    break;
            } 
        }
    }
}