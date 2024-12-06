using System;
using System.IO;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;
using static System.Windows.Forms.LinkLabel;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Security.Policy;

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

        public class Node
        {
            public string Url;
            public List<Node> Children;
            public bool Image;
            public Node(string url, bool image)
            {
                Image = image;
                Url = url;
                Children = new List<Node>();
            }
        }

        public class Crawler
        {
            private readonly HttpClient _httpClient;
            private readonly ConcurrentDictionary<string, bool> _visitedUrls;
            private readonly object _lockObject = new();
            private readonly System.Windows.Forms.TextBox _textBox;
            private string domainUrl;

            public Crawler(System.Windows.Forms.TextBox _textBox)
            {
                _httpClient = new HttpClient();
                _visitedUrls = new ConcurrentDictionary<string, bool>();
                this._textBox = _textBox;
            }

            public async Task<Node> CrawlAsync(string rootUrl, int maxDepth)
            {
                string[] splitUrl = rootUrl.Split(new[] { "://" }, StringSplitOptions.RemoveEmptyEntries);
                string protocol = splitUrl.Length > 1 ? splitUrl[0] : string.Empty;
                string domain = splitUrl.Length > 1 ? splitUrl[1].Split('/')[0] : string.Empty;

                this.domainUrl = $"{protocol}://{domain}";
                int len = rootUrl.Length;
                
                var rootNode = new Node(rootUrl, false);
                await CrawlUrlAsync(rootNode, rootUrl, 0, maxDepth);
                return rootNode;
            }

            public async Task CrawlUrlAsync(Node node, string url, int currentDepth, int maxDepth)
            {
                if (currentDepth >= maxDepth || _visitedUrls.ContainsKey(url)) return;

                _visitedUrls[url] = true;

                try
                {
                    var pageContent = await _httpClient.GetStringAsync(url);
                    var images = ExtractImages(pageContent);
                    foreach (var image in images)
                    {
                        if (!image.Contains("://"))
                            node.Children.Add(new Node(domainUrl + image, true)); 
                        else if (image.Contains(domainUrl))
                            node.Children.Add(new Node(image, true));
                    }

                    node.Children.Add(new Node(url, false)); // Сохраняем узел с URL

                    // Извлекаем ссылки
                    var links = ExtractLinks(pageContent);

                    var tasks = links.Select(link => Task.Run(() =>
                    {
                        if (link.Contains("://"))
                        {
                            return;
                        }
                        link = domainUrl + link;
                        var childNode = new Node(link, false);
                        node.Children.Add(childNode);
                        CrawlUrlAsync(childNode, link, currentDepth + 1, maxDepth).Wait();
                    }));

                    await Task.WhenAll(tasks);
                }
                catch (Exception ex)
                {
                    _textBox.AppendText($"\r\nОшибка при обработке {url}: {ex.Message}\r\n");
                }
            }

            private IEnumerable<string> ExtractLinks(string html)
            {
                var links = new List<string>();
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(html);
                var anchorTags = doc.DocumentNode.SelectNodes("//a[@href]");

                if (anchorTags != null)
                {
                    links.AddRange(anchorTags.Select(x => x.GetAttributeValue("href", string.Empty))
                                              .Where(link => !string.IsNullOrEmpty(link)));
                }
                return links;
            }

            private IEnumerable<string> ExtractImages(string html)
            {
                var images = new List<string>();
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(html);
                var imgTags = doc.DocumentNode.SelectNodes("//img[@src]");

                if (imgTags != null)
                {
                    images.AddRange(imgTags.Select(x => x.GetAttributeValue("src", string.Empty))
                                           .Where(src => !string.IsNullOrEmpty(src)));
                }
                return images;
            }

            public void PrintTree(Node node, int depth = 0)
            {
                if (node.Image)
                    _textBox.AppendText($"{node.Url}\r\n");
                foreach (var child in node.Children)
                {
                    PrintTree(child, depth + 1);
                }
            }
        }

        async Task Task2()
        {
            output2.Clear();

            string rootUrl = input2.Lines[0];

            int maxDepth = 2;

            var crawler = new Crawler(output2);
            var stopwatch = Stopwatch.StartNew();

            var tree = await crawler.CrawlAsync(rootUrl, maxDepth);

            stopwatch.Stop();
            
            crawler.PrintTree(tree);
            output2.AppendText($"Время обработки: {stopwatch.Elapsed}\r\n");
            output2.AppendText($"Высота дерева: {GetHeight(tree)}\r\n");
            output2.AppendText($"Количество узлов: {GetNodeCount(tree)}\r\n");
        }

        static int GetHeight(Node node)
        {
            if (node == null) return 0;

            int height = 0;
            foreach (var child in node.Children)
            {
                height = Math.Max(height, GetHeight(child));
            }
            return height + 1;
        }

        static int GetNodeCount(Node node)
        {
            if (node == null) return 0;

            int count = 1; // Считаем текущий узел
            foreach (var child in node.Children)
            {
                count += GetNodeCount(child);
            }
            return count;
        }
        

        private void input1_TextChanged(object sender, EventArgs e)
        {

        }

        private async void task2Button_Click(object sender, EventArgs e)
        {
            Task task2 = new Task(() => Task2());
            task2.Start();
        }
    }
}
