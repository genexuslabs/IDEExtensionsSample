# GeneXus IDE Extensions Sample

Code samples of **GeneXus IDE extensions** ("packages") that plug into the Windows GeneXus IDE. Use them as a starting point for building your own commands, tools, and dialogs on top of the GeneXus SDK.

## Samples

| Folder | What it does |
| --- | --- |
| [SupportTools](SupportTools/) | Adds a **Shorten Names** command under *Tools → Advanced* that truncates attribute, table, and KB-object names in the current model to the configured significant length. |

## Prerequisites

- Windows
- Visual Studio 2022 (or MSBuild) with .NET Framework **4.7.1** developer pack
- A local install of **GeneXus** (the IDE you're extending)
- One environment variable set:
  - `GX_PROGRAM_DIR` — path to the GeneXus install (extensions are deployed to `%GX_PROGRAM_DIR%\Packages\`)

All `Artech.*` references are resolved as NuGet `PackageReference`s by the [`GeneXus.Package.UI.Sdk`](global.json) MSBuild SDK, restored from the feed configured in [Nuget.config](Nuget.config). No local GeneXus SDK install is required to build.

## Build

Open the solution in Visual Studio, or from a developer prompt:

```cmd
cd SupportTools
msbuild SupportTools.sln /p:Configuration=Debug
```

## Deploy and try it out

After a successful build, copy the binaries into the IDE's `Packages` folder and restart GeneXus:

```cmd
cd SupportTools
UpdateDeploy.bat            :: Debug build
UpdateDeploy.bat Release    :: Release build
```

The new command appears in the IDE menus declared by each sample's `.package` manifest (for **SupportTools**, under *Tools → Advanced → Shorten Names*).

## Anatomy of a package

Each sample is structured the same way — useful when you build your own:

- **`Package.cs`** — class derived from `AbstractPackageUI`, identified by a `[Guid]`. The IDE discovers it via the `[assembly: Package(typeof(...))]` declaration at the top of the same file.
- **`*.package`** (embedded XML manifest) — declares command IDs and binds them into existing IDE menu groups.
- **`CommandKeys.cs` + `CommandManager.cs`** — register `Exec` / `Query` delegates per command; `Query` sets the command's enabled/visible state based on KB context.
- **`Resources.resx`** — strings and images. A resource string whose name matches a command id becomes that command's localized menu label.

## License

[MIT](LICENSE) © GeneXus
