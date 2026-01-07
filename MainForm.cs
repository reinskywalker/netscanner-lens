using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using NetworkScannerUtilities;

namespace netscan_lens
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private CancellationTokenSource? _cts;

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (cmbScanType.Items.Count > 0 && cmbScanType.SelectedIndex < 0)
            {
                cmbScanType.SelectedIndex = 0;
            }
            lblStatus.Text = "Ready to scan.";
            SetScanningUiState(false);
            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.Minimum = 0;
            progressBar.Maximum = 100;
            progressBar.Value = 0;
        }

        private void chkOwnIp_CheckedChanged(object? sender, EventArgs e)
        {
            if (chkOwnIp.Checked)
            {
                string ip = GetLocalIPv4Address() ?? "";
                txtStartIp.Text = ip;
                txtEndIp.Text = ip;
                lblStatus.Text = string.IsNullOrEmpty(ip) ? "Unable to get local IP" : $"Target: {ip}";
                txtStartIp.Enabled = false;
                txtEndIp.Enabled = false;
            }
            else
            {
                txtStartIp.Enabled = true;
                txtEndIp.Enabled = true;
                lblStatus.Text = "Ready to scan.";
            }
        }

        private static string? GetLocalIPv4Address()
        {
            try
            {
                foreach (var ni in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (ni.OperationalStatus != System.Net.NetworkInformation.OperationalStatus.Up) continue;
                    var ipProps = ni.GetIPProperties();
                    foreach (var ua in ipProps.UnicastAddresses)
                    {
                        if (ua.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork &&
                            !System.Net.IPAddress.IsLoopback(ua.Address))
                        {
                            return ua.Address.ToString();
                        }
                    }
                }
            }
            catch { }
            return null;
        }

        private async void btnScan_Click(object? sender, EventArgs e)
        {
            SetScanningUiState(true);
            _cts = new CancellationTokenSource();
            var token = _cts.Token;
            try
            {
                dgvResults.Rows.Clear();
                progressBar.Value = 0;

                var targets = BuildTargetIps();
                
                // FEATURE: Passing Arrays - Validate IPs before scanning using utility
                string[] targetArray = targets.ToArray();
                int validTargetCount = ScanHelper.CountValidIps(targetArray);

                if (targets.Count == 0)
                {
                    MessageBox.Show(this, "No target IP(s) specified.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // FEATURE: Date, Time - Track session start
                ScanHelper.ResetSession();
                DateTime scanStartTime = DateTime.Now;

                int timeout = (int)numTimeout.Value;
                int total = targets.Count;
                int completed = 0;
                
                // FEATURE: ByRef - Statistics counters
                int totalScanned = 0;
                int successCount = 0;
                int failCount = 0;

                bool isPortSweep = cmbScanType.SelectedItem?.ToString() == "Port Sweep";
                var (startPort, endPort) = GetPortRange();

                foreach (var ip in targets)
                {
                    token.ThrowIfCancellationRequested();

                    // FEATURE: Do-While Loop - Retry logic for ping
                    int retry = 0;
                    bool pingSuccess = false;
                    long rtt = 0;
                    int maxRetries = 1;

                    do
                    {
                        var result = await PingAsync(ip, timeout, token);
                        pingSuccess = result.success;
                        rtt = result.rttMs;
                        retry++;
                    } while (!pingSuccess && retry <= maxRetries);

                    string host = "";
                    
                    // FEATURE: Switch Statement - Determine status message using utility
                    ScanResultType resultType = pingSuccess ? ScanResultType.Online : ScanResultType.Timeout;
                    string status = ScanHelper.GetStatusMessage(resultType);
                    
                    string latency = pingSuccess ? rtt.ToString() : "-";
                    string openPorts = string.Empty;

                    // FEATURE: ByRef - Update statistics via reference
                    ScanHelper.UpdateStatistics(ref totalScanned, ref successCount, ref failCount, pingSuccess);

                    if (pingSuccess)
                    {
                        host = await ResolveHostAsync(ip, token);

                        if (isPortSweep && startPort > 0 && endPort > 0)
                        {
                            lblStatus.Text = $"Scanning ports on {ip}...";
                            openPorts = await ScanPortsAsync(ip, startPort, endPort, timeout, token);
                        }
                    }

                    dgvResults.Rows.Add(ip, host, status, latency, openPorts);

                    completed++;
                    progressBar.Value = Math.Min(100, (int)(completed * 100.0 / total));
                    
                    // FEATURE: Date, Time - Show elapsed time in status
                    TimeSpan uptime = ScanHelper.GetSessionUptime();
                    lblStatus.Text = $"Progress: {completed}/{total} (Elapsed: {uptime:mm\\:ss})";

                    Application.DoEvents();
                }

                // FEATURE: Date, Time, and TimeSpan - Final duration
                string totalDuration = ScanHelper.GetScanDuration(scanStartTime, DateTime.Now);
                lblStatus.Text = $"Scan complete. Duration: {totalDuration}. Online: {successCount}, Offline: {failCount}";
            }
            catch (OperationCanceledException)
            {
                lblStatus.Text = "Scan cancelled.";
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Scan error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Scan failed.";
            }
            finally
            {
                SetScanningUiState(false);
                _cts?.Dispose();
                _cts = null;
            }
        }

        private void btnCancel_Click(object? sender, EventArgs e)
        {
            try
            {
                if (_cts?.Token.CanBeCanceled == true && !_cts.Token.IsCancellationRequested)
                {
                    var result = MessageBox.Show(this, "Cancel scanning?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        _cts.Cancel();
                    }
                }
            }
            catch (ObjectDisposedException)
            {
                lblStatus.Text = "Scan completed.";
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Error cancelling scan: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExport_Click(object? sender, EventArgs e)
        {
            // FEATURE: Directory - Create exports directory if it doesn't exist
            string exportDir = Path.Combine(Application.StartupPath, "Exports");
            if (!Directory.Exists(exportDir))
            {
                Directory.CreateDirectory(exportDir);
            }

            // FEATURE: File Dates and Time - Include timestamp in default filename
            string timestamp = ScanHelper.GetTimestamp().Replace(":", "").Replace(" ", "_").Replace("-", "");
            string defaultFilename = $"scan_results_{timestamp}";

            using var sfd = new SaveFileDialog 
            { 
                Filter = "CSV files (*.csv)|*.csv|Text Report (*.txt)|*.txt", 
                FileName = defaultFilename + ".csv",
                InitialDirectory = exportDir
            };
            
            if (sfd.ShowDialog(this) == DialogResult.OK)
            {
                string ext = Path.GetExtension(sfd.FileName).ToLower();

                // FEATURE: Switch Statement - Select export format
                switch (ext)
                {
                    case ".txt":
                        ExportAsTxt(sfd.FileName);
                        break;
                    case ".csv":
                    default:
                        ExportAsCsv(sfd.FileName);
                        break;
                }
                
                lblStatus.Text = $"Exported: {sfd.FileName}";
            }
        }

        private void ExportAsCsv(string fileName)
        {
            using var sw = new System.IO.StreamWriter(fileName);
            sw.WriteLine("ip,host,status,latency,ports");
            foreach (DataGridViewRow row in dgvResults.Rows)
            {
                var ip = row.Cells[0].Value?.ToString() ?? "";
                var host = row.Cells[1].Value?.ToString() ?? "";
                var status = row.Cells[2].Value?.ToString() ?? "";
                var latency = row.Cells[3].Value?.ToString() ?? "";
                var ports = row.Cells[4].Value?.ToString() ?? "";
                sw.WriteLine($"\"{ip}\",\"{host}\",\"{status}\",\"{latency}\",\"{ports}\"");
            }
        }

        // FEATURE: Control Printing and Report Printing - Formatted text export
        private void ExportAsTxt(string fileName)
        {
            using var sw = new System.IO.StreamWriter(fileName);
            sw.WriteLine("NETWORK SCAN REPORT");
            sw.WriteLine($"Generated: {ScanHelper.GetTimestamp()}");
            sw.WriteLine(new string('=', 85));

            // FEATURE: Alignment - Align headers
            string header = TextAlignmentHelper.AlignText("IP Address", 18, TextAlignment.Left) +
                            TextAlignmentHelper.AlignText("Host", 30, TextAlignment.Left) +
                            TextAlignmentHelper.AlignText("Status", 25, TextAlignment.Left) +
                            TextAlignmentHelper.AlignText("Latency", 10, TextAlignment.Right);
            sw.WriteLine(header);
            sw.WriteLine(new string('-', 85));

            foreach (DataGridViewRow row in dgvResults.Rows)
            {
                var ip = row.Cells[0].Value?.ToString() ?? "";
                var host = row.Cells[1].Value?.ToString() ?? "";
                var status = row.Cells[2].Value?.ToString() ?? "";
                var latency = row.Cells[3].Value?.ToString() ?? "";

                // FEATURE: Alignment - Align data rows
                string line = TextAlignmentHelper.AlignText(ip, 18, TextAlignment.Left) +
                              TextAlignmentHelper.AlignText(host, 30, TextAlignment.Left) +
                              TextAlignmentHelper.AlignText(status, 25, TextAlignment.Left) +
                              TextAlignmentHelper.AlignText(latency, 10, TextAlignment.Right);
                sw.WriteLine(line);
            }
        }

        private void btnCredits_Click(object? sender, EventArgs e)
        {
            using var f = new CreditsForm();
            f.ShowDialog(this);
        }

        private void SetScanningUiState(bool scanning)
        {
            btnScan.Enabled = !scanning;
            btnExport.Enabled = !scanning;
            btnCredits.Enabled = !scanning;
            txtStartIp.Enabled = !scanning && !chkOwnIp.Checked;
            txtEndIp.Enabled = !scanning && !chkOwnIp.Checked;
            cmbScanType.Enabled = !scanning;
            numTimeout.Enabled = !scanning;
            numPortStart.Enabled = !scanning;
            numPortEnd.Enabled = !scanning;
            chkOwnIp.Enabled = !scanning;
            btnCancel.Enabled = scanning;
        }

        private List<string> BuildTargetIps()
        {
            var list = new List<string>();
            var start = txtStartIp.Text.Trim();
            var end = txtEndIp.Text.Trim();
            if (string.IsNullOrWhiteSpace(start) && !string.IsNullOrWhiteSpace(end))
            {
                start = end;
            }
            if (System.Net.IPAddress.TryParse(start, out var startIp))
            {
                if (string.IsNullOrWhiteSpace(end) || start == end)
                {
                    list.Add(start);
                }
                else if (System.Net.IPAddress.TryParse(end, out var endIp))
                {
                    foreach (var ip in EnumerateIpRange(startIp, endIp))
                        list.Add(ip.ToString());
                }
            }
            return list;
        }

        private static IEnumerable<System.Net.IPAddress> EnumerateIpRange(System.Net.IPAddress start, System.Net.IPAddress end)
        {
            var s = BitConverter.ToUInt32(start.GetAddressBytes().Reverse().ToArray(), 0);
            var e = BitConverter.ToUInt32(end.GetAddressBytes().Reverse().ToArray(), 0);
            if (s > e) (s, e) = (e, s);
            
            // FEATURE: While Loop - Iterate through IP range
            uint i = s;
            while (i <= e)
            {
                var bytes = BitConverter.GetBytes(i).Reverse().ToArray();
                yield return new System.Net.IPAddress(bytes);
                i++;
            }
        }

        private (int start, int end) GetPortRange()
        {
            int start = (int)numPortStart.Value;
            int end = (int)numPortEnd.Value;

            if (start == 1 && end == 1 && numPortStart.Value == 1 && numPortEnd.Value == 1)
            {
                return (0, 0);
            }

            if (end > 0 && start == 0)
            {
                start = 1;
            }
            if (start > 0 && end == 0)
            {
                end = start;
            }
            if (start > end)
            {
                (start, end) = (end, start);
            }
            if (start < 1) start = 1;
            if (end < 1) end = start;
            return (start, end);
        }

        private async Task<(bool success, long rttMs)> PingAsync(string ip, int timeout, CancellationToken ct)
        {
            using var ping = new System.Net.NetworkInformation.Ping();
            try
            {
                var reply = await ping.SendPingAsync(ip, timeout);
                ct.ThrowIfCancellationRequested();
                return (reply.Status == System.Net.NetworkInformation.IPStatus.Success, reply.RoundtripTime);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch
            {
                return (false, 0);
            }
        }

        private async Task<string> ResolveHostAsync(string ip, CancellationToken ct)
        {
            try
            {
                var entry = await System.Net.Dns.GetHostEntryAsync(ip);
                ct.ThrowIfCancellationRequested();
                return entry.HostName;
            }
            catch { return string.Empty; }
        }

        private async Task<string> ScanPortsAsync(string ip, int start, int end, int timeoutMs, CancellationToken ct)
        {
            var open = new List<int>();
            var semaphore = new SemaphoreSlim(50);
            var tasks = new List<Task>();

            for (int port = start; port <= end; port++)
            {
                ct.ThrowIfCancellationRequested();

                int currentPort = port;
                var task = Task.Run(async () =>
                {
                    await semaphore.WaitAsync(ct);
                    try
                    {
                        bool isOpen = await IsPortOpenAsync(ip, currentPort, Math.Min(timeoutMs, 1000)); // Faster timeout for ports
                        if (isOpen)
                        {
                            lock (open)
                            {
                                open.Add(currentPort);
                            }
                        }
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                });

                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
            open.Sort();
            return open.Count > 0 ? string.Join(";", open) : string.Empty;
        }

        private static async Task<bool> IsPortOpenAsync(string host, int port, int timeoutMs)
        {
            try
            {
                using var client = new System.Net.Sockets.TcpClient();
                var connectTask = client.ConnectAsync(host, port);
                var timeoutTask = Task.Delay(timeoutMs);
                var finished = await Task.WhenAny(connectTask, timeoutTask);
                return finished == connectTask && client.Connected;
            }
            catch { return false; }
        }

        private void topPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dgvResults_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
