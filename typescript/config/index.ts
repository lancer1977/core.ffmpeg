export type FfmpegTransport = 'file' | 'hls' | 'rtmp';

export interface FfmpegOverlayImage {
  path: string;
  x?: number;
  y?: number;
  enabled?: boolean;
}

export interface FfmpegRuntimeText {
  path: string;
  enabled?: boolean;
  reloadSeconds?: number;
  fontColor?: string;
  fontSize?: number;
  x?: string;
  y?: string;
}

export interface FfmpegEncodingConfig {
  videoCodec?: string;
  audioCodec?: string;
  videoBitrateKbps?: number;
  audioBitrateKbps?: number;
  fps?: number;
  gop?: number;
  preset?: string;
  tune?: string;
  crf?: number;
  extraArgs?: string[];
}

export interface FfmpegOutputConfig {
  transport: FfmpegTransport;
  target: string;
  extraArgs?: string[];
}

export interface FfmpegPipelineConfig {
  input: string;
  encoding?: FfmpegEncodingConfig;
  overlays?: FfmpegOverlayImage[];
  runtimeText?: FfmpegRuntimeText;
  output: FfmpegOutputConfig;
}
