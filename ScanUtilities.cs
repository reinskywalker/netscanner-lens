using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

// FEATURE: My Namespace - Custom namespace for utility classes
namespace NetworkScannerUtilities
{
    // FEATURE: My Namespace - Enum for scan result types used in switch statements
    public enum ScanResultType
    {
        Online,
        Timeout,
        Error,
        Filtered,
        Unknown
    }

    // FEATURE: My Namespace - Enum for export format types
    public enum ExportFormat
    {
        CSV,
        TXT,
        HTML,
        JSON
    }

    /// <summary>
    /// Utility class for scan-related helper methods
    /// </summary>
    public static class ScanHelper
    {
        // FEATURE: Date, Time, and TimeSpan - Track scan session start time
        private static DateTime _sessionStartTime = DateTime.Now;

        /// <summary>
        /// FEATURE: Switch Statement - Determines status message based on scan result type
        /// </summary>
        /// <param name="resultType">The type of scan result</param>
        /// <returns>Human-readable status message</returns>
        public static string GetStatusMessage(ScanResultType resultType)
        {
            // FEATURE: Select Case Statement (Switch) - Different actions based on result type
            switch (resultType)
            {
                case ScanResultType.Online:
                    return "Host is reachable and responding";
                case ScanResultType.Timeout:
                    return "Connection timed out";
                case ScanResultType.Error:
                    return "Error occurred during scan";
                case ScanResultType.Filtered:
                    return "Port appears to be filtered";
                case ScanResultType.Unknown:
                    return "Unable to determine status";
                default:
                    return "Invalid result type";
            }
        }

        /// <summary>
        /// FEATURE: Switch Statement with Pattern Matching - Determines color based on status
        /// </summary>
        public static System.Drawing.Color GetStatusColor(string status)
        {
            // FEATURE: Select Case Statement (Switch) - Color coding based on status
            return status switch
            {
                "Online" => System.Drawing.Color.Green,
                "Timeout" => System.Drawing.Color.Red,
                "Error" => System.Drawing.Color.Orange,
                _ => System.Drawing.Color.Gray
            };
        }

        /// <summary>
        /// FEATURE: While Loop - Validates IP address format by checking each octet
        /// </summary>
        public static bool ValidateIpFormat(string ipAddress)
        {
            // STRING METHOD: Split string into parts
            string[] parts = ipAddress.Split('.');
            
            if (parts.Length != 4)
                return false;

            int index = 0;
            // FEATURE: While Loop - Iterate through IP octets until all are validated
            while (index < parts.Length)
            {
                // STRING METHOD: Check if string is null or empty
                if (string.IsNullOrWhiteSpace(parts[index]))
                    return false;

                // STRING METHOD: TryParse to validate numeric format
                if (!int.TryParse(parts[index], out int octet))
                    return false;

                // Validate octet range
                if (octet < 0 || octet > 255)
                    return false;

                index++;
            }

            return true;
        }

        /// <summary>
        /// FEATURE: Do-While Loop - Retry mechanism for network operations
        /// </summary>
        public static bool TryConnectWithRetry(string host, int port, int maxRetries)
        {
            int attempts = 0;
            bool connected = false;

            // FEATURE: Do-While Loop - Execute at least once, then check condition
            do
            {
                try
                {
                    using var client = new System.Net.Sockets.TcpClient();
                    var result = client.BeginConnect(host, port, null, null);
                    var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1));
                    
                    if (success && client.Connected)
                    {
                        connected = true;
                        client.EndConnect(result);
                    }
                }
                catch
                {
                    // Ignore exception and retry
                }

                attempts++;
            } while (!connected && attempts < maxRetries);

