import { mkdirSync } from 'node:fs';

export function ensureDirectory(path: string): void {
  mkdirSync(path, { recursive: true });
}

export function quoteForFfmpegPath(path: string): string {
  return path.replace(/\\/g, '\\\\').replace(/'/g, "\\'");
}

export function escapeForFfmpegFilterValue(value: string): string {
  return value
    .replace(/\\/g, '\\\\')
    .replace(/:/g, '\\:')
    .replace(/,/g, '\\,')
    .replace(/ /g, '\\ ')
    .replace(/'/g, "\\'");
}
