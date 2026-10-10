# AGENTS.md - MovimientoParabólico

## Project Overview

C# WinForms desktop app (.NET Framework 4.8) that simulates parabolic motion
as a **golf game**: the player sets the launch angle and force, the ball flies
under gravity with a random per-frame wind, and must land within a tolerance of
the hole.

**The single most important rule of this codebase:**

> All physics (`x(t)`, `y(t)`, wind) and all randomness (wind direction and
> strength) live in **GNU Octave** (`Simulacion/simular.m`).
> C# only (a) invokes Octave as an external process, (b) reads the two CSV
> files Octave writes, (c) converts metres to pixels, and (d) formats values.
> **Never** add a physics formula or an RNG call to a `.cs` file.

## Build & Run Commands

This environment has **msbuild/Mono**, not the .NET SDK. `dotnet build` is not
available.

### Build (Linux, verification only)
```bash
msbuild MovimientoParabólico.sln -t:Rebuild -p:Configuration=Debug
```
Must finish with **0 warnings, 0 errors**.

### Build (Windows, the real delivery)
Open `MovimientoParabólico.sln` in Visual Studio 2022 → `Ctrl+Shift+B` → `F5`.

### Run
```bash
./MovimientoParabólico/bin/Debug/MovimientoParabólico.exe   # or F5 in VS
```

### Project format — do not "modernise" it
The `.csproj` is intentionally in the **legacy non-SDK format** with an
explicit `<Compile Include>` list. Do not migrate it to SDK-style: it breaks
the WinForms Designer in Visual Studio. When adding a `.cs` file you must add
its `<Compile Include>` entry by hand.

### Adding new controls
If you add a control in `ProcessForm.Designer.cs`, you must also add its
`Controls.Add(...)`, any event wiring, and `BeginInit/EndInit` +
`SuspendLayout/ResumeLayout` pairs. The original version of this project was
broken because `tabControl1.TabPages.Add`, `timer1.Tick +=` and
`Controls.Add` were all missing — the app launched as an empty window.

## Testing

No test framework is configured. Verification is done by:

1. `msbuild ... -t:Rebuild` → 0 warnings / 0 errors.
2. Launching the exe briefly under Mono to confirm the form loads without a
   `TypeInitializationException`/`MissingMethodException`.
3. Compiling a throwaway harness (see `/tmp/opencode/octest`) that links
   `DatosSimulacion.cs`, `OctaveIntegration.cs` and `PhysicsCalculator.cs`
   and runs them against a **stub of `octave-cli`** which reproduces the same
   contract (same argv, same two CSV files, same working directory). This
   covers CSV parsing, the 0.5 s step, `resumen.csv` coherence, wind bounds
   and the two error paths (Octave missing, `simular.m` missing).

Octave itself is **not installed here**; `simular.m` must be executed on
Windows.

## Code Style Guidelines

### General
- Use **PascalCase** for class names, method names, property names
- Use **camelCase** for local variables and method parameters
- Use **ALL_CAPS** for constants

### Namespaces
```csharp
namespace MovimientoParabólico
```

### Classes
- Keep WinForms partial classes (Form, Designer) in separate files
- Designer files (.Designer.cs) are auto-generated - do not manually edit
- Use explicit access modifiers (private, public, protected)

### Imports
```csharp
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
```

### Types
- Use explicit types for WinForms controls (int, double, Point, Size)
- Use `double` for anything numeric that may be fractional
- Use `int` for pixel coordinates
- Use `Decimal.ToDouble(...)` to read values out of a `NumericUpDown`

### Localization / numbers
- The UI is Spanish (`es-ES`) but **numbers are formatted with
  `CultureInfo.InvariantCulture`** (dot decimal) everywhere, and parsed the
  same way. This is deliberate: a comma decimal would break Octave's argument
  syntax. Do not "fix" this with the current culture.

