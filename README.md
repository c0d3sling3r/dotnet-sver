# dotnet-sver
***
`dotnet-sver` is a CLI tool for managing project versions using Semantic Versioning in .NET applications. It provides a set of commands to list, set, upgrade and degrade project versions, making it easy to handle versioning within your development workflow.

Generally semantic versioning has three main parts. *MAJOR*, *MINOR* and *PATCH* and represents with the following template: X.Y.Z which X holds the *MAJOR* part, Y holds the *MINOR* and Z holds the *PATCH*.

It also supports pre-release and build versions which will be appended to X.Y.Z. 

> For more information, I refer you to [here](https://semver.org/).

# Features
***
- List the project versions within the target solution using `list` command.
- Upgrade the current version of the projects using `upgrade` command.
- Degrade the current version of the projects using `degrade` command.
- Set the version of the projects using the `set` command.
- Support for Semantic Versioning (MAJOR.MINOR.PATCH).
- Option to specify the project number for version manipulation.
- Option to operate on all projects at once.

# Getting Started
***
## Prerequisites
- Make sure you have latest [.NET SDK](https://dotnet.microsoft.com/en-us/download) installed on your machine.

## Installation
Install `dotnet-sver` from the universal source as a global tool using the following command:
```shell
$ dotnet tool install -g dotnet-sver
```

Build locally and install globally:
```shell
$ git clone https://github.com/shojajou/dotnet-sver
$ cd SemanticVersioning.Net/src/SemanticVersioning.Net.Cli
$ dotnet pack -c Release -o nupkg
$ dotnet tool install -g dotnet-sver --add-source .\nupkg
```

## Usage
> You can call `dotnet sver` as well as `dotnet-sver` to use the tool.

First, your cwd(current working directory) must be somewhere inside your solution directory.
```shell
cd <somewhere-inside-yout-target-solution>
```

### List Command

By executing this command, your projects and their versions will be printed out.

```shell
$ dotnet-sver list
1) Project-1.csproj [version]
2) Project-2.csproj [version]
3) Project-3.csproj [version]
4) Project-4.csproj [version]
5) Project-5.csproj [version]
```
  
### Upgrade Command
 
*Options:*

- `--project-number` or `-p`: specifying the project targeting for upgrading.
- `--all` or `-a`: upgrades all the projects inside the solution.
  > Obviously, there should be either -p or -a in the command.
- `--major`: upgrades the X part of the version.
- `--minor`: upgrades the Y part of the version.
- `--patch`: upgrades the Z part of the version.

There are two ways of upgrading the projects: 
- By specifying the project number printing in `dotnet-sver list` result using `--project-number` or `-p` option.
  ```shell
  $ dotnet-sver upgrade -p <project-number> [version-part-option]
  
  # For instance, the following command, upgrades the Z part of the version of the project number #3:
  $ dotnet-sver upgrade -p 3 --patch 
  ```
- And *IF* you want to upgrade all the versions of the all projects, you can pass `-a` or `--all` option.
  ```shell
  $ dotnet-sver upgrade -a [version-part-option]
  
  # For instance, the following command, upgrades the Y part of the version of the all projects.
  $ dotnet-sver upgrade -a --minor 
  ```
  
### Degrade Command
 
*Options:*

- `--project-number` or `-p`: specifying the project targeting for degrading.
- `--all` or `-a`: degrades all the projects inside the solution.
  > Obviously, there should be either -p or -a in the command.
- `--major`: degrades the X part of the version.
- `--minor`: degrades the Y part of the version.
- `--patch`: degrades the Z part of the version.

There are two ways of degrading the projects: 
- By specifying the project number printing in `dotnet-sver list` result using `--project-number` or `-p` option.
  ```shell
  $ dotnet-sver degrade -p <project-number> [version-part-option]
  
  # For instance, the following command, degrades the Z part of the version of the project number #3:
  $ dotnet-sver degrade -p 3 --patch 
  ```
- And *IF* you want to degrade all the versions of the all projects, you can pass `-a` or `--all` option.
  ```shell
  $ dotnet-sver degrade -a [version-part-option]
  
  # For instance, the following command, degrades the Y part of the version of the all projects.
  $ dotnet-sver degrade -a --minor 
  ```
  
### Set Command
Maybe you'll be more comfortable to set the versions explicitly. I have your back 😎. Use the magical `set` command.
 
*Options:*

- `--project-number` or `-p`: specifying the project targeting for upgrading.
- `--all` or `-a`: degrades all the projects inside the solution.
  > Obviously, there should be either -p or -a in the command.

There are two ways of setting the projects: 
- By specifying the project number printing in `dotnet-sver list` result using `--project-number` or `-p` option.
  ```shell
  $ dotnet-sver degrade -p <project-number> [version-part-option]
  
  # For instance, the following command, degrades the Z part of the version of the project number #3:
  $ dotnet-sver degrade -p 3 --patch 
  ```
- And *IF* you want to degrade all the versions of the all projects, you can pass `-a` or `--all` option.
  ```shell
  $ dotnet-sver degrade -a [version-part-option]
  
  # For instance, the following command, degrades the Y part of the version of the all projects.
  $ dotnet-sver degrade -a --minor 
  ```

# Contributing
Contributions are welcome! If you encounter any issues or have suggestions for improvement, please open an issue on the GitHub repository. Pull requests are also appreciated.

# License
This project is licensed under the [MIT License](https://opensource.org/license/mit/).