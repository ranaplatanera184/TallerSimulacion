# Taller 2 — Simulador de movimiento parabólico con viento aleatorio

## Integración C# (Visual Studio) + GNU Octave — *Juego de Golf*

---

## 1. Contexto físico

Se simula el golpe de un golfista que debe dejar la pelota dentro del hoyo.

El **eje Y** es un movimiento parabólico puro, gobernado únicamente por la
gravedad:

```
y(t)  = y₀ + v₀y · t − ½ · g · t²
vy(t) = v₀y − g · t
```

El **eje X** recibe además una aceleración horizontal producida por el viento.
Como el viento cambia en cada fotograma, la aceleración se considera constante
dentro de cada tramo de duración `dt` y la integración es **exacta**:

```
x(t+dt)  = x + vx · dt + ½ · ax · dt²
vx(t+dt) = vx + ax · dt
```

Condiciones iniciales en el instante `t = 0`:

```
v₀x = F · cos(θ)        v₀y = F · sin(θ)        x₀ = 0        y₀ = 0
```

| Magnitud | Valor |
|---|---|
| Gravedad `g` | 9,8 m/s² |
| Paso de tiempo `dt` | 0,5 s (un fotograma, requisito del taller) |
| Tiempo máximo de vuelo de seguridad | 120 s |
| Escala de dibujo | **uniforme en ambos ejes** (píxeles por metro) |

> La escala uniforme es lo que garantiza que la trayectoria dibujada sea
> realmente una parábola y no una figura deformada.

---

## 2. Modelo del viento (aleatorio, unidimensional)

El viento se modela como un vector plano cuya proyección sobre la dirección
del tiro es una aceleración horizontal:

```
ax = fuerza_viento · cos(dir_viento)
```

| `dir_viento` | Efecto |
|---|---|
| 0° | viento a favor (`ax > 0`) |
| 90° | viento cruzado (`ax = 0`) |
| 180° | viento en contra (`ax < 0`) |

El viento **no** afecta el eje vertical, de modo que `y(t)` permanece siendo
una parábola pura.

---

## 3. Todo el cálculo vive en GNU Octave

El script **`MovimientoParabólico/Simulacion/simular.m`** recibe los nueve
parámetros cargados por el usuario y es el único responsable de:

1. **Validar** los límites (reordena `dirMin/dirMax` y `fMin/fMax`).
2. **Fijar la semilla** — `rand("state", semilla)` — para que una misma
   configuración produzca siempre exactamente la misma corrida.
3. **Sortear el viento** en cada fotograma:
   ```matlab
   dir_viento = dirMin + (dirMax - dirMin) * rand();
   faz_viento = fMin  + (fMax  - fMin ) * rand();
   ax         = faz_viento * cos(dir_viento * pi / 180);
   ```
4. **Integrar** la trayectoria paso a paso con `dt = 0.5`.
5. **Interpolar** el instante exacto en que `y = 0` dentro del tramo en que
   ocurre el impacto, para no perder precisión por el paso de tiempo.
6. **Verificar el objetivo** y **escribir los dos CSV**.

### Parámetros de entrada

```text
simular(angulo, fuerza, dirMin, dirMax, fMin, fMax, distanciaHoyo, tolerancia, semilla)
```

### Archivos de salida

**`resultados.csv`** — un registro por fotograma:

```text
t,x,y,vx,vy,magnitud,dir_viento,fuerza_viento,ax_viento
```

**`resumen.csv`** — resultado global:

```text
alcanzo,distancia_impacto,tolerancia,tiempo_vuelo,x_impacto,y_max,impacto
```

---

## 4. Integración C# ↔ Octave

```
┌──────────────┐   parámetros    ┌──────────────────────┐
│  ProcessForm │ ───────────────▶│  octave-cli.exe       │
│   (WinForms) │  --norc --quiet │  --eval "simular(...)"│
│              │  --no-window-   │                        │
│              │      system     │  Simulacion/simular.m  │
│              │ ◀───────────────│  (física + aleatorio)  │
└──────┬───────┘   exit code     └──────────┬─────────────┘
       │                                    │
       │  lee resultados.csv ◀──────────────┘  escribe CSV
       │  lee resumen.csv
       ▼
  DataGridView (Tabla 1 y Tabla 2) + animación
```

El puente está en **`OctaveIntegration.cs`**:

* **Localización del ejecutable**, en este orden:
  1. ruta escrita por el usuario en la interfaz,
  2. variable de entorno `OCTAVE_EXECUTABLE`,
  3. directorios del `PATH`,
  4. rutas típicas de instalación de Octave en Windows
     (`C:\Program Files\GNU Octave\Octave-*\bin` y
     `%LOCALAPPDATA%\Programs\GNU Octave\Octave-*\mingw64\bin`).
  Se busca primero `octave-cli.exe` (no abre ventana ni consola interactiva).
