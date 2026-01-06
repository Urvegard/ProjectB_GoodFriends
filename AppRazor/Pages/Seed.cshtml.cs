using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;
using Models.Interfaces;

namespace AppRazor.Pages;

public class SeedModel : PageModel
{
    private readonly IFriendsService _friendsService;

    // DI via konstruktorn
    public SeedModel(IFriendsService friendsService)
    {
        _friendsService = friendsService;
    }

    // Lista över alla friends som ska visas
    public List<IFriend> Friends { get; set; } = new List<IFriend>();

    // Meddelande till användaren (valfritt)
    public string? Message { get; set; }

    public async Task OnGetAsync()
    {
        try
        {
            // Hämta alla friends utan filter (seeded = true, flat = false, filter = "")
            var result = await _friendsService.ReadFriendsAsync(
                seeded: true, 
                flat: false, 
                filter: "", 
                pageNumber: 0, 
                pageSize: 1000 // tillräckligt stor för att hämta alla
            );

            Friends = result.PageItems;
        }
        catch (Exception ex)
        {
            // Om något går fel
            Message = $"Ett fel uppstod: {ex.Message}";
        }
    }
}
