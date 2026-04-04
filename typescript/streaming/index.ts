export function buildRtmpOutput(targetUrl: string): string[] {
  if (!targetUrl?.trim()) {
    throw new Error('targetUrl is required');
  }

  return ['-f', 'flv', targetUrl];
}

export function buildHlsOutput(playlistPath: string, segmentPattern: string, segmentTimeSeconds = 4): string[] {
  if (!playlistPath?.trim()) {
    throw new Error('playlistPath is required');
  }

  if (!segmentPattern?.trim()) {
    throw new Error('segmentPattern is required');
  }

  return [
    '-f', 'hls',
    '-hls_time', String(segmentTimeSeconds),
    '-hls_playlist_type', 'event',
    '-hls_segment_filename', segmentPattern,
    playlistPath,
  ];
}

export function buildFileOutput(outputPath: string): string[] {
  if (!outputPath?.trim()) {
    throw new Error('outputPath is required');
  }

  return [outputPath];
}
