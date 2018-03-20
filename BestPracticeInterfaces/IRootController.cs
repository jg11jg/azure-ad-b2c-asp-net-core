using Microsoft.AspNetCore.Mvc;

namespace BestPracticeInterfaces
{
    public interface IRootController
    {
        IActionResult Me();
    }
}
