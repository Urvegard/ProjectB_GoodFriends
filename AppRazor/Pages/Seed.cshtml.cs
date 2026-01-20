using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace AppRazor.Pages;

public class Seed : PageModel
{
    private readonly IAdminService _adminService;

    public Seed(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [TempData]
    public string? Message { get; set; }

    public void OnGet()
    {
        // Ingen logik på GET
    }

    public async Task<IActionResult> OnPostSeedAsync()
    {
        try
        {
            await _adminService.RemoveSeedAsync(true);
            await _adminService.SeedAsync(100);

            Message = "Database reseeded with 100 friends!";
        }
        catch (Exception ex)
        {
            Message = $"SEED ERROR: {ex.Message}";
        }

        return RedirectToPage();
    }
}
