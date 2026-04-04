import type { FfmpegPipelineConfig } from '../config/index.js';
import { buildDrawTextFilter, buildOverlayFilterChain } from '../overlay/index.js';

export interface BuiltFfmpegCommand {
  executable: string;
  args: string[];
}

function pushIfDefined(args: string[], flag: string, value: string | number | undefined): void {
  if (value === undefined || value === '') return;
  args.push(flag, String(value));
}

export function buildFfmpegCommand(config: FfmpegPipelineConfig): BuiltFfmpegCommand {
  const args: string[] = ['-y', '-i', config.input];

  for (const overlay of config.overlays ?? []) {
    if (overlay.enabled === false) continue;
    args.push('-i', overlay.path);
  }

  const filters: string[] = [];
  const overlayFilter = buildOverlayFilterChain(config.overlays ?? []);
  if (overlayFilter) filters.push(overlayFilter);
  const drawTextFilter = buildDrawTextFilter(config.runtimeText);
  if (drawTextFilter) filters.push(drawTextFilter);
  if (filters.length > 0) {
    args.push('-filter_complex', filters.join(';'));
  }

  const encoding = config.encoding;
  pushIfDefined(args, '-c:v', encoding?.videoCodec);
  pushIfDefined(args, '-c:a', encoding?.audioCodec);
  pushIfDefined(args, '-b:v', encoding?.videoBitrateKbps ? `${encoding.videoBitrateKbps}k` : undefined);
  pushIfDefined(args, '-b:a', encoding?.audioBitrateKbps ? `${encoding.audioBitrateKbps}k` : undefined);
  pushIfDefined(args, '-r', encoding?.fps);
  pushIfDefined(args, '-g', encoding?.gop);
  pushIfDefined(args, '-preset', encoding?.preset);
  pushIfDefined(args, '-tune', encoding?.tune);
  pushIfDefined(args, '-crf', encoding?.crf);
  if (encoding?.extraArgs?.length) args.push(...encoding.extraArgs);

  if (config.output.transport === 'hls') {
    args.push('-f', 'hls', '-hls_time', '4', '-hls_playlist_type', 'event', '-hls_segment_filename', `${config.output.target.replace(/\.[^.]+$/, '')}-%03d.ts`);
  } else if (config.output.transport === 'rtmp') {
    args.push('-f', 'flv');
  }
  if (config.output.extraArgs?.length) args.push(...config.output.extraArgs);
  args.push(config.output.target);

  return { executable: 'ffmpeg', args };
}
