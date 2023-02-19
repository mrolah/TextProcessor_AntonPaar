using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TextProcessor;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TextProcessor_AntonPaar.Forms.GUI
{
    public partial class Gui
    {
        /// <summary>
        /// Event for file selection
        /// </summary>
        private void BtnSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt",
                Title = "Select a text file"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                _txtFilePath = dialog.FileName;
                BtnProcess.Enabled = true;
            }
        }

        /// <summary>
        /// Event for file processing
        ///     - Checks if file exists, file name is correct
        ///     - Manages event handling and Progress bar
        ///     - Calls the File processing method
        ///     - Fills up DataGridView
        ///     - Manages cancellation event
        /// </summary>
        private async void BtnProcess_Click(object sender, EventArgs e)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            DataGridView.DataSource = null;
            DataGridView.Columns.Clear();
            DataGridView.Refresh();

            SetButtonsEnabled(false);

            if (string.IsNullOrWhiteSpace(_txtFilePath))
            {
                MessageBox.Show("Please select a file to parse.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(_txtFilePath))
            {
                MessageBox.Show("The selected file does not exist.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            ProgressBar.Value = 0;

            // Process starts...

            try
            {
                // Unsubscribe from previous event to avoid multiple subscriptions
                _fileParser.ProgressChanged -= OnProgressChanged;

                // Create a new FileParser object
                _fileParser = new FileParser(_txtFilePath, new WordCounter());

                // Subscribe to the new FileParser's event
                _fileParser.ProgressChanged += OnProgressChanged;

                _wordCounts = await Task.Run(() => _fileParser.ParseFile(_cancellationTokenSource.Token))
                    .ConfigureAwait(true);


                DataGridView.Columns.Add("Key", "KEY");
                DataGridView.Columns.Add("Values", "VALUES");

                foreach (KeyValuePair<string, int> item in _wordCounts.OrderByDescending(x => x.Value))
                {
                    DataGridView.Rows.Add(item.Key, item.Value);
                }

                //DataGridView.DataSource = _wordCounts;

                if (_wordCounts.Any())
                {
                    DataGridView.AutoResizeColumns();
                }

                UpdateStatus("ParsingStatus completed");
            }

            catch (OperationCanceledException)
            {
                UpdateStatus("Parsing cancelled.");
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error: {ex.Message}");
            }

            SetButtonsEnabled(true);
        }

        /// <summary>
        /// Starts the cancellation process
        /// </summary>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            _cancellationTokenSource?.Cancel();
        }

        /// <summary>
        /// Event for Progress bar value change
        /// </summary>
        private void OnProgressChanged(object sender, double percentComplete)
        {
            // update the progress bar with the percent complete value
            UpdateProgressBar(percentComplete);

        }
    }
}