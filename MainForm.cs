using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using YoutubeDLSharp;
using YoutubeDLSharp.Metadata;
using static System.Windows.Forms.Design.AxImporter;
using YoutubeDLSharp.Options;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace YLYL_Download
{
    public partial class MainForm : Form
    {
        DataGridView URLsdata;
        bool ready = false;
        YoutubeDL ytdl = new YoutubeDL();
        public MainForm()
        {
            InitializeComponent();
            URLsdata = this.URLs;
            readyStatus.Text = ready.ToString();
            pathLabelValue.Text = System.IO.Directory.GetCurrentDirectory() + "\\Downloads\\";
            ytdl.SetMaxNumberOfProcesses((byte)Environment.ProcessorCount);



        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void URLs_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public static void GenerateVlcPlaylist(string directory)
        {
            // Specify the top 20 video file extensions
            var videoExtensions = new[] {
            ".mp4", ".mkv", ".avi", ".mov", ".wmv", ".flv", ".webm",
            ".mpg", ".mpeg", ".3gp", ".ogv", ".m4v", ".ts", ".f4v",
            ".rm", ".rmvb", ".vob", ".gifv", ".iso", ".mxf"
        };

            // Get all video files in the directory
            var videoFiles = Directory.GetFiles(directory)
                                      .Where(file => videoExtensions.Contains(Path.GetExtension(file).ToLower()))
                                      .ToList();

            if (videoFiles.Count == 0)
            {
                MessageBox.Show("No video files found in the directory.");
                return;
            }

            // Define the output playlist file path (xspf format)
            string outputPlaylist = Path.Combine(directory, "output_playlist.xspf");

            // Create a VLC XSPF playlist manually
            try
            {
                XDocument xspf = new XDocument(
                    new XElement("playlist",
                        new XAttribute("version", "1"),
                        new XElement("trackList",
                            videoFiles.Select(file =>
                                new XElement("track",
                                    new XElement("location", $"file:///{file}"))))));

                // Save the XSPF file to the specified location
                xspf.Save(outputPlaylist);
                MessageBox.Show($"VLC playlist (.xspf) created successfully: {outputPlaylist}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating VLC playlist: {ex.Message}");
            }
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
                var urls = GetFirstCellValues();

                // Create tasks for all video data fetch operations
                var tasks = urls.Select(async url =>
                {
                    try
                    {
                        // Fetch video data asynchronously

                        if (url.Contains("playlist?"))
                        {
                                    

                        } else
                        {
                            var res = await ytdl.RunVideoDataFetch(url);

                            // Get video information
                            VideoData video = res.Data;
                            string title = video.Title;
                            string uploader = video.Uploader;
                            long? views = video.ViewCount;

                            // Find the row corresponding to the URL
                            var dgr = FindRowByFirstCellValue(url);

                            if (dgr != null)
                            {
                                // Update columns with video data safely on the UI thread
                                this.Invoke(new Action(() =>
                                {
                                    UpdateColumnValue(dgr, 3, title); // Column 3: Title
                                    UpdateColumnValue(dgr, 4, uploader); // Column 4: Uploader
                                    UpdateColumnValue(dgr, 5, views); // Column 5: Views
                                    UpdateColumnValue(dgr, 6, "Not Started"); // Column 6: Status
                                    string commaSeparatedString = string.Join(",", res.ErrorOutput);
                                    UpdateColumnValue(dgr, 7, commaSeparatedString); // Column 7: Errors
                                }));
                            }
                        }
                            
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions for each URL individually
                        MessageBox.Show($"Error reading file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }).ToArray();

                // Wait for all tasks to complete
                await Task.WhenAll(tasks);
            }
        }
        private async void setOutputButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            pathLabelValue.Text = dialog.SelectedPath;
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
            
            readyStatus.Text = ready.ToString();
            getData();
            ready = true;
        }
        private async void executeButton_Click(object sender, EventArgs e)
        {
            if (ready)
            {
                ytdl.OutputFolder = pathLabelValue.Text;
                var urls = GetFirstCellValues();
                
                // Create a cancellation token source (optional, if you need to cancel the operation)
                var cts = new CancellationTokenSource();

                // Create tasks to run downloads in parallel
                var tasks = urls.Select(async (url, index) =>
                {
                    try
                    {
                        var dgr = FindRowByFirstCellValue(url); // Find the row using the URL

                        if (dgr != null)
                        {
                            //string customFileName = $"video{index + 1}.webm";  // video1.webm, video2.webm, etc.
                            //string outputFilePath = Path.Combine(ytdl.OutputFolder, customFileName);

                            // Create a progress instance and pass the URL to update progress
                            var progress = new Progress<DownloadProgress>(p =>
                            {
                                // Only update progress if it's within the expected range
                                if (p.Progress >= 0 && p.Progress <= 100)
                                {
                                    // Update progress safely on the UI thread
                                    this.Invoke(new Action(() =>
                                    {
                                        UpdateColumnValue(dgr, 6, p.Progress * 100); // Update progress column (e.g., 6)
                                    }));
                                }
                            });

                            // Run the video download task asynchronously with the custom filename

                            var options = new OptionSet()
                            {
                                RestrictFilenames = true
                            };
                            //recodeFormat: VideoRecodeFormat.Mp4 outputFilePath
                            var res = await ytdl.RunVideoDownload(url, progress: progress, ct: cts.Token,  overrideOptions: options);
                            
                            // Convert error output to comma-separated string
                            string commaSeparatedString = string.Join(",", res.ErrorOutput);

                            // Update the DataGridView with the results
                            this.Invoke(new Action(() =>
                            {
                                UpdateColumnValue(dgr, 7, commaSeparatedString); // Update error output column (e.g., 7)
                            }));
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle errors if necessary (logging, error display, etc.)
                        MessageBox.Show($"Error reading file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }).ToArray();

                // Wait for all download tasks to complete
                await Task.WhenAll(tasks);

                // Generate playlist if checked
                if (generatePlaylist.Checked)
                {
                    GenerateVlcPlaylist(pathLabelValue.Text);
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
