using DivinationOnRunes.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DivinationOnRunes.Web.Pages;

public class IndexModel : PageModel
{
    private readonly DivinationService _divinationService;

    public IndexModel(DivinationService divinationService)
    {
        _divinationService = divinationService;
    }
    
    [BindProperty]
    public string Question { get; set; } = "";
    
    [BindProperty]
    public SpreadType SpreadType { get; set; } = SpreadType.Answer;
    
    public DivinationResult? Result { get; private set; }
    
    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if (string.IsNullOrWhiteSpace(Question))
        {
            ModelState.AddModelError(nameof(Question), "Пожалуйста, введи запрос.");
            return Page();
        }
        
        Result = _divinationService.Divine(Question, SpreadType);
        return Page();
    }
}