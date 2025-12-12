using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OnlineStoreWeb.Pages
{
    public class OrderSuccessModel : PageModel
    {
        public int OrderId { get; private set; }

        public void OnGet(int id)
        {
            OrderId = id;
        }
    }
}