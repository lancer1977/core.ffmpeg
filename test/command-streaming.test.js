import test from 'node:test';
import assert from 'node:assert/strict';
import { buildFfmpegCommand } from '../dist/index.js';

test('buildFfmpegCommand builds hls output via shared helper', () => {
  const command = buildFfmpegCommand({
    input: 'input.mp4',
    output: { transport: 'hls', target: 'playlist.m3u8' },
  });

  assert.deepEqual(
    command.args.slice(-9),
    ['-f', 'hls', '-hls_time', '4', '-hls_playlist_type', 'event', '-hls_segment_filename', 'playlist-%03d.ts', 'playlist.m3u8']
  );
});

test('buildFfmpegCommand builds rtmp output via shared helper', () => {
  const command = buildFfmpegCommand({
    input: 'input.mp4',
    output: { transport: 'rtmp', target: 'rtmp://example/live/stream' },
  });

  assert.deepEqual(command.args.slice(-3), ['-f', 'flv', 'rtmp://example/live/stream']);
});

test('buildFfmpegCommand builds file output via shared helper', () => {
  const command = buildFfmpegCommand({
    input: 'input.mp4',
    output: { transport: 'file', target: 'output.mp4' },
  });

  assert.equal(command.args.at(-1), 'output.mp4');
});
