using Microsoft.AspNetCore.Mvc;

namespace Algorithms.Api.Controllers;

[ApiController]
[Route("api/max-subarray")]
public class MaxSubarrayController : ControllerBase
{
    [HttpPost("bruteforce")]
    public IActionResult BruteForce([FromBody] int[] nums)
    {
        if (nums == null || nums.Length == 0)
        {
            return BadRequest("Input array cannot be null or empty");
        }

        var performance = MaxSubarray.MeasurePerformance(MaxSubarray.FindMaxSubarrayBruteForce, nums);
        
        return Ok(new
        {
            result = performance.Result,
            executionTimeMs = performance.ExecutionTimeMs,
            memoryUsedBytes = performance.MemoryUsedBytes
        });
    }

    [HttpPost("dp")]
    public IActionResult DynamicProgramming([FromBody] int[] nums)
    {
        if (nums == null || nums.Length == 0)
        {
            return BadRequest("Input array cannot be null or empty");
        }

        var performance = MaxSubarray.MeasurePerformance(MaxSubarray.FindMaxSubarrayDP, nums);
        
        return Ok(new
        {
            result = performance.Result,
            executionTimeMs = performance.ExecutionTimeMs,
            memoryUsedBytes = performance.MemoryUsedBytes
        });
    }

    [HttpPost("kadane")]
    public IActionResult Kadane([FromBody] int[] nums)
    {
        if (nums == null || nums.Length == 0)
        {
            return BadRequest("Input array cannot be null or empty");
        }

        var performance = MaxSubarray.MeasurePerformance(MaxSubarray.FindMaxSubarrayKadane, nums);
        
        return Ok(new
        {
            result = performance.Result,
            executionTimeMs = performance.ExecutionTimeMs,
            memoryUsedBytes = performance.MemoryUsedBytes
        });
    }
} 