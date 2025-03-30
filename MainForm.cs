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
using System.Collections.ObjectModel;

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
            browserList.SelectedIndex = 0;
            CheckForExecutables();
            

        }
        async Task<string[]> getVidcount(string url)
        {
            Collection<string> linesRet = new Collection<string>();
            var ytdlProc = new YoutubeDLProcess();
            // capture the standard output and error output
            ytdlProc.OutputReceived += (o, e) => linesRet.Add(e.Data);
            ytdlProc.ErrorReceived += (o, e) => Console.WriteLine("ERROR: " + e.Data);
            // start running
            string[] urls = [url];
            var options = new OptionSet()
            {
                NoContinue = true,
                RestrictFilenames = true
            };
            options.AddCustomOption<string>("--flat-playlist", "");
            options.AddCustomOption<string>("--print", "url");
            await ytdlProc.RunAsync(urls, options);
            return linesRet.ToArray();
        }
        static async void CheckForExecutables()
        {
            // Get the current working directory
            string workingDir = Directory.GetCurrentDirectory();

            // Define the file names to check
            string[] filesToCheck = { "yt-dlp.exe", "ffmpeg.exe" };

            // Check for missing files
            var missingFiles = new System.Collections.Generic.List<string>();
            foreach (string file in filesToCheck)
            {
                string filePath = Path.Combine(workingDir, file);
                if (!File.Exists(filePath))
                {
                    missingFiles.Add(file);
                }
            }

            if (missingFiles.Count > 0)
            {
                // Show a dialog with options
                string message = "The following files are missing:\n" + string.Join("\n", missingFiles) + "\nWould you like to download them now or close the program?";
                DialogResult result = MessageBox.Show(message, "Missing Dependencies", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // Open a URL to download the missing dependencies (example URL)
                    await YoutubeDLSharp.Utils.DownloadYtDlp();
                    await YoutubeDLSharp.Utils.DownloadFFmpeg();


                }
                else
                {
                    // Close the program
                    Environment.Exit(0);
                }
            }

        }
        private void Form1_Load(object sender, EventArgs e)
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
                                      .Select(file => Path.GetFileName(file)) // Use only the file name
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
                                    new XElement("title", file),
                                    new XElement("location", $"file:///{file}")))))
                );

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
                        RunResult<VideoData> resp;
                        if (url.Contains("playlist?"))
                        {
                            if (useCookies.Checked)
                            {
                                var options = new OptionSet()
                                {
                                    RestrictFilenames = true,
                                    CookiesFromBrowser = browserList.SelectedItem.ToString()
                                };
                                resp = await ytdl.RunVideoDataFetch(url,overrideOptions: options);
                            } else
                            {
                                resp = await ytdl.RunVideoDataFetch(url);
                            }

                            var dgr = FindRowByFirstCellValue(url);

                            if (dgr != null)
                            {
                                // Update columns with video data safely on the UI thread
                                this.Invoke(new Action(() =>
                                {
                                    UpdateColumnValue(dgr, 3, "Playlist"); // Column 3: Title
                                    UpdateColumnValue(dgr, 4, "Playlist"); // Column 4: Uploader
                                    UpdateColumnValue(dgr, 5, -1); // Column 5: Views
                                    UpdateColumnValue(dgr, 6, "Not Started"); // Column 6: Status
                                    string commaSeparatedString = string.Join(",", resp.ErrorOutput);
                                    UpdateColumnValue(dgr, 7, commaSeparatedString); // Column 7: Errors
                                }));
                            }

                        }
                        else
                        {
                            RunResult<VideoData> res;
                            if (useCookies.Checked)
                            {
                                var options = new OptionSet()
                                {
                                    RestrictFilenames = true,
                                    CookiesFromBrowser = browserList.SelectedItem.ToString()
                                };
                                 res = await ytdl.RunVideoDataFetch(url, overrideOptions: options);
                            }
                            else
                            {
                                res = await ytdl.RunVideoDataFetch(url);
                            }

                            
                            // Get video information
                            VideoData video = res.Data;
                            string? title = video?.Title;
                            string? uploader = video?.Uploader;
                            long? views = video?.ViewCount;

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
                ready = true;
                readyStatus.Text = true.ToString();
            }
        }
        private void setOutputButton_Click(object sender, EventArgs e)
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
                            UpdateColumnValue(row, 1, "Youtube");
                            UpdateColumnValue(row, 2, "No");
                        }
                        if (rowData.Contains("instagram.com"))
                        {
                            UpdateColumnValue(row, 1, "Insta");
                            UpdateColumnValue(row, 2, "Maybe");
                        }
                        if (rowData.Contains("tiktok.com"))
                        {
                            UpdateColumnValue(row, 1, "TikTok");
                            UpdateColumnValue(row, 2, "No");
                        }
                    }

                }
            }

            readyStatus.Text = ready.ToString();
            ready = true;
            readyStatus.Text = true.ToString();
            getData();

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
                                        if (p.State.ToString() == "Success")
                                        {
                                            UpdateColumnValue(dgr, 6, 100);
                                        } else
                                        {
                                            UpdateColumnValue(dgr, 6, p.Progress * 100); // Update progress column (e.g., 6)
                                        }
                                            
                                    }));

                                }
                            });

                            // Run the video download task asynchronously with the custom filename

                            OptionSet options;
                            //new OptionSet()
                            //{
                            //    RestrictFilenames = true
                            //}
                            ;
                            if (useCookies.Checked)
                            {
                                
                                options = new OptionSet()
                                {
                                    RestrictFilenames = true,
                                    CookiesFromBrowser = browserList.SelectedItem.ToString()
                                };
                            } else
                            {
                                options = new OptionSet()
                                {
                                    RestrictFilenames = true
                                };
                            }
                            //recodeFormat: VideoRecodeFormat.Mp4 outputFilePath
                            if (url.Contains("playlist?"))
                            {
                                var res = await ytdl.RunVideoPlaylistDownload(url, progress: progress, ct: cts.Token, overrideOptions: options);
                                string commaSeparatedString = string.Join(",", res.ErrorOutput);
                                this.Invoke(new Action(() =>
                                {
                                    UpdateColumnValue(dgr, 7, commaSeparatedString); // Update error output column (e.g., 7)
                                }));

                            }
                            else
                            {
                                var res = await ytdl.RunVideoDownload(url, progress: progress, ct: cts.Token, overrideOptions: options);
                                string commaSeparatedString = string.Join(",", res.ErrorOutput);
                                this.Invoke(new Action(() =>
                                {
                                    UpdateColumnValue(dgr, 7, commaSeparatedString); // Update error output column (e.g., 7)
                                }));
                            }


                            // Convert error output to comma-separated string


                            // Update the DataGridView with the results

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

        private void generatePlaylist_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void useCookies_CheckedChanged(object sender, EventArgs e)
        {
            string message = "Warning, Enabling this will allow the underlying yt-dlp to read all cookies of the selected browser";
            DialogResult result = MessageBox.Show(message, "Privacy Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                // Open a URL to download the missing dependencies (example URL)
                useCookies.Checked = true;

            }
            else
            {
                useCookies.Checked = false;
            }
        }
    }
}
