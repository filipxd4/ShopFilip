using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFilip.Helpers
{
    public enum Gender
    {
        Woman = 0,
        Men = 1
    }

    public enum Group
    {
        Spodnie = 0,
        Koszule = 1,
        Kurtki = 2,
        TShirt = 3
    }

    public enum SizeOfPruduct
    {
        M = 0,
        L = 1,
        XL = 2,
        XXL = 3,
    }

    public enum Status
    {
        New = 0,
        Paid = 1,
        Success = 1,
        Send = 1,
        Cancel = 1,
    }
}