* **Invocación** como proceso externo con redirección de `stdout`/`stderr`,
  ventana oculta y *timeout* de 30 s. Los números se formatean con
  `CultureInfo.InvariantCulture` para que la coma decimal regional no rompa
  la sintaxis de Octave:

  ```
  --norc --quiet --no-window-system --eval "addpath('<carpeta>'); simular(40,40,...)"
  ```

* **Lectura** de los dos CSV con cultura invariante, orden por tiempo, y
  traducción a objetos (`FilaRecorrido`, `FilaViento`, `ResumenSimulacion`).

* `WorkingDirectory` = carpeta `Simulacion` junto al ejecutable, y la ruta usa
  `AppDomain.CurrentDomain.BaseDirectory` (no `Environment.CurrentDirectory`,
  que depende de dónde se lance el programa).

**Regla central:** C# **no** calcula ninguna fórmula física ni genera ningún
valor aleatorio. Lo único que hace con números es convertir metros a píxeles
(`PhysicsCalculator.CalcularEscala`, escala uniforme) y dar formato a los
valores que vienen de Octave.

---

## 5. Interfaz WinForms

Campos precargados con valores razonables para la primera partida:

| Campo | Por defecto |
|---|---|
| Ángulo de salida | 40° |
| Fuerza de salida | 40 m/s |
| Dirección del viento | 0° … 180° |
| Fuerza del viento | 0 … 2 m/s² |
| Distancia al hoyo | 160 m |
| Tolerancia | 5 m |
| Semilla aleatoria | 42 |
| Ruta de Octave | *(vacía = buscar en PATH)* |

Con estos valores, sin viento la pelota cae a ≈160,8 m — prácticamente encima
del hoyo —, de modo que la partida arranca balanceada: la dificultad es
compensar la ráfaga aleatoria, no adivinar la fuerza.

Pantalla — dos pestañas dentro de un `TabControl`, porque con una sola ventana
de 1280×670 de área cliente las dos tablas no caben en una pantalla de
1366×768:

* **Pestaña «Partida»**
  * **Área de golf** (izquierda): fondo, golfista, hoyo y pelota; la escala se
    recalcula con los datos reales de cada corrida para que la parábola siempre
    quepa. Se redibuja también al redimensionar la ventana.
  * **Panel de entradas** (derecha): los nueve campos + botones **Lanzar** y
    **Reiniciar** + rótulo del resultado.
* **Pestaña «Tablas de información»**
  * **Tabla 1 — Recorrido**: `t, x, y, vx, vy, |v|`.
  * **Tabla 2 — Agentes aleatorios**: `t, dirección, fuerza, ax`.

Al pulsar **Lanzar** se salta a la pestaña «Partida» y, al terminar la
animación, el rótulo de estado avisa de que hay que consultar la pestaña de
tablas.

Todo el `TabControl` está anclado a los cuatro lados, de modo que al maximizar
las tablas ganan altura (≈35 filas visibles) y al encoger nunca se solapan:
`MinimumSize` impide bajar de 1310×720 de ventana (≈1294×670 de área cliente),
que es justo lo que necesita el diseño. Ese mínimo se fija en `Form_Load` y
**no** en el diseñador: si se pone en el diseñador, la ventana se infla antes
de calcular los márgenes de anclaje y las dos columnas acaban pisándose.

---

## 6. Animación paso a paso (0,5 s)

Un `System.Windows.Forms.Timer` con `Interval = 500` ms. Cada *tick*:

1. agrega **una fila** a la Tabla 1 y **una fila** a la Tabla 2,
2. recoloca la pelota en la posición del fotograma,
3. actualiza el rótulo de estado (`Fotograma k de n  t = …`),
4. cuando se agotan los fotogramas, detiene el reloj y evalúa el objetivo.

La animación sólo reproduce filas ya calculadas por Octave: no re-calcula
nada.

---

## 7. Verificación del objetivo

La verificación ocurre **dentro de Octave**, con la tolerancia que el usuario
cargo en la interfaz:

```matlab
distancia = abs(xImpacto - distanciaHoyo);
alcanzo   = (impacto && yFinal <= 1e-9) && (distancia <= tolerancia);
```

El resultado se escribe en `resumen.csv` y C# sólo lo presenta:

* rótulo **OBJETIVO ALCANZADO** (verde) o **NO ALCANZADO** (naranja),
* `MessageBox` con distancia al hoyo, tolerancia, tiempo de vuelo, altura
  máxima y punto de impacto.

---

## 8. Instalación y ejecución

### Windows (entrega)

1. Instalar GNU Octave:
   ```powershell
   winget install -e --id GNU.Octave
   ```
   > Se usa **`octave-cli.exe`**, no `octave.exe`.
