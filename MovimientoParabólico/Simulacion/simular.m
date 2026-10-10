function simular (angulo, fuerza, dirMin, dirMax, fMin, fMax, distanciaHoyo, tolerancia, semilla)
% SIMULAR  Simulador de movimiento parabólico con viento aleatorio (juego de golf).
%
%   simular(angulo, fuerza, dirMin, dirMax, fMin, fMax, distanciaHoyo, tolerancia, semilla)
%
%   TODA la física del movimiento y TODA la generación de valores aleatorios
%   se resuelven aquí, dentro de GNU Octave. El programa en C# sólo invoca
%   este script y luego lee los archivos CSV que genera.
%
%   PARÁMETROS DE ENTRADA
%   ---------------------
%   angulo         Ángulo de salida del golpe, en grados sobre la horizontal [5, 85].
%   fuerza         Velocidad de salida del proyectil, en m/s (> 0).
%   dirMin         Límite inferior del ángulo del viento, en grados [0, 180].
%   dirMax         Límite superior del ángulo del viento, en grados [0, 180].
%   fMin           Límite inferior de la fuerza del viento, en m/s² (>= 0).
%   fMax           Límite superior de la fuerza del viento, en m/s² (>= fMin).
%   distanciaHoyo  Distancia de la meta (hoyo) desde la salida, en metros.
%   tolerancia     Margen de tolerancia para considerar que se alcanzó la meta, en m.
%   semilla        Semilla del generador aleatorio (hace reproducible la corrida).
%
%   ARCHIVOS DE SALIDA (se escriben en el directorio de trabajo actual)
%   ------------------------------------------------------------------
%   resultados.csv  Un registro por fotograma (paso dt = 0.5 s):
%                   t,x,y,vx,vy,magnitud,dir_viento,fuerza_viento,ax_viento
%   resumen.csv     Resultado global de la corrida:
%                   alcanzo,distancia_impacto,tolerancia,tiempo_vuelo,x_impacto,y_max,impacto
%
%   MODELO MATEMÁTICO
%   -----------------
%   El eje Y no es afectado por el viento (parábola pura):
%       y(t)  = y0 + v0y*t - 0.5*g*t^2
%       vy(t) = v0y - g*t
%
%   El eje X sí recibe el viento como una aceleración adicional ax. Como el
%   viento se renueva en cada fotograma, la aceleración se considera
%   constante dentro de cada tramo de duración dt y la integración es exacta:
%       x(t+dt)  = x + vx*dt + 0.5*ax*dt^2
%       vx(t+dt) = vx + ax*dt
%
%   El viento se genera como un vector cuya proyección sobre la dirección del
%   tiro es:
%       ax = fuerza_viento * cos(dir_viento)
%   con dir_viento = 0°  -> viento a favor  (ax > 0)
%        dir_viento = 90° -> viento cruzado (ax = 0)
%        dir_viento = 180°-> viento en contra (ax < 0)
%   Ambos parámetros (dirección y fuerza) se sortean uniformemente dentro de
%   los límites que el usuario cargó en la interfaz.

  % ------------------------------------------------------------------
  % 1. Constantes físicas y de la simulación
  % ------------------------------------------------------------------
  g    = 9.8;    % aceleración de la gravedad [m/s²]
  dt   = 0.5;    % paso de tiempo = un fotograma [s]  (requisito del taller)
  tMax = 120;    % tiempo máximo de vuelo de seguridad [s]

  % ------------------------------------------------------------------
  % 2. Validación de la entrada (defensa adicional; C# ya valida antes)
  % ------------------------------------------------------------------
  if (dirMax < dirMin)
    tmp = dirMin; dirMin = dirMax; dirMax = tmp;
  endif
  if (fMax < fMin)
    tmp = fMin; fMin = fMax; fMax = tmp;
  endif
  if (fMin < 0)
    fMin = 0;
  endif

  % ------------------------------------------------------------------
  % 3. Generación aleatoria REPRODUCIBLE
  %    Con la misma semilla siempre se obtiene exactamente la misma corrida.
  % ------------------------------------------------------------------
  rand("state", semilla);
  randn("state", semilla);

  % ------------------------------------------------------------------
  % 4. Condiciones iniciales del tiro
  % ------------------------------------------------------------------
  theta = angulo * pi / 180;      % ángulo en radianes
  v0x   = fuerza * cos(theta);    % componente horizontal de la velocidad [m/s]
  v0y   = fuerza * sin(theta);    % componente vertical de la velocidad   [m/s]

  t  = 0;      % tiempo transcurrido [s]
  x  = 0;      % posición horizontal [m]
  y  = 0;      % posición vertical   [m]
  vx = v0x;    % velocidad horizontal [m/s]
  vy = v0y;    % velocidad vertical   [m/s]

  % ------------------------------------------------------------------
  % 5. Bucle principal: un fotograma por iteración (dt = 0.5 s)
  % ------------------------------------------------------------------
  res          = [];   % matriz de resultados: una fila por fotograma
  impacto      = false;
  k            = 0;
  maxIteracoes = ceil(tMax / dt) + 4;

  while (k < maxIteracoes)
    % --- 5.1 Agente de generación aleatoria para este fotograma -------
    % Se sortea dentro de los límites cargados por el usuario.
    dir_viento = dirMin + (dirMax - dirMin) * rand();
    faz_viento = fMin  + (fMax  - fMin ) * rand();

    % Proyección del viento sobre la dirección del tiro (1D horizontal).
    ax = faz_viento * cos(dir_viento * pi / 180);

    % --- 5.2 Registrar el fotograma actual ---------------------------
    % La fila k guarda el estado en el instante t y el viento que se
    % aplicará durante el intervalo [t, t + dt).
    k = k + 1;
    res(k, 1) = t;
    res(k, 2) = x;
    res(k, 3) = y;
    res(k, 4) = vx;
    res(k, 5) = vy;
    res(k, 6) = sqrt(vx^2 + vy^2);
    res(k, 7) = dir_viento;
    res(k, 8) = faz_viento;
    res(k, 9) = ax;

    % --- 5.3 Integración exacta del tramo ----------------------------
    % En Y la aceleración es constante (-g): se obtiene la parábola exacta.
    % En X la aceleración es constante (ax) dentro del tramo.
    tau   = dt;
    xN    = x + vx * tau + 0.5 * ax * tau^2;
    yN    = y + vy * tau - 0.5 * g * tau^2;
    vxN   = vx + ax * tau;
    vyN   = vy - g * tau;

    % --- 5.4 ¿Impactó contra el suelo dentro de este tramo? ----------
    if (yN <= 0)
      % Se interpola el instante exacto en que y = 0 dentro del tramo.
      % Resolviendo y + vy*tau - 0.5*g*tau^2 = 0:
      %   tau = (vy + sqrt(vy^2 + 2*g*y)) / g   (raíz no negativa)
      raiz = sqrt(max(0, vy^2 + 2 * g * y));
      tauImpacto = (vy + raiz) / g;

      if (tauImpacto < 0)
        tauImpacto = 0;
      endif
      if (tauImpacto > dt)
        tauImpacto = dt;
      endif

      xImpacto = x + vx * tauImpacto + 0.5 * ax * tauImpacto^2;
      vxFinal  = vx + ax * tauImpacto;
      vyFinal  = vy - g * tauImpacto;

      % Última fila: el proyectil sobre el suelo, en el punto de impacto.
      k = k + 1;
      res(k, 1) = t + tauImpacto;
      res(k, 2) = xImpacto;
      res(k, 3) = 0;
      res(k, 4) = vxFinal;
      res(k, 5) = vyFinal;
      res(k, 6) = sqrt(vxFinal^2 + vyFinal^2);
      res(k, 7) = dir_viento;
      res(k, 8) = faz_viento;
      res(k, 9) = ax;

      impacto = true;
      break;
    endif

    % --- 5.5 Avance al fotograma siguiente --------------------------
    x  = xN;
    y  = yN;
    vx = vxN;
    vy = vyN;
    t  = t + dt;

    % --- 5.6 Cortes de seguridad ------------------------------------
    % Si ya se alejó demasiado del hoyo o del área de juego, se corta.
    if (t > tMax)
      break;
    endif
    if (x > 4 * distanciaHoyo || x < -2 * distanciaHoyo)
      break;
    endif
  endwhile

  if (rows(res) == 0)
    error("simular: no se generó ningún fotograma.");
  endif

  % ------------------------------------------------------------------
  % 6. Verificación del objetivo
  % ------------------------------------------------------------------
  xImpacto = res(end, 2);                  % última posición horizontal
  yFinal   = res(end, 3);
  tVuelo   = res(end, 1);                  % tiempo hasta el último fotograma
  yMax     = max(res(:, 3));               % altura máxima alcanzada

  % La meta sólo se alcanza si el proyectil efectivamente tocó el suelo
  % y su punto de impacto cae dentro de la tolerancia alrededor del hoyo.
  if (impacto && yFinal <= 1e-9)
    distancia = abs(xImpacto - distanciaHoyo);
    alcanzo   = (distancia <= tolerancia);
  else
    distancia = abs(xImpacto - distanciaHoyo);
    alcanzo   = false;
  endif

  % ------------------------------------------------------------------
  % 7. Escritura de los archivos de salida
  % ------------------------------------------------------------------

  % --- 7.1 resultados.csv: recorrido completo ----------------------
  fid = fopen("resultados.csv", "w");
  if (fid < 0)
    error("simular: no se pudo abrir resultados.csv para escritura.");
  endif
  fprintf(fid, "t,x,y,vx,vy,magnitud,dir_viento,fuerza_viento,ax_viento\n");
  for i = 1:rows(res)
    fprintf(fid, "%.6f,%.6f,%.6f,%.6f,%.6f,%.6f,%.6f,%.6f,%.6f\n", ...
            res(i, 1), res(i, 2), res(i, 3), res(i, 4), res(i, 5), ...
            res(i, 6), res(i, 7), res(i, 8), res(i, 9));
  endfor
  fclose(fid);

  % --- 7.2 resumen.csv: resultado global ---------------------------
  fid = fopen("resumen.csv", "w");
  if (fid < 0)
    error("simular: no se pudo abrir resumen.csv para escritura.");
  endif
  fprintf(fid, "alcanzo,distancia_impacto,tolerancia,tiempo_vuelo,x_impacto,y_max,impacto\n");
  fprintf(fid, "%d,%.6f,%.6f,%.6f,%.6f,%.6f,%d\n", ...
          alcanzo, distancia, tolerancia, tVuelo, xImpacto, yMax, impacto);
  fclose(fid);

  % Mensaje informativo para la consola (C# lo captura si lo desea).
  printf("simular: %d fotogramas, t_vuelo=%.2f s, x_impacto=%.2f m, distancia=%.2f m, alcanzo=%d\n", ...
         rows(res), tVuelo, xImpacto, distancia, alcanzo);

endfunction
