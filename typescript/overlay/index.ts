import type { FfmpegOverlayImage, FfmpegRuntimeText } from '../config/index.js';

export function buildOverlayFilterChain(overlays: FfmpegOverlayImage[] = []): string {
  return overlays
    .filter((overlay) => overlay.enabled !== false)
    .map((overlay, index) => {
      const x = overlay.x ?? 0;
      const y = overlay.y ?? 0;
      return `[0:v][${index + 1}:v]overlay=${x}:${y}`;
    })
    .join(';');
}

export function buildDrawTextFilter(runtimeText?: FfmpegRuntimeText): string | undefined {
  if (!runtimeText || runtimeText.enabled === false) return undefined;

  const reload = runtimeText.reloadSeconds ?? 1;
  const fontColor = runtimeText.fontColor ?? 'white';
  const fontSize = runtimeText.fontSize ?? 24;
  const x = runtimeText.x ?? '(w-text_w)/2';
  const y = runtimeText.y ?? 'h-(text_h*2)';

  return [
    `drawtext=textfile='${runtimeText.path}'`,
    `reload=${reload}`,
    `fontcolor=${fontColor}`,
    `fontsize=${fontSize}`,
    `x=${x}`,
    `y=${y}`,
  ].join(':');
}
