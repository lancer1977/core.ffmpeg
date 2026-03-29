import test from 'node:test';
import assert from 'node:assert/strict';

import { parseProbeOutput } from '../dist/index.js';

test('parseProbeOutput extracts duration and codecs', () => {
  const result = parseProbeOutput(JSON.stringify({
    format: { duration: '123.45' },
    streams: [
      { codec_type: 'audio', codec_name: 'aac' },
      { codec_type: 'video', codec_name: 'h264' },
    ],
  }));

  assert.deepEqual(result, {
    durationSeconds: 123.45,
    videoCodec: 'h264',
    audioCodec: 'aac',
  });
});

test('parseProbeOutput ignores missing and invalid duration values', () => {
  const result = parseProbeOutput(JSON.stringify({
    format: { duration: 'not-a-number' },
    streams: [{ codec_type: 'video', codec_name: 'hevc' }],
  }));

  assert.deepEqual(result, {
    durationSeconds: undefined,
    videoCodec: 'hevc',
    audioCodec: undefined,
  });
});
