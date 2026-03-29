import { execFile } from 'node:child_process';
import { promisify } from 'node:util';

const execFileAsync = promisify(execFile);

export interface FfmpegProbeResult {
  durationSeconds: number | undefined;
  videoCodec: string | undefined;
  audioCodec: string | undefined;
}

export interface FfprobeJsonPayload {
  format?: { duration?: string };
  streams?: Array<{ codec_type?: string; codec_name?: string }>;
}

export function parseProbeOutput(stdout: string): FfmpegProbeResult {
  const parsed = JSON.parse(stdout) as FfprobeJsonPayload;

  const duration = parsed.format?.duration ? Number(parsed.format.duration) : undefined;
  const videoCodec = parsed.streams?.find((s) => s.codec_type === 'video')?.codec_name;
  const audioCodec = parsed.streams?.find((s) => s.codec_type === 'audio')?.codec_name;

  return {
    durationSeconds: Number.isFinite(duration) ? duration : undefined,
    videoCodec,
    audioCodec,
  };
}

export async function probeMedia(filePath: string): Promise<FfmpegProbeResult> {
  const { stdout } = await execFileAsync('ffprobe', [
    '-v', 'error',
    '-show_entries', 'format=duration:stream=codec_type,codec_name',
    '-of', 'json',
    filePath,
  ]);

  return parseProbeOutput(stdout);
}
