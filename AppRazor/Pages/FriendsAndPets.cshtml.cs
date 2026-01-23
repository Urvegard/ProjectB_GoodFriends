using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;
using Models.Interfaces;

namespace AppRazor.Pages;

public class FriendsAndPets : PageModel
{
    private readonly IFriendsService _friendsService;

    public FriendsAndPets(IFriendsService friendsService)
    {
        _friendsService = friendsService;
    }

    // Alla friends som matchar filtret
    public List<IFriend> Friends { get; set; } = new();
    public List<string> Countries { get; set; } = new();

    // Vald 'Country' från dropdown 
    [BindProperty(SupportsGet = true)]
    public string? SelectedCountry { get; set; }

    // Meddelande till användare
    public string? Message { get; set; }

    public async Task OnGetAsync()
    {
        try
        {
            var result = await _friendsService.ReadFriendsAsync(
                seeded: true,
                flat: false,
                filter: "",
                pageNumber: 0,
                pageSize: 100
            );

            var allFriends = result.PageItems
                .Where(f =>
                    f.Pets != null &&
                    f.Pets.Any() &&
                    f.Address != null &&
                    f.Address.Country != null &&
                    f.Address.City != null)
                .ToList();

            // Alla länder till dropdown 
            Countries = allFriends
                .Select(f => f.Address.Country!)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            // Filtrera på valt land
            if (!string.IsNullOrEmpty(SelectedCountry))
            {
                Friends = allFriends
                    .Where(f => f.Address.Country == SelectedCountry)
                    .ToList();
            }
            else
            {
                Friends = allFriends;
            }
        }
        catch (Exception ex)
        {
            Message = $"Ett fel uppstod: {ex.Message}";
        }
    }
}