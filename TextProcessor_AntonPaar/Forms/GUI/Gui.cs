using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using TextProcessor;

namespace TextProcessor_AntonPaar.Forms.GUI
{

    /// <summary>
    /// Main Form
    /// Separated into different partial classes for easier reading, such as:
    ///     - Main: Gui.cs
    ///     - Events: Gui.Events.cs
    ///     - Design elements: Gui.Design.cs
    ///
    /// Application tested with 150 MB files
    /// </summary>

    public partial class Gui : Form
    {
        private string _txtFilePath;
        private IFileParser _fileParser;

        private CancellationTokenSource _cancellationTokenSource;
        private Dictionary<string, int> _wordCounts;

        private struct ParsingStatus
        {
            const string Completed = "Parsing completed.";
            const string Cancelled = "Parsing cancelled.";
            const string Error = "Parsing error.";

        }


    public Gui()
        {
            InitializeComponent();
            _fileParser = new FileParser();
            _fileParser.ProgressChanged += OnProgressChanged;
            BtnProcess.Enabled = false;
            BtnCancel.Enabled = false;
            LblStatus.Enabled = false;
            LblStatus.Text = "";
           
        }
    }

   
}