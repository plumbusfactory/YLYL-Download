using System.Runtime.CompilerServices;
using System.Windows.Forms;
using YoutubeDLSharp;
using YoutubeDLSharp.Metadata;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace YLYL_Download
{
    public partial class MainForm : Form
    {
        DataGridView URLsdata;
        bool ready = false;
        public MainForm()
        {
            InitializeComponent();
            URLsdata = this.URLs;
            readyStatus.Text = ready.ToString();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void URLs_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private async void updateYTDLP_Click(object sender, EventArgs e)
        {
            await YoutubeDLSharp.Utils.DownloadYtDlp();
            await YoutubeDLSharp.Utils.DownloadFFmpeg();
        }
        private List<string> GetFirstCellValues()
        {
            List<string> firstCellValues = new List<string>();

            foreach (DataGridViewRow row in URLsdata.Rows)
            {
                if (!row.IsNewRow) // Skip the new row placeholder
                {
                    var firstCellValue = row.Cells[0].Value?.ToString(); // Get the first column's value
                    if (firstCellValue != null)
                    {
                        firstCellValues.Add(firstCellValue);
                    }
                }
            }

            return firstCellValues;
        }
        private DataGridViewRow? FindRowByFirstCellValue(string searchValue)
        {
            foreach (DataGridViewRow row in URLsdata.Rows)
            {
                if (!row.IsNewRow) // Skip the new row placeholder
                {
                    var firstCellValue = row.Cells[0].Value?.ToString();
                    if (firstCellValue != null && firstCellValue.Equals(searchValue, StringComparison.OrdinalIgnoreCase))
                    {
                        return row; // Return the matching row
                    }
                }
            }
            return null; // Return null if no match is found
        }
        private void UpdateColumnValue(DataGridViewRow row, int columnIndex, object newValue)
        {
            if (row != null && columnIndex >= 0 && columnIndex < row.Cells.Count)
            {
                row.Cells[columnIndex].Value = newValue; // Update the cell value
            }
            else
            {
                throw new ArgumentException("Invalid row or column index.");
            }
        }

        private async void getData()
        {
            if (ready)
            {
                var ytdl = new YoutubeDL();
                var urls = GetFirstCellValues();
                foreach (string url in urls)
                {
                    var res = await ytdl.RunVideoDataFetch(url);
                    // get some video information
                    VideoData video = res.Data;
                    string title = video.Title;
                    string uploader = video.Uploader;
                    long? views = video.ViewCount;
                    var dgr = FindRowByFirstCellValue(url);

                    UpdateColumnValue(dgr, 3, title);
                    UpdateColumnValue(dgr, 4, uploader);
                    UpdateColumnValue(dgr, 5, views);
                    
                    UpdateColumnValue(dgr, 6, "Not Started");
                    string commaSeparatedString = string.Join(",", res.ErrorOutput);
                    UpdateColumnValue(dgr, 7, commaSeparatedString);
                }
                
            }
        }
        private void generateCommand_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in URLsdata.Rows)
            {
                if (!row.IsNewRow) // Avoid the last empty row used for new entries
                {
                    string rowData = "";
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        rowData += cell.Value?.ToString() + " | "; // Concatenating values with a separator
                        if (rowData.Contains("youtube.com"))
                        {
                            string[] temp = { rowData, "Youtube", "Maybe" };
                            row.SetValues(temp);
                        }
                    }
                   
                }
            }
            ready = true;
            readyStatus.Text = ready.ToString();
            getData();
        }
        private async void executeButton_Click(object sender, EventArgs e)
        {
            if (ready)
            {
                var ytdl = new YoutubeDL();
                ytdl.OutputFolder = System.IO.Directory.GetCurrentDirectory() + "\\Downloads\\";
                var urls = GetFirstCellValues();
                foreach (string url in urls)
                {
                    var dgr = FindRowByFirstCellValue(url);
                    var progress = new Progress<DownloadProgress>(p => UpdateColumnValue(dgr, 6, p.Progress));
                    var cts = new CancellationTokenSource();
                    var res = await ytdl.RunVideoDownload(url, progress: progress, ct: cts.Token);
                    
                    string commaSeparatedString = string.Join(",", res.ErrorOutput);
                    UpdateColumnValue(dgr, 7, commaSeparatedString);
                }

            }
        }
        private async void loadList_Click(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                Title = "Select a Text File"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;

                try
                {
                    // Clear the DataGridView
                    URLsdata.Columns.Clear();
                    URLsdata.Rows.Clear();

                    // Read all lines from the file asynchronously
                    string[] lines = await File.ReadAllLinesAsync(filePath);

                    if (lines.Length > 0)
                    {
                        // Assuming the first line contains column headers
                        
                            URLsdata.Columns.Add("URL", "URL");
                            URLsdata.Columns.Add("Source", "Source");
                            URLsdata.Columns.Add("Cookies Needed", "Cookies Needed");
                            URLsdata.Columns.Add("Title", "Title");
                            URLsdata.Columns.Add("Uploader", "Uploader");
                            URLsdata.Columns.Add("Views", "Views");
                            
                            URLsdata.Columns.Add("Progress", "Progress");
                            URLsdata.Columns.Add("Errors", "Errors");
                        // Add remaining lines as rows
                        foreach (string line in lines.Skip(1))
                        {
                            string[] cells = line.Split('\t'); // Split by tabs
                            URLsdata.Rows.Add(cells);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error reading file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

     }
}
