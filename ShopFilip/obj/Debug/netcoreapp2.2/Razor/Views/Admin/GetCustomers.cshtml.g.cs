#pragma checksum "C:\Users\Filip\source\repos\Shop\OnlineShop\ShopFilip\Views\Admin\GetCustomers.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "2616f2fb73db25c540c7d9ebeb3c5e47786d608b"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Admin_GetCustomers), @"mvc.1.0.view", @"/Views/Admin/GetCustomers.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Admin/GetCustomers.cshtml", typeof(AspNetCore.Views_Admin_GetCustomers))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "C:\Users\Filip\source\repos\Shop\OnlineShop\ShopFilip\Views\_ViewImports.cshtml"
using ShopFilip;

#line default
#line hidden
#line 2 "C:\Users\Filip\source\repos\Shop\OnlineShop\ShopFilip\Views\_ViewImports.cshtml"
using ShopFilip.Models;

#line default
#line hidden
#line 1 "C:\Users\Filip\source\repos\Shop\OnlineShop\ShopFilip\Views\Admin\GetCustomers.cshtml"
using ShopFilip.IdentityModels;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"2616f2fb73db25c540c7d9ebeb3c5e47786d608b", @"/Views/Admin/GetCustomers.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6e02f5193c6eae5364ab7ffd099dfce3867fd7b7", @"/Views/_ViewImports.cshtml")]
    public class Views_Admin_GetCustomers : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<List<ApplicationUser>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 3 "C:\Users\Filip\source\repos\Shop\OnlineShop\ShopFilip\Views\Admin\GetCustomers.cshtml"
  
    Layout = "_Layout_Dashboard";

#line default
#line hidden
            BeginContext(105, 400, true);
            WriteLiteral(@"<div class=""container"">
    <h2>Klienci</h2>
    <div class=""table-responsive"">
        <table class=""table table-striped table-sm"">
            <thead>
                <tr>
                    <th>Imie</th>
                    <th>Nazwisko</th>
                    <th>Nick</th>
                    <th>Numer telefonu</th>
                </tr>
            </thead>
            <tbody>
");
            EndContext();
#line 19 "C:\Users\Filip\source\repos\Shop\OnlineShop\ShopFilip\Views\Admin\GetCustomers.cshtml"
                 foreach (var item in Model)
                {

#line default
#line hidden
            BeginContext(570, 84, true);
            WriteLiteral("                    <tr>\r\n                        <td>\r\n                            ");
            EndContext();
            BeginContext(655, 39, false);
#line 23 "C:\Users\Filip\source\repos\Shop\OnlineShop\ShopFilip\Views\Admin\GetCustomers.cshtml"
                       Write(Html.DisplayFor(modelItem => item.Name));

#line default
#line hidden
            EndContext();
            BeginContext(694, 91, true);
            WriteLiteral("\r\n                        </td>\r\n                        <td>\r\n                            ");
            EndContext();
            BeginContext(786, 42, false);
#line 26 "C:\Users\Filip\source\repos\Shop\OnlineShop\ShopFilip\Views\Admin\GetCustomers.cshtml"
                       Write(Html.DisplayFor(modelItem => item.Surname));

#line default
#line hidden
            EndContext();
            BeginContext(828, 91, true);
            WriteLiteral("\r\n                        </td>\r\n                        <td>\r\n                            ");
            EndContext();
            BeginContext(920, 43, false);
#line 29 "C:\Users\Filip\source\repos\Shop\OnlineShop\ShopFilip\Views\Admin\GetCustomers.cshtml"
                       Write(Html.DisplayFor(modelItem => item.UserName));

#line default
#line hidden
            EndContext();
            BeginContext(963, 91, true);
            WriteLiteral("\r\n                        </td>\r\n                        <td>\r\n                            ");
            EndContext();
            BeginContext(1055, 46, false);
#line 32 "C:\Users\Filip\source\repos\Shop\OnlineShop\ShopFilip\Views\Admin\GetCustomers.cshtml"
                       Write(Html.DisplayFor(modelItem => item.PhoneNumber));

#line default
#line hidden
            EndContext();
            BeginContext(1101, 60, true);
            WriteLiteral("\r\n                        </td>\r\n                    </tr>\r\n");
            EndContext();
#line 35 "C:\Users\Filip\source\repos\Shop\OnlineShop\ShopFilip\Views\Admin\GetCustomers.cshtml"
                }

#line default
#line hidden
            BeginContext(1180, 60, true);
            WriteLiteral("            </tbody>\r\n        </table>\r\n    </div>\r\n</div>\r\n");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<List<ApplicationUser>> Html { get; private set; }
    }
}
#pragma warning restore 1591
