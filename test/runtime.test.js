import test from 'node:test';
import assert from 'node:assert/strict';
import { mkdtempSync, readFileSync, existsSync } from 'node:fs';
import { tmpdir } from 'node:os';
import { join } from 'node:path';

import { atomicWriteText, writeRuntimeText } from '../dist/index.js';

test('atomicWriteText writes content atomically to the target path', () => {
  const dir = mkdtempSync(join(tmpdir(), 'core-ffmpeg-runtime-'));
  const file = join(dir, 'now_playing.txt');

  atomicWriteText(file, 'Now Playing: Demo\n');

  assert.equal(readFileSync(file, 'utf8'), 'Now Playing: Demo\n');
  assert.equal(existsSync(file), true);
});

test('writeRuntimeText delegates to atomic writing', () => {
  const dir = mkdtempSync(join(tmpdir(), 'core-ffmpeg-runtime-'));
  const file = join(dir, 'status.txt');

  writeRuntimeText(file, 'hello\n');

  assert.equal(readFileSync(file, 'utf8'), 'hello\n');
});
