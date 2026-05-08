namespace Core.FFmpeg.Tests;

internal static class Program
{
    private static async Task<int> Main()
    {
        Run(CommandBuilderTests.BuildsBasicFileCommand);
        Run(CommandBuilderTests.BuildsRtmpCommandWithCodecAndBitrates);
        Run(CommandBuilderTests.BuildsOverlayAndRuntimeTextFilters);
        Run(BuilderTests.BuildsHlsPreset);
        Run(BuilderTests.BuildsRtmpPreset);
        Run(VersioningTests.SupportsInlineTextOverlay);
        Run(HostHealthTests.ReportsHealthySnapshotForSeededState);
        await RunAsync(HostAuthorizationTests.HealthEndpoint_RemainsAnonymous);
        await RunAsync(HostAuthorizationTests.ProtectedRoutes_RejectAnonymousRequests);
        await RunAsync(HostAuthorizationTests.ProtectedRoutes_AllowConfiguredOperatorCredentials);
        Run(ParityTests.HlsPresetUsesDestinationFileStemForSegmentPattern);
        Run(ParityTests.RtmpPresetWithoutStreamKeyUsesTrimmedDestination);
        Run(ParityTests.MetadataAddsRepeatedMetadataFlagsAndSkipsBlankKeys);
        Run(ParityTests.OverlayNamedPositionAndOpacityAreAppliedWithClamp);
        Run(ParityTests.OverlayInvalidPositionFallsBackToOrigin);
        Run(ParityTests.DrawTextPrefersTextFileAndIncludesFontFileAndFallbackFontSize);
        Run(ParityTests.FileOutputFallsBackToOutputPathWhenTargetDestinationIsBlank);
        await RunAsync(IntegrationTests.CanHandleMissingFfprobeGracefully);
        await RunAsync(IntegrationTests.ProcessRunnerReturnsFailureWhenCommandCannotStart);
        await RunAsync(IntegrationTests.ProcessRunnerCanExecuteDotnetVersion);
        Run(SmokeTests.CommandBuilds);
        Console.WriteLine("All tests passed.");
        return 0;
    }

    private static void Run(Action test)
    {
        test();
        Console.WriteLine($"PASS {test.Method.Name}");
    }

    private static async Task RunAsync(Func<Task> test)
    {
        await test();
        Console.WriteLine($"PASS {test.Method.Name}");
    }
}