            return connected;
        }

        /// <summary>
        /// FEATURE: Date, Time, and TimeSpan - Calculate scan duration
        /// </summary>
        public static string GetScanDuration(DateTime startTime, DateTime endTime)
        {
            // FEATURE: TimeSpan - Calculate time difference
            TimeSpan duration = endTime - startTime;

            // STRING METHOD: Format string with interpolation
            return $"{duration.Hours:D2}:{duration.Minutes:D2}:{duration.Seconds:D2}.{duration.Milliseconds:D3}";
        }

        /// <summary>
        /// FEATURE: Date and Time - Get formatted timestamp for logging
        /// </summary>
        public static string GetTimestamp()
        {
            // FEATURE: DateTime - Various date/time formatting methods
            DateTime now = DateTime.Now;
            return now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        /// <summary>
        /// FEATURE: TimeSpan - Check if operation exceeded timeout
        /// </summary>
        public static bool IsTimedOut(DateTime startTime, int timeoutMs)
        {
            // FEATURE: TimeSpan - Calculate elapsed time
            TimeSpan elapsed = DateTime.Now - startTime;
            return elapsed.TotalMilliseconds > timeoutMs;
        }

        /// <summary>
        /// FEATURE: Passing Arrays (ByVal) - Process array of IP addresses
        /// Arrays are reference types but passed "by value" of the reference
        /// </summary>
        public static int CountValidIps(string[] ipAddresses)
        {
            // FEATURE: Passing Arrays - Receiving array parameter
            int count = 0;

            // FOREACH LOOP: Iterate through array
            foreach (string ip in ipAddresses)
            {
                if (ValidateIpFormat(ip))
                    count++;
            }

            return count;
        }

        /// <summary>
        /// FEATURE: Passing Arrays - Filter and return new array of valid IPs
        /// </summary>
        public static string[] FilterValidIps(string[] ipAddresses)
        {
            // COLLECTION: List to store valid IPs
            var validIps = new List<string>();

            // FOREACH LOOP: Process each IP
            foreach (string ip in ipAddresses)
            {
                // IF STATEMENT: Conditional check
                if (ValidateIpFormat(ip))
                {
                    validIps.Add(ip);
                }
            }

            // ARRAY: Convert list back to array
            return validIps.ToArray();
        }

        /// <summary>
        /// FEATURE: ByRef Simulation - Modify values through out parameters
        /// C# uses 'out' and 'ref' keywords for by-reference parameter passing
        /// </summary>
        public static void ParseIpAndPort(string input, out string ip, out int port)
        {
            // FEATURE: ByRef - Output parameters that modify caller's variables
            ip = "";
            port = 0;

            // STRING METHOD: Split, Trim, and Parse operations
            string[] parts = input.Split(':');
            
            if (parts.Length > 0)
            {
                ip = parts[0].Trim();
            }

            if (parts.Length > 1)
            {
                int.TryParse(parts[1].Trim(), out port);
            }
        }

        /// <summary>
        /// FEATURE: ByRef - Increment counter using ref parameter
        /// </summary>
        public static void IncrementCounter(ref int counter)
        {
            // FEATURE: ByRef - Modifying parameter passed by reference
            counter++;
        }

        /// <summary>
        /// FEATURE: ByRef - Update multiple statistics using ref parameters
        /// </summary>
        public static void UpdateStatistics(ref int total, ref int successful, ref int failed, bool wasSuccessful)
        {
            // FEATURE: ByRef - Multiple reference parameters
            total++;
            
            // IF STATEMENT: Conditional logic
            if (wasSuccessful)
            {
                successful++;
            }
            else
            {
                failed++;
            }
        }

        /// <summary>
        /// FEATURE: While Loop - Generate IP range as array
        /// </summary>
        public static string[] GenerateIpRange(string startIp, string endIp)
        {
            // COLLECTION: List to store generated IPs
            var ips = new List<string>();

            try
            {
                var start = System.Net.IPAddress.Parse(startIp);
                var end = System.Net.IPAddress.Parse(endIp);

                uint startInt = BitConverter.ToUInt32(start.GetAddressBytes().Reverse().ToArray(), 0);
                uint endInt = BitConverter.ToUInt32(end.GetAddressBytes().Reverse().ToArray(), 0);

                uint current = startInt;
                
                // FEATURE: While Loop - Generate IPs until end is reached
                while (current <= endInt && ips.Count < 1000) // Limit to prevent memory issues
                {
                    var bytes = BitConverter.GetBytes(current).Reverse().ToArray();
                    var ip = new System.Net.IPAddress(bytes);
                    ips.Add(ip.ToString());
                    current++;
                }
            }
            catch
            {
                // EXCEPTION HANDLING: Return empty array on error
            }

            // FEATURE: Passing Arrays - Return array
            return ips.ToArray();
        }

        /// <summary>
        /// FEATURE: Date, Time - Get session uptime
        /// </summary>
        public static TimeSpan GetSessionUptime()
        {
            // FEATURE: TimeSpan - Calculate elapsed time since session start
            return DateTime.Now - _sessionStartTime;
        }

        /// <summary>
        /// Reset session start time
        /// </summary>
        public static void ResetSession()
        {
            // FEATURE: DateTime - Update timestamp
            _sessionStartTime = DateTime.Now;
        }
    }

    /// <summary>
    /// FEATURE: My Namespace - Class for text alignment and calculation
    /// </summary>
    public static class TextAlignmentHelper
    {
        /// <summary>
        /// FEATURE: Alignment - Align text within specified width
        /// </summary>
        public static string AlignText(string text, int width, TextAlignment alignment)
        {
            // STRING METHOD: Handle null or empty strings
            if (string.IsNullOrEmpty(text))
                text = "";

            // STRING METHOD: Trim text
            text = text.Trim();

            // IF STATEMENT: Check if text is longer than width
            if (text.Length > width)
            {
                // STRING METHOD: Substring to truncate
                return text.Substring(0, width);
            }

            int padding = width - text.Length;

            // FEATURE: Switch Statement - Different alignment strategies
            return alignment switch
            {
                TextAlignment.Left => text.PadRight(width),
                TextAlignment.Right => text.PadLeft(width),
                TextAlignment.Center => text.PadLeft(text.Length + padding / 2).PadRight(width),
                _ => text
            };
        }

        /// <summary>
        /// FEATURE: Calculate text dimensions for printing
        /// </summary>
        public static (int width, int height) CalculateTextDimensions(string text, System.Drawing.Font font)
        {
            // STRING METHOD: Split text into lines
            string[] lines = text.Split('\n');

            int maxWidth = 0;
            int totalHeight = 0;

            // FEATURE: While Loop - Calculate dimensions for each line
            int index = 0;
            while (index < lines.Length)
            {
                // STRING METHOD: Get length of line
                int lineWidth = lines[index].Length * (font.Height / 2); // Approximate width
                
                // IF STATEMENT: Update max width
                if (lineWidth > maxWidth)
                {
                    maxWidth = lineWidth;
                }

                totalHeight += font.Height;
                index++;
            }

            return (maxWidth, totalHeight);
        }

        /// <summary>
        /// FEATURE: Passing Arrays - Format array of strings with alignment
        /// </summary>
        public static string[] AlignTextArray(string[] textArray, int width, TextAlignment alignment)
        {
            // ARRAY: Create result array
            string[] result = new string[textArray.Length];

            // FOR LOOP: Process each element
            for (int i = 0; i < textArray.Length; i++)
            {
                result[i] = AlignText(textArray[i], width, alignment);
            }

            return result;
        }
    }

    /// <summary>
    /// Text alignment enumeration
    /// </summary>
    public enum TextAlignment
    {
        Left,
        Center,
        Right
    }
}
