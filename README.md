# html-compiler-tool

![html-compiler-tool](https://raw.githubusercontent.com/lk-code/html-compiler-tool/main/icon_128.png)

[![.NET Version](https://img.shields.io/badge/dotnet%20version-net6.0-blue?style=flat-square)](https://www.nuget.org/packages/htmlc/)
[![License](https://img.shields.io/github/license/lk-code/html-compiler-tool.svg?style=flat-square)](https://github.com/lk-code/html-compiler-tool/blob/master/LICENSE)
[![Build](https://github.com/lk-code/html-compiler-tool/actions/workflows/dotnet.yml/badge.svg)](https://github.com/lk-code/html-compiler-tool/actions/workflows/dotnet.yml)
[![Downloads](https://img.shields.io/nuget/dt/htmlc.svg?style=flat-square)](https://www.nuget.org/packages/htmlc/)
[![NuGet](https://img.shields.io/nuget/v/htmlc.svg?style=flat-square)](https://www.nuget.org/packages/htmlc/)

[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=lk-code_html-compiler-tool&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=lk-code_html-compiler-tool)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=lk-code_html-compiler-tool&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=lk-code_html-comvvpiler-tool)

[![buy me a coffe](https://cdn.buymeacoffee.com/buttons/v2/default-yellow.png)](https://www.buymeacoffee.com/lk.code)

This is the HTML Compiler Tool for your cli

## installation and update

### first: install the .NET Runtime
you need to install the .NET Runtime (its free and available for macos, linux and windows)
* [macOS](https://learn.microsoft.com/en-us/dotnet/core/install/macos)
* [Windows](https://learn.microsoft.com/en-us/dotnet/core/install/windows)
* [Linux](https://learn.microsoft.com/en-us/dotnet/core/install/linux)

### then: install the tool
then you can install the html-tool very simple via this command:

`dotnet tool install --global htmlc`

### update the tool

`dotnet tool update --global htmlc`

## usage

### compile a single file

generates HTML from the template file demo.html and writes it into the folder ".\output" with the same file name ".\output\demo.html":<br />
`htmlc compile .\source\demo.html .\output\`

generates HTML from the template file test.html and writes it into the file ".\output\sample.html":<br />
`htmlc compile .\source\test.html .\output\sample.html`

#### specify a template file
this file contains only the individual content:

`@Layout=_layout.html`<br />
`<section>`<br />
`    <div>Hello again</div>`<br />
`</section>`<br />

**important:** add the ***@Layout***-element to specify which file the layout is in.

#### specify a layout file
The layout file contains the HTML framework, which is identical for all generated files:<br />
`<html>`<br />
`    <head>`<br />
`        ...`<br />
`    </head>`<br />
`    <body>`<br />
`        @Body`<br />
`    </body>`<br />
`</html>`<br />

**important:** add the ***@Body***-element to specify where the content of the page should be written.

#### generated output

The above example generates the following code:
`<html>`<br />
`    <head>`<br />
`        ...`<br />
`    </head>`<br />
`    <body>`<br />
`<section>`<br />
`    <div>Hello again</div>`<br />
`</section>`<br />
`    </body>`<br />
`</html>`<br />

### watch your project directory

You can monitor your whole HTML project directory and compile it automatically. There are two options:

1. you specify only the path to the project folder. In this folder htmlc will look for /src and /dist (or will create these two folders). Under /src htmlc will monitor for file changes. /dist is used as output directory for the compiler:

`htmlc watch .\path\to\source\`

2. you specify the source directory and the output directory. These then behave like /src and /dist:

`htmlc watch .\path\to\source\ .\path\to\output\`

#### optional: add a style file to compile
You can also specify a style file to the watch command. This will then also be monitored and compiled in case of changes. Currently supported are Sass/SCSS and Less.

`htmlc watch [....] -s {relative/path/to/style.scss}`

for example:

`htmlc watch /Users/lk-code/Projects/Website/src /Users/lk-code/Projects/Website/dist -s /styles/main.scss`

htmlc now searches for the style file at /Users/lk-code/Projects/Website/src/styles/main.scss. The content is compiled and then saved to this location: /Users/lk-code/Projects/Website/dist/styles/main.css

## notices

this tool uses:
* [Cocona (MIT)](https://github.com/mayuki/Cocona) for console app environment
* [DartSassHost (MIT)](https://github.com/Taritsyn/DartSassHost) for scss compiling