2. Abrir `MovimientoParabólico.sln` en Visual Studio 2022.
3. **Compilar** (`Ctrl+Shift+B`) y **Ejecutar** (`F5`).
4. Si Octave no está en el `PATH`, escribir la ruta de `octave-cli.exe` en el
   campo indicado, o definir la variable de entorno `OCTAVE_EXECUTABLE`.
5. Pulsa **Lanzar** y observa las dos tablas llenarse una fila por fotograma.

### Verificación en Linux (desarrollo)

```bash
msbuild MovimientoParabólico.sln -t:Rebuild -p:Configuration=Debug
```

Compila con **0 advertencias y 0 errores** (msbuild/Mono 16.10.1, .NET 4.8).

> **Nota de verificación.** GNU Octave no está instalado en esta máquina, por
> lo que `simular.m` todavía no se ha ejecutado aquí. La capa de integración
> se validó con un **doble de Octave** que respeta el mismo contrato (mismos
> argumentos, mismos CSV, misma ruta de trabajo): 26 comprobaciones cubren la
> invocación del proceso, la lectura de ambos CSV, el orden y el paso de
> 0,5 s, la coherencia del `resumen.csv`, los límites del viento sortea­do, la
> escala de dibujo y los dos caminos de error (Octave ausente y `simular.m`
> ausente). El `simular.m` real debe ejecutarse en Windows junto con la
> entrega.

> **Nota de maquetación.** Un segundo arnés (33 comprobaciones) crea la
> ventana, la oculta y luego la mide en tres tamaños —mínimo (1294×670),
> maximizado (1900×1040) y el que impone `MinimumSize`— comprobando en cada
> uno que ningún control se sale de su pestaña, que las dos columnas no se
> solapan y que cada tabla deja ver al menos 13 filas sin scroll (18–20 en el
> tamaño normal, 35 al maximizar). Para poder medir tamaños concretos la
> ventana se oculta antes de medir: en este entorno el gestor de ventanas
> (Hyprland, modo teselado) la reajusta a 664×718 apenas aparece.

---

## 9. Estructura del proyecto

```text
MovimientoParabólico/
├── Program.cs                 # Punto de entrada
├── ProcessForm.cs             # Lógica: entradas, animación, tablas, objetivo
├── ProcessForm.Designer.cs    # UI generada (cablea controles, timer y pestañas)
├── ProcessForm.resx
├── PhysicsCalculator.cs       # Sólo conversión px↔m y formateo (sin física)
├── DatosSimulacion.cs         # DTOs compartidos (parámetros, filas, resumen)
├── OctaveIntegration.cs       # Búsqueda/invocación de Octave + lectura de CSV
├── Simulacion/
│   └── simular.m              # FÍSICA + ALEATORIEDAD  (GNU Octave)
├── imagenes_nuevas/           # Fondo, Golfista, Hoyo, Pelota
└── Properties/
    ├── Resources.resx         # 4 recursos (Fondo, Golfista, Hoyo, Pelota)
    └── Resources.Designer.cs
```

---

## 10. Correspondencia con la rúbrica

| # | Criterio | Pts | Dónde está |
|---|---|---|---|
| 1 | Contexto físico definido | 10 | §1, §2; comentarios de `simular.m` |
| 2 | Modelo parábola + viento | 15 | `simular.m` líneas 29–48, 121–163 |
| 3 | Física y aleatoriedad en Octave | 20 | `simular.m` completo (§3) |
| 4 | Integración C# ↔ Octave | 15 | `OctaveIntegration.cs` (§4) |
| 5 | Interfaz WinForms con campos precargados | 10 | `ProcessForm.Designer.cs` (§5) |
| 6 | Animación paso a paso de 0,5 s | 10 | `timerAnimacion.Interval = 500` (§6) |
| 7 | Dos tablas de información | 10 | `dgvRecorrido` + `dgvViento` (§5) |
| 8 | Verificación de objetivo + informe | 10 | §7, este documento |
| | **Total** | **100** | |

---

## 11. Decisiones de diseño

* **Viento 1D horizontal** — sólo afecta `ax`; así `y(t)` es una parábola pura,
  tal como pide el modelo.
* **Semilla configurable** — permite repetir exactamente una corrida para
  corregir el tiro, y además hace reproducible la demostración.
* **Integración exacta por tramo** — en lugar de un Euler acumulado, que
  introduciría un error creciente y rompería la forma parabólica.
* **Escala de dibujo uniforme** — derivada de los datos reales de cada corrida
  (recorrido horizontal y altura máxima), con un 5–10 % de margen.
* **Formato numérico invariante (punto decimal)** en la interfaz y en el
  intercambio con Octave; evita el clásico fallo de las máquinas con
  configuración regional `es-ES`.
