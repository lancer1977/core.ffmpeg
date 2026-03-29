import test from 'node:test';
import assert from 'node:assert/strict';

import { startFfmpegProcess } from '../dist/index.js';

test('startFfmpegProcess can gracefully stop a child that handles SIGTERM', async () => {
  const script = [
    'process.on("SIGTERM", () => {',
    '  setTimeout(() => process.exit(0), 50);',
    '});',
    'setInterval(() => {}, 1000);',
  ].join(' ');

  const handle = startFfmpegProcess(process.execPath, ['-e', script]);
  await new Promise((resolve) => setTimeout(resolve, 100));
  const result = await handle.stop(1000);

  assert.equal(result.exitCode, 0);
  assert.equal(result.signalCode, null);
});

test('stop resolves for an already exited process', async () => {
  const handle = startFfmpegProcess(process.execPath, ['-e', 'process.exit(0)']);
  await new Promise((resolve) => setTimeout(resolve, 50));

  const result = await handle.stop(1000);

  assert.equal(result.exitCode, 0);
});
