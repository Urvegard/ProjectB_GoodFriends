using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;
using Models.Interfaces;
using System.Data.Common;

namespace AppRazor.Pages;

public class FriendsByCountry : PageModel
{
    private readonly IFriendsService _friendsService;

    // DI via konstruktorn
    public FriendsByCountry(IFriendsService friendsService)
    {
        _friendsService = friendsService;
    }

    // Lista över alla friends som ska visas
    public List<IFriend> Friends { get; set; } = new List<IFriend>();
    public List<IGrouping<string, IFriend>> FriendsByCountries { get; set; } = new(); 

    // Meddelande till användare
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
                pageSize: 100 
            );
            
            FriendsByCountries = result.PageItems.Where(f => f.Address != null)
            .GroupBy(x => x.Address.Country)
            .ToList();

            Friends = result.PageItems;
        }
        catch (Exception ex)
        {
            Message = $"Ett fel uppstod: {ex.Message}";
        }
    }
}
