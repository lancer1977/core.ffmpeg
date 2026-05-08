# Core API sketch

## Public surface goals
The public API should be simple enough for host apps to use without knowing FFmpeg filter syntax.

## Suggested types

### Configuration
- `FfmpegOptions`
- `OverlayOptions`
- `TextOverlayOptions`
- `StreamTarget`
- `EncoderProfile`

### Command building
- `FfmpegCommandBuilder`
- `OverlayCommandBuilder`
- `TextOverlayBuilder`
- `OutputTargetBuilder`
- `composeTwitchRtmpUrl`
- `buildTwitchRtmpOutput`
- `buildBuseyBoxBroadcastCommand`
- `applyRenderPreset`
- `resolveActiveRenderPreset`
- `buildResolvedPipelineConfig`
- `parseProbeOutput`
- `tryParseProbeOutput`

### Probing and utilities
- `FfprobeService`
- `EncoderCapabilityDetector`
- `AtomicTextWriter`
- `VideoFileScanner`
- `resolveEncoderConfig`

### Runtime
- `FfmpegProcessRunner`
- `ProcessResult`
- `StreamSessionState`

## Minimal operation set
- build command args
- probe input media
- start streaming or render job
- update runtime text atomically
- stop or restart the running process
- inspect last run result

## Host expectations
The host app should provide:
- media source path
- output target
- overlay metadata
- runtime text content
- any control URLs or manual tweak settings

## Example usage
```ts
const command = builder.build({
  input: '/media/input.mp4',
  output: { kind: 'rtmp', url: 'rtmp://...' },
  overlay: { image: '/assets/logo.png' },
  text: { file: '/run/now_playing.txt', enabled: true }
});

const twitchArgs = buildTwitchRtmpOutput('rtmp://live.twitch.tv/app/', process.env.TWITCH_STREAM_KEY ?? '');

const buseyBoxCommand = buildBuseyBoxBroadcastCommand({
  inputPath: '/media/input.mp4',
  ingestUrl: 'rtmp://live.twitch.tv/app/',
  streamKey: process.env.TWITCH_STREAM_KEY ?? '',
});

const presetConfig = applyRenderPreset(
  { input: '/media/input.mp4', output: { transport: 'rtmp', target: 'rtmp://example/live/base' } },
  { id: 'soft', name: 'Soft', category: 'basic', enabled: true, filters: [{ name: 'tone', vf: 'eq=contrast=1.2' }] },
);

const activePreset = resolveActiveRenderPreset(
  [
    { id: 'soft', name: 'Soft', enabled: true },
    { id: 'hard', name: 'Hard', enabled: false },
  ],
  'soft',
);

const encoder = resolveEncoderConfig({ videoCodec: 'h264_nvenc', audioCodec: 'aac' }, false);

const pipeline = buildResolvedPipelineConfig({
  config: {
    input: '/media/input.mp4',
    encoding: { videoCodec: 'h264_nvenc', audioCodec: 'aac' },
    output: { transport: 'rtmp', target: 'rtmp://example/live/base' },
  },
  presets,
  activePresetId: 'soft',
  nvencAvailable: false,
});

const probe = tryParseProbeOutput(stdout);
```

The intent is that app code stays small and focused on workflow, not filter graph assembly.
