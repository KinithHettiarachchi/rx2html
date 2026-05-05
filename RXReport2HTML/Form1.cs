using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace RXReport2HTML
{
    public partial class Form1 : Form
    {
        private string rxlogFilePath = string.Empty;

        public Form1()
        {
            InitializeComponent();
        }

        private void panelDropZone_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0 && files[0].EndsWith(".rxlog", StringComparison.OrdinalIgnoreCase))
                {
                    e.Effect = DragDropEffects.Copy;
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
        }

        private void panelDropZone_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length > 0)
            {
                ProcessRxlogFile(files[0]);
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Ranorex Log Files (*.rxlog)|*.rxlog|All Files (*.*)|*.*";
                openFileDialog.Title = "Select a Ranorex Log File";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ProcessRxlogFile(openFileDialog.FileName);
                }
            }
        }

        private void ProcessRxlogFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show("File does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            rxlogFilePath = filePath;
            txtFilePath.Text = filePath;
            btnGenerate.Enabled = true;
            lblStatus.Text = "Ready to generate report";
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(rxlogFilePath))
            {
                MessageBox.Show("Please select a .rxlog file first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                progressBar.Value = 0;
                lblStatus.Text = "Generating report...";
                Application.DoEvents();

                GenerateHtmlReport();

                progressBar.Value = 100;
                lblStatus.Text = "Report generated successfully!";
                MessageBox.Show("HTML report generated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error generating report";
                MessageBox.Show($"Error generating report: {ex.Message}\n\n{ex.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GenerateHtmlReport()
        {
            string directory = Path.GetDirectoryName(rxlogFilePath);
            string dataFilePath = rxlogFilePath + ".data";
            string cssFilePath = Path.Combine(directory, "RanorexReport.css");
            string imagesFolderPath = GetImagesFolder(directory);

            progressBar.Value = 10;

            // Check if required files exist
            if (!File.Exists(dataFilePath))
            {
                throw new FileNotFoundException("Data file not found: " + dataFilePath);
            }

            progressBar.Value = 20;

            // Parse XML data
            XDocument xmlDoc = XDocument.Load(dataFilePath);
            XElement reportRoot = xmlDoc.Root;

            progressBar.Value = 30;

            // Read CSS
            string css = string.Empty;
            if (File.Exists(cssFilePath))
            {
                css = File.ReadAllText(cssFilePath);
            }

            progressBar.Value = 40;

            // Generate HTML content
            string htmlContent = GenerateReportContent(reportRoot, directory, imagesFolderPath);

            progressBar.Value = 70;

            // Create final HTML with embedded CSS and JavaScript
            string finalHtml = CreateCompleteHtml(htmlContent, css, reportRoot);

            // Save the output
            string outputPath = Path.Combine(directory, Path.GetFileNameWithoutExtension(rxlogFilePath) + "_report.html");
            File.WriteAllText(outputPath, finalHtml, Encoding.UTF8);

            progressBar.Value = 90;

            // Open the generated report
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = outputPath,
                UseShellExecute = true
            });
        }

        public void GenerateReportFromPath(string rxlogPath, string outputPath)
        {
            string directory = Path.GetDirectoryName(rxlogPath) ?? "";
            string dataFilePath = rxlogPath + ".data";
            string cssFilePath = Path.Combine(directory, "RanorexReport.css");
            string imagesFolderPath = GetImagesFolder(directory);

            // Check if required files exist
            if (!File.Exists(dataFilePath))
            {
                throw new FileNotFoundException("Data file not found: " + dataFilePath);
            }

            // Parse XML data
            XDocument xmlDoc = XDocument.Load(dataFilePath);
            XElement reportRoot = xmlDoc.Root;

            // Read CSS
            string css = string.Empty;
            if (File.Exists(cssFilePath))
            {
                css = File.ReadAllText(cssFilePath);
            }

            // Generate HTML content
            string htmlContent = GenerateReportContent(reportRoot, directory, imagesFolderPath);

            // Create final HTML with embedded CSS and JavaScript
            string finalHtml = CreateCompleteHtml(htmlContent, css, reportRoot);

            // Save the output
            File.WriteAllText(outputPath, finalHtml, Encoding.UTF8);
        }

        private string GetImagesFolder(string directory)
        {
            // Look for folders matching pattern images_*
            var dirs = Directory.GetDirectories(directory, "images_*");
            return dirs.Length > 0 ? Path.GetFileName(dirs[0]) : "images";
        }

        private string GenerateReportContent(XElement report, string directory, string imagesFolderName)
        {
            StringBuilder html = new StringBuilder();

            // Get root activity
            XElement activity = report.Element("activity");
            if (activity == null) return "<p>No test data found</p>";

            // Extract summary info
            string testsuite = activity.Attribute("testsuite")?.Value ?? "Test Suite";
            string result = activity.Attribute("result")?.Value ?? "Unknown";
            string duration = activity.Attribute("duration")?.Value ?? "0";
            string timestamp = activity.Attribute("timestamp")?.Value ?? "";
            string endtime = activity.Attribute("endtime")?.Value ?? "";

            int totalTests = int.Parse(activity.Attribute("totalsuccesstestcasecount")?.Value ?? "0") + 
                           int.Parse(activity.Attribute("totalfailedtestcasecount")?.Value ?? "0");
            int passed = int.Parse(activity.Attribute("totalsuccesstestcasecount")?.Value ?? "0");
            int failed = int.Parse(activity.Attribute("totalfailedtestcasecount")?.Value ?? "0");

            // Generate header section
            html.AppendLine("<div class='report-header'>");
            html.AppendLine($"<h1>{System.Security.SecurityElement.Escape(testsuite)}</h1>");
            html.AppendLine($"<div class='summary'>");
            html.AppendLine($"<div class='summary-item'>");
            html.AppendLine($"<strong>Test Suite:</strong> {System.Security.SecurityElement.Escape(testsuite)}");
            html.AppendLine($"</div>");
            html.AppendLine($"<div class='summary-item result-{result.ToLower()}'>");
            html.AppendLine($"<strong>Result:</strong> {result}");
            html.AppendLine($"</div>");
            html.AppendLine($"<div class='summary-item'>");
            html.AppendLine($"<strong>Total Tests:</strong> {totalTests}");
            html.AppendLine($"</div>");
            html.AppendLine($"<div class='summary-item success'>");
            html.AppendLine($"<strong>Passed:</strong> {passed}");
            html.AppendLine($"</div>");
            html.AppendLine($"<div class='summary-item failed'>");
            html.AppendLine($"<strong>Failed:</strong> {failed}");
            html.AppendLine($"</div>");
            html.AppendLine($"<div class='summary-item'>");
            html.AppendLine($"<strong>Duration:</strong> {FormatDuration(duration)}");
            html.AppendLine($"</div>");
            html.AppendLine($"<div class='summary-item'>");
            html.AppendLine($"<strong>Started:</strong> {FormatTimestamp(timestamp)}");
            html.AppendLine($"</div>");
            html.AppendLine($"<div class='summary-item'>");
            html.AppendLine($"<strong>Ended:</strong> {FormatTimestamp(endtime)}");
            html.AppendLine($"</div>");
            html.AppendLine($"<div class='summary-item filter-item'>");
            html.AppendLine("<label class='filter-label'>");
            html.AppendLine("<input type='checkbox' id='filterFailed' onchange='applyFilter()'>");
            html.AppendLine("<span>Failed</span>");
            html.AppendLine("</label>");
            html.AppendLine($"</div>");
            html.AppendLine($"</div>");
            html.AppendLine("</div>");

            // Generate test results tree
            html.AppendLine("<div class='report-content'>");
            html.AppendLine("<h2>Test Execution Details</h2>");
            html.AppendLine("<div class='test-tree'>");

            GenerateActivityTree(activity, html, directory, imagesFolderName, 0);

            html.AppendLine("</div>");
            html.AppendLine("</div>");

            return html.ToString();
        }

        private void GenerateActivityTree(XElement activity, StringBuilder html, string directory, string imagesFolderName, int level)
        {
            string type = activity.Attribute("type")?.Value ?? "activity";

            // Try multiple attributes for the title: displayName, testcontainername, modulename, headertext
            string displayName = activity.Attribute("displayName")?.Value 
                              ?? activity.Attribute("testcontainername")?.Value 
                              ?? activity.Attribute("modulename")?.Value 
                              ?? activity.Attribute("headertext")?.Value 
                              ?? "Test Item";

            string result = activity.Attribute("result")?.Value ?? "";
            string duration = activity.Attribute("duration")?.Value ?? "";
            string timestamp = activity.Attribute("timestamp")?.Value ?? "";

            string resultClass = result.ToLower();

            string indent = new string(' ', level * 20);

            html.AppendLine($"<div class='activity-item level-{level} result-filter-{resultClass}' style='margin-left: {level * 20}px;'>");
            html.AppendLine($"<div class='activity-header result-{resultClass}'>");
            html.AppendLine($"<span class='toggle'>▼</span>");
            html.AppendLine($"<span class='icon-{type}'></span>");
            html.AppendLine($"<strong>{System.Security.SecurityElement.Escape(displayName)}</strong>");

            // Duration badge before result badge
            if (!string.IsNullOrEmpty(duration))
            {
                html.AppendLine($"<span class='duration'>{FormatDuration(duration)}</span>");
            }

            html.AppendLine($"<span class='result-badge {resultClass}'>{result}</span>");
            html.AppendLine($"</div>");

            html.AppendLine($"<div class='activity-details'>");

            // Add metainfo if present
            var metainfo = activity.Element("metainfo");
            if (metainfo != null)
            {
                html.AppendLine("<div class='metainfo'>");
                foreach (var info in metainfo.Elements())
                {
                    string key = info.Attribute("key")?.Value ?? info.Name.LocalName;
                    string value = info.Attribute("value")?.Value ?? info.Value;
                    html.AppendLine($"<div><strong>{System.Security.SecurityElement.Escape(key)}:</strong> {System.Security.SecurityElement.Escape(value)}</div>");
                }
                html.AppendLine("</div>");
            }

            // Add items (log messages, screenshots, etc.)
            foreach (var item in activity.Elements("item"))
            {
                string itemLevel = item.Attribute("level")?.Value ?? "Info";
                string itemCategory = item.Attribute("category")?.Value ?? "";
                string itemMessage = item.Attribute("message")?.Value ?? item.Element("message")?.Value ?? "";

                // Ranorex uses different attributes for screenshots
                string screenshotFile = item.Attribute("screenshotfile")?.Value 
                                     ?? item.Attribute("errimg")?.Value 
                                     ?? item.Attribute("imgpath")?.Value;

                string levelClass = itemLevel.ToLower();
                html.AppendLine($"<div class='log-item level-{levelClass}'>");
                html.AppendLine($"<span class='log-level {levelClass}'>{itemLevel}</span>");

                if (!string.IsNullOrEmpty(itemCategory))
                {
                    html.AppendLine($"<span class='log-category'>[{System.Security.SecurityElement.Escape(itemCategory)}]</span>");
                }

                if (!string.IsNullOrEmpty(itemMessage))
                {
                    html.AppendLine($"<span class='log-message'>{System.Security.SecurityElement.Escape(itemMessage)}</span>");
                }

                // Embed screenshot if present
                if (!string.IsNullOrEmpty(screenshotFile))
                {
                    // Screenshot path might be relative or full
                    string imagePath = screenshotFile;

                    // If it's a relative path, combine with directory
                    if (!Path.IsPathRooted(screenshotFile))
                    {
                        imagePath = Path.Combine(directory, screenshotFile);
                    }

                    if (File.Exists(imagePath))
                    {
                        string base64Image = ConvertImageToBase64(imagePath);
                        string mimeType = GetMimeType(Path.GetExtension(imagePath));
                        string dataUri = $"data:{mimeType};base64,{base64Image}";

                        html.AppendLine($"<div class='screenshot'>");
                        html.AppendLine($"<div class='screenshot-actions'>");
                        html.AppendLine($"<button class='screenshot-btn' onclick='openOverlay(\"{dataUri}\")'>🔍 View in Overlay</button>");
                        html.AppendLine($"<button class='screenshot-btn' onclick='openInViewer(\"{dataUri}\")'>🖼️ Open in Viewer</button>");
                        html.AppendLine($"</div>");
                        html.AppendLine($"<img src='{dataUri}' alt='Screenshot' onclick='openOverlay(\"{dataUri}\")' title='Click to view in overlay' />");
                        html.AppendLine($"</div>");
                    }
                    else
                    {
                        // Debug: show that screenshot path was found but file doesn't exist
                        html.AppendLine($"<div class='screenshot-missing'>");
                        html.AppendLine($"<span>⚠️ Screenshot not found: {System.Security.SecurityElement.Escape(screenshotFile)}</span>");
                        html.AppendLine($"</div>");
                    }
                }

                html.AppendLine("</div>");
            }

            // Recursively process child activities
            foreach (var childActivity in activity.Elements("activity"))
            {
                GenerateActivityTree(childActivity, html, directory, imagesFolderName, level + 1);
            }

            html.AppendLine("</div>");
            html.AppendLine("</div>");
        }

        private string ConvertImageToBase64(string imagePath)
        {
            byte[] imageBytes = File.ReadAllBytes(imagePath);
            return Convert.ToBase64String(imageBytes);
        }

        private string FormatDuration(string duration)
        {
            if (string.IsNullOrEmpty(duration)) return "0s";

            if (duration.Contains("ms"))
            {
                return duration;
            }

            if (double.TryParse(duration, out double seconds))
            {
                if (seconds < 1)
                {
                    return $"{(int)(seconds * 1000)}ms";
                }
                else if (seconds < 60)
                {
                    return $"{seconds:F2}s";
                }
                else
                {
                    int minutes = (int)(seconds / 60);
                    int secs = (int)(seconds % 60);
                    return $"{minutes}m {secs}s";
                }
            }

            return duration;
        }

        private string FormatTimestamp(string timestamp)
        {
            if (DateTime.TryParse(timestamp, out DateTime dt))
            {
                return dt.ToString("yyyy-MM-dd HH:mm:ss");
            }
            return timestamp;
        }

        private string GetMimeType(string extension)
        {
            return extension.ToLower() switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                _ => "application/octet-stream"
            };
        }

        private string CreateCompleteHtml(string htmlContent, string css, XElement report)
        {
            string testsuite = report.Element("activity")?.Attribute("testsuite")?.Value ?? "Test Report";

            return $@"<!DOCTYPE html>
<html>
<head>
    <meta charset=""utf-8"">
    <title>{System.Security.SecurityElement.Escape(testsuite)} - Test Report</title>
    <style>
{css}

/* Professional Light Mode Custom Styles */
* {{
    box-sizing: border-box;
}}

body {{
    font-family: 'Segoe UI', -apple-system, BlinkMacSystemFont, 'Helvetica Neue', Arial, sans-serif;
    margin: 0;
    padding: 20px;
    background: linear-gradient(135deg, #f5f7fa 0%, #e8ecf1 100%);
    color: #2c3e50;
    text-align: left;
    min-height: 100vh;
}}

.report-header {{
    background: linear-gradient(135deg, #ffffff 0%, #f8f9fa 100%);
    padding: 30px;
    border-radius: 12px;
    margin-bottom: 25px;
    box-shadow: 0 4px 12px rgba(0,0,0,0.08);
    border: 1px solid #e1e8ed;
    text-align: left;
}}

.report-header h1 {{
    margin: 0 0 20px 0;
    color: #1a202c;
    font-size: 32px;
    font-weight: 700;
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
    -webkit-background-clip: text;
    -webkit-text-fill-color: transparent;
    background-clip: text;
}}

.summary {{
    display: flex;
    flex-wrap: wrap;
    gap: 15px;
}}

.summary-item {{
    padding: 14px 20px;
    background: #ffffff;
    border-radius: 10px;
    border-left: 4px solid #cbd5e0;
    color: #2d3748;
    font-size: 14px;
    box-shadow: 0 2px 8px rgba(0,0,0,0.06);
    transition: all 0.3s ease;
}}

.summary-item:hover {{
    transform: translateY(-2px);
    box-shadow: 0 4px 12px rgba(0,0,0,0.1);
}}

.summary-item.result-success {{
    border-left-color: #48bb78;
    background: linear-gradient(135deg, #f0fdf4 0%, #e8f9f0 100%);
}}

.summary-item.result-failed {{
    border-left-color: #f56565;
    background: linear-gradient(135deg, #fff5f5 0%, #fed7d7 100%);
}}

.summary-item.success {{
    color: #2f855a;
    font-weight: 700;
}}

.summary-item.failed {{
    color: #c53030;
    font-weight: 700;
}}

.summary-item.filter-item {{
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
    border-left-color: #667eea;
    padding: 10px 16px;
}}

.filter-label {{
    display: flex;
    align-items: center;
    gap: 8px;
    font-size: 14px;
    font-weight: 600;
    cursor: pointer;
    user-select: none;
    margin: 0;
}}

.filter-label input[type=""checkbox""] {{
    width: 18px;
    height: 18px;
    cursor: pointer;
    accent-color: #ffffff;
}}

.filter-label span {{
    color: #ffffff;
    font-weight: 700;
}}

.report-content {{
    background: linear-gradient(135deg, #ffffff 0%, #f8f9fa 100%);
    padding: 30px;
    border-radius: 12px;
    box-shadow: 0 4px 12px rgba(0,0,0,0.08);
    border: 1px solid #e1e8ed;
    text-align: left;
}}

.report-content h2 {{
    margin-top: 0;
    color: #1a202c;
    border-bottom: 3px solid #667eea;
    padding-bottom: 12px;
    font-size: 24px;
    font-weight: 700;
}}

.test-tree {{
    margin-top: 20px;
}}

.activity-item {{
    margin-bottom: 12px;
    border: 1px solid #e2e8f0;
    border-radius: 10px;
    overflow: hidden;
    background: #ffffff;
    box-shadow: 0 2px 8px rgba(0,0,0,0.04);
    transition: all 0.3s ease;
}}

.activity-item:hover {{
    box-shadow: 0 4px 16px rgba(0,0,0,0.08);
    transform: translateY(-1px);
}}

.activity-header {{
    padding: 16px;
    background: linear-gradient(135deg, #fafbfc 0%, #f7f9fb 100%);
    cursor: pointer;
    display: flex;
    align-items: center;
    gap: 10px;
    font-size: 14px;
    flex-wrap: wrap;
    transition: background 0.3s ease;
    border-bottom: 1px solid #e2e8f0;
}}

.activity-header:hover {{
    background: linear-gradient(135deg, #f1f3f5 0%, #e9ecef 100%);
}}

.activity-header.result-success {{
    border-left: 5px solid #48bb78;
}}

.activity-header.result-failed {{
    border-left: 5px solid #f56565;
}}

.activity-header.result-blocked {{
    border-left: 5px solid #ed8936;
}}

.toggle {{
    cursor: pointer;
    user-select: none;
    font-size: 14px;
    color: #4a5568;
    font-weight: bold;
}}

.result-badge {{
    padding: 6px 12px;
    border-radius: 6px;
    font-size: 11px;
    font-weight: 700;
    text-transform: uppercase;
    letter-spacing: 0.5px;
    flex-shrink: 0;
}}

.result-badge.success {{
    background: linear-gradient(135deg, #48bb78 0%, #38a169 100%);
    color: #ffffff;
    box-shadow: 0 2px 8px rgba(72,187,120,0.3);
}}

.result-badge.failed {{
    background: linear-gradient(135deg, #f56565 0%, #e53e3e 100%);
    color: #ffffff;
    box-shadow: 0 2px 8px rgba(245,101,101,0.3);
}}

.result-badge.blocked {{
    background: linear-gradient(135deg, #ed8936 0%, #dd6b20 100%);
    color: #ffffff;
    box-shadow: 0 2px 8px rgba(237,137,54,0.3);
}}

.duration {{
    color: #718096;
    font-size: 12px;
    white-space: nowrap;
    margin-left: auto;
    margin-right: 8px;
    background: #edf2f7;
    padding: 6px 12px;
    border-radius: 6px;
    font-weight: 600;
    border: 1px solid #e2e8f0;
    flex-shrink: 0;
}}

.activity-details {{
    padding: 20px;
    background: #ffffff;
    border-top: 1px solid #e2e8f0;
}}

.metainfo {{
    background: #f7fafc;
    padding: 15px;
    margin-bottom: 15px;
    border-radius: 8px;
    font-size: 13px;
    text-align: left;
    border: 1px solid #e2e8f0;
}}

.metainfo div {{
    margin: 8px 0;
    text-align: left;
    color: #4a5568;
}}

.metainfo strong {{
    color: #667eea;
    font-weight: 600;
}}

.log-item {{
    padding: 12px 16px;
    margin: 10px 0;
    border-left: 4px solid #cbd5e0;
    background: #f7fafc;
    font-size: 13px;
    text-align: left;
    border-radius: 6px;
    transition: all 0.3s ease;
}}

.log-item:hover {{
    background: #edf2f7;
    box-shadow: 0 2px 6px rgba(0,0,0,0.05);
}}

.log-item.level-info {{
    border-left-color: #4299e1;
    background: #ebf8ff;
}}

.log-item.level-warn, .log-item.level-warning {{
    border-left-color: #ed8936;
    background: #fffaf0;
}}

.log-item.level-error, .log-item.level-failure {{
    border-left-color: #f56565;
    background: #fff5f5;
}}

.log-item.level-success {{
    border-left-color: #48bb78;
    background: #f0fff4;
}}

.log-level {{
    display: inline-block;
    padding: 4px 10px;
    border-radius: 5px;
    font-size: 11px;
    font-weight: 700;
    margin-right: 10px;
    text-transform: uppercase;
    letter-spacing: 0.5px;
}}

.log-level.info {{
    background: linear-gradient(135deg, #4299e1 0%, #3182ce 100%);
    color: white;
    box-shadow: 0 2px 6px rgba(66,153,225,0.3);
}}

.log-level.warn, .log-level.warning {{
    background: linear-gradient(135deg, #ed8936 0%, #dd6b20 100%);
    color: white;
    box-shadow: 0 2px 6px rgba(237,137,54,0.3);
}}

.log-level.error, .log-level.failure {{
    background: linear-gradient(135deg, #f56565 0%, #e53e3e 100%);
    color: white;
    box-shadow: 0 2px 6px rgba(245,101,101,0.3);
}}

.log-level.success {{
    background: linear-gradient(135deg, #48bb78 0%, #38a169 100%);
    color: white;
    box-shadow: 0 2px 6px rgba(72,187,120,0.3);
}}

.log-category {{
    color: #718096;
    font-style: italic;
    margin-right: 10px;
    font-weight: 500;
}}

.log-message {{
    color: #2d3748;
    line-height: 1.6;
}}

.screenshot {{
    margin-top: 20px;
    text-align: left;
    padding: 20px;
    background: #f7fafc;
    border-radius: 10px;
    border: 2px solid #e2e8f0;
}}

.screenshot-actions {{
    display: flex;
    gap: 12px;
    margin-bottom: 15px;
}}

.screenshot-btn {{
    padding: 10px 18px;
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
    color: white;
    border: none;
    border-radius: 8px;
    cursor: pointer;
    font-size: 13px;
    font-weight: 600;
    transition: all 0.3s ease;
    box-shadow: 0 3px 10px rgba(102,126,234,0.3);
}}

.screenshot-btn:hover {{
    background: linear-gradient(135deg, #5568d3 0%, #6741d9 100%);
    box-shadow: 0 5px 15px rgba(102,126,234,0.4);
    transform: translateY(-2px);
}}

.screenshot-btn:active {{
    transform: translateY(0);
}}

.screenshot-missing {{
    margin-top: 10px;
    padding: 10px 15px;
    background: #fff5f5;
    border-left: 4px solid #f56565;
    border-radius: 6px;
    color: #c53030;
    font-size: 12px;
}}

.screenshot img {{
    max-width: 100%;
    max-height: 600px;
    border: 2px solid #cbd5e0;
    border-radius: 8px;
    box-shadow: 0 4px 12px rgba(0,0,0,0.1);
    display: block;
    cursor: pointer;
    transition: all 0.3s ease;
}}

.screenshot img:hover {{
    transform: scale(1.02);
    box-shadow: 0 8px 24px rgba(102,126,234,0.3);
    border-color: #667eea;
}}

/* Overlay styles */
.overlay {{
    display: none;
    position: fixed;
    z-index: 9999;
    left: 0;
    top: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(0,0,0,0.92);
    overflow: auto;
    animation: fadeIn 0.3s ease;
}}

@keyframes fadeIn {{
    from {{ opacity: 0; }}
    to {{ opacity: 1; }}
}}

.overlay-content {{
    position: relative;
    margin: 2% auto;
    padding: 20px;
    max-width: 95%;
    max-height: 95%;
    display: flex;
    justify-content: center;
    align-items: center;
}}

.overlay-content img {{
    max-width: 100%;
    max-height: 90vh;
    object-fit: contain;
    border-radius: 8px;
    box-shadow: 0 8px 32px rgba(0,0,0,0.5);
}}

.overlay-close {{
    position: absolute;
    top: 20px;
    right: 40px;
    color: #fff;
    font-size: 40px;
    font-weight: bold;
    cursor: pointer;
    z-index: 10000;
    background: rgba(0,0,0,0.7);
    width: 50px;
    height: 50px;
    border-radius: 50%;
    display: flex;
    align-items: center;
    justify-content: center;
    transition: all 0.3s ease;
}}

.overlay-close:hover {{
    background: linear-gradient(135deg, #f56565 0%, #e53e3e 100%);
    transform: rotate(90deg);
}}
    </style>
    <script>
        // Filter function
        function applyFilter() {{
            const filterCheckbox = document.getElementById('filterFailed');
            const isFilterActive = filterCheckbox.checked;
            const allItems = document.querySelectorAll('.activity-item');

            allItems.forEach(item => {{
                const isSuccess = item.classList.contains('result-filter-success');
                const isFailed = item.classList.contains('result-filter-failed');
                const isBlocked = item.classList.contains('result-filter-blocked');

                if (isFilterActive) {{
                    // Show only failed and blocked
                    if (isFailed || isBlocked) {{
                        item.style.display = 'block';
                    }} else {{
                        item.style.display = 'none';
                    }}
                }} else {{
                    // Show all
                    item.style.display = 'block';
                }}
            }});
        }}

        // Overlay functions
        function openOverlay(imageDataUri) {{
            const overlay = document.getElementById('imageOverlay');
            const overlayImg = document.getElementById('overlayImage');
            overlayImg.src = imageDataUri;
            overlay.style.display = 'block';
            document.body.style.overflow = 'hidden';
        }}

        function closeOverlay() {{
            const overlay = document.getElementById('imageOverlay');
            overlay.style.display = 'none';
            document.body.style.overflow = 'auto';
        }}

        function openInViewer(imageDataUri) {{
            // Create a temporary link to download/open the image
            const link = document.createElement('a');
            link.href = imageDataUri;
            link.download = 'screenshot_' + new Date().getTime() + '.png';

            // For viewing: open in new tab
            const newWindow = window.open();
            newWindow.document.write('<html><head><title>Screenshot</title><style>body{{margin:0;display:flex;justify-content:center;align-items:center;background:#000;height:100vh;}}</style></head><body><img src=""' + imageDataUri + '"" style=""max-width:100%;max-height:100vh;object-fit:contain;""></body></html>');
        }}

        document.addEventListener('DOMContentLoaded', function() {{
            // Add toggle functionality
            document.querySelectorAll('.activity-header').forEach(header => {{
                header.addEventListener('click', function() {{
                    const details = this.nextElementSibling;
                    const toggle = this.querySelector('.toggle');

                    if (details.style.display === 'none') {{
                        details.style.display = 'block';
                        toggle.textContent = '▼';
                    }} else {{
                        details.style.display = 'none';
                        toggle.textContent = '▶';
                    }}
                }});
            }});

            // Initially collapse all passed sections and expand failed ones
            document.querySelectorAll('.activity-item').forEach(item => {{
                const header = item.querySelector('.activity-header');
                const details = item.querySelector('.activity-details');
                const toggle = header.querySelector('.toggle');

                // Check if this is a failed item
                const isFailed = header.classList.contains('result-failed') || 
                                header.classList.contains('result-blocked');

                if (isFailed) {{
                    // Expand failed items
                    details.style.display = 'block';
                    toggle.textContent = '▼';
                }} else {{
                    // Collapse passed items
                    details.style.display = 'none';
                    toggle.textContent = '▶';
                }}
            }});

            // Close overlay on ESC key
            document.addEventListener('keydown', function(e) {{
                if (e.key === 'Escape') {{
                    closeOverlay();
                }}
            }});
        }});
    </script>
</head>
<body>
{htmlContent}

<!-- Image Overlay -->
<div id=""imageOverlay"" class=""overlay"" onclick=""if(event.target.id === 'imageOverlay') closeOverlay()"">
    <span class=""overlay-close"" onclick=""closeOverlay()"">&times;</span>
    <div class=""overlay-content"">
        <img id=""overlayImage"" src="""" alt=""Screenshot"">
    </div>
</div>

</body>
</html>";
        }
    }
}

