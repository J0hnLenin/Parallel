#define _CRT_SECURE_NO_WARNINGS
#define _CRT_SECURE_NO_DEPRECATE

#include <iostream>
#include <cstdio>
#include <cstring>
#include <unordered_map>
#include <omp.h>
#include <vector>

using namespace std;
const char* pathToFile = ".log";

void readLogFile()
{
    FILE* logFile = fopen(pathToFile, "r");
    if (!logFile)
    {
        cout << "Open file error" << endl;
        return;
    }
    char line[1024];
    unordered_map<string, int> methods = { {"GET", 0}, {"POST", 0}, {"PUT", 0}, {"DELETE", 0}, {"HEAD", 0} };
    vector<string> logLines;
    while (fgets(line, sizeof(line), logFile))
    {
        logLines.push_back(line);
    }
    fclose(logFile);
    double time = omp_get_wtime();
    for (size_t i = 0; i < logLines.size(); ++i)
    {
        const char* method = strstr(logLines[i].c_str(), "GET") ? "GET"
            : strstr(logLines[i].c_str(), "POST") ? "POST"
            : strstr(logLines[i].c_str(), "PUT") ? "PUT"
            : strstr(logLines[i].c_str(), "DELETE") ? "DELETE"
            : strstr(logLines[i].c_str(), "HEAD") ? "HEAD"
            : nullptr;

        if (method)
        {
            methods[method]++;
        }
    }
    cout << endl << 1 << " thread results:" << endl;
    for (const auto& pair : methods)
    {
        cout << pair.first << ": " << pair.second << endl;
    }
    cout << "Time:  " << (omp_get_wtime() - time)*1000 << " ms" << endl;
}

void readLogFileParallel(int nThreads)
{
    FILE* logFile = fopen(pathToFile, "r");
    if (!logFile)
    {
        cout << "Open file error" << endl;
        return;
    }

    char line[1024];
    unordered_map<string, int> methods = {{"GET", 0}, {"POST", 0}, {"PUT", 0}, {"DELETE", 0}, {"HEAD", 0} };
    vector<string> logLines;
    while (fgets(line, sizeof(line), logFile))
    {
        logLines.push_back(line);
    }
    fclose(logFile);

    double time = omp_get_wtime();
#pragma omp parallel num_threads(nThreads)
    {
        unordered_map<string, int> localMethods = {{"GET", 0},
                                                    {"POST", 0},
                                                    {"PUT", 0},
                                                    {"DELETE", 0},
                                                    {"HEAD", 0} };

#pragma omp for
        for (size_t i = 0; i < logLines.size(); ++i)
        {
            const char* method = strstr(logLines[i].c_str(), "GET") ? "GET"
                : strstr(logLines[i].c_str(), "POST") ? "POST"
                : strstr(logLines[i].c_str(), "PUT") ? "PUT"
                : strstr(logLines[i].c_str(), "DELETE") ? "DELETE"
                : strstr(logLines[i].c_str(), "HEAD") ? "HEAD"
                : nullptr;

            if (method)
            {
                localMethods[method]++;
            }
        }

#pragma omp critical
        {
            for (const auto& pair : localMethods)
            {
                methods[pair.first] += pair.second;
            }
        }
    }

    cout << endl << nThreads << " threads results:" << endl;
    for (const auto& pair : methods)
    {
        cout << pair.first << ": " << pair.second << endl;
    }
    cout << "Time:  " << (omp_get_wtime() - time) * 1000 << " ms" << endl;
}

int main()
{
    readLogFile();
    readLogFileParallel(2);
    readLogFileParallel(4);
    readLogFileParallel(8);
    readLogFileParallel(12);
    return 0;
}