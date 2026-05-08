import { buildDrawTextFilter } from '../overlay/index.js';
import { composeTwitchRtmpUrl } from '../streaming/index.js';
import { escapeForFfmpegFilterValue } from '../utils/index.js';

export interface BuseyBoxStreamingConfig {
  video_codec?: string;
  video_bitrate?: string;
  audio_bitrate?: string;
  fps?: number;
  gop?: number;
}

export interface BuseyBoxWatermarkConfig {
  path?: string;
  x?: string;
  y?: string;
}

export interface BuseyBoxDynamicTextConfig {
  enabled?: boolean;
  textfile?: string;
  x?: number;
  y?: number;
  font_size?: number;
  box?: boolean;
  boxborderw?: number;
  fontfile?: string;
}

export interface BuseyBoxVideoConfig {
  scale_w?: number;
  scale_h?: number;
}

export interface BuseyBoxOverlayProfile {
  watermark?: BuseyBoxWatermarkConfig;
  dynamic_text?: BuseyBoxDynamicTextConfig;
  video?: BuseyBoxVideoConfig;
}

export interface BuseyBoxBroadcastCommandOptions {
  inputPath: string;
  ingestUrl: string;
  streamKey: string;
  streaming?: BuseyBoxStreamingConfig;
  overlay?: BuseyBoxOverlayProfile;
}

export interface BuiltBuseyBoxCommand {
  executable: string;
  args: string[];
}

function toNumber(value: unknown, fallback: number): number {
  const parsed = Number(value);
  return Number.isFinite(parsed) ? parsed : fallback;
}

function buildRuntimeTextFilter(dynamicText: BuseyBoxDynamicTextConfig | undefined): string | undefined {
  if (!dynamicText?.enabled || !dynamicText.textfile) {
    return undefined;
  }

  const runtimeText = {
    path: dynamicText.textfile,
    enabled: true,
    reloadSeconds: 1,
    fontColor: 'white',
    fontSize: toNumber(dynamicText.font_size, 36),
    x: String(toNumber(dynamicText.x, 24)),
    y: String(toNumber(dynamicText.y, 24)),
    box: dynamicText.box ?? true,
    boxBorderWidth: toNumber(dynamicText.boxborderw, 12),
    ...(dynamicText.fontfile ? { fontFile: dynamicText.fontfile } : {}),
  };

  return buildDrawTextFilter(runtimeText);
}

function buildVideoFilter(overlay: BuseyBoxOverlayProfile | undefined, fps: number): string {
  const video = overlay?.video ?? {};
  const scaleW = toNumber(video.scale_w, 1280);
  const scaleH = toNumber(video.scale_h, 720);
  const parts = [`scale=${scaleW}:${scaleH}`, `fps=${fps}`];

  const drawTextFilter = buildRuntimeTextFilter(overlay?.dynamic_text);
  if (drawTextFilter) {
    parts.push(drawTextFilter);
  }

  return parts.join(',');
}

export function buildBuseyBoxBroadcastCommand(options: BuseyBoxBroadcastCommandOptions): BuiltBuseyBoxCommand {
  if (!options.inputPath?.trim()) {
    throw new Error('inputPath is required');
  }

  const streaming = options.streaming ?? {};
  const overlay = options.overlay ?? {};
  const watermark = overlay.watermark ?? {};
  const fps = toNumber(streaming.fps, 30);
  const gop = toNumber(streaming.gop, 120);
  const rtmpUrl = composeTwitchRtmpUrl(options.ingestUrl, options.streamKey);
  const videoFilter = buildVideoFilter(overlay, fps);

  const args: string[] = [
    '-hide_banner',
    '-loglevel',
    'info',
    '-re',
    '-i',
    options.inputPath,
  ];

  if (watermark.path) {
    args.push(
      '-i',
      watermark.path,
      '-filter_complex',
      `[0:v]${videoFilter}[v0];[1:v]format=rgba[wm];[v0][wm]overlay=${escapeForFfmpegFilterValue(watermark.x ?? 'W-w-20')}:${escapeForFfmpegFilterValue(watermark.y ?? 'H-h-20')}:format=auto[vout]`,
      '-map',
      '[vout]',
      '-map',
      '0:a?',
    );
  } else {
    args.push('-vf', videoFilter);
  }

  args.push(
    '-g',
    String(gop),
    '-c:v',
    streaming.video_codec ?? 'h264_v4l2m2m',
    '-b:v',
    streaming.video_bitrate ?? '3000k',
    '-maxrate',
    streaming.video_bitrate ?? '3000k',
    '-bufsize',
    '6000k',
    '-c:a',
    'aac',
    '-b:a',
    streaming.audio_bitrate ?? '160k',
    '-ar',
    '48000',
    '-f',
    'flv',
    rtmpUrl,
  );

  return {
    executable: 'ffmpeg',
    args,
  };
}
