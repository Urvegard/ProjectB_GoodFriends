using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;
using Models.DTO;
using Models.Interfaces;

namespace AppRazor.Pages;

public class EditFriend : PageModel
{
    private readonly IFriendsService _friendsService;
    private readonly IPetsService _petsService;
    private readonly IQuotesService _quotesService;

    public EditFriend(IFriendsService friendsService, IPetsService petsService, IQuotesService quotesService)
    {
        _friendsService = friendsService;
        _petsService = petsService;
        _quotesService = quotesService;
    }

    [BindProperty]
    public FriendCuDto? Friend { get; set; }
    public IFriend? FriendFull { get; set; } // den fullständiga vännen med Pets & Quotes
    
    public async Task<IActionResult> OnGetAsync(Guid friendId)
    {
        var response = await _friendsService.ReadFriendAsync(friendId, flat: false);
        if (response.Item == null)
            return NotFound();

        FriendFull = response.Item;

        // Mappar till DTO för formuläret
        Friend = new FriendCuDto(FriendFull);

        return Page();
    }
    // POST
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            // Uppdatera vän
            await _friendsService.UpdateFriendAsync(Friend);

            // Redirect tillbaka till detaljsidan för att se uppdateringen direkt
            return RedirectToPage("/FriendsInDetail", new { friendId = Friend?.FriendId });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Ett fel uppstod: {ex.Message}");
            return Page();
        }
    }

    // DELETE PET
    public async Task<IActionResult> OnPostDeletePetAsync(Guid petId)
    {
        if (petId != Guid.Empty)
        {
            await _petsService.DeletePetAsync(petId);
        }

        // Ladda om samma vän efter delete
        return RedirectToPage("/EditFriend", new { friendId = Friend?.FriendId });
    }

    // DELETE QUOTE
    public async Task<IActionResult> OnPostDeleteQuoteAsync(Guid quoteId)
    {
        if (quoteId == Guid.Empty)
            return BadRequest();

        await _quotesService.DeleteQuoteAsync(quoteId);

        // Ladda om sidan med uppdaterad data
        return RedirectToPage("/EditFriend", new { friendId = Friend?.FriendId });
    }
}
