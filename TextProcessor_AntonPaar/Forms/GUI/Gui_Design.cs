using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextProcessor_AntonPaar.Forms.GUI
{
    public partial class Gui
    {
        /// <summary>
        /// Set Buttons Visibility
        /// </summary>
        private void SetButtonsEnabled(bool enabled)
        {
            BtnSelectFile.Enabled = enabled;
            BtnProcess.Enabled = enabled;
            BtnCancel.Enabled = !enabled;
        }

        /// <summary>
        /// Sets label status
        /// </summary>
        private void UpdateStatus(string status)
        {
            LblStatus.Enabled = true;

            if (LblStatus.InvokeRequired)
            {
                LblStatus.Invoke(new Action(() => LblStatus.Text = status));
            }
            else
            {
                LblStatus.Text = status;
            }
        }

        /// <summary>
        /// Draws progress bar
        /// </summary>
        private void UpdateProgressBar(double progressPercentage)
        {
            if (ProgressBar.InvokeRequired)
            {
                ProgressBar.Invoke(new Action(() => ProgressBar.Value = (int)Math.Round(progressPercentage)));
            }
            else
            {
                ProgressBar.Value = (int)Math.Round(progressPercentage);
            }
        }
    }
}
