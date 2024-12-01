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
            Task1();
        }

        private async Task Task1()
        {
            output1.Text = "";
            await Task.Run(() =>
            {
                string[] fileEntries = Directory.GetFiles("../../../library");
                foreach (string fileName in fileEntries)
                {
                    var stopwatch = Stopwatch.StartNew();
                    string result = "";
                    int[] globalCounter = new int[input1.Lines.Length];
                    for (int i = 0; i < input1.Lines.Length; i++)
                    {
                        globalCounter[i] = 0;
                    }
                    result += $"{fileName}\r\n";

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
                    //ParallelOptions options = new() { MaxDegreeOfParallelism = 4 };
                    //Parallel.ForAsync(0, linesArray.Length, options, async (lineIndex, cancellationToken) =>
                    //{
                    //    for (int checkIndex = 0; checkIndex < input1.Lines.Length; checkIndex++)
                    //    {
                    //        int idx = checkIndex;
                    //        lineCounter[lineIndex, idx] = await Task.Run(() =>
                    //        {
                    //            return Convert.ToInt32(
                    //                linesArray[lineIndex].Contains(input1.Lines[idx])
                    //                );
                    //        });
                    //    }
                    //});
                    for (int lineIndex = 0; lineIndex < linesArray.Length; lineIndex++)
                    {
                        for (int checkIndex = 0; checkIndex < input1.Lines.Length; checkIndex++)
                        {
                            lineCounter[lineIndex, checkIndex] = Convert.ToInt32(
                                linesArray[lineIndex].Contains(input1.Lines[checkIndex]));
                        }
                    }

                    // Reduce - параллельно собираем результаты в общий буфер
                    //Parallel.For(0, input1.Lines.Length, checkIndex =>
                    //{
                    //    for (int lineIndex = 0; lineIndex < linesArray.Length; lineIndex++)
                    //    {
                    //        globalCounter[checkIndex] += lineCounter[lineIndex, checkIndex];
                    //    }
                    //});
                    for (int checkIndex = 0; checkIndex < input1.Lines.Length; checkIndex++)
                    {
                        for (int lineIndex = 0; lineIndex < linesArray.Length; lineIndex++)
                        {
                            globalCounter[checkIndex] += lineCounter[lineIndex, checkIndex];
                        }
                    }

                    // Вывод результата
                    for (int i = 0; i < input1.Lines.Length; i++)
                    {
                        result += $"{input1.Lines[i]}: {globalCounter[i]}\r\n";
                    }
                    stopwatch.Stop();
                    result += $"Время обработки: {stopwatch.ElapsedMilliseconds} мс\r\n\r\n";
                    output1.AppendText(result);
                }
            });
            
        }
    }
}
