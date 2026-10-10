# Taller2Simulacion

**Taller 2 — Simulador de movimiento parabólico con viento aleatorio.**
Integración **C# (WinForms, .NET Framework 4.8)** + **GNU Octave**.

Juego de golf: se cargan el ángulo y la fuerza del golpe, Octave calcula la
trayectoria y el viento aleatorio, y la app anima la pelota un fotograma cada
0,5 s mientras llena las dos tablas de información.

La ventana se divide en dos pestañas —**Partida** (campo de golf y entradas)
y **Tablas de información** (recorrido y viento, una al lado de la otra)— para
que quepan en una pantalla de 1366×768. Al maximizar cada tabla muestra
≈35 filas.

- **[`INFORME.md`](INFORME.md)** — modelo matemático, integración y mapeo con
  la rúbrica.
- **`MovimientoParabólico.sln`** — abrir en Visual Studio 2022.

## Requisitos

- Visual Studio 2022 (.NET Framework 4.8)
- [GNU Octave](https://octave.org) para Windows:
  `winget install -e --id GNU.Octave`
  (se usa `octave-cli.exe`; si no está en el `PATH`, escribir la ruta en el
  campo indicado o definir `OCTAVE_EXECUTABLE`)

## Compilar

```bash
msbuild MovimientoParabólico.sln -t:Rebuild -p:Configuration=Debug
```

## Estructura

```text
MovimientoParabólico/
├── ProcessForm.cs / .Designer.cs   # Interfaz + lógica (sin física)
├── OctaveIntegration.cs            # Invoca Octave y lee los CSV
├── PhysicsCalculator.cs            # Sólo conversión px↔m y formateo
├── DatosSimulacion.cs              # DTOs compartidos
└── Simulacion/simular.m            # FÍSICA + ALEATORIEDAD (GNU Octave)
```
