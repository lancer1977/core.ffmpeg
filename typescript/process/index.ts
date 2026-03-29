import { spawn, type ChildProcessWithoutNullStreams } from 'node:child_process';

export interface FfmpegProcessHandle {
  pid: number;
  process: ChildProcessWithoutNullStreams;
  stop: (timeoutMs?: number) => Promise<{ exitCode: number | null; signalCode: NodeJS.Signals | null }>;
}

function waitForExit(proc: ChildProcessWithoutNullStreams): Promise<{ exitCode: number | null; signalCode: NodeJS.Signals | null }> {
  if (proc.exitCode !== null || proc.signalCode !== null) {
    return Promise.resolve({ exitCode: proc.exitCode, signalCode: proc.signalCode });
  }

  return new Promise((resolve) => {
    const onExit = (exitCode: number | null, signalCode: NodeJS.Signals | null) => {
      cleanup();
      resolve({ exitCode, signalCode });
    };

    const cleanup = () => {
      proc.off('exit', onExit);
      proc.off('error', onError);
    };

    const onError = () => {
      cleanup();
      resolve({ exitCode: proc.exitCode, signalCode: proc.signalCode });
    };

    proc.once('exit', onExit);
    proc.once('error', onError);
  });
}

export function startFfmpegProcess(command: string, args: string[]): FfmpegProcessHandle {
  const proc = spawn(command, args, { stdio: 'pipe' });
  if (!proc.pid) throw new Error('Failed to start ffmpeg process.');

  return {
    pid: proc.pid,
    process: proc,
    stop: async (timeoutMs = 3000) => {
      const exited = waitForExit(proc);

      if (proc.exitCode === null && proc.signalCode === null) {
        proc.kill('SIGTERM');
      }

      const timedOut = await Promise.race([
        exited.then(() => false),
        new Promise<boolean>((resolve) => setTimeout(() => resolve(true), timeoutMs)),
      ]);

      if (timedOut && proc.exitCode === null && proc.signalCode === null) {
        proc.kill('SIGKILL');
      }

      return exited;
    },
  };
}
