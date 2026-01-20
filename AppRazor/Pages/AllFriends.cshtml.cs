using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;
using Models.Interfaces;

namespace AppRazor.Pages;

public class AllFriends : PageModel
{
    private readonly IFriendsService _friendsService;

    // DI via konstruktorn
    public AllFriends(IFriendsService friendsService)
    {
        _friendsService = friendsService;
    }

    // Lista över alla friends som ska visas
    public List<IFriend> Friends { get; set; } = new List<IFriend>();

    // Meddelande till användaren (valfritt)
    public string? Message { get; set; }

    public async Task OnGetAsync()
    {
        // För att hämta hem vänner från databasen
        var seededFriends = await _friendsService.ReadFriendsAsync(
            seeded: true,
            flat: false,
            filter: "",
            pageNumber: 0,
            pageSize: 100
        );

        // För att visa uppdaterade vänner i databasen
        var editedFriends = await _friendsService.ReadFriendsAsync(
            seeded: false,
            flat: false,
            filter: "",
            pageNumber: 0,
            pageSize: 100
        );

        Friends = seededFriends.PageItems
            .Concat(editedFriends.PageItems)
            .OrderBy(f => f.LastName)
            .ThenBy(f => f.FirstName)
            .ToList();
    }
}
