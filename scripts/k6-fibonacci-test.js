import http from 'k6/http';
import { check, sleep } from 'k6';
import { Trend, Rate } from 'k6/metrics';

// Custom metrics
const recursiveTrend = new Trend('recursive_response_time');
const memoizationTrend = new Trend('memoization_response_time');
const iterativeTrend = new Trend('iterative_response_time');
const errorRate = new Rate('error_rate');

// Test configuration
export const options = {
  stages: [
    { duration: '30s', target: 10 }, // Ramp up to 10 users
    { duration: '1m', target: 10 },  // Stay at 10 users for 1 minute
    { duration: '30s', target: 0 }, // Ramp down to 0 users
  ],
  thresholds: {
    'http_req_duration': ['p(95)<300'], // 95% of requests should be below 2s
    'error_rate': ['rate<0.1'],          // Error rate should be less than 10%
  },
};

// Define test scenarios for different Fibonacci implementations
const API_BASE_URL = 'http://localhost:5256/api'; // Update with your API URL

// Input sizes for testing - we'll use small values for recursive to avoid timeouts
const RECURSIVE_INPUTS = [5, 10, 20, 30, 40, 50];
const OPTIMIZED_INPUTS = [5, 10, 20, 30, 40, 50];

const RUN_ITERATIVE = true;
const RUN_MEMOIZATION = true;
const RUN_RECURSIVE = true;

export default function () {
  // Test recursive implementation (with smaller n values)
  if (RUN_RECURSIVE) {
    console.log('Running recursive implementation');
    for (const n of RECURSIVE_INPUTS) {
      const recursiveUrl = `${API_BASE_URL}/fibonacci/recursive/${n}`;
      const recursiveRes = http.get(recursiveUrl);
      
      // Record metrics
      recursiveTrend.add(recursiveRes.timings.duration, { n: n });
      
      // Check response
      const recursiveSuccess = check(recursiveRes, {
        'recursive status 200': (r) => r.status === 200,
        'recursive result is valid': (r) => {
          try {
            const data = JSON.parse(r.body);
            return data.result !== undefined;
          } catch (e) {
            return false;
          }
        }
      });
      
      if (!recursiveSuccess) {
        errorRate.add(1);
        console.log(`Error with recursive n=${n}: ${recursiveRes.status}, ${recursiveRes.body}`);
      }
      
      sleep(0.5); // Short pause between requests
    }
  }

  // Test memoization implementation
  if (RUN_MEMOIZATION) {
    console.log('Running memoization implementation');
    for (const n of OPTIMIZED_INPUTS) {
      const memoUrl = `${API_BASE_URL}/fibonacci/memoization/${n}`;
      const memoRes = http.get(memoUrl);
      
      // Record metrics
      memoizationTrend.add(memoRes.timings.duration, { n: n });
      
      // Check response
      const memoSuccess = check(memoRes, {
        'memoization status 200': (r) => r.status === 200,
        'memoization result is valid': (r) => {
          try {
            const data = JSON.parse(r.body);
            return data.result !== undefined;
          } catch (e) {
            return false;
          }
        }
      });
      
      if (!memoSuccess) {
        errorRate.add(1);
        console.log(`Error with memoization n=${n}: ${memoRes.status}, ${memoRes.body}`);
      }
      
      sleep(0.5);
    }
  }
  
  // Test iterative implementation
  if (RUN_ITERATIVE) {
    console.log('Running iterative implementation');
    for (const n of OPTIMIZED_INPUTS) {
      const iterativeUrl = `${API_BASE_URL}/fibonacci/iterative/${n}`;
      const iterativeRes = http.get(iterativeUrl);
      
      // Record metrics
      iterativeTrend.add(iterativeRes.timings.duration, { n: n });
      
      // Check response
      const iterativeSuccess = check(iterativeRes, {
        'iterative status 200': (r) => r.status === 200,
        'iterative result is valid': (r) => {
          try {
            const data = JSON.parse(r.body);
            return data.result !== undefined;
          } catch (e) {
            return false;
          }
        }
      });
      
      if (!iterativeSuccess) {
        errorRate.add(1);
        console.log(`Error with iterative n=${n}: ${iterativeRes.status}, ${iterativeRes.body}`);
      }
      
      sleep(0.5);
    }
  }
} 