using Microsoft.AspNetCore.Mvc;

namespace Algorithms.Api.Controllers;

[ApiController]
[Route("api/fibonacci")]
public class FibonacciController : ControllerBase
{
    [HttpGet("recursive/{n}")]
    public IActionResult RecursiveFibonacci(int n)
    {            
        var performance = Fibonacci.MeasurePerformance(Fibonacci.RecursiveFib, n);

        return Ok(performance); 
    }

    [HttpGet("memoization/{n}")]
    public IActionResult MemoizationFibonacci(int n)
    {            
        var performance = Fibonacci.MeasurePerformance(Fibonacci.MemoizationFib, n);

        return Ok(performance);
    }

    [HttpGet("iterative/{n}")]
    public IActionResult IterativeFibonacci(int n)
    {       
        var performance = Fibonacci.MeasurePerformance(Fibonacci.IterativeFib, n);

        return Ok(performance);
    }

}
