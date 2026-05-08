import type { FfmpegEncodingConfig, FfmpegOutputConfig, FfmpegPipelineConfig } from '../config/index.js';
import { resolveEncoderConfig } from '../encoder/index.js';

export interface FfmpegPresetFilter {
  name: string;
  description?: string | null;
  vf?: string | null;
  af?: string | null;
  extraArgs?: string[] | null;
}

export interface FfmpegRenderPreset {
  id: string;
  name: string;
  description?: string | null;
  category?: string | null;
  enabled?: boolean;
  filters?: FfmpegPresetFilter[] | null;
  encoding?: Partial<FfmpegEncodingConfig> | null;
  output?: Partial<FfmpegOutputConfig> | null;
}

export function resolveActiveRenderPreset(
  presets: ReadonlyArray<FfmpegRenderPreset> | undefined,
  activePresetId: string | null | undefined,
): FfmpegRenderPreset | undefined {
  if (!presets?.length || !activePresetId?.trim()) {
    return undefined;
  }

  return presets.find(
    (preset) =>
      preset.enabled !== false &&
      preset.id.trim().toLowerCase() === activePresetId.trim().toLowerCase(),
  );
}

function mergeEncoding(
  base: FfmpegEncodingConfig | undefined,
  preset: Partial<FfmpegEncodingConfig> | null | undefined,
): FfmpegEncodingConfig | undefined {
  if (!base && !preset) {
    return undefined;
  }

  return {
    ...base,
    ...preset,
  };
}

function mergeOutput(
  base: FfmpegOutputConfig,
  preset: Partial<FfmpegOutputConfig> | null | undefined,
): FfmpegOutputConfig {
  return {
    ...base,
    ...preset,
  };
}

export function applyRenderPreset(
  config: FfmpegPipelineConfig,
  preset?: FfmpegRenderPreset | null,
): FfmpegPipelineConfig {
  if (!preset || preset.enabled === false) {
    return config;
  }

  const presetFilters = (preset.filters ?? []).filter((filter) => Boolean(filter.vf || filter.af || filter.extraArgs?.length));

  return {
    ...config,
    encoding: mergeEncoding(config.encoding, preset.encoding),
    output: mergeOutput(config.output, preset.output),
    presetFilters,
  } as FfmpegPipelineConfig;
}

export interface BuildResolvedPipelineConfigOptions {
  config: FfmpegPipelineConfig;
  presets?: ReadonlyArray<FfmpegRenderPreset>;
  activePresetId?: string | null;
  nvencAvailable?: boolean;
}

export function buildResolvedPipelineConfig(options: BuildResolvedPipelineConfigOptions): FfmpegPipelineConfig {
  const activePreset = resolveActiveRenderPreset(options.presets, options.activePresetId);
  const withPreset = applyRenderPreset(options.config, activePreset);

  if (options.nvencAvailable === undefined || !withPreset.encoding) {
    return withPreset;
  }

  return {
    ...withPreset,
    encoding: resolveEncoderConfig(withPreset.encoding, options.nvencAvailable),
  };
}
