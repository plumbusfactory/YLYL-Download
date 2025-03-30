using System.Xml.Linq;
using YoutubeDLSharp;
using YoutubeDLSharp.Metadata;
using YoutubeDLSharp.Options;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace YLYL_Download
{
    public partial class MainForm : Form
    {
        private readonly DataGridView _urLsdata;
        private bool _ready;
        private readonly YoutubeDL _ytdl = new();
        public MainForm()
        {
            InitializeComponent();
            _urLsdata = URLs;
            readyStatus.Text = _ready.ToString();
            pathLabelValue.Text = Directory.GetCurrentDirectory() + @"\Downloads\";
            _ytdl.SetMaxNumberOfProcesses((byte)Environment.ProcessorCount);
            browserList.SelectedIndex = 0;
            CheckForExecutables();
            generateCommand.Enabled = false;
            executeButton.Enabled = false;

        }

        private static async Task<string[]> GetVidcount(string url)
        {
            Collection<string> linesRet = [];
            var ytdlProc = new YoutubeDLProcess();
            // capture the standard output and error output
            ytdlProc.OutputReceived += (_, e) =>
            {
                if (e.Data != null) linesRet.Add(e.Data);
            };
            ytdlProc.ErrorReceived += (_, e) => Console.WriteLine(@"ERROR: " + e.Data);
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
        public static bool IsDirectoryWritable(string dirPath, bool throwIfFails = false)
        {
            try
            {
                using (var fs = File.Create(
                           Path.Combine(
                               dirPath, 
                               Path.GetRandomFileName()
                           ), 
                           1,
                           FileOptions.DeleteOnClose)
                      )
                { }
                return true;
            }
            catch
            {
                if (throwIfFails)
                    throw;
                else
                    return false;
            }
        }
        private static async void CheckForExecutables()
        {
            try
            {
                // Get the current working directory
                var workingDir = Directory.GetCurrentDirectory();
                
                string[] filesToCheck = ["yt-dlp.exe", "ffmpeg.exe"];
                var missingFiles = (from file in filesToCheck let filePath = Path.Combine(workingDir, file) where !File.Exists(filePath) select file).ToList();
                if (missingFiles.Count <= 0) return;
                var message = "The following files are missing:\n" + string.Join("\n", missingFiles) + "\nWould you like to download them now or close the program?";
                var result = MessageBox.Show(message, @"Missing Dependencies", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // Open a URL to download the missing dependencies (example URL)
                    try
                    {
                        // Attempt to download YtDlp
                        await Utils.DownloadYtDlp();
                        await Utils.DownloadFFmpeg();
                    }
                    catch (Exception ex)
                    {
                        if (!IsUserAdministrator())
                        {
                            // If not, launch the application with administrator privileges
                            MessageBox.Show(@"Admin permissions required");
                            RestartAsAdmin();
                            return;
                        }
                    }
                }
                else
                {
                    // Close the program
                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Error reading file: {ex.Message}", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private static bool IsUserAdministrator()
        {
            // Check if the current user is an administrator
            var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            var principal = new System.Security.Principal.WindowsPrincipal(identity);
            return principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
        }

        private static void RestartAsAdmin()
        {
            try
            {
                // Create a process start info with elevated privileges
                var startInfo = new ProcessStartInfo
                {
                    FileName = Application.ExecutablePath,
                    Verb = "runas", // This will trigger the UAC prompt
                    UseShellExecute = true
                };

                // Start the process with administrator privileges
                Process.Start(startInfo);
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Error starting with admin privileges: {ex.Message}");
            }
        }
        private static void GenerateVlcPlaylist(string directory)
        {
            // Specify the top 20 video file extensions
            var videoExtensions = new[] {
                ".mp4", ".mkv", ".avi", ".mov", ".wmv", ".flv", ".webm",
                ".mpg", ".mpeg", ".3gp", ".ogv", ".m4v", ".ts", ".f4v",
                ".rm", ".rmvb", ".vob", ".gifv", ".iso", ".mxf" };

            // Get all video files in the directory
            var videoFiles = Directory.GetFiles(directory)
                                      .Where(file => videoExtensions.Contains(Path.GetExtension(file).ToLower()))
                                      .Select(Path.GetFileName) // Use only the file name
                                      .ToList();

            if (videoFiles.Count == 0)
            {
                MessageBox.Show(@"No video files found in the directory.");
                return;
            }

            // Define the output playlist file path (xspf format)
            var outputPlaylist = Path.Combine(directory, "output_playlist.xspf");

            // Create a VLC XSPF playlist manually
            try
            {
                var xspf = new XDocument(
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
                MessageBox.Show($@"VLC playlist (.xspf) created successfully: {outputPlaylist}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Error creating VLC playlist: {ex.Message}");
            }
        }


        private async void updateYTDLP_Click(object sender, EventArgs e)
        {
            try
            {
                CheckForExecutables();
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Error reading file: {ex.Message}", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private List<string> GetFirstCellValues()
        {
            return (from DataGridViewRow row in _urLsdata.Rows where !row.IsNewRow select row.Cells[0].Value?.ToString()).OfType<string>().ToList();
        }
        private DataGridViewRow? FindRowByFirstCellValue(string searchValue)
        {
            foreach (DataGridViewRow row in _urLsdata.Rows)
            {
                if (row.IsNewRow) continue; // Skip the new row placeholder
                var firstCellValue = row.Cells[0].Value?.ToString();
                if (firstCellValue != null && firstCellValue.Equals(searchValue, StringComparison.OrdinalIgnoreCase))
                {
                    return row; // Return the matching row
                }
            }
            return null; // Return null if no match is found
        }
        private static void UpdateColumnValue(DataGridViewRow? row, int columnIndex, object newValue)
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

        private async void GetData()
        {
            try
            {
                if (!_ready) return;
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
                                var options = new OptionSet
                                {
                                    RestrictFilenames = true,
                                    CookiesFromBrowser = browserList.SelectedItem?.ToString()
                                };
                                resp = await _ytdl.RunVideoDataFetch(url,overrideOptions: options);
                            } else
                            {
                                resp = await _ytdl.RunVideoDataFetch(url);
                            }

                            var dgr = FindRowByFirstCellValue(url);

                            if (dgr != null)
                            {
                                // Update columns with video data safely on the UI thread
                                Invoke(() =>
                                {
                                    UpdateColumnValue(dgr, 3, "Playlist"); // Column 3: Title
                                    UpdateColumnValue(dgr, 4, "Playlist"); // Column 4: Uploader
                                    UpdateColumnValue(dgr, 5, -1); // Column 5: Views
                                    UpdateColumnValue(dgr, 6, "Not Started"); // Column 6: Status
                                    var commaSeparatedString = string.Join(",", resp.ErrorOutput);
                                    UpdateColumnValue(dgr, 7, commaSeparatedString); // Column 7: Errors
                                });
                            }

                        }
                        else
                        {
                            RunResult<VideoData> res;
                            if (useCookies.Checked)
                            {
                                var options = new OptionSet
                                {
                                    RestrictFilenames = true,
                                    CookiesFromBrowser = browserList.SelectedItem?.ToString()
                                };
                                res = await _ytdl.RunVideoDataFetch(url, overrideOptions: options);
                            }
                            else
                            {
                                res = await _ytdl.RunVideoDataFetch(url);
                            }

                            
                            // Get video information
                            var video = res.Data;
                            var title = video?.Title;
                            var uploader = video?.Uploader;
                            var views = video?.ViewCount;

                            // Find the row corresponding to the URL
                            var dgr = FindRowByFirstCellValue(url);

                            if (dgr != null)
                            {
                                // Update columns with video data safely on the UI thread
                                Invoke(() =>
                                {
                                    if (title != null) UpdateColumnValue(dgr, 3, title); // Column 3: Title
                                    if (uploader != null) UpdateColumnValue(dgr, 4, uploader); // Column 4: Uploader
                                    if (views != null) UpdateColumnValue(dgr, 5, views); // Column 5: Views
                                    UpdateColumnValue(dgr, 6, "Not Started"); // Column 6: Status
                                    var commaSeparatedString = string.Join(",", res.ErrorOutput);
                                    UpdateColumnValue(dgr, 7, commaSeparatedString); // Column 7: Errors
                                });
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions for each URL individually
                        MessageBox.Show($@"Error reading file: {ex.Message}", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }).ToArray();

                // Wait for all tasks to complete
                await Task.WhenAll(tasks);
                _ready = true;
                readyStatus.Text = true.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Error reading file: {ex.Message}", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void setOutputButton_Click(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            pathLabelValue.Text = dialog.SelectedPath;
        }
        private void generateCommand_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow? row in _urLsdata.Rows)
            {
                if (row is not { IsNewRow: false }) continue; // Avoid the last empty row used for new entries
                var rowData = "";

                foreach (DataGridViewCell cell in row.Cells)
                {
                    rowData += cell.Value + " | "; // Concatenating values with a separator
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

            readyStatus.Text = _ready.ToString();
            _ready = true;
            readyStatus.Text = true.ToString();
            GetData();
            executeButton.Enabled = true;

        }
        private async void executeButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_ready) return;
                _ytdl.OutputFolder = pathLabelValue.Text;
                var urls = GetFirstCellValues();

                // Create a cancellation token source (optional, if you need to cancel the operation)
                var cts = new CancellationTokenSource();

                // Create tasks to run downloads in parallel
                var tasks = urls.Select(async (url, _) =>
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
                                if (p.Progress is >= 0 and <= 100)
                                {
                                    // Update progress safely on the UI thread
                                    this.Invoke(() =>
                                    {
                                        if (p.State.ToString() == "Success")
                                        {
                                            UpdateColumnValue(dgr, 6, 100);
                                        } else
                                        {
                                            UpdateColumnValue(dgr, 6, p.Progress * 100); // Update progress column (e.g., 6)
                                        }
                                            
                                    });

                                }
                            });

                            // Run the video download task asynchronously with the custom filename

                            OptionSet options;
                            
                            if (useCookies.Checked)
                            {
                                
                                options = new OptionSet()
                                {
                                    RestrictFilenames = true,
                                    CookiesFromBrowser = browserList.SelectedItem?.ToString()
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
                                var res = await _ytdl.RunVideoPlaylistDownload(url, progress: progress, ct: cts.Token, overrideOptions: options);
                                var commaSeparatedString = string.Join(",", res.ErrorOutput);
                                this.Invoke(() =>
                                {
                                    UpdateColumnValue(dgr, 7, commaSeparatedString); // Update error output column (e.g., 7)
                                });

                            }
                            else
                            {
                                var res = await _ytdl.RunVideoDownload(url, progress: progress, ct: cts.Token, overrideOptions: options);
                                var commaSeparatedString = string.Join(",", res.ErrorOutput);
                                this.Invoke(() =>
                                {
                                    UpdateColumnValue(dgr, 7, commaSeparatedString); // Update error output column (e.g., 7)
                                });
                            }


                            // Convert error output to comma-separated string


                            // Update the DataGridView with the results

                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle errors if necessary (logging, error display, etc.)
                        MessageBox.Show($@"Error reading file: {ex.Message}", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            catch (Exception ex)
            {
                MessageBox.Show($@"Error reading file: {ex.Message}", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }





        private async void loadList_Click(object sender, EventArgs e)
        {
            try
            {
                using var openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = @"Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                openFileDialog.Title = @"Select a Text File";

                if (openFileDialog.ShowDialog() != DialogResult.OK) return;
                var filePath = openFileDialog.FileName;

                try
                {
                    // Clear the DataGridView
                    _urLsdata.Columns.Clear();
                    _urLsdata.Rows.Clear();

                    // Read all lines from the file asynchronously
                    var lines = await File.ReadAllLinesAsync(filePath);

                    if (lines.Length <= 0) return;
                    // Assuming the first line contains column headers

                    _urLsdata.Columns.Add("URL", "URL");
                    _urLsdata.Columns.Add("Source", "Source");
                    _urLsdata.Columns.Add("Cookies Needed", "Cookies Needed");
                    _urLsdata.Columns.Add("Title", "Title");
                    _urLsdata.Columns.Add("Uploader", "Uploader");
                    _urLsdata.Columns.Add("Views", "Views");

                    _urLsdata.Columns.Add("Progress", "Progress");
                    _urLsdata.Columns.Add("Errors", "Errors");
                    _urLsdata.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                    _urLsdata.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    // Add remaining lines as rows
                    foreach (var line in lines.Skip(1))
                    {
                        var cells = line.Split('\t'); // Split by tabs
                        _urLsdata.Rows.Add(cells);
                    }
                    generateCommand.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($@"Error reading file: {ex.Message}", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Error reading file: {ex.Message}", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void generatePlaylist_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void useCookies_CheckedChanged(object sender, EventArgs e)
        {
            const string message = "Warning, Enabling this will allow the underlying yt-dlp to read all cookies of the selected browser";
            var result = MessageBox.Show(message, @"Privacy Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            // Open a URL to download the missing dependencies (example URL)
            useCookies.Checked = result == DialogResult.Yes;
        }
    }
}
