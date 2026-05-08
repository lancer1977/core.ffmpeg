import test from 'node:test';
import assert from 'node:assert/strict';

import {
  applyRenderPreset,
  buildFfmpegCommand,
  buildResolvedPipelineConfig,
  resolveActiveRenderPreset,
} from '../dist/index.js';

test('applyRenderPreset merges encoding, output, and preset filters', () => {
  const config = applyRenderPreset(
    {
      input: 'input.mp4',
      output: { transport: 'rtmp', target: 'rtmp://example/live/base', extraArgs: ['-shortest'] },
      encoding: { videoCodec: 'libx264', audioCodec: 'aac', videoBitrateKbps: 2500 },
    },
    {
      id: 'soft',
      name: 'Soft',
      category: 'basic',
      enabled: true,
      filters: [
        {
          name: 'tone',
          vf: 'eq=contrast=1.2',
          af: 'aecho=0.8:0.9:200:0.3',
          extraArgs: ['-metadata', 'comment=preset-test'],
        },
      ],
      output: { target: 'rtmp://example/live/preset' },
      encoding: { preset: 'veryfast' },
    },
  );

  assert.equal(config.output.target, 'rtmp://example/live/preset');
  assert.equal(config.encoding?.preset, 'veryfast');
  assert.equal(config.presetFilters?.length, 1);
  assert.equal(config.presetFilters?.[0].vf, 'eq=contrast=1.2');
  assert.equal(config.presetFilters?.[0].af, 'aecho=0.8:0.9:200:0.3');
});

test('buildFfmpegCommand applies preset filters and extra args', () => {
  const command = buildFfmpegCommand(
    applyRenderPreset(
      {
        input: 'input.mp4',
        output: { transport: 'rtmp', target: 'rtmp://example/live/base' },
        encoding: { videoCodec: 'libx264', audioCodec: 'aac' },
      },
      {
        id: 'soft',
        name: 'Soft',
        category: 'basic',
        enabled: true,
        filters: [
          {
            name: 'tone',
            vf: 'eq=contrast=1.2',
            af: 'aecho=0.8:0.9:200:0.3',
            extraArgs: ['-metadata', 'comment=preset-test'],
          },
        ],
      },
    ),
  );

  assert.equal(command.executable, 'ffmpeg');
  assert.ok(command.args.includes('-filter_complex'));
  assert.ok(command.args.includes('eq=contrast=1.2'));
  assert.ok(command.args.includes('-af'));
  assert.ok(command.args.includes('aecho=0.8:0.9:200:0.3'));
  assert.ok(command.args.includes('-metadata'));
  assert.ok(command.args.includes('comment=preset-test'));
});

test('resolveActiveRenderPreset returns enabled preset matches', () => {
  const presets = [
    { id: 'soft', name: 'Soft', enabled: true },
    { id: 'hard', name: 'Hard', enabled: false },
  ];

  assert.deepEqual(resolveActiveRenderPreset(presets, 'soft'), presets[0]);
  assert.equal(resolveActiveRenderPreset(presets, 'hard'), undefined);
  assert.equal(resolveActiveRenderPreset(presets, ''), undefined);
});

test('buildResolvedPipelineConfig applies active preset and nvenc fallback', () => {
  const config = buildResolvedPipelineConfig({
    config: {
      input: 'input.mp4',
      encoding: { videoCodec: 'h264_nvenc', audioCodec: 'aac' },
      output: { transport: 'rtmp', target: 'rtmp://example/live/base' },
    },
    presets: [
      {
        id: 'soft',
        name: 'Soft',
        enabled: true,
        encoding: { preset: 'veryfast' },
        filters: [{ name: 'tone', vf: 'eq=contrast=1.2' }],
      },
    ],
    activePresetId: 'soft',
    nvencAvailable: false,
  });

  assert.equal(config.encoding?.videoCodec, 'libx264');
  assert.equal(config.encoding?.preset, 'veryfast');
  assert.equal(config.presetFilters?.[0].vf, 'eq=contrast=1.2');
});
