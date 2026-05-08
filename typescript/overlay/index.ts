import type { FfmpegOverlayImage, FfmpegRuntimeText } from '../config/index.js';
import { escapeForFfmpegFilterValue } from '../utils/index.js';

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
  const escapedTextFile = escapeForFfmpegFilterValue(runtimeText.path);
  const escapedFontFile = runtimeText.fontFile ? escapeForFfmpegFilterValue(runtimeText.fontFile) : undefined;
  const box = runtimeText.box ?? true;
  const boxBorderWidth = runtimeText.boxBorderWidth ?? 12;

  return [
    escapedFontFile ? `drawtext=fontfile=${escapedFontFile}` : `drawtext=textfile=${escapedTextFile}`,
    escapedFontFile ? `textfile=${escapedTextFile}` : undefined,
    `reload=${reload}`,
    `fontcolor=${fontColor}`,
    `fontsize=${fontSize}`,
    `x=${x}`,
    `y=${y}`,
    `box=${box ? 1 : 0}`,
    `boxborderw=${boxBorderWidth}`,
    'boxcolor=black@0.5',
  ]
    .filter(Boolean)
    .join(':');
}
