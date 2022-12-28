﻿using System;
using HtmlCompiler.Core.Components;
using HtmlCompiler.Core.Extensions;
using HtmlCompiler.Core.Interfaces;

namespace HtmlCompiler.Core;

public class HtmlWatcher : IHtmlWatcher
{
    private readonly IHtmlRenderer _htmlRenderer;

    private string _sourceDirectoryPath = string.Empty;
    private string _outputDirectoryPath = string.Empty;
    private FileChangeDetector? _fileDetector = null;

    public HtmlWatcher(IHtmlRenderer htmlRenderer)
    {
        this._htmlRenderer = htmlRenderer ?? throw new ArgumentNullException(nameof(htmlRenderer));
    }

    ~HtmlWatcher()
    {
        this.UnregisterFileDetector();
    }

    private void UnregisterFileDetector()
    {
        if (this._fileDetector != null)
        {
            this._fileDetector.FileChanged -= FileChangeDetector_FileChanged;
        }
    }

    public async Task WatchDirectoryAsync(string? sourcePath, string? outputPath)
    {
        // prepare
        this.SetProjectPaths(sourcePath, outputPath);

        this._sourceDirectoryPath.EnsurePath();
        this._outputDirectoryPath.EnsurePath();

        // compile files
        await this.CompileFiles();

        // watch for changes
        this.UnregisterFileDetector();
        this._fileDetector = new FileChangeDetector(this._sourceDirectoryPath);
        this._fileDetector.FileChanged += FileChangeDetector_FileChanged;

        ConsoleKeyInfo key = Console.ReadKey(true);

        // wait for Ctrl+C
        while (true)
        {
            if (key.Modifiers == ConsoleModifiers.Control
                && key.Key == ConsoleKey.C)
            {
                Console.WriteLine("exited htmlc");
                return;
            }
        }
    }

    private async void FileChangeDetector_FileChanged(object? sender, FileSystemEventArgs e)
    {
        //string filePath = e.FullPath;
        //string fileExtension = Path.GetExtension(filePath);

        //Console.WriteLine($"file changing detected ({filePath})...");

        //switch (fileExtension.ToLower().Replace(".", ""))
        //{
        //    case "html":
        //        {
        //            await this.CompileFiles();
        //        }
        //        break;
        //}

        await this.CompileFiles();
    }

    private async Task CompileFiles()
    {
        Console.WriteLine($"compiling...");

        List<string> files = this._sourceDirectoryPath.GetAllFiles();
        List<string> layoutFiles = this.GetLayoutFiles(files);
        List<string> sourceFiles = this.GetHtmlFiles(files);

        // get files
        //Console.WriteLine($"found {layoutFiles.Count()} layout files:");
        //foreach (string filePath in layoutFiles)
        //{
        //    Console.WriteLine(filePath);
        //}

        //Console.WriteLine($"found {sourceFiles.Count()} source files:");
        //foreach (string filePath in sourceFiles)
        //{
        //    Console.WriteLine(filePath);
        //}

        foreach (string sourceFile in sourceFiles)
        {
            string fileToCompile = sourceFile;
            string outputFile = Path.Combine(this._outputDirectoryPath, Path.GetFileName(fileToCompile));

            Console.WriteLine($"compile {fileToCompile} to {outputFile}");

            try
            {
                await this._htmlRenderer.RenderToFileAsync(fileToCompile, outputFile);
            }
            catch (FileNotFoundException err)
            {
                Console.WriteLine($"file {err.FileName} not found");
            }
        }
    }

    private List<string> GetHtmlFiles(List<string> files)
    {
        List<string> htmlFilePaths = files.Where(file => Path.GetExtension(file) == ".html")
            .ToList();

        return htmlFilePaths.Where(filePath => !Path.GetFileName(filePath).StartsWith("_"))
            .ToList();
    }

    public List<string> GetLayoutFiles(List<string> files)
    {
        List<string> htmlFiles = files.Where(file => Path.GetExtension(file) == ".html")
            .ToList();

        return htmlFiles.Where(filePath => Path.GetFileName(filePath).StartsWith("_"))
            .ToList();
    }

    private void SetProjectPaths(string? sourcePath, string? outputPath)
    {
        if (!string.IsNullOrEmpty(sourcePath)
            && !string.IsNullOrEmpty(outputPath))
        {
            // Source and destination directories were specified

            this._sourceDirectoryPath = sourcePath;
            this._outputDirectoryPath = outputPath;

            return;
        }
        else if (!string.IsNullOrEmpty(sourcePath)
            && string.IsNullOrEmpty(outputPath))
        {
            // Only one directory was specified.
            // Use as project directory =>/dist for output and /src for source

            this._sourceDirectoryPath = Path.Combine(sourcePath, "src");
            this._outputDirectoryPath = Path.Combine(sourcePath, "dist");

            return;
        }
        else
        {
            // No path was specified => use the current directory

            string baseDirectory = Directory.GetCurrentDirectory();
            this._sourceDirectoryPath = Path.Combine(baseDirectory, "src");
            this._outputDirectoryPath = Path.Combine(baseDirectory, "dist");

            return;
        }
    }
}