### Variables
```csharp
// Fields (private, at class level)
private ResultadoSimulacion datos;
private double escala;
private int origenXPixeles, baseYPixeles;
private double distanciaHoyoMetros, toleranciaMetros;
```

### Event Handlers
- Name format: `{ControlName}_{EventName}` (e.g., `btnLanzar_Click`,
  `timerAnimacion_Tick`)
- Parameters: `(object sender, EventArgs e)` / `MouseEventArgs e` /
  `FormClosingEventArgs e`

### Error Handling
- Show user-friendly MessageBox for errors
- Octave failures return `ResultadoOctave.Exitoso == false` with
  `MensajeError` — always check `Exitoso` before touching `Datos`

### WinForms Specific
- Use `InitializeComponent()` in constructors
- Use `System.Windows.Forms.Timer` for the animation (`Interval = 500`)
- `DataGridView` columns are declared and `Add`-ed in the Designer
- Never access a control from a non-UI thread

### Physics Calculations
- **There are none in C#.** `PhysicsCalculator` only does
  metres ↔ pixels (uniform scale) and formatting.
- If you need a new physical quantity, compute it in `simular.m` and expose
  it as a CSV column.

### Code Organization
1. Fields/Constants
2. Constructors
3. Event Handlers (Load, Click, Tick, FormClosing)
4. Helper methods

### Comments
- Use Spanish comments as the codebase is in Spanish
- Comment physics formulas and calculations (in `simular.m`)
- Explain complex logic

### File Structure
```
MovimientoParabólico/
├── Program.cs                 # Entry point
├── ProcessForm.cs             # Logic: inputs, animation, tables, goal
├── ProcessForm.Designer.cs    # Auto-generated UI (wires EVERY control + timer)
├── ProcessForm.resx
├── PhysicsCalculator.cs       # px<->m conversion + formatting (no physics)
├── DatosSimulacion.cs         # DTOs: ParametrosSimulacion, Fila*, Resumen*
├── OctaveIntegration.cs       # Locate/invoke Octave, read the two CSVs
├── Simulacion/
│   └── simular.m              # PHYSICS + RANDOMNESS (GNU Octave)
├── imagenes_nuevas/           # Fondo.jpg, Golfista.png, Hoyo.png, pelota.png
├── MovimientoParabólico.csproj
└── Properties/
    ├── Resources.resx         # ResXFileRef -> ..\imagenes_nuevas\...
    └── Resources.Designer.cs
```

### Common Patterns
- **Single form** (there is no `BackForm`): `ProcessForm` owns everything.
- The scenario graphics (`picPelota`, `picHoyo`, `picGolfista`) are children
  of `picCampo`; all coordinates are relative to that picture box.
- Flow: `LeerParametros()` → `OctaveIntegration.Ejecutar()` →
  `PrepararEscena()` (scale from the real data) → `IniciarAnimacion()` →
  one row per table per `timerAnimacion_Tick` → `FinalizarAnimacion()`.

### The C# ↔ Octave contract
```
octave-cli.exe --norc --quiet --no-window-system \
  --eval "addpath('<...>/Simulacion'); simular(angulo,fuerza,dirMin,dirMax,fMin,fMax,distanciaHoyo,tolerancia,semilla)"
```
- Arguments formatted with `CultureInfo.InvariantCulture`.
- `WorkingDirectory` = `<BaseDirectory>/Simulacion`.
- Outputs `resultados.csv` (`t,x,y,vx,vy,magnitud,dir_viento,fuerza_viento,ax_viento`)
  and `resumen.csv` (`alcanzo,distancia_impacto,tolerancia,tiempo_vuelo,x_impacto,y_max,impacto`).
- `simular.m` is copied to the output folder by a `<None>` item with
  `CopyToOutputDirectory = PreserveNewest` — keep that entry.

### Debugging
- Use `Console.WriteLine()` / `Debug.WriteLine()` for debug output
- Check `MessageBox` output for user-facing errors
