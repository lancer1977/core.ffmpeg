import test from 'node:test';
import assert from 'node:assert/strict';

import { hasEncoder, parseFfmpegEncoders, resolveEncoderConfig, selectVideoCodec } from '../dist/index.js';

test('parseFfmpegEncoders extracts codec names from ffmpeg encoder output', () => {
  const output = [
    'Encoders:',
    ' V..... h264_nvenc           NVIDIA NVENC H.264 encoder (codec h264)',
    ' V..... hevc_nvenc           NVIDIA NVENC hevc encoder (codec hevc)',
    ' A..... aac                  AAC (Advanced Audio Coding)',
  ].join('\n');

  assert.deepEqual(parseFfmpegEncoders(output), ['h264_nvenc', 'hevc_nvenc', 'aac']);
});

test('hasEncoder reports whether a specific encoder exists', () => {
  const output = ' V..... h264_nvenc           NVIDIA NVENC H.264 encoder';

  assert.equal(hasEncoder(output, 'h264_nvenc'), true);
  assert.equal(hasEncoder(output, 'libx264'), false);
});

test('selectVideoCodec falls back from nvenc to software codecs', () => {
  assert.equal(selectVideoCodec('h264_nvenc', false), 'libx264');
  assert.equal(selectVideoCodec('hevc_nvenc', false), 'libx265');
  assert.equal(selectVideoCodec('libx264', false), 'libx264');
  assert.equal(selectVideoCodec(undefined, false), undefined);
});

test('resolveEncoderConfig returns a codec-adjusted copy', () => {
  const config = { videoCodec: 'h264_nvenc', audioCodec: 'aac' };

  assert.deepEqual(resolveEncoderConfig(config, false), { videoCodec: 'libx264', audioCodec: 'aac' });
  assert.deepEqual(resolveEncoderConfig(config, true), { videoCodec: 'h264_nvenc', audioCodec: 'aac' });
});
