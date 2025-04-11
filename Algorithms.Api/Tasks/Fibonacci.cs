using Algorithms.Api.Helpers;
using System.Diagnostics;

namespace Algorithms.Api;

public static class Fibonacci
{
    // Recursive approach: O(2^n) time complexity, O(n) space complexity (call stack)
    public static long RecursiveFib(int n)
    {
        if (n <= 1)
            return n;
            
        return RecursiveFib(n - 1) + RecursiveFib(n - 2);
    }
    
    // Dynamic Programming with memoization: O(n) time complexity, O(n) space complexity
    public static long MemoizationFib(int n)
    {
        if (n <= 1)
            return n;
            
        var memo = new long[n + 1];
        memo[0] = 0;
        memo[1] = 1;
        
        return MemoizationFibHelper(n, memo);
    }
    
    private static long MemoizationFibHelper(int n, long[] memo)
    {
        // Base cases
        if (n <= 1)
            return n;
            
        // If value is already computed, return it
        if (memo[n] != 0)
            return memo[n];
            
        // Compute and store the value
        memo[n] = MemoizationFibHelper(n - 1, memo) + MemoizationFibHelper(n - 2, memo);
        return memo[n];
    }
    
    // Iterative approach: O(n) time complexity, O(1) space complexity
    public static long IterativeFib(int n)
    {
        if (n <= 1)
            return n;
            
        long a = 0;
        long b = 1;
        long result = 0;
        
        for (int i = 2; i <= n; i++)
        {
            result = a + b;
            a = b;
            b = result;
        }
        
        return result;
    }
    
    // Helper method to measure performance
    public static AlgorithmPerformance MeasurePerformance(Func<int, long> fibMethod, int n)
    {
        var stopwatch = new Stopwatch();
        
        stopwatch.Start();
        var result = fibMethod(n);
        stopwatch.Stop();
        
        return new AlgorithmPerformance
        {
            Result = result,
            ExecutionTimeMs = stopwatch.ElapsedMilliseconds,
        };
    }
}