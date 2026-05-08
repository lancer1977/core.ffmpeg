import test from 'node:test';
import assert from 'node:assert/strict';

import { buildBuseyBoxBroadcastCommand } from '../dist/index.js';

test('buildBuseyBoxBroadcastCommand builds the current broadcaster command shape', () => {
  const command = buildBuseyBoxBroadcastCommand({
    inputPath: 'input.mp4',
    ingestUrl: 'rtmp://live.twitch.tv/app/',
    streamKey: '/live_12345',
    streaming: {
      video_codec: 'h264_v4l2m2m',
      video_bitrate: '3000k',
      audio_bitrate: '160k',
      fps: 30,
      gop: 120,
    },
    overlay: {
      watermark: {
        path: 'watermark.png',
        x: 'W-w-20',
        y: 'H-h-20',
      },
      dynamic_text: {
        enabled: true,
        textfile: '/tmp/now_playing.txt',
        x: 24,
        y: 24,
        font_size: 36,
        box: true,
        boxborderw: 12,
        fontfile: '/usr/share/fonts/DejaVuSans.ttf',
      },
      video: {
        scale_w: 1280,
        scale_h: 720,
      },
    },
  });

  assert.equal(command.executable, 'ffmpeg');
  assert.deepEqual(command.args, [
    '-hide_banner',
    '-loglevel',
    'info',
    '-re',
    '-i',
    'input.mp4',
    '-i',
    'watermark.png',
    '-filter_complex',
    '[0:v]scale=1280:720,fps=30,drawtext=fontfile=/usr/share/fonts/DejaVuSans.ttf:textfile=/tmp/now_playing.txt:reload=1:fontcolor=white:fontsize=36:x=24:y=24:box=1:boxborderw=12:boxcolor=black@0.5[v0];[1:v]format=rgba[wm];[v0][wm]overlay=W-w-20:H-h-20:format=auto[vout]',
    '-map',
    '[vout]',
    '-map',
    '0:a?',
    '-g',
    '120',
    '-c:v',
    'h264_v4l2m2m',
    '-b:v',
    '3000k',
    '-maxrate',
    '3000k',
    '-bufsize',
    '6000k',
    '-c:a',
    'aac',
    '-b:a',
    '160k',
    '-ar',
    '48000',
    '-f',
    'flv',
    'rtmp://live.twitch.tv/app/live_12345',
  ]);
});

test('buildBuseyBoxBroadcastCommand uses a simple vf path when no watermark is present', () => {
  const command = buildBuseyBoxBroadcastCommand({
    inputPath: 'input.mp4',
    ingestUrl: 'rtmp://live.twitch.tv/app/',
    streamKey: 'live_12345',
  });

  assert.equal(command.executable, 'ffmpeg');
  assert.ok(command.args.includes('-vf'));
  assert.ok(command.args.includes('scale=1280:720,fps=30'));
  assert.ok(command.args.includes('-g'));
  assert.ok(command.args.includes('120'));
  assert.ok(command.args.includes('-c:v'));
  assert.ok(command.args.includes('h264_v4l2m2m'));
  assert.ok(command.args.includes('-b:v'));
  assert.ok(command.args.includes('3000k'));
  assert.equal(command.args.at(-1), 'rtmp://live.twitch.tv/app/live_12345');
  assert.ok(!command.args.includes('-filter_complex'));
  assert.ok(!command.args.includes('-map'));
});
