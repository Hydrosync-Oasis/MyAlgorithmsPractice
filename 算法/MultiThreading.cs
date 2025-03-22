using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 算法题目 {
public class FizzBuzz {
        private int n;
        private int cur = 0;
        private volatile bool flag = true;

        private int next { get => cur + 1; }

        Semaphore fizz = new(0, 1);
        Semaphore buzz = new(0, 1);
        Semaphore fizzbuzz = new(0, 1);
        Semaphore num = new(1, 1);

        public FizzBuzz(int n) {
            this.n = n;
        }

        // printFizz() outputs "fizz".
        public void Fizz(Action printFizz) {
            while (flag) {
                fizz.WaitOne();
                if (!flag) {
                    return;
                }
                cur++;
                printFizz();
                if (Stop()) { return; }

                Release();
            }
        }

        // printBuzz() outputs "buzz".
        public void Buzz(Action printBuzz) {
            while (flag) {
                buzz.WaitOne();
                if (!flag) {
                    return;
                }

                cur++;
                printBuzz();
                if (Stop()) { return; }

                Release();
            }
        }

        // printFizzBuzz() outputs "fizzbuzz".
        public void Fizzbuzz(Action printFizzBuzz) {
            while (flag) {
                fizzbuzz.WaitOne();
                if (!flag) { return; }

                cur++;
                printFizzBuzz();
                if (Stop()) { return; }

                Release();
            }
        }

        // printNumber(x) outputs "x", where x is an integer.
        public void Number(Action<int> printNumber) {
            while (flag) {
                num.WaitOne();
                if (!flag) { return; }

                cur++;
                printNumber(cur);
                if (Stop()) { return; }

                Release();
            }

        }

        private bool Stop() {
            if (cur == n) {
                flag = false;
                fizz.Release();
                buzz.Release();
                fizzbuzz.Release();
                num.Release();
                return true;
            }

            return false;
        }

        private void Release() {
            if (next % 3 == 0 && next % 5 != 0) {
                fizz.Release();
            } else if (next % 15 == 0) {
                fizzbuzz.Release();
            } else if (next % 5 == 0 && next % 3 != 0) {
                buzz.Release();
            } else {
                num.Release();
            }

        }
    }
}
