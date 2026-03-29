import { execSync } from 'node:child_process';

export function parseFfmpegEncoders(output: string): string[] {
  return output
    .split(/\r?\n/)
    .map((line) => line.trim())
    .map((line) => line.match(/^[A-Z\.]{6}\s+(\S+)/))
    .filter((match): match is RegExpMatchArray => Boolean(match))
    .map((match) => match[1])
    .filter((codec): codec is string => Boolean(codec));
}

export function hasEncoder(output: string, encoderName: string): boolean {
  return parseFfmpegEncoders(output).includes(encoderName);
}

export function checkNvencAvailable(): boolean {
  try {
    const result = execSync('ffmpeg -encoders', { encoding: 'utf-8', stdio: ['ignore', 'pipe', 'ignore'] });
    return hasEncoder(result, 'h264_nvenc');
  } catch {
    return false;
  }
}

export function selectVideoCodec(preferredCodec: string | undefined, nvencAvailable: boolean): string | undefined {
  if (!preferredCodec) return undefined;

  if (preferredCodec === 'h264_nvenc' && !nvencAvailable) {
    return 'libx264';
  }

  if (preferredCodec === 'hevc_nvenc' && !nvencAvailable) {
    return 'libx265';
  }

  return preferredCodec;
}
