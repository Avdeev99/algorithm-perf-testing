# Algorithm Performance Testing

This project demonstrates the importance of algorithm time and space complexity using the Fibonacci sequence as an example.

## Implementations

The project includes three different Fibonacci implementations:

1. **Recursive** (O(2^n) time, O(n) space)
   - Simple but exponentially slow for larger inputs
   - Creates a large call stack

2. **Memoization** (O(n) time, O(n) space)
   - Uses dynamic programming to store previously calculated values
   - Much faster than recursive for larger inputs
   - Trades memory for speed

3. **Iterative** (O(n) time, O(1) space)
   - Most efficient implementation overall
   - Constant space complexity
   - Linear time complexity

## Testing Methods

### Unit Tests

The project includes unit tests that:
- Verify correctness of each implementation
- Demonstrate time complexity differences
- Compare memory usage

To run unit tests:
```
dotnet test
```

### K6 Performance Tests

To run the API performance tests with k6:

1. Start the API:
```
cd Algorithms.Api
dotnet run
```

2. In a separate terminal, run the k6 test:
```
k6 run scripts/k6-fibonacci-test.js
```

The k6 test will provide metrics on:
- Request durations for each implementation
- Error rates
- Comparative performance between implementations

## Interpreting the Results

### K6 Results

The k6 tests demonstrate:
- How response times scale with increasing input size for each algorithm
- How the recursive implementation quickly becomes unusable for larger inputs
- The relative performance advantage of the iterative implementation over memoization

### Business Impact

This demonstration shows why algorithm selection matters:
- Inefficient algorithms can lead to timeout errors, server crashes, and poor user experience
- Even for small inputs, the differences can be significant under load
- Proper algorithm selection has a direct impact on infrastructure costs and scalability

## Requirements

- .NET 8.0
- k6 (for performance testing)