namespace BierFroh.State;

internal record AppState(int Version, RootState RootState)
{
    public const int ProgramVersion = 1;

    public static AppState Default = new(
        Version: ProgramVersion,
        RootState: RootState.Default
            );
}

internal record RootState(int Version, DateTime? LastVisit, bool IsDarkMode)
{
    public const int ProgramVersion = 1;

    public static RootState Default => new(
        Version: ProgramVersion,
        LastVisit: null,
        IsDarkMode: false);
}
