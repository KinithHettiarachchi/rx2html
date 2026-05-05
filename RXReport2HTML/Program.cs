namespace RXReport2HTML
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // Check if command-line argument is provided
            if (args.Length > 0)
            {
                // Silent mode - generate report from command line
                string rxlogPath = args[0];

                if (!File.Exists(rxlogPath))
                {
                    Console.WriteLine($"Error: File not found: {rxlogPath}");
                    Environment.Exit(1);
                    return;
                }

                if (!rxlogPath.EndsWith(".rxlog", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Error: File must be a .rxlog file");
                    Environment.Exit(1);
                    return;
                }

                try
                {
                    Console.WriteLine($"Processing: {rxlogPath}");

                    // Generate the report
                    string directory = Path.GetDirectoryName(rxlogPath) ?? "";
                    string outputPath = Path.Combine(directory, "report.html");

                    var form = new Form1();
                    form.GenerateReportFromPath(rxlogPath, outputPath);

                    Console.WriteLine($"Report generated successfully: {outputPath}");
                    Environment.Exit(0);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error generating report: {ex.Message}");
                    Environment.Exit(1);
                }
            }
            else
            {
                // GUI mode - open the form normally
                ApplicationConfiguration.Initialize();
                Application.Run(new Form1());
            }
        }
    }
}