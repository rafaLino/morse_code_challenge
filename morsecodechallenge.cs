 [Fact]
        public void test()
        {
            var signals = ".";
            var result = MorseTranslator.GetResult(signals);

            Console.WriteLine(result);
        }



        public class MorseTranslator
        {
            
            private Signal root = new Signal();

            private static char[] ENGLISH = {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l',
    'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x',
    'y', 'z', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0',
    ',', '.', '?'};

            private static string[] MORSE = {".-", "-...", "-.-.", "-..", ".", "..-.", "--.", "....", "..",
    ".---", "-.-", ".-..", "--", "-.", "---", ".---.", "--.-", ".-.",
    "...", "-", "..-", "...-", ".--", "-..-", "-.--", "--..", ".----",
    "..---", "...--", "....-", ".....", "-....", "--...", "---..", "----.",
    "-----", "--..--", ".-.-.-", "..--.."};

            private MorseTranslator()
            {
                for (int i = 0; i < MORSE.Length; i++)
                {
                    Insert(MORSE[i], ENGLISH[i]);
                }
            }

            private Signal Insert(string signal, char alpha)
            {
                Signal currentNode = root;
                for (int i = 0; i < signal.Length; i++)
                {
                    var step = signal.ElementAt(i);

                    if (step == '.')
                    {
                        if (currentNode.Dot == null)
                        {
                            currentNode.Dot = new Signal();
                            currentNode.Dot.Morse = signal.Substring(0, i + 1);
                        }
                        currentNode = currentNode.Dot;
                    }
                    else
                    {
                        if (currentNode.Dash == null)
                        {
                            currentNode.Dash = new Signal();
                            currentNode.Dash.Morse = signal.Substring(0, i + 1);
                        }
                        currentNode = currentNode.Dash;
                    }
                }
                currentNode.Symbol = alpha.ToString().ToUpper();
                currentNode.Morse = signal;
                return currentNode;
            }

            
            public static string[] GetResult(string signals)
            {
                return new MorseTranslator().FuzzyMatch(signals).Select(x => x.Symbol).ToArray();
            }

            private void AddToSearch(LinkedList<Signal> signals, Signal node)
            {
                if (node is not null)
                    signals.AddFirst(node);
            }

            private List<Signal> FuzzyMatch(string signal)
            {
                var result = new List<Signal>();
                var toSearch = new LinkedList<Signal>();

                toSearch.AddLast(root);

                while (toSearch.Any())
                {
                    var curr = toSearch.First.Value;
                    toSearch.RemoveFirst();

                    if (curr.Morse?.Length == signal.Length && curr.Symbol != null)
                    {
                        result.Add(curr);
                        continue;
                    }

                    switch (signal.ElementAt(curr.Morse == null ? 0 : curr.Morse.Length))
                    {
                        case '.':
                            AddToSearch(toSearch, curr.Dot);
                            break;
                        case '-':
                            AddToSearch(toSearch, curr.Dash);
                            break;
                        default:
                            AddToSearch(toSearch, curr.Dash);
                            AddToSearch(toSearch, curr.Dot);
                            break;
                    }
                }
                return result;
            }
        }

        public class Signal
        {
            public string Morse { get; set; }

            public string Symbol { get; set; }
            public Signal Dash { get; set; }

            public Signal Dot { get; set; }
        }