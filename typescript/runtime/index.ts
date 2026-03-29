import { mkdirSync, renameSync, writeFileSync } from 'node:fs';
import { dirname } from 'node:path';
import { randomUUID } from 'node:crypto';

export function atomicWriteText(path: string, value: string): void {
  mkdirSync(dirname(path), { recursive: true });

  const tempPath = `${path}.${randomUUID()}.tmp`;
  writeFileSync(tempPath, value, 'utf8');
  renameSync(tempPath, path);
}

export function writeRuntimeText(path: string, value: string): void {
  atomicWriteText(path, value);
}
