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

    public void OnGet(){}
    
    public async Task<IActionResult> OnPostSeedAsync()
    {
        try
        {
            // Rensar befintlig data i databasen först
            await _adminService.RemoveSeedAsync(seeded: true);

            // Seedar om databasen med 100 nya vänner
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
