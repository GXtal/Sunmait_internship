using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions;

public class BrandNotFoundException : Exception
{
    public BrandNotFoundException()
        : base()
    {
    }

    public BrandNotFoundException(string message)
        : base(message)
    {
    }
}
