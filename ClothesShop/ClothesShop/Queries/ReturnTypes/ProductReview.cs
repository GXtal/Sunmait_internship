using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesShop.Queries.ReturnTypes;

public class ProductReview
{
    public int ProductId { get; set; }

    public int Rating { get; set; }

    public string Comment { get; set; }

    public string CommentatorName { get; set; }

    public string CommentatorEmail { get; set;}
}
