//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using OnlineStore.Domain.Core.Contract.IAppService;
//using OnlineStore.Domain.Core.Dtos;
//using OnlineStore.Domain.Core.enums;
//using System.ComponentModel.DataAnnotations;

//namespace OnlineStore.Web.Pages
//{
//    public class LoginModel : PageModel
//    {
//        private readonly IUserAppService _userAppService;
//        private readonly ILogger<LoginModel> _logger;

//        public LoginModel(IUserAppService userAppService, ILogger<LoginModel> logger)
//        {
//            _userAppService = userAppService;
//            _logger = logger;
//        }

//        [BindProperty]
//        [Required(ErrorMessage = "نام کاربری الزامی است.")]
//        [StringLength(150, ErrorMessage = "نام کاربری نمی‌تواند بیشتر از {1} کاراکتر باشد.")]
//        public string Username { get; set; }

//        [BindProperty]
//        [Required(ErrorMessage = "رمز عبور الزامی است.")]
//        [DataType(DataType.Password)]
//        public string Password { get; set; }

//        public string? ErrorMessage { get; set; }

//        public void OnGet()
//        {
//        }

//        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
//        {
//            if (!ModelState.IsValid)
//            {
//                _logger.LogWarning("Login form invalid. Errors: {Errors}",
//                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
//                return Page();
//            }

//            var dto = new UserLoginDto
//            {
//                Username = Username.Trim(),
//                Password = Password
//            };

//            var result = await _userAppService.LoginAsync(dto, cancellationToken);

//            if (!result.IsSuccess)
//            {
//                _logger.LogWarning("Login failed for user '{Username}': {Message}", Username, result.Message);
//                ErrorMessage = result.Message;
//                return Page();
//            }

//            var user = result.Data!;
//            _logger.LogInformation("User '{Username}' logged in successfully. UserId={UserId}", user.Username, user.Id);

//            HttpContext.Session.SetInt32("UserId", user.Id);
//            HttpContext.Session.SetInt32("UserRole", (int)user.Role);


//            if (user.Role == UserRole.Admin)
//            {
//                return RedirectToPage("/Admin/Dashboard");
//            }
//            else
//            {
//                return RedirectToPage("/Index"); 
//            }

//        }
//    }
//}
