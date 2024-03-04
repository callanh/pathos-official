# Pathos Official

This repository contains all content definitions and module generations for the traditional roguelike game **Pathos**. Fork this repository to start creating a new variant of Pathos! All game assets such as images, sounds and translation files reside in this repository.

## Licensing

Pathos has never been monetised and it is required that **variants must not be commercialised** in any way.

## Getting started

The content definitions and module generations are declared as C# code. This code must be built into an assembly before it can be executed by Pathos. The main project compiles against the assemblies for the installed version of Pathos for Windows Desktop. The game installation files include the `PathosMaker.exe` tool which is automated to process the asset pipeline.

## Installation steps

1. Download [**Pathos for Windows Desktop**](https://pathos.azurewebsites.net/).
2. Install Pathos to `C:\Games\Pathos` **.
3. Fork the [**Official public repository**](https://github.com/callanh/pathos-official.git).
4. Install **Microsoft Visual Studio 2022** _or_ **Microsoft Visual Code** plus build tools.

** _you will need to edit the paths in PathosOfficial.csproj if you don't use this default location._

### Visual Studio 2022

1. Install [**Visual Studio Community**](https://visualstudio.microsoft.com/) with the **.NET Desktop Development** workload.
2. Open `PathosOfficial.sln`.
3. Run **PathosOfficial** project.

### Visual Studio Code and Build Tools

1. Install [**Visual Studio Code**](https://code.visualstudio.com).
2. Install [**Visual Studio Build Tools**](https://aka.ms/vs/17/release/vs_BuildTools.exe) with the **.NET desktop build tools** workload.
3. Install Visual Studio Code extension **C# for Visual Studio Code (powered by OmniSharp)**.
4. Open `PathosOfficial.code-workspace`.
5. Press `Ctrl+Shift+B` to build.
6. Press `F5` to run **PathosOfficial** project.

If these steps are successful, the Pathos game will launch, running the forked code. Exit the game and make a minor change to the code before building and running again to see the difference in the game.

> NOTE: when the Pathos game self-updates you need to pull the latest commits from the Official repository and vice versa.

## Project overview

The main file is `PathosOfficial.cs` as it the top-level class that declares the campaign content and modules.

| Folder    | Description                                            |
|-----------|--------------------------------------------------------|
| Albums    | mp3 files for sound effects                            |
| Assets    | translation files, change updates and music tracks     |
| Atlases   | png files for tilesets                                 |
| Codex     | C# files for content definition                        |
| Modules   | C# files for module generation                         |
| Resources | Additional files referenced by the content and modules |
| Reports   | Generated text file to visualise the declared content  |

## Concepts and terminology

The `Manifest` is a neutral content definition that is executed by the game engine. It is intended to have no knowledge of the individually declared content.

The `Codex` is a way of naming the individual content that is registered in the manifest. This is useful for declaring content and for writing module generation algorithms as it allows direct reference to content. For example, the `barbarian` class starts with the `two-handed sword` item.

Because of the way .NET JIT (Just-In-Time) compilation works, it is desirable to serialise the codex into a binary file. This is named `Pathos.Codex` and is automatically compiled by the `PathosMaker` tool.

---

Please reach out to me for any help or even just to share your variant :)

**Email**
<mailto:hodgskin.callan@gmail.com>

**Twitter**
<https://twitter.com/callan_hodgskin>

**Reddit**
<https://www.reddit.com/r/pathos_nethack>

**Discord**
<https://discord.gg/TRhRZhX>

**Made with Invention**
<https://gitlab.com/hodgskin-callan/Invention>
