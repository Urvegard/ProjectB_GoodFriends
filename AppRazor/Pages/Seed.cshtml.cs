using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;
using Models.Interfaces;

namespace AppRazor.Pages;

public class SeedModel : PageModel
{
    private readonly IFriendsService _friendsService;

    public SeedModel(IFriendsService friendsService)
    {
        _friendsService = friendsService;
    }

    // Lista med alla friends som hämtas från databasen
    public List<IFriend> Friends { get; set; } = new List<IFriend>();

    // Meddelande till användaren
    public string? Message { get; set; }

    // Körs när sidan laddas
    public async Task OnGetAsync()
    {
        try
        {
            // Läs alla friends utan filter (seeded = true/false, flat = true för simplifierad DTO)
            var response = await _friendsService.ReadFriendsAsync(
                seeded: true,      // eller false om du vill hämta allt
                flat: true,        // returnerar “platt” version utan navigation properties
                filter: "",        // tomt filter = alla
                pageNumber: 0,
                pageSize: 1000     // stort nummer för att hämta alla på en gång
            );

            Friends = response.PageItems.ToList();
            Message = $"Hittade {Friends.Count} friends i databasen.";
        }
        catch (Exception ex)
        {
            Message = $"Fel vid hämtning av friends: {ex.Message}";
        }
    }
}
