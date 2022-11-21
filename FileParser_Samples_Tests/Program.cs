using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using FileParser_Samples_Tests.Models;
using Microsoft.Extensions.Configuration;
using SpreadsheetLight;

namespace FileParser_Samples_Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("App Running...");

            string completedFilesPath, fileName, filePath;
            var dateToday = DateTime.Today.ToString("d").Replace("/", "-");
            var time = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            DirectoryInfo newDirectory;
            fileName = time + "_results" + ".xlsx";

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json", optional: false);

            IConfiguration config = builder.Build();

            var basePath = config.GetSection("EnvironmentPath").Get<string>();

            completedFilesPath = basePath + "FilesComplete";

            //determine which file type
            string filePath_ICP = basePath + "FilesToRead_ICP";

            //do we have any files
            var filesToRead_ICP = Directory.EnumerateFiles(filePath_ICP, "*.csv");

            if (filesToRead_ICP.Any())
            {
                //create directories and files
                var IcpDirectory = Directory.CreateDirectory(completedFilesPath + "//ICP");
                newDirectory = Directory.CreateDirectory(IcpDirectory + "//" + dateToday);
                var archiveDirectory = Directory.CreateDirectory(newDirectory + "//Archive");

                filePath = newDirectory + "\\" + fileName;

                var models = new List<ICPFile>();
                foreach (var file in filesToRead_ICP)
                {
                    //file name
                    models.Add(new ICPFile()
                    {
                        FileName = Path.GetFileName(file),
                        Lines = System.IO.File.ReadAllLines(file)
                    });

                    File.Copy(file, Path.Combine(archiveDirectory.FullName, time + "_" + Path.GetFileName(file)));
                }

                var sheet = Generate_ICP_CSV(newDirectory, models);

                sheet.SaveAs(filePath);

                RemoveFiles(filesToRead_ICP, filePath_ICP);
            }
        }

        static SLDocument Generate_ICP_CSV(DirectoryInfo newDirectory, List<ICPFile> files)
        {
            SLDocument sl = new SLDocument();

            //Save the file names
            sl.RenameWorksheet("Sheet1", "File Names");
            var row = 1;
            var col = 1;

            SLStyle header = sl.CreateStyle();
            header.SetFontBold(true);

            foreach (var file in files)
            {
                sl.SetCellValue(row, col, file.FileName);
                row++;
            }

            //Save all data
            sl.AddWorksheet("All Data");
            row = 1;
            int fileCount = 0;
            sl.SetRowStyle(1, header);

            foreach (var file in files)
            {
                fileCount++;

                if(fileCount > 1)
                {
                    //remove headers
                    file.Lines = file.Lines.Skip(1).ToArray();
                }

                foreach (var line in file.Lines)
                {
                    col = 1;
                    var dataCells = line.Split(',');
                    foreach(var data in dataCells)
                    {
                        sl.SetCellValue(row, col, data);
                        col++;
                    }

                    if (row != 1)
                    {
                        sl.SetCellValue(row, col, file.FileName);
                    }

                    row++;
                }
            }

            //save ICP data
            row = 1;
            col = 1;
            fileCount = 0;
            sl.AddWorksheet("ICP Data");
            sl.SetRowStyle(1, header);
            foreach (var file in files)
            {
                fileCount++;

                if (fileCount > 1)
                {
                    //remove headers
                    file.Lines = file.Lines.Skip(1).ToArray();
                }

                foreach (var line in file.Lines)
                {
                    col = 1;
                    var dataCells = line.Split(',');
                    foreach (var data in dataCells)
                    {
                        if (col <= 3)
                        {
                            sl.SetCellValue(row, col, data);
                            col++;
                        } else if(col == 4 && row != 1)
                        {
                            //add file name
                            sl.SetCellValue(row, col, file.FileName);
                        }
                    }

                    row++;
                }
            }

            return sl;
        }

        static void RemoveFiles(IEnumerable<string> files, string path)
        {
            foreach (var file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                fileInfo.Delete();
            }

            var direc = Directory.CreateDirectory(path);
        }

    }
}
