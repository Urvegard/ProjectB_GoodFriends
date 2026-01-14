using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;
using Models.Interfaces;

namespace AppRazor.Pages;

public class FriendsInDetail : PageModel
{
    private readonly IFriendsService _friendsService;

    public FriendsInDetail(IFriendsService friendsService)
    {
        _friendsService = friendsService;
    }

    // En vän att visa
    public IFriend? Friend { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid friendId)
    {
        var response = await _friendsService.ReadFriendAsync(friendId, flat: false);

        if (response.Item == null)
            return NotFound();

        Friend = response.Item; 

        return Page();
    }
}
