using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TextProcessor
{
    /// <summary>
    /// Represents an interface for File Parsing
    /// </summary>
    public interface IFileParser
    {
        /// <summary>
        /// EventHandler for managing progress (0-100 %) of file parsing
        /// </summary>
        event EventHandler<double> ProgressChanged;

        /// <summary>
        /// Parsing Task
        /// </summary>
        Task<Dictionary<string, int>> ParseFile(CancellationToken cancellationToken);
        
    }
}