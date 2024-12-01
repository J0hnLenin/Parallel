using System;
using System.IO;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;
using static System.Windows.Forms.LinkLabel;
namespace Lab07
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private async void task1Button_Click(object sender, EventArgs e)
        {
            Task task1 = new Task(() => Task1());
            task1.Start();
        }

        private async Task Task1()
        {
            input1.Enabled = false;
            output1.Clear();

            await Task.Run(() =>
            {
                string[] fileEntries = Directory.GetFiles("../../../library");
                // последовательная обработка файлов
                foreach (string fileName in fileEntries)
                {
                    var stopwatch = Stopwatch.StartNew();
                    int[] globalCounter = new int[input1.Lines.Length];
                    for (int i = 0; i < input1.Lines.Length; i++)
                    {
                        globalCounter[i] = 0;
                    }
                    output1.AppendText($"{fileName}\r\n");

                    // Считываем файл в память
                    List<string> lines = new List<string>();
                    using (StreamReader streamReader = new StreamReader(fileName))
                    {
                        while (!streamReader.EndOfStream)
                        {
                            lines.Add(streamReader.ReadLine());
                        }
                    }
                    string[] linesArray = lines.ToArray();

                    // Создаём массив для сохранения результатов параллельной обработки
                    int[,] lineCounter = new int[linesArray.Length, input1.Lines.Length];

                    // Map - параллельно обрабатываем считанные строки
                    ParallelOptions options = new() { MaxDegreeOfParallelism = 2 };
                    Parallel.For(0, linesArray.Length, options, lineIndex =>
                    {
                        int idx = lineIndex;
                        for (int checkIndex = 0; checkIndex < input1.Lines.Length; checkIndex++)
                        {
                            lineCounter[idx, checkIndex] = Convert.ToInt32(
                                    linesArray[idx].Contains(input1.Lines[checkIndex]));
                        }
                    });

                    // Reduce - параллельно собираем результаты в общий буфер
                    Parallel.For(0, input1.Lines.Length, options, checkIndex =>
                    {
                        int idx = checkIndex;
                        for (int lineIndex = 0; lineIndex < linesArray.Length; lineIndex++)
                        {
                            globalCounter[idx] += lineCounter[lineIndex, idx];
                        }
                    });

                    // Вывод результата
                    for (int i = 0; i < input1.Lines.Length; i++)
                    {
                        output1.AppendText($"{input1.Lines[i]}: {globalCounter[i]}\r\n");
                    }
                    stopwatch.Stop();
                    output1.AppendText($"Время обработки: {stopwatch.ElapsedMilliseconds} мс\r\n\r\n");
                }
                output1.AppendText("TASK DONE\r\n\r\n");
                input1.Enabled = true;
            });
        }

        private void output1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
