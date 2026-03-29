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

### Probing and utilities
- `FfprobeService`
- `EncoderCapabilityDetector`
- `AtomicTextWriter`
- `VideoFileScanner`

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
```

The intent is that app code stays small and focused on workflow, not filter graph assembly.
