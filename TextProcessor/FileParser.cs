using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TextProcessor
{
    /// <summary>
    /// Represents a class for File Parsing
    /// </summary>
    public class FileParser : IFileParser
    {
        // Interface for Word Counting
        private readonly IWordCounter _wordCounter;

        // EventHandler for managing progress (0-100 %) of file parsing
        public event EventHandler<double> ProgressChanged;
        
        // File Reading Properties
        private readonly long _fileSize;
        private long _totalRead;
        private readonly StreamReader _streamReader;

        public FileParser()
        {
        }

        public FileParser(string filePath, IWordCounter wordCounter)
        {
            this._wordCounter = wordCounter;
            _streamReader = new StreamReader(filePath);
            _fileSize = new FileInfo(filePath).Length;
            _totalRead = 0;
        }

        public async Task<Dictionary<string, int>> ParseFile(CancellationToken cancellationToken)
        {
        
            var words = new List<string>();
            using (var reader = _streamReader)
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null && !cancellationToken.IsCancellationRequested)
                {
                    // Testing purposes - Easier to test cancel event
                    // Thread.Sleep(1000);

                    string[] chunkWords = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    words.AddRange(chunkWords);
                    _totalRead += line.Length;

                    // Invoking the event for managing progress (0-100 %) of file parsing
                    OnProgressChanged();

                }

                // Check if the total bytes read is equal to the file size, and update progress bar to 100% if necessary
                if (_totalRead < _fileSize)
                {
                    _totalRead = _fileSize;
                    OnProgressChanged();
                }


                // When process is cancelled
                if (cancellationToken.IsCancellationRequested)
                {
                    throw new OperationCanceledException
                        ($"File-read-async has been canceled by user !");
                }


                // Count the words in the List -> WordCounter
                return _wordCounter.CountWords(words);
            }
        }

        // Invoking the event for managing progress (0-100 %) of file parsing
        protected virtual void OnProgressChanged()
        {
            double percentComplete = _totalRead * 100.0 / _fileSize;

            ProgressChanged?
                .Invoke(this, percentComplete);
        }

    }
}

