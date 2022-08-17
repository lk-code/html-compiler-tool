﻿using Cocona;
using Cocona.Builder;
using HtmlCompiler.Core.Extensions;
using HtmlCompiler.Core.Interfaces;
using HtmlCompiler.Core.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;

CoconaAppBuilder? builder = CoconaApp.CreateBuilder();

builder.Services.TryAddSingleton<IHtmlRenderer, HtmlRenderer>();

CoconaApp? app = builder.Build();

app.AddCommand(async (
    [Argument(Description = "path to the source file")] string sourceFile,
    [Argument(Description = "path to the output file")] string outputFile,
    IHtmlRenderer htmlRenderer) =>
{
    string fullOutputFilePath = outputFile.FromSourceFilePath(sourceFile);

    Console.WriteLine($"compile file {sourceFile} to {fullOutputFilePath}");

    try
    {
        string content = await htmlRenderer.RenderAsync(sourceFile);
    }
    catch (FileNotFoundException)
    {
        Console.WriteLine($"file {sourceFile} not found");
    }
});

app.Run();