#include <iostream>
#include <omp.h>

using namespace std;

void firstTask() {
    long long N;
    long long sum = 0;
    int num_threads = 2;
    printf("Task 1\n");
    printf("N: ");
    cin >> N;

    omp_set_dynamic(0);
    #pragma omp parallel num_threads(num_threads) reduction(+:sum)
    {
        int thread_id = omp_get_thread_num();
        if (thread_id == 0) { // бежим по нечётным
            for (int i = 1; i <= N; i += num_threads) {
                sum += i;
            }
        }
        else if(thread_id == 1) { // бежим по чётным
            for (int i = 2; i <= N; i += num_threads) {
                sum += i;
            }
        }
        printf("[%d]: Sum = %lld \n", thread_id, sum);
    }

    printf("Sum = %lld \n", sum);
    printf("Answer = %lld \n\n", ((1 + N) * N / 2));
}   

void secondTask() {
    long long N;
    long long sum = 0;
    int num_threads;

    printf("Task 2\n");
    printf("N: ");
    cin >> N;
    
    printf("Number of threads: ");
    cin >> num_threads;
    
    omp_set_dynamic(0);
    #pragma omp parallel num_threads(num_threads) reduction(+:sum)
    {
        int thread_id = omp_get_thread_num();
        for (int i = thread_id + 1; i <= N; i += num_threads) {
            sum += i;
        }

        printf("[%d]: Sum = %lld \n", thread_id, sum);
    }

    printf("Sum = %lld \n", sum);
    printf("Answer = %lld \n", ((1 + N) * N / 2));
}

void thirdTask() {
    long long N;
    long long sum = 0;
    int num_threads;

    printf("Task 3\n");
    printf("N: ");
    cin >> N;

    printf("Number of threads: ");
    cin >> num_threads;

    omp_set_dynamic(0);
    #pragma omp for 
    for (int i = omp_get_thread_num() + 1; i <= N; i += num_threads)
    {
        sum++;

        printf("[%d]: Sum = %lld \n", omp_get_thread_num(), sum);
    }
    


    printf("Sum = %lld \n", sum);
    printf("Answer = %lld \n", ((1 + N) * N / 2));
}
int main() {
    firstTask();
    secondTask();
    thirdTask();
    return 0;
}