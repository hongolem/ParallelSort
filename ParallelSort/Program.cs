/*
Thread thread = new Thread(new ThreadStart(DoWork));

thread.Start();

for (int i = 0; i < 5; i++)
{
    Console.WriteLine("Hlavní vlákno: " + i);
    Thread.Sleep(1000);
}

static void DoWork()
{
    for (int i = 0; i < 5; i++)
    {
        Console.WriteLine("Vedlejší vlákno: " + i);
        Thread.Sleep(1000);
    }
}
*/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

public class BubbleSortBenchmark
{
    private List<string> words;

    [GlobalSetup]
    public void Setup()
    {
        words = new List<string> {
            "zebra", "apple", "mango", "banana", "grape", "orange", "kiwi", "lemon", "strawberry", "blueberry", "raspberry", "peach", "pineapple", "cherry", "papaya", "plum", "watermelon", "coconut", "nectarine", "grapefruit", "banana", "apple", "orange", "kiwi", "grape", "mango", "pineapple", "strawberry", "elephant", "balloon", "xylophone", "pizza", "zebra", "waterfall", "zebra", "quintessential", "banana", "charming", "juxtapose", "mountain", "ambiguous", "extravagant", "pencil", "umbrella", "kangaroo", "xylophone", "daring", "mysterious", "cucumber", "avocado", "yesterday", "grape", "orange", "apple", "elaborate", "fracture", "horizon", "incredible", "jovial", "knowledge", "lightning", "mango", "noble", "obscure", "pineapple", "quarantine", "raspberry", "strawberry", "trivial", "umbrella", "vacation", "whisper", "zealous", "adventure", "butterfly", "cloud", "dragon", "entertain", "fascinate", "glimmer", "harvest", "inspire", "jubilant", "kindness", "luminous", "magnificent", "nostalgia", "ocean", "paradox", "quirky", "rhapsody", "serendipity", "tangerine", "unique", "vortex", "wanderlust", "xenon", "youthful", "zephyr", "azure", "breeze", "cascade", "delight", "echo", "firefly", "garden", "harmony", "illusion", "journey", "kaleidoscope", "labyrinth", "moonlight", "nebula", "opal", "phoenix", "quiver", "radiance", "solitude", "tranquility", "utopia", "vivid", "whimsical", "xenophobia", "yearn", "zenith", "bliss", "courage", "destiny", "eclipse", "fable", "galaxy", "hope", "infinity", "jubilation", "karma"
        };
    }

    [Benchmark]
    public void SerialBubbleSort()
    {
        List<string> wordsForSerialSort = new List<string>(words);
        SerialBubbleSortMethod(wordsForSerialSort);
    }

    [Benchmark]
    public void ParallelBubbleSort()
    {
        List<string> wordsForParallelSort = new List<string>(words);
        ParallelBubbleSortMethod(wordsForParallelSort);
    }

    public static void SerialBubbleSortMethod(List<string> words)
    {
        int n = words.Count;
        bool swapped;

        for (int i = 0; i < n - 1; i++)
        {
            swapped = false;

            for (int j = 0; j < n - i - 1; j++)
            {
                if (string.Compare(words[j], words[j + 1], StringComparison.OrdinalIgnoreCase) > 0)
                {
                    string temp = words[j];
                    words[j] = words[j + 1];
                    words[j + 1] = temp;
                    swapped = true;
                }
            }

            if (!swapped)
                break;
        }
    }

    public static void ParallelBubbleSortMethod(List<string> words)
    {
        int n = words.Count;
        bool swapped;

        for (int i = 0; i < n - 1; i++)
        {
            swapped = false;
            List<Thread> threads = new List<Thread>();
            for (int j = 0; j < n - i - 1; j++)
            {
                Thread thread = new Thread(() =>
                {
                    lock (words)
                    {
                        if (string.Compare(words[j], words[j + 1], StringComparison.OrdinalIgnoreCase) > 0)
                        {
                            string temp = words[j];
                            words[j] = words[j + 1];
                            words[j + 1] = temp;
                            swapped = true;
                        }
                    }
                });

                threads.Add(thread);
                thread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            if (!swapped)
                break;
        }
    }

    public static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<BubbleSortBenchmark>();
    }
}