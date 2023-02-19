using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextProcessor
{
    /// <summary>
    /// Represents a class for Word Counting in String
    /// </summary>
    public class WordCounter : IWordCounter
    {
        public Dictionary<string, int> CountWords(IEnumerable<string> words)
        {
            return words.GroupBy(w => w)
                
                .ToDictionary(g => g.Key, g => g.Count());
                
        }
    }
}
