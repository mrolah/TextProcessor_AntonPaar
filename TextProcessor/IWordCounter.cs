using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextProcessor
{
    /// <summary>
    /// Represents an interface for Word Counting in String
    /// </summary>
    public interface IWordCounter
    {
        Dictionary<string, int> CountWords(IEnumerable<string> words);
    }
}
