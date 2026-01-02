using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services; // ditt service-lager namespace
using Models;
using Models.Interfaces;
using Services.Interfaces;   // Friend, Address, Pet osv.

namespace AppRazor.Pages;

public class SeedModel : PageModel
{
    private readonly IFriendsService _friendsService;

    public SeedModel(IFriendsService friendsService)
    {
        _friendsService = friendsService;
    }

    public List<IFriend> Friends { get; private set; } = new();

    public async Task OnGetAsync()
    {
        var resp = await _friendsService.ReadFriendsAsync(
            seeded: true,
            flat: false,
            filter: "",
            pageNumber: 0,
            pageSize: int.MaxValue
        );

        Friends = resp.PageItems ?? new List<IFriend>();
    }
}
