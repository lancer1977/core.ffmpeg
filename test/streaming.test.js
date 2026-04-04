import test from 'node:test';
import assert from 'node:assert/strict';
import { buildFileOutput, buildHlsOutput, buildRtmpOutput } from '../dist/index.js';

test('buildRtmpOutput builds flv target args', () => {
  assert.deepEqual(buildRtmpOutput('rtmp://example/live/stream'), ['-f', 'flv', 'rtmp://example/live/stream']);
});

test('buildHlsOutput builds hls target args', () => {
  assert.deepEqual(
    buildHlsOutput('/tmp/playlist.m3u8', '/tmp/segment-%03d.ts', 6),
    ['-f', 'hls', '-hls_time', '6', '-hls_playlist_type', 'event', '-hls_segment_filename', '/tmp/segment-%03d.ts', '/tmp/playlist.m3u8']
  );
});

test('buildFileOutput returns the output path', () => {
  assert.deepEqual(buildFileOutput('/tmp/output.mp4'), ['/tmp/output.mp4']);
});
