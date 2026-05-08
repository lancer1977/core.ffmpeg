import test from 'node:test';
import assert from 'node:assert/strict';

import {
  buildDrawTextFilter,
  buildFfmpegCommand,
  buildOverlayFilterChain,
  quoteForFfmpegPath,
} from '../dist/index.js';

test('buildOverlayFilterChain skips disabled overlays and preserves order', () => {
  const filter = buildOverlayFilterChain([
    { path: 'first.png', x: 10, y: 20 },
    { path: 'skip.png', enabled: false },
    { path: 'second.png', x: 5, y: 6 },
  ]);

  assert.equal(filter, '[0:v][1:v]overlay=10:20;[0:v][2:v]overlay=5:6');
});

test('buildDrawTextFilter applies defaults when enabled', () => {
  const filter = buildDrawTextFilter({ path: '/tmp/now_playing.txt' });

  assert.equal(
    filter,
    'drawtext=textfile=/tmp/now_playing.txt:reload=1:fontcolor=white:fontsize=24:x=(w-text_w)/2:y=h-(text_h*2):box=1:boxborderw=12:boxcolor=black@0.5',
  );
});

test('buildDrawTextFilter includes fontfile and box settings when provided', () => {
  const filter = buildDrawTextFilter({
    path: '/tmp/now playing.txt',
    fontFile: '/usr/share/fonts/DejaVuSans.ttf',
    box: false,
    boxBorderWidth: 8,
  });

  assert.equal(
    filter,
    'drawtext=fontfile=/usr/share/fonts/DejaVuSans.ttf:textfile=/tmp/now\\ playing.txt:reload=1:fontcolor=white:fontsize=24:x=(w-text_w)/2:y=h-(text_h*2):box=0:boxborderw=8:boxcolor=black@0.5',
  );
});

test('buildFfmpegCommand assembles inputs, filters, encoding, and output', () => {
  const command = buildFfmpegCommand({
    input: 'input.mp4',
    overlays: [
      { path: 'overlay-a.png', x: 10, y: 20 },
      { path: 'overlay-b.png', enabled: false },
      { path: 'overlay-c.png', x: 5, y: 6 },
    ],
    runtimeText: { path: '/tmp/now_playing.txt' },
    encoding: {
      videoCodec: 'libx264',
      audioCodec: 'aac',
      videoBitrateKbps: 2500,
      audioBitrateKbps: 192,
      fps: 30,
      gop: 60,
      preset: 'veryfast',
      tune: 'zerolatency',
      crf: 23,
      extraArgs: ['-pix_fmt', 'yuv420p'],
    },
    output: {
      transport: 'rtmp',
      target: 'rtmp://example/live/stream',
      extraArgs: ['-shortest'],
    },
  });

  assert.equal(command.executable, 'ffmpeg');
  assert.deepEqual(command.args, [
    '-y',
    '-i',
    'input.mp4',
    '-i',
    'overlay-a.png',
    '-i',
    'overlay-c.png',
    '-filter_complex',
    '[0:v][1:v]overlay=10:20;[0:v][2:v]overlay=5:6;drawtext=textfile=/tmp/now_playing.txt:reload=1:fontcolor=white:fontsize=24:x=(w-text_w)/2:y=h-(text_h*2):box=1:boxborderw=12:boxcolor=black@0.5',
    '-c:v',
    'libx264',
    '-c:a',
    'aac',
    '-b:v',
    '2500k',
    '-b:a',
    '192k',
    '-r',
    '30',
    '-g',
    '60',
    '-preset',
    'veryfast',
    '-tune',
    'zerolatency',
    '-crf',
    '23',
    '-pix_fmt',
    'yuv420p',
    '-f',
    'flv',
    '-shortest',
    'rtmp://example/live/stream',
  ]);
});

test('quoteForFfmpegPath escapes backslashes and single quotes', () => {
  assert.equal(quoteForFfmpegPath(`C:\\Media\\O'Brien.mp4`), `C:\\\\Media\\\\O\\'Brien.mp4`);
});
