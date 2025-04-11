import http from 'k6/http';
import { check, sleep } from 'k6';
import { Trend, Rate } from 'k6/metrics';

// Custom metrics
const bruteForceTimeTrend = new Trend('bruteforce_response_time');
const dpTimeTrend = new Trend('dp_response_time');
const kadaneTimeTrend = new Trend('kadane_response_time');
const errorRate = new Rate('error_rate');

// Test configuration
export const options = {
  stages: [
    { duration: '30s', target: 10 }, // Ramp up to 10 users
    { duration: '1m', target: 10 },  // Stay at 10 users for 1 minute
    { duration: '30s', target: 0 }, // Ramp down to 0 users
  ],
  thresholds: {
    'http_req_duration': ['p(95)<500'], // 95% of requests should be below 500ms
    'error_rate': ['rate<0.1'],         // Error rate should be less than 10%
  },
};

// Define test scenarios for different implementations
const API_BASE_URL = 'http://localhost:5256/api';

// Input sizes for testing - we'll use smaller values for brute force to avoid timeouts
const BRUTE_FORCE_SIZES = [10, 50, 100, 200, 500, 1000, 2000];
const OPTIMIZED_SIZES = [10, 50, 100, 200, 500, 1000, 2000];

// Configuration flags
const RUN_BRUTE_FORCE = true;
const RUN_DP = true;
const RUN_KADANE = true;

// Generate test arrays of different sizes
function generateTestArray(size) {
  const array = [];
  for (let i = 0; i < size; i++) {
    // Generate random numbers between -100 and 100
    array.push(Math.floor(Math.random() * 201) - 100);
  }
  return array;
}

export default function () {
  // Test brute force implementation (with smaller sizes)
  if (RUN_BRUTE_FORCE) {
    console.log('Running brute force implementation');
    for (const size of BRUTE_FORCE_SIZES) {
      const testArray = generateTestArray(size);
      const bruteForceUrl = `${API_BASE_URL}/max-subarray/bruteforce`;
      const bruteForceRes = http.post(bruteForceUrl, JSON.stringify(testArray), {
        headers: { 'Content-Type': 'application/json' },
      });
      
      // Record metrics
      const bruteForceSuccess = check(bruteForceRes, {
        'brute force status 200': (r) => r.status === 200,
        'brute force valid response': (r) => {
          try {
            const data = JSON.parse(r.body);
            return data.result !== undefined;
          } catch (e) {
            return false;
          }
        }
      });
      
      if (bruteForceSuccess) {
        bruteForceTimeTrend.add(bruteForceRes.timings.duration, { size: size });
        console.log(`Brute Force (size ${size}): ${bruteForceRes.timings.duration}ms`);
      } else {
        errorRate.add(1);
        console.log(`Error with Brute Force (size ${size}): ${bruteForceRes.status}, ${bruteForceRes.body}`);
      }
    }
  }

  // Test DP implementation
  if (RUN_DP) {
    console.log('Running DP implementation');
    for (const size of OPTIMIZED_SIZES) {
      const testArray = generateTestArray(size);
      const dpUrl = `${API_BASE_URL}/max-subarray/dp`;
      const dpRes = http.post(dpUrl, JSON.stringify(testArray), {
        headers: { 'Content-Type': 'application/json' },
      });
      
      // Record metrics
      const dpSuccess = check(dpRes, {
        'DP status 200': (r) => r.status === 200,
        'DP valid response': (r) => {
          try {
            const data = JSON.parse(r.body);
            return data.result !== undefined;
          } catch (e) {
            return false;
          }
        }
      });
      
      if (dpSuccess) {
        dpTimeTrend.add(dpRes.timings.duration, { size: size });
        console.log(`DP (size ${size}): ${dpRes.timings.duration}ms`);
      } else {
        errorRate.add(1);
        console.log(`Error with DP (size ${size}): ${dpRes.status}, ${dpRes.body}`);
      }
    }
  }
  
  // Test Kadane's algorithm
  if (RUN_KADANE) {
    console.log('Running Kadane implementation');
    for (const size of OPTIMIZED_SIZES) {
      const testArray = generateTestArray(size);
      const kadaneUrl = `${API_BASE_URL}/max-subarray/kadane`;
      const kadaneRes = http.post(kadaneUrl, JSON.stringify(testArray), {
        headers: { 'Content-Type': 'application/json' },
      });
      
      // Record metrics
      const kadaneSuccess = check(kadaneRes, {
        'Kadane status 200': (r) => r.status === 200,
        'Kadane valid response': (r) => {
          try {
            const data = JSON.parse(r.body);
            return data.result !== undefined;
          } catch (e) {
            return false;
          }
        }
      });
      
      if (kadaneSuccess) {
        kadaneTimeTrend.add(kadaneRes.timings.duration, { size: size });
        console.log(`Kadane (size ${size}): ${kadaneRes.timings.duration}ms`);
      } else {
        errorRate.add(1);
        console.log(`Error with Kadane (size ${size}): ${kadaneRes.status}, ${kadaneRes.body}`);
      }
    }
  }